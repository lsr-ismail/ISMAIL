/* =====================================================================
   CentraleBeton — SCHEMA RECONSTRUIT A PARTIR DU CODE DE L'ERP
   ---------------------------------------------------------------------
   ATTENTION : la base d'origine (et ses scripts) n'est pas disponible.
   Ce script est DEDUIT des requetes SQL de l'application (INSERT, UPDATE,
   SELECT, dr("Colonne")). Les noms de colonnes sont fiables ; les TYPES et
   les contraintes sont des choix raisonnables, pas la verite terrain.
   A comparer avec la base d'un client des qu'une sauvegarde est disponible.
   Idempotent : peut etre relance sans erreur (IF OBJECT_ID ... IS NULL).
   ===================================================================== */
IF DB_ID('CentraleBeton') IS NULL CREATE DATABASE CentraleBeton;
GO
USE CentraleBeton;
GO

/* ───── Utilisateurs / societe / machines ───── */
IF OBJECT_ID('dbo.Utilisateurs','U') IS NULL
CREATE TABLE dbo.Utilisateurs (
    IdUtilisateur   INT IDENTITY(1,1) CONSTRAINT PK_Utilisateurs PRIMARY KEY,
    NomUtilisateur  NVARCHAR(100) NOT NULL CONSTRAINT UQ_Utilisateurs_Nom UNIQUE,
    MotDePasse      NVARCHAR(100) NOT NULL,          -- en clair dans l'ERP d'origine (LoginForm.vb)
    RoleUtilisateur NVARCHAR(50)  NOT NULL DEFAULT 'Operateur',
    Actif           BIT NOT NULL DEFAULT 1
);
GO
IF OBJECT_ID('dbo.Machines','U') IS NULL
CREATE TABLE dbo.Machines (
    IdMachine       INT IDENTITY(1,1) CONSTRAINT PK_Machines PRIMARY KEY,
    NomMachine      NVARCHAR(100) NOT NULL,
    TypeMachine     NVARCHAR(50) NULL,
    Adresse_IP_PLC  NVARCHAR(50) NULL,               -- lue par Module1.ChargerConfigPLC
    Actif           BIT NOT NULL DEFAULT 1
);
GO
IF OBJECT_ID('dbo.Societe','U') IS NULL
CREATE TABLE dbo.Societe (
    IdSociete   INT IDENTITY(1,1) CONSTRAINT PK_Societe PRIMARY KEY,
    NomSociete  NVARCHAR(150) NULL, Adresse NVARCHAR(250) NULL, Ville NVARCHAR(100) NULL,
    Telephone   NVARCHAR(50) NULL,  Fax NVARCHAR(50) NULL,      NumeroICE NVARCHAR(50) NULL
);
GO
IF OBJECT_ID('dbo.ConfigurationSociete','U') IS NULL
CREATE TABLE dbo.ConfigurationSociete (
    IdConfigSociete INT IDENTITY(1,1) CONSTRAINT PK_ConfigurationSociete PRIMARY KEY,
    NomSociete NVARCHAR(150) NULL, Adresse NVARCHAR(250) NULL, Ville NVARCHAR(100) NULL,
    Telephone NVARCHAR(50) NULL, Fax NVARCHAR(50) NULL, Email NVARCHAR(150) NULL,
    NumeroRC NVARCHAR(50) NULL, NumeroIF NVARCHAR(50) NULL, NumeroICE NVARCHAR(50) NULL,
    NumeroPatente NVARCHAR(50) NULL, LogoPath NVARCHAR(500) NULL,
    -- colonnes normalement ajoutees par ModuleConfigImpression.VerifierColonnes
    ImprimanteBL NVARCHAR(200) NULL, ImprimantePeseesBPE NVARCHAR(200) NULL, ImprimantePeseesPrefa NVARCHAR(200) NULL,
    ImprimanteFacture NVARCHAR(200) NULL, ImprimantePrincipale NVARCHAR(200) NULL,
    NbreCopiesBL INT NULL, NbreCopiesFactures INT NULL,
    EnteteBL NVARCHAR(250) NULL, EnteteTicket NVARCHAR(250) NULL, EnteteFacture NVARCHAR(250) NULL, EnteteListing NVARCHAR(250) NULL,
    LargeurBL INT NULL, HauteurBL INT NULL, LargeurFacture INT NULL, HauteurFacture INT NULL,
    CommentairesBL NVARCHAR(500) NULL, SansEntete BIT NULL, SansNorme BIT NULL, Draft BIT NULL, Temperature BIT NULL,
    PapierBL BIT NULL, PapierFacture BIT NULL, ModeChantier NVARCHAR(30) NULL, Operateur NVARCHAR(100) NULL
);
GO

/* ───── Licence (stockee dans une table au nom trompeur) ───── */
IF OBJECT_ID('dbo.GranulatsCertification','U') IS NULL
CREATE TABLE dbo.GranulatsCertification (
    IdCertification INT IDENTITY(1,1) CONSTRAINT PK_GranulatsCertification PRIMARY KEY,
    CodeGranulat    NVARCHAR(50)  NULL,   -- code client
    Designation     NVARCHAR(150) NULL,   -- nom client
    DateControle    DATE NULL,
    Granulometrie   INT NULL,             -- nombre de postes
    Disponible      BIT NOT NULL DEFAULT 1,
    CertificatQualite NVARCHAR(200) NULL, -- empreinte machine (ModuleLicence)
    Conforme        BIT NOT NULL DEFAULT 0 -- 1 = licence activee
);
GO

/* ───── Referentiels commerciaux ───── */
IF OBJECT_ID('dbo.Clients','U') IS NULL
CREATE TABLE dbo.Clients (
    IdClient INT IDENTITY(1,1) CONSTRAINT PK_Clients PRIMARY KEY,
    CodeClient NVARCHAR(50) NOT NULL, NomClient NVARCHAR(150) NOT NULL, Adresse NVARCHAR(250) NULL,
    CodePostal NVARCHAR(20) NULL, Pays NVARCHAR(80) NULL, ICE NVARCHAR(50) NULL, Telephone NVARCHAR(50) NULL,
    Fax NVARCHAR(50) NULL, Email NVARCHAR(150) NULL, Zone NVARCHAR(100) NULL, Radial NVARCHAR(100) NULL,
    Contacts NVARCHAR(500) NULL, Remarques NVARCHAR(500) NULL,
    ControleCredit BIT NOT NULL DEFAULT 0, AutorisationDosage BIT NOT NULL DEFAULT 1, Verrouille BIT NOT NULL DEFAULT 0, Actif BIT NOT NULL DEFAULT 1,
    DerniereModification DATETIME NULL
);
GO
IF OBJECT_ID('dbo.Chantier','U') IS NULL
CREATE TABLE dbo.Chantier (
    IdChantier INT IDENTITY(1,1) CONSTRAINT PK_Chantier PRIMARY KEY,
    IdClient INT NULL CONSTRAINT FK_Chantier_Client REFERENCES dbo.Clients(IdClient),
    CodeChantier NVARCHAR(50) NOT NULL, CleChantier NVARCHAR(50) NULL, NomChantier NVARCHAR(150) NOT NULL,
    Rue NVARCHAR(250) NULL, CodePostal NVARCHAR(20) NULL, Ville NVARCHAR(100) NULL, Pays NVARCHAR(80) NULL, Telephone NVARCHAR(50) NULL,
    ZoneFacturation NVARCHAR(100) NULL, ZoneTransport NVARCHAR(100) NULL, PlusValue NVARCHAR(50) NULL,
    NatureChantier NVARCHAR(100) NULL, TypeChantier NVARCHAR(100) NULL,
    MarcheWattmetre BIT NULL, ImpressionPeses BIT NULL, CreationChantierBL BIT NULL, CommandeObligatoire BIT NULL,
    Commande NVARCHAR(100) NULL, DerniereModification DATETIME NULL
);
GO
/* L'ERP interroge aussi dbo.Chantiers (Module1.ChargerPlanificationActive) : alias de compatibilite */
IF OBJECT_ID('dbo.Chantiers','V') IS NULL EXEC('CREATE VIEW dbo.Chantiers AS SELECT * FROM dbo.Chantier');
GO
IF OBJECT_ID('dbo.Chauffeurs','U') IS NULL
CREATE TABLE dbo.Chauffeurs (
    IdChauffeur INT IDENTITY(1,1) CONSTRAINT PK_Chauffeurs PRIMARY KEY,
    CodeChauffeur NVARCHAR(50) NOT NULL, NomChauffeur NVARCHAR(150) NOT NULL, Adresse NVARCHAR(250) NULL,
    CodePostal NVARCHAR(20) NULL, Telephone NVARCHAR(50) NULL, Email NVARCHAR(150) NULL, Remarques NVARCHAR(500) NULL,
    Actif BIT NOT NULL DEFAULT 1, DerniereModification DATETIME NULL
);
GO
IF OBJECT_ID('dbo.Camions','U') IS NULL
CREATE TABLE dbo.Camions (
    IdCamion INT IDENTITY(1,1) CONSTRAINT PK_Camions PRIMARY KEY,
    CodeCamion NVARCHAR(50) NOT NULL, CleCamion NVARCHAR(50) NULL, Matricule NVARCHAR(50) NULL, NomCamion NVARCHAR(150) NULL,
    Fournisseur NVARCHAR(100) NULL, Famille NVARCHAR(100) NULL, Equipement NVARCHAR(100) NULL, Chauffeur NVARCHAR(150) NULL,
    Capacite FLOAT NULL, ChargeUtile FLOAT NULL, CodeRadio NVARCHAR(50) NULL, Telephone NVARCHAR(50) NULL, DerniereModification DATETIME NULL
);
GO
IF OBJECT_ID('dbo.Fournisseurs','U') IS NULL
CREATE TABLE dbo.Fournisseurs (
    IdFournisseur INT IDENTITY(1,1) CONSTRAINT PK_Fournisseurs PRIMARY KEY,
    CodeFournisseur NVARCHAR(50) NOT NULL, CleFournisseur NVARCHAR(50) NULL, NomFournisseur NVARCHAR(150) NOT NULL,
    Rue NVARCHAR(250) NULL, Ville NVARCHAR(100) NULL, CodePostal NVARCHAR(20) NULL,
    MatierePremiere BIT NULL, Transport BIT NULL, Service BIT NULL, DerniereModification DATETIME NULL
);
GO
IF OBJECT_ID('dbo.Transporteurs','U') IS NULL
CREATE TABLE dbo.Transporteurs (
    IdTransporteurs INT IDENTITY(1,1) CONSTRAINT PK_Transporteurs PRIMARY KEY,
    CodeTransporteurs NVARCHAR(50) NOT NULL, CleTransporteurs NVARCHAR(50) NULL, NomTransporteurs NVARCHAR(150) NOT NULL,
    Famille NVARCHAR(100) NULL, Equipement NVARCHAR(100) NULL, Service1 NVARCHAR(100) NULL, Service2 NVARCHAR(100) NULL, Service3 NVARCHAR(100) NULL,
    Valeur1 FLOAT NULL, Valeur2 FLOAT NULL, Valeur3 FLOAT NULL, DerniereModification DATETIME NULL
);
GO
IF OBJECT_ID('dbo.Zones','U') IS NULL
CREATE TABLE dbo.Zones (
    IdZone INT IDENTITY(1,1) CONSTRAINT PK_Zones PRIMARY KEY,
    CodeZone NVARCHAR(50) NOT NULL, CleZone NVARCHAR(50) NULL, NomZone NVARCHAR(150) NULL, TypeZone NVARCHAR(50) NULL,
    CodePostal NVARCHAR(20) NULL, NumeroZone NVARCHAR(20) NULL, DerniereModification DATETIME NULL
);
GO
IF OBJECT_ID('dbo.Tables','U') IS NULL
CREATE TABLE dbo.Tables (
    IdTable INT IDENTITY(1,1) CONSTRAINT PK_Tables PRIMARY KEY,
    NomTable NVARCHAR(100) NOT NULL, CodeElement NVARCHAR(50) NOT NULL, NomElement NVARCHAR(150) NULL,
    ServiceAssocie1 NVARCHAR(100) NULL, ServiceAssocie2 NVARCHAR(100) NULL, ServiceAssocie3 NVARCHAR(100) NULL,
    CodeExport NVARCHAR(50) NULL, Normee BIT NULL, Impression BIT NULL, DerniereModification DATETIME NULL
);
GO
IF OBJECT_ID('dbo.T_TVA','U') IS NULL
CREATE TABLE dbo.T_TVA (
    TVA_Id INT IDENTITY(1,1) CONSTRAINT PK_T_TVA PRIMARY KEY,
    TVA_Code NVARCHAR(50) NOT NULL, TVA_Cle NVARCHAR(50) NULL, TVA_Nom NVARCHAR(150) NULL,
    TVA_DateTarif1 DATETIME NULL, TVA_DateTarif2 DATETIME NULL, TVA_Montant1 FLOAT NULL, TVA_Montant2 FLOAT NULL,
    TVA_Flag_Interdiction BIT NULL, TVA_DateModification DATETIME NULL
);
GO
IF OBJECT_ID('dbo.Taxes','U') IS NULL
CREATE TABLE dbo.Taxes (
    IdTaxe INT IDENTITY(1,1) CONSTRAINT PK_Taxes PRIMARY KEY,
    CodeTaxe NVARCHAR(50) NOT NULL, NomTaxe NVARCHAR(150) NULL, Taux FLOAT NULL, CompteComptable NVARCHAR(50) NULL,
    Actif BIT NOT NULL DEFAULT 1, DerniereModification DATETIME NULL
);
GO
IF OBJECT_ID('dbo.UnitesPS','U') IS NULL
CREATE TABLE dbo.UnitesPS (IdUnite INT IDENTITY(1,1) CONSTRAINT PK_UnitesPS PRIMARY KEY, NomUnite NVARCHAR(50) NOT NULL);
GO
IF OBJECT_ID('dbo.ProduitsServices','U') IS NULL
CREATE TABLE dbo.ProduitsServices (
    IdProduit INT IDENTITY(1,1) CONSTRAINT PK_ProduitsServices PRIMARY KEY,
    CodeProduit NVARCHAR(50) NOT NULL, NomProduit NVARCHAR(150) NULL, Actif BIT NULL DEFAULT 1
);
GO
IF OBJECT_ID('dbo.Produits','U') IS NULL
CREATE TABLE dbo.Produits (
    ProduitID INT IDENTITY(1,1) CONSTRAINT PK_Produits PRIMARY KEY,
    Reference NVARCHAR(50) NULL, NomProduit NVARCHAR(150) NULL
);
GO
IF OBJECT_ID('dbo.ReferencesCommerciales','U') IS NULL
CREATE TABLE dbo.ReferencesCommerciales (
    IdReference INT IDENTITY(1,1) CONSTRAINT PK_ReferencesCommerciales PRIMARY KEY,
    CodeReference NVARCHAR(50) NOT NULL, Designation NVARCHAR(250) NULL
    -- colonnes de classes/normes lues par ModuleImpressionBL (aucun plantage si absentes)
);
GO
IF OBJECT_ID('dbo.Normes','U') IS NULL
CREATE TABLE dbo.Normes (
    IdNorme INT IDENTITY(1,1) CONSTRAINT PK_Normes PRIMARY KEY,
    ClasseExposition NVARCHAR(50) NULL, RapportECLimite FLOAT NULL, ClasseResistance NVARCHAR(50) NULL, TeneurMiniLiant FLOAT NULL,
    CoefD FLOAT NULL, CoefL FLOAT NULL, CoefS FLOAT NULL, CoefU FLOAT NULL, CoefV FLOAT NULL
);
GO

/* ───── Matieres, stocks, formules ───── */
IF OBJECT_ID('dbo.Materiaux','U') IS NULL
CREATE TABLE dbo.Materiaux (
    IdMateriau INT IDENTITY(1,1) CONSTRAINT PK_Materiaux PRIMARY KEY,
    CodeMateriau NVARCHAR(50) NOT NULL, NomMateriau NVARCHAR(150) NULL,
    TypeMateriau NVARCHAR(50) NULL,           -- GRANULAT / CIMENT / EAU / ADJUVANT / ADDITION (ModuleStock.vb)
    CodeMatierePremiere NVARCHAR(50) NULL, UniteStockage NVARCHAR(10) NULL DEFAULT 'kg',
    Stock FLOAT NULL DEFAULT 0, GestionStocks BIT NULL DEFAULT 1, DerniereModification DATETIME NULL
);
GO
IF OBJECT_ID('dbo.T_STOCK','U') IS NULL
CREATE TABLE dbo.T_STOCK (
    STO_ID INT IDENTITY(1,1) CONSTRAINT PK_T_STOCK PRIMARY KEY,
    STO_Date DATETIME NULL, MAT_Code NVARCHAR(15) NOT NULL, POF_Code NVARCHAR(15) NULL, MAT_Nom NVARCHAR(50) NULL, MAT_Type INT NULL,
    STO_Stock FLOAT NULL DEFAULT 0, STO_Stock_Initial FLOAT NULL DEFAULT 0, STO_Somme_Entrees FLOAT NULL DEFAULT 0,
    STO_Somme_Sorties FLOAT NULL DEFAULT 0, STO_Unite NVARCHAR(5) NULL, STO_DateModification DATETIME NULL, STO_Somme_Corrections FLOAT NULL DEFAULT 0
);
GO
IF OBJECT_ID('dbo.Formules','U') IS NULL
CREATE TABLE dbo.Formules (
    IdFormule INT IDENTITY(1,1) CONSTRAINT PK_Formules PRIMARY KEY,
    CodeFormule NVARCHAR(50) NOT NULL, CleRecherche NVARCHAR(50) NULL, NomFormule NVARCHAR(150) NULL, Gamme NVARCHAR(100) NULL,
    TempsMalaxage FLOAT NULL, MES FLOAT NULL, VolumeAir FLOAT NULL, TauxChlorureCalc FLOAT NULL, MasseAlcalinCalc FLOAT NULL,
    ClasseExposition NVARCHAR(50) NULL, AdditionA NVARCHAR(50) NULL, Designation NVARCHAR(250) NULL, Certification NVARCHAR(100) NULL,
    Attestation NVARCHAR(100) NULL, NatureCiment NVARCHAR(100) NULL, Consistance NVARCHAR(50) NULL, GranulariteDmax NVARCHAR(50) NULL,
    Resistance NVARCHAR(50) NULL, Controle NVARCHAR(50) NULL, ClasseChlorure NVARCHAR(50) NULL, RapportEC FLOAT NULL,
    Adjuvants NVARCHAR(250) NULL, RemarqueNorme NVARCHAR(500) NULL, DerniereModification DATETIME NULL
);
GO
IF OBJECT_ID('dbo.FormuleComposition','U') IS NULL
CREATE TABLE dbo.FormuleComposition (
    IdComposition INT IDENTITY(1,1) CONSTRAINT PK_FormuleComposition PRIMARY KEY,
    IdFormule INT NOT NULL CONSTRAINT FK_FormuleComposition_Formule REFERENCES dbo.Formules(IdFormule) ON DELETE CASCADE,
    IdMateriau INT NULL CONSTRAINT FK_FormuleComposition_Materiau REFERENCES dbo.Materiaux(IdMateriau),
    TypeMateriau NVARCHAR(50) NULL, CodeMateriau NVARCHAR(50) NULL,
    Quantite FLOAT NULL,                      -- kg (ou L) PAR m3 (Form_ConfirmationBL.vb l.703)
    Unite NVARCHAR(10) NULL, Consigne FLOAT NULL, Phase INT NULL, PourcentageConsigne FLOAT NULL,
    DecalageSecondes INT NULL, Controle BIT NULL, AdjuvantEnPourcentDosage BIT NULL
);
GO

/* ───── Configuration de la centrale (silos, bascules, cases) ───── */
IF OBJECT_ID('dbo.TypesBascule','U') IS NULL
CREATE TABLE dbo.TypesBascule (TypeID INT IDENTITY(1,1) CONSTRAINT PK_TypesBascule PRIMARY KEY, NomType NVARCHAR(50) NOT NULL);
GO
IF OBJECT_ID('dbo.Bascules','U') IS NULL
CREATE TABLE dbo.Bascules (
    BasculeID INT IDENTITY(1,1) CONSTRAINT PK_Bascules PRIMARY KEY,
    NomBascule NVARCHAR(100) NOT NULL,
    TypeID INT NOT NULL CONSTRAINT FK_Bascules_Type REFERENCES dbo.TypesBascule(TypeID),
    CapaciteMax_kg FLOAT NULL, IdMateriau INT NULL CONSTRAINT FK_Bascules_Materiau REFERENCES dbo.Materiaux(IdMateriau),
    Ordre INT NULL, Actif BIT NOT NULL DEFAULT 1, DB_PLC INT NULL, Offset_Pose INT NULL, Offset_Consigne INT NULL, DateModif DATETIME NULL
);
GO
IF OBJECT_ID('dbo.Silos','U') IS NULL
CREATE TABLE dbo.Silos (
    SiloID INT IDENTITY(1,1) CONSTRAINT PK_Silos PRIMARY KEY,
    TypeElement NVARCHAR(10) NOT NULL,        -- 'TR' tremie, 'S' silo ciment, 'ADJ', 'EAU'
    NumeroElement INT NOT NULL,
    NomSilo NVARCHAR(100) NULL, CapaciteMax_kg FLOAT NULL, NiveauMini FLOAT NULL, NiveauActuel FLOAT NULL, StockKg FLOAT NULL,
    IdMateriau INT NULL CONSTRAINT FK_Silos_Materiau REFERENCES dbo.Materiaux(IdMateriau),
    ProduitID INT NULL CONSTRAINT FK_Silos_Produit REFERENCES dbo.Produits(ProduitID),
    BasculeID INT NULL CONSTRAINT FK_Silos_Bascule REFERENCES dbo.Bascules(BasculeID),
    CouleurGIF NVARCHAR(50) NULL, EstActif BIT NOT NULL DEFAULT 1, DB_PLC INT NULL, Offset_Niveau INT NULL, Position INT NULL, DateModif DATETIME NULL
);
GO
IF OBJECT_ID('dbo.HistoriqueNiveaux','U') IS NULL
CREATE TABLE dbo.HistoriqueNiveaux (
    IdHisto INT IDENTITY(1,1) CONSTRAINT PK_HistoriqueNiveaux PRIMARY KEY,
    SiloID INT NOT NULL CONSTRAINT FK_HistoNiv_Silo REFERENCES dbo.Silos(SiloID),
    NiveauPct FLOAT NULL, NiveauKg FLOAT NULL, DateMesure DATETIME NOT NULL DEFAULT GETDATE()
);
GO
IF OBJECT_ID('dbo.Cases','U') IS NULL
CREATE TABLE dbo.Cases (
    NumeroCase INT NOT NULL CONSTRAINT PK_Cases PRIMARY KEY,
    TypeMateriau NVARCHAR(50) NULL, NomCase NVARCHAR(100) NULL, CodeCase NVARCHAR(50) NULL,
    IdMateriau INT NULL CONSTRAINT FK_Cases_Materiau REFERENCES dbo.Materiaux(IdMateriau),
    DosageValide BIT NULL, Actif BIT NULL DEFAULT 1,
    TempsDosageMax FLOAT NULL, CapaciteCase FLOAT NULL, SeuilAlerte FLOAT NULL, NiveauCase FLOAT NULL, OrdreDosage INT NULL,
    DebitRattrapage FLOAT NULL, NumCaseCommune INT NULL,
    GR_DestBascule INT NULL, GR_SeuilPV FLOAT NULL, GR_SeuilDV FLOAT NULL, GR_CanonSeuilD FLOAT NULL, GR_CanonSeuil FLOAT NULL, GR_CanonTemps FLOAT NULL,
    GR_TapisSeuil FLOAT NULL, GR_TapisTemps FLOAT NULL, GR_BatDuree FLOAT NULL, GR_BatMort FLOAT NULL, GR_Hygro BIT NULL, GR_SensRotation INT NULL,
    L_TypeLiant INT NULL, L_BascLiants INT NULL, L_Compartiment INT NULL, L_SeuilPV FLOAT NULL, L_SeuilDV FLOAT NULL, L_CanonSeuilD FLOAT NULL, L_CanonTemps FLOAT NULL,
    E_DestBascule INT NULL, E_TypeDosage INT NULL, E_EauChargee BIT NULL, E_Compartiment INT NULL, E_SeuilPV FLOAT NULL, E_ChargeMES BIT NULL,
    A_DestBascule INT NULL, A_TypeDosage INT NULL, A_SeuilPV FLOAT NULL, A_DebitPompe FLOAT NULL
);
GO

/* ───── Planning, commandes, bons de livraison, production ───── */
IF OBJECT_ID('dbo.DemandesDosageBeton','U') IS NULL
CREATE TABLE dbo.DemandesDosageBeton (
    IdDemande INT IDENTITY(1,1) CONSTRAINT PK_DemandesDosage PRIMARY KEY,
    DateDemande DATETIME NOT NULL DEFAULT GETDATE(),
    IdClient INT NULL REFERENCES dbo.Clients(IdClient), IdChantier INT NULL REFERENCES dbo.Chantier(IdChantier),
    IdFormule INT NULL REFERENCES dbo.Formules(IdFormule), IdCamion INT NULL REFERENCES dbo.Camions(IdCamion),
    VolumeM3 FLOAT NULL, StatutDemande NVARCHAR(30) NULL
);
GO
IF OBJECT_ID('dbo.Planning_journalier','U') IS NULL
CREATE TABLE dbo.Planning_journalier (
    IdPlanning INT IDENTITY(1,1) CONSTRAINT PK_Planning_journalier PRIMARY KEY,
    IdClient INT NULL CONSTRAINT FK_PJ_Client REFERENCES dbo.Clients(IdClient),
    IdChantier INT NULL CONSTRAINT FK_PJ_Chantier REFERENCES dbo.Chantier(IdChantier),
    IdFormule INT NULL CONSTRAINT FK_PJ_Formule REFERENCES dbo.Formules(IdFormule),
    DatePlanning DATE NOT NULL, HeureFabrication TIME(0) NULL, HeureLivraison TIME(0) NULL,
    QuantiteTotale FLOAT NULL DEFAULT 0, QuantiteLivree FLOAT NULL DEFAULT 0,
    Complement BIT NULL DEFAULT 0, AConfirmer BIT NULL DEFAULT 0, CadenceMinutes INT NULL, VolumeRotationMaxi FLOAT NULL,
    IdCamion INT NULL CONSTRAINT FK_PJ_Camion REFERENCES dbo.Camions(IdCamion),
    IdChauffeur INT NULL CONSTRAINT FK_PJ_Chauffeur REFERENCES dbo.Chauffeurs(IdChauffeur),
    IdEquipementService INT NULL, QuantiteService FLOAT NULL, Remarque NVARCHAR(500) NULL,
    StatutPlanning NVARCHAR(30) NULL DEFAULT 'Planifié', DateCreation DATETIME NULL DEFAULT GETDATE(), DateModification DATETIME NULL
);
GO
IF OBJECT_ID('dbo.Planning_ProduitsServices','U') IS NULL
CREATE TABLE dbo.Planning_ProduitsServices (
    IdPPS INT IDENTITY(1,1) CONSTRAINT PK_Planning_PS PRIMARY KEY,
    IdPlanning INT NOT NULL CONSTRAINT FK_PPS_Planning REFERENCES dbo.Planning_journalier(IdPlanning) ON DELETE CASCADE,
    NumeroLigne INT NOT NULL, IdProduit INT NOT NULL CONSTRAINT FK_PPS_Produit REFERENCES dbo.ProduitsServices(IdProduit),
    Quantite FLOAT NULL, Unite NVARCHAR(50) NULL
);
GO
IF OBJECT_ID('dbo.Planning_camions','U') IS NULL
CREATE TABLE dbo.Planning_camions (
    IdPlanningCamion INT IDENTITY(1,1) CONSTRAINT PK_Planning_camions PRIMARY KEY,
    DatePlanning DATE NULL, HeureDepart TIME(0) NULL, HeureRetour TIME(0) NULL,
    IdCamion INT NULL REFERENCES dbo.Camions(IdCamion), IdChauffeur INT NULL REFERENCES dbo.Chauffeurs(IdChauffeur),
    NumeroBL NVARCHAR(30) NULL, QuantiteM3 FLOAT NULL, NumeroTour INT NULL, Observation NVARCHAR(500) NULL, Statut NVARCHAR(30) NULL
);
GO
IF OBJECT_ID('dbo.BonsLivraison','U') IS NULL
CREATE TABLE dbo.BonsLivraison (
    IdBL INT IDENTITY(1,1) CONSTRAINT PK_BonsLivraison PRIMARY KEY,
    NumeroBL NVARCHAR(30) NOT NULL CONSTRAINT UQ_BonsLivraison_Numero UNIQUE,
    DateHeure DATETIME NOT NULL DEFAULT GETDATE(),
    IdClient INT NULL CONSTRAINT FK_BL_Client REFERENCES dbo.Clients(IdClient), CodeClient NVARCHAR(50) NULL, NomClient NVARCHAR(150) NULL,
    IdChantier INT NULL CONSTRAINT FK_BL_Chantier REFERENCES dbo.Chantier(IdChantier), CodeChantier NVARCHAR(50) NULL, NomChantier NVARCHAR(150) NULL,
    IdFormule INT NULL CONSTRAINT FK_BL_Formule REFERENCES dbo.Formules(IdFormule), CodeFormule NVARCHAR(50) NULL, NomFormule NVARCHAR(150) NULL,
    CodeCamion NVARCHAR(50) NULL, NomChauffeur NVARCHAR(150) NULL, VolumeM3 FLOAT NULL,
    Statut NVARCHAR(20) NOT NULL DEFAULT 'EN_COURS',   -- EN_COURS / LIVRE
    Imprime BIT NOT NULL DEFAULT 0, Operateur NVARCHAR(100) NULL, DateCreation DATETIME NULL DEFAULT GETDATE(), DateModification DATETIME NULL,
    IdPlanning INT NULL, NumeroCommande NVARCHAR(100) NULL, QteCommandee FLOAT NULL, QteCumulee FLOAT NULL,
    PoidsGranulat FLOAT NULL, PoidsCiment FLOAT NULL, PoidsEau FLOAT NULL,
    CpuReelTR1 FLOAT NULL, CpuTheoTR1 FLOAT NULL, CpuReelTR2 FLOAT NULL, CpuTheoTR2 FLOAT NULL, CpuReelTR3 FLOAT NULL, CpuTheoTR3 FLOAT NULL,
    CpuReelTR4 FLOAT NULL, CpuTheoTR4 FLOAT NULL, CpuReelCIM FLOAT NULL, CpuTheoCIM FLOAT NULL, CpuReelEAU FLOAT NULL, CpuTheoEAU FLOAT NULL,
    CpuReelADJ1 FLOAT NULL, CpuTheoADJ1 FLOAT NULL, CpuReelADJ2 FLOAT NULL, CpuTheoADJ2 FLOAT NULL,
    CpuReelADJ3 FLOAT NULL, CpuTheoADJ3 FLOAT NULL, CpuReelADJ4 FLOAT NULL, CpuTheoADJ4 FLOAT NULL
);
GO
IF OBJECT_ID('dbo.PlanningLivraisons','U') IS NULL
CREATE TABLE dbo.PlanningLivraisons (
    IdPlanningLivraison INT IDENTITY(1,1) CONSTRAINT PK_PlanningLivraisons PRIMARY KEY,
    IdPlanning INT NOT NULL CONSTRAINT FK_PL_Planning REFERENCES dbo.Planning_journalier(IdPlanning),
    NumeroTour INT NULL, NumeroBL NVARCHAR(30) NULL, Vehicule NVARCHAR(100) NULL, QuantiteM3 FLOAT NULL,
    HeureFabrication TIME(0) NULL, HeureLivraison TIME(0) NULL, CumulM3 FLOAT NULL, Operateur NVARCHAR(100) NULL, Observation NVARCHAR(500) NULL
);
GO
IF OBJECT_ID('dbo.GacheesDetails','U') IS NULL
CREATE TABLE dbo.GacheesDetails (
    IdGachee INT IDENTITY(1,1) CONSTRAINT PK_GacheesDetails PRIMARY KEY,
    DateHeure DATETIME NOT NULL DEFAULT GETDATE(), NumeroBL NVARCHAR(30) NULL, NumeroGachee INT NULL,
    MatriculeGachee BIGINT NULL, CompteurCPU BIGINT NULL,
    ConsTr1 FLOAT NULL, PoseTr1 FLOAT NULL, EcartTr1 FLOAT NULL, ConsTr2 FLOAT NULL, PoseTr2 FLOAT NULL, EcartTr2 FLOAT NULL,
    ConsTr3 FLOAT NULL, PoseTr3 FLOAT NULL, EcartTr3 FLOAT NULL, ConsTr4 FLOAT NULL, PoseTr4 FLOAT NULL, EcartTr4 FLOAT NULL,
    ConsS1 FLOAT NULL, PoseS1 FLOAT NULL, ConsS2 FLOAT NULL, PoseS2 FLOAT NULL, EcartCIM FLOAT NULL,
    ConsEAU FLOAT NULL, PoseEAU FLOAT NULL, EcartEAU FLOAT NULL,
    ConsADJ1 FLOAT NULL, PoseADJ1 FLOAT NULL, EcartADJ1 FLOAT NULL, ConsADJ2 FLOAT NULL, PoseADJ2 FLOAT NULL, EcartADJ2 FLOAT NULL,
    ConsADJ3 FLOAT NULL, PoseADJ3 FLOAT NULL, EcartADJ3 FLOAT NULL, ConsADJ4 FLOAT NULL, PoseADJ4 FLOAT NULL, EcartADJ4 FLOAT NULL,
    TotalConsTr FLOAT NULL, TotalPoseTr FLOAT NULL, TotalEcartTr FLOAT NULL, Operateur NVARCHAR(100) NULL, AjoutEau INT NULL
);
GO
IF OBJECT_ID('dbo.Production','U') IS NULL
CREATE TABLE dbo.Production (
    IdProduction INT IDENTITY(1,1) CONSTRAINT PK_Production PRIMARY KEY,
    DateHeure DATETIME NOT NULL DEFAULT GETDATE(), Operateur NVARCHAR(100) NULL, NumeroBL NVARCHAR(30) NULL, NombreGachees INT NULL,
    ConsTr1 FLOAT NULL, PoseTr1 FLOAT NULL, EcartTr1 FLOAT NULL, ConsTr2 FLOAT NULL, PoseTr2 FLOAT NULL, EcartTr2 FLOAT NULL,
    ConsTr3 FLOAT NULL, PoseTr3 FLOAT NULL, EcartTr3 FLOAT NULL, ConsTr4 FLOAT NULL, PoseTr4 FLOAT NULL, EcartTr4 FLOAT NULL,
    ConsS1 FLOAT NULL, PoseS1 FLOAT NULL, EcartS1 FLOAT NULL, ConsS2 FLOAT NULL, PoseS2 FLOAT NULL, EcartS2 FLOAT NULL,
    ConsEAU FLOAT NULL, PoseEAU FLOAT NULL, EcartEAU FLOAT NULL,
    ConsADJ1 FLOAT NULL, PoseADJ1 FLOAT NULL, EcartADJ1 FLOAT NULL, ConsADJ2 FLOAT NULL, PoseADJ2 FLOAT NULL, EcartADJ2 FLOAT NULL,
    ConsADJ3 FLOAT NULL, PoseADJ3 FLOAT NULL, EcartADJ3 FLOAT NULL,
    TotalGranCons FLOAT NULL, TotalGranPose FLOAT NULL, TotalGranEcart FLOAT NULL, TotalCamionCons FLOAT NULL, TotalCamionPose FLOAT NULL, TempsMalaxage FLOAT NULL
);
GO
IF OBJECT_ID('dbo.BonsPompage','U') IS NULL
CREATE TABLE dbo.BonsPompage (
    IdBP INT IDENTITY(1,1) CONSTRAINT PK_BonsPompage PRIMARY KEY,
    NumeroBP NVARCHAR(30) NOT NULL, DateHeure DATETIME NULL, IdClient INT NULL, NomClient NVARCHAR(150) NULL, IdChantier INT NULL, NomChantier NVARCHAR(150) NULL,
    CodePompe NVARCHAR(50) NULL, NomPompe NVARCHAR(150) NULL, IdChauffeurPompe INT NULL, NomChauffeurPompe NVARCHAR(150) NULL, VolumeM3 FLOAT NULL,
    [HeureDébut] NVARCHAR(10) NULL, HeureFin NVARCHAR(10) NULL, DureePompageMin FLOAT NULL,
    PUHT FLOAT NULL, MontantHT FLOAT NULL, TauxTVA FLOAT NULL, MontantTTC FLOAT NULL,
    Statut NVARCHAR(30) NULL, Remarques NVARCHAR(500) NULL, Operateur NVARCHAR(100) NULL, DateCreation DATETIME NULL
);
GO
IF OBJECT_ID('dbo.Factures','U') IS NULL
CREATE TABLE dbo.Factures (
    IdFacture INT IDENTITY(1,1) CONSTRAINT PK_Factures PRIMARY KEY,
    NumeroFacture NVARCHAR(30) NOT NULL, DateFacture DATETIME NULL, DateEcheance DATETIME NULL, NomClient NVARCHAR(150) NULL, ICE NVARCHAR(50) NULL,
    AdresseClient NVARCHAR(250) NULL, MontantHT FLOAT NULL, RemisePercent FLOAT NULL, RemiseMontant FLOAT NULL, MontantHT_Net FLOAT NULL,
    TauxTVA FLOAT NULL, MontantTVA FLOAT NULL, MontantTTC FLOAT NULL, MontantRegle FLOAT NULL, PeriodeDu DATETIME NULL, PeriodeAu DATETIME NULL,
    NombreBL INT NULL, VolumeTotal FLOAT NULL, Statut NVARCHAR(30) NULL, ModeReglement NVARCHAR(50) NULL, Remarques NVARCHAR(500) NULL,
    Operateur NVARCHAR(100) NULL, DateCreation DATETIME NULL,
    MontantRestant AS (ISNULL(MontantTTC,0) - ISNULL(MontantRegle,0))
);
GO
IF OBJECT_ID('dbo.FacturesLignes','U') IS NULL
CREATE TABLE dbo.FacturesLignes (
    IdLigne INT IDENTITY(1,1) CONSTRAINT PK_FacturesLignes PRIMARY KEY,
    IdFacture INT NOT NULL CONSTRAINT FK_FL_Facture REFERENCES dbo.Factures(IdFacture),
    NumeroLigne INT NULL, CodeProduit NVARCHAR(50) NULL, Designation NVARCHAR(250) NULL, Unite NVARCHAR(20) NULL, Quantite FLOAT NULL,
    PUHT FLOAT NULL, RemisePercent FLOAT NULL, MontantHT FLOAT NULL, TauxTVA FLOAT NULL, MontantTTC FLOAT NULL, NumeroBL NVARCHAR(30) NULL
);
GO

/* ───── Alarmes, journal, synoptique, tags ───── */
IF OBJECT_ID('dbo.Alarmes','U') IS NULL
CREATE TABLE dbo.Alarmes (
    Id INT IDENTITY(1,1) CONSTRAINT PK_Alarmes PRIMARY KEY,
    DateHeure DATETIME NOT NULL DEFAULT GETDATE(), DateAlarme DATETIME NULL, CodeAlarme NVARCHAR(100) NULL, Description NVARCHAR(500) NULL,
    Etat NVARCHAR(20) NULL, TypeAlarme NVARCHAR(50) NULL, Operateur NVARCHAR(100) NULL, DateResolution DATETIME NULL
);
GO
IF OBJECT_ID('dbo.AlarmesConfig','U') IS NULL
CREATE TABLE dbo.AlarmesConfig (
    IdAlarmeConfig INT IDENTITY(1,1) CONSTRAINT PK_AlarmesConfig PRIMARY KEY,
    Numero INT NULL, Texte NVARCHAR(250) NULL, Classe NVARCHAR(20) NULL, VariableDeclenchement NVARCHAR(100) NULL,
    NumeroBit INT NULL, AdresseDeclenchement NVARCHAR(100) NULL
);
GO
IF OBJECT_ID('dbo.JournalActions','U') IS NULL
CREATE TABLE dbo.JournalActions (
    Id INT IDENTITY(1,1) CONSTRAINT PK_JournalActions PRIMARY KEY,
    DateHeure DATETIME NOT NULL DEFAULT GETDATE(), Operateur NVARCHAR(100) NULL, Module NVARCHAR(50) NULL, Action NVARCHAR(250) NULL,
    Detail NVARCHAR(1000) NULL, Adresse_PLC NVARCHAR(100) NULL, Valeur_PLC NVARCHAR(50) NULL, TypeAction NVARCHAR(30) NULL
);
GO
IF OBJECT_ID('dbo.Synoptique_Etats','U') IS NULL
CREATE TABLE dbo.Synoptique_Etats (
    SYN_Code NVARCHAR(50) NOT NULL CONSTRAINT PK_Synoptique_Etats PRIMARY KEY, SYN_Etat INT NULL, SYN_DateMAJ DATETIME NULL
);
GO
IF OBJECT_ID('dbo.T_PLC_TAGS','U') IS NULL
CREATE TABLE dbo.T_PLC_TAGS (
    TAG_ID INT IDENTITY(1,1) CONSTRAINT PK_T_PLC_TAGS PRIMARY KEY,
    TAG_NOM NVARCHAR(100) NOT NULL, TAG_VALEUR_REAL FLOAT NULL, TAG_VALEUR_BOOL BIT NULL, TAG_DATEUPDATE DATETIME NULL, TAG_ACTIF BIT NOT NULL DEFAULT 1
);
GO

/* ───── Vues ───── */
IF OBJECT_ID('dbo.V_JournalRecent','V') IS NULL
EXEC('CREATE VIEW dbo.V_JournalRecent AS
      SELECT TOP 50 DateHeure, Operateur, Module, Action, Detail, Adresse_PLC, Valeur_PLC, TypeAction
      FROM dbo.JournalActions ORDER BY DateHeure DESC, Id DESC');
GO
IF OBJECT_ID('dbo.vw_ChargePlanning','V') IS NULL
EXEC('CREATE VIEW dbo.vw_ChargePlanning AS
      SELECT pj.IdPlanning, pj.DatePlanning, pj.HeureFabrication, pj.HeureLivraison,
             cl.NomClient, cl.CodeClient, ch.NomChantier, ch.CodeChantier, f.CodeFormule, f.NomFormule,
             pj.QuantiteTotale, pj.QuantiteLivree, (ISNULL(pj.QuantiteTotale,0) - ISNULL(pj.QuantiteLivree,0)) AS Reste,
             pj.Complement, pj.AConfirmer, pj.CadenceMinutes, pj.VolumeRotationMaxi, pj.StatutPlanning, pj.Remarque,
             pj.IdCamion, ca.CodeCamion, ca.NomCamion, pj.IdChauffeur, chf.NomChauffeur
      FROM dbo.Planning_journalier pj
      LEFT JOIN dbo.Clients cl ON cl.IdClient = pj.IdClient
      LEFT JOIN dbo.Chantier ch ON ch.IdChantier = pj.IdChantier
      LEFT JOIN dbo.Formules f ON f.IdFormule = pj.IdFormule
      LEFT JOIN dbo.Camions ca ON ca.IdCamion = pj.IdCamion
      LEFT JOIN dbo.Chauffeurs chf ON chf.IdChauffeur = pj.IdChauffeur');
GO
