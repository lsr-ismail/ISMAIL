/* =====================================================================
   DONNEES DE DEVELOPPEMENT (a ne PAS utiliser telles quelles chez un client)
   - Utilisateur : admin / admin
   - Machine "Simulateur" : IP 127.0.0.1 (le simulateur d'automate)
   - Configuration minimale : 4 tremies, 2 silos ciment, 1 eau, 2 adjuvants
   - Un client, un chantier, une formule (kg/m3), un camion, un chauffeur, une planification du jour
   ===================================================================== */
USE CentraleBeton;
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Utilisateurs WHERE NomUtilisateur='admin')
    INSERT INTO dbo.Utilisateurs (NomUtilisateur, MotDePasse, RoleUtilisateur) VALUES ('admin','admin','Admin');
IF NOT EXISTS (SELECT 1 FROM dbo.Utilisateurs WHERE NomUtilisateur='operateur')
    INSERT INTO dbo.Utilisateurs (NomUtilisateur, MotDePasse, RoleUtilisateur) VALUES ('operateur','operateur','Operateur');

IF NOT EXISTS (SELECT 1 FROM dbo.Machines)
    INSERT INTO dbo.Machines (NomMachine, TypeMachine, Adresse_IP_PLC, Actif) VALUES ('Simulateur S7-1200','S7-1200','127.0.0.1',1);

IF NOT EXISTS (SELECT 1 FROM dbo.Societe)
    INSERT INTO dbo.Societe (NomSociete, Adresse, Ville, Telephone, Fax, NumeroICE) VALUES ('Centrale de test','1 rue du Beton','Casablanca','0500000000','','000000000000000');
IF NOT EXISTS (SELECT 1 FROM dbo.ConfigurationSociete)
    INSERT INTO dbo.ConfigurationSociete (NomSociete, Adresse, Ville, Telephone, NbreCopiesBL, NbreCopiesFactures, ModeChantier)
    VALUES ('Centrale de test','1 rue du Beton','Casablanca','0500000000',1,1,'TOUJOURS');

IF NOT EXISTS (SELECT 1 FROM dbo.TypesBascule)
    INSERT INTO dbo.TypesBascule (NomType) VALUES ('EAU'),('ADJUVANT'),('GRANULATS'),('CIMENT');

/* Matieres */
IF NOT EXISTS (SELECT 1 FROM dbo.Materiaux)
INSERT INTO dbo.Materiaux (CodeMateriau, NomMateriau, TypeMateriau, UniteStockage, Stock, GestionStocks) VALUES
 ('GRAVIER','Gravier 8/15','GRANULAT','kg',200000,1),
 ('GRAVILLON','Gravillon 3/8','GRANULAT','kg',200000,1),
 ('SABLE','Sable 0/3','GRANULAT','kg',200000,1),
 ('SABLEF','Sable fin','GRANULAT','kg',200000,1),
 ('CPJ45','Ciment CPJ 45','CIMENT','kg',100000,1),
 ('EAU','Eau de gachage','EAU','kg',1000000,1),
 ('ADJ1','Plastifiant','ADJUVANT','kg',5000,1),
 ('ADJ2','Retardateur','ADJUVANT','kg',5000,1);

/* Bascules eau / adjuvants */
IF NOT EXISTS (SELECT 1 FROM dbo.Bascules)
INSERT INTO dbo.Bascules (NomBascule, TypeID, CapaciteMax_kg, IdMateriau, Ordre, Actif)
SELECT 'Bascule eau', (SELECT TypeID FROM dbo.TypesBascule WHERE NomType='EAU'), 1000, (SELECT IdMateriau FROM dbo.Materiaux WHERE CodeMateriau='EAU'), 1, 1
UNION ALL SELECT 'Adjuvant 1', (SELECT TypeID FROM dbo.TypesBascule WHERE NomType='ADJUVANT'), 50, (SELECT IdMateriau FROM dbo.Materiaux WHERE CodeMateriau='ADJ1'), 2, 1
UNION ALL SELECT 'Adjuvant 2', (SELECT TypeID FROM dbo.TypesBascule WHERE NomType='ADJUVANT'), 50, (SELECT IdMateriau FROM dbo.Materiaux WHERE CodeMateriau='ADJ2'), 3, 1;

/* Elements de la centrale (table unique dbo.Silos) */
IF NOT EXISTS (SELECT 1 FROM dbo.Silos)
INSERT INTO dbo.Silos (TypeElement, NumeroElement, NomSilo, CapaciteMax_kg, NiveauMini, NiveauActuel, StockKg, IdMateriau, CouleurGIF, EstActif, Position)
SELECT 'TR',1,'Tremie 1',30000,10,80,24000,(SELECT IdMateriau FROM dbo.Materiaux WHERE CodeMateriau='GRAVIER'),'silo_saphir',1,1
UNION ALL SELECT 'TR',2,'Tremie 2',30000,10,80,24000,(SELECT IdMateriau FROM dbo.Materiaux WHERE CodeMateriau='GRAVILLON'),'silo_saphir',1,2
UNION ALL SELECT 'TR',3,'Tremie 3',30000,10,80,24000,(SELECT IdMateriau FROM dbo.Materiaux WHERE CodeMateriau='SABLE'),'silo_saphir',1,3
UNION ALL SELECT 'TR',4,'Tremie 4',30000,10,80,24000,(SELECT IdMateriau FROM dbo.Materiaux WHERE CodeMateriau='SABLEF'),'silo_saphir',1,4
UNION ALL SELECT 'S',1,'Silo ciment 1',50000,20,80,40000,(SELECT IdMateriau FROM dbo.Materiaux WHERE CodeMateriau='CPJ45'),'silo_saphir',1,5
UNION ALL SELECT 'S',2,'Silo ciment 2',50000,20,80,40000,(SELECT IdMateriau FROM dbo.Materiaux WHERE CodeMateriau='CPJ45'),'silo_saphir',1,6
UNION ALL SELECT 'EAU',1,'Eau',1000,0,100,1000,(SELECT IdMateriau FROM dbo.Materiaux WHERE CodeMateriau='EAU'),'silo_saphir',1,7
UNION ALL SELECT 'ADJ',1,'Adjuvant 1',50,10,80,40,(SELECT IdMateriau FROM dbo.Materiaux WHERE CodeMateriau='ADJ1'),'silo_saphir',1,8
UNION ALL SELECT 'ADJ',2,'Adjuvant 2',50,10,80,40,(SELECT IdMateriau FROM dbo.Materiaux WHERE CodeMateriau='ADJ2'),'silo_saphir',1,9;

/* Cases automate (tremies 1 a 4) */
IF NOT EXISTS (SELECT 1 FROM dbo.Cases)
INSERT INTO dbo.Cases (NumeroCase, TypeMateriau, NomCase, CodeCase, IdMateriau, DosageValide, Actif)
SELECT s.NumeroElement, 'GRANULAT', s.NomSilo, m.CodeMateriau, s.IdMateriau, 1, 1
FROM dbo.Silos s JOIN dbo.Materiaux m ON m.IdMateriau = s.IdMateriau WHERE s.TypeElement='TR';

/* Stock (reconstruit comme le fait ModuleStock.SynchroniserStock) */
IF NOT EXISTS (SELECT 1 FROM dbo.T_STOCK)
INSERT INTO dbo.T_STOCK (STO_Date, MAT_Code, POF_Code, MAT_Nom, MAT_Type, STO_Stock, STO_Stock_Initial, STO_Somme_Entrees, STO_Somme_Sorties, STO_Unite, STO_DateModification, STO_Somme_Corrections)
SELECT GETDATE(), LEFT(CodeMateriau,15), '', LEFT(NomMateriau,50),
       CASE TypeMateriau WHEN 'GRANULAT' THEN 0 WHEN 'CIMENT' THEN 1 WHEN 'EAU' THEN 2 WHEN 'ADJUVANT' THEN 3 ELSE 4 END,
       Stock, Stock, 0, 0, LEFT(ISNULL(UniteStockage,'kg'),3), GETDATE(), 0 FROM dbo.Materiaux;

/* Referentiels commerciaux de test */
IF NOT EXISTS (SELECT 1 FROM dbo.UnitesPS) INSERT INTO dbo.UnitesPS (NomUnite) VALUES ('m3'),('kg'),('heure'),('unite');
IF NOT EXISTS (SELECT 1 FROM dbo.Clients) INSERT INTO dbo.Clients (CodeClient, NomClient, Adresse, Actif, AutorisationDosage) VALUES ('CL001','Client de test','Rue 1',1,1);
IF NOT EXISTS (SELECT 1 FROM dbo.Chantier) INSERT INTO dbo.Chantier (IdClient, CodeChantier, NomChantier, Ville)
    SELECT TOP 1 IdClient, 'CH001', 'Chantier de test', 'Casablanca' FROM dbo.Clients;
IF NOT EXISTS (SELECT 1 FROM dbo.Chauffeurs) INSERT INTO dbo.Chauffeurs (CodeChauffeur, NomChauffeur, Actif) VALUES ('CHF01','Chauffeur de test',1);
IF NOT EXISTS (SELECT 1 FROM dbo.Camions) INSERT INTO dbo.Camions (CodeCamion, NomCamion, Matricule, Capacite, ChargeUtile) VALUES ('TB01','Toupie 1','1234-A-1',8,20000);
IF NOT EXISTS (SELECT 1 FROM dbo.Formules)
BEGIN
    INSERT INTO dbo.Formules (CodeFormule, CleRecherche, NomFormule, TempsMalaxage, Resistance, Consistance)
    VALUES ('B25','B25','Beton B25 test',30,'C25/30','S3');
    DECLARE @f INT = SCOPE_IDENTITY();
    INSERT INTO dbo.FormuleComposition (IdFormule, IdMateriau, TypeMateriau, CodeMateriau, Quantite, Unite, Phase)
    SELECT @f, IdMateriau, TypeMateriau, CodeMateriau,
           CASE CodeMateriau WHEN 'GRAVIER' THEN 700 WHEN 'GRAVILLON' THEN 300 WHEN 'SABLE' THEN 500 WHEN 'SABLEF' THEN 200
                             WHEN 'CPJ45' THEN 350 WHEN 'EAU' THEN 180 WHEN 'ADJ1' THEN 3 ELSE 0 END,
           'kg', 1
    FROM dbo.Materiaux WHERE CodeMateriau IN ('GRAVIER','GRAVILLON','SABLE','SABLEF','CPJ45','EAU','ADJ1');
END
IF NOT EXISTS (SELECT 1 FROM dbo.Planning_journalier)
    INSERT INTO dbo.Planning_journalier (IdClient, IdChantier, IdFormule, DatePlanning, HeureFabrication, HeureLivraison, QuantiteTotale, QuantiteLivree, IdCamion, IdChauffeur, StatutPlanning)
    SELECT c.IdClient, ch.IdChantier, f.IdFormule, CAST(GETDATE() AS DATE), '08:00', '09:00', 16, 0, ca.IdCamion, chf.IdChauffeur, 'Planifié'
    FROM dbo.Clients c, dbo.Chantier ch, dbo.Formules f, dbo.Camions ca, dbo.Chauffeurs chf;

/* Etats synoptique + tags */
IF NOT EXISTS (SELECT 1 FROM dbo.Synoptique_Etats)
INSERT INTO dbo.Synoptique_Etats (SYN_Code, SYN_Etat, SYN_DateMAJ) VALUES
 ('TREMIE_1',0,GETDATE()),('TREMIE_2',0,GETDATE()),('TREMIE_3',0,GETDATE()),('TREMIE_4',0,GETDATE()),('TAPIS',0,GETDATE()),('N_GACHEE',0,GETDATE());
IF NOT EXISTS (SELECT 1 FROM dbo.T_PLC_TAGS)
INSERT INTO dbo.T_PLC_TAGS (TAG_NOM, TAG_VALEUR_REAL, TAG_VALEUR_BOOL, TAG_ACTIF) VALUES
 ('OUT SCAL AGR',0,NULL,1),('OUT SCAL CIM',0,NULL,1),('OUT SCAL EAU',0,NULL,1),('BASCULE ADJ',0,NULL,1),('def malaxeur',NULL,0,1);

/* TVA / taxes courantes */
IF NOT EXISTS (SELECT 1 FROM dbo.T_TVA) INSERT INTO dbo.T_TVA (TVA_Code, TVA_Cle, TVA_Nom, TVA_Montant1, TVA_DateModification) VALUES ('TVA20','TVA20','TVA 20%',20,GETDATE());
GO
