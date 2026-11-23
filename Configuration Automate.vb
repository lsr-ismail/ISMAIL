Imports System.Data
Imports Microsoft.Data.SqlClient

Public Class Configuration_Automate

    ' ═══════════════════════════════════════════════════════════════
    '  CONNEXION
    ' ═══════════════════════════════════════════════════════════════
    Private cn As SqlConnection
    Private Const ChaineConnexion As String =
        "Server=.\SQLEXPRESS;Database=CentraleBeton;Trusted_Connection=True;TrustServerCertificate=True"

    Private Sub OuvrirConnexion()
        If cn Is Nothing Then cn = New SqlConnection(ChaineConnexion)
        If cn.State <> ConnectionState.Open Then cn.Open()
    End Sub

    Private Sub FermerConnexion()
        If cn IsNot Nothing AndAlso cn.State = ConnectionState.Open Then cn.Close()
    End Sub

    ' ═══════════════════════════════════════════════════════════════
    '  VARIABLES CASES
    ' ═══════════════════════════════════════════════════════════════
    Private _listeCases As New List(Of Integer)   ' numéros de cases chargés
    Private _indexCase As Integer = 0             ' index courant dans la liste
    Private _chargementCase As Boolean = False    ' évite les événements en cascade

    ' ═══════════════════════════════════════════════════════════════
    '  FORM LOAD
    ' ═══════════════════════════════════════════════════════════════
    Private Sub Configuration_Automate_Load(sender As Object, e As EventArgs) _
        Handles MyBase.Load
        InitialiserComboBox()
        ChargerConfig()
        ChargerListeCases()
    End Sub

    ' ═══════════════════════════════════════════════════════════════
    '  INITIALISATION COMBOBOX
    ' ═══════════════════════════════════════════════════════════════
    Private Sub InitialiserComboBox()
        ' --- PesageGR ---
        cmbPesageGR.Items.AddRange({"1 Bascule", "2 Bascules", "1 Tapis", "2 Tapis",
                                    "1 Bascule & 1 Tapis", "1 Tapis & 1 Bascule",
                                    "1 Skip peseur", "1 Débit"})
        ' --- RepriseGR ---
        cmbRepriseGR.Items.AddRange({"Aucun", "1 Tapis", "1 Skip", "1 Tapis 1 Skip",
                                     "1 Tapis 1 Trémie tampon",
                                     "1 Skip 1 Trémie tampon", "2 Skips"})
        ' --- PesageL / PesageEAU / PesageADJ ---
        For Each cmb In {cmbPesageL, cmbPesageEAU, cmbPesageADJ}
            cmb.Items.AddRange({"Bascule", "Compteur"})
        Next
        ' --- NbCases ---
        For Each cmb In {cmbNbCasesGR, cmbNbCasesL, cmbNbCasesEAU, cmbNbCasesADJ}
            For i = 1 To 16 : cmb.Items.Add(i) : Next
        Next
        ' --- NbMalaxeurs / NbBolsADJ / NbTrappes ---
        For i = 1 To 4 : cmbNbMalaxeurs.Items.Add(i) : Next
        For i = 1 To 8 : cmbNbBolsADJ.Items.Add(i) : Next
        For Each cmb In {cmbNbTrappes1, cmbNbTrappes2}
            For i = 1 To 4 : cmb.Items.Add(i) : Next
        Next
        ' --- Watt TypeLiaison ---
        For Each cmb In {cmbWatt1TypeLiaison, cmbWatt2TypeLiaison}
            cmb.Items.AddRange({"Linéaire", "Polynôme 2", "Polynôme 3", "Polynôme 4"})
        Next
        ' --- Sondes TypeLiaison ---
        Dim liaisonsItems() As String = {"Linéaire", "Polynôme 2", "Polynôme 3", "Polynôme 4"}
        For Each cmb In {cmbTypeLiaisonS1, cmbTypeLiaisonS2, cmbTypeLiaisonS3,
                         cmbTypeLiaisonS4, cmbTypeLiaisonS5, cmbTypeLiaisonS6,
                         cmbTypeLiaisonS7, cmbTypeLiaisonTH, cmbTypeLiaisonDEN}
            cmb.Items.AddRange(liaisonsItems)
        Next
        ' --- GestionHygro ---
        cmbGestionHygro.Items.AddRange({"Aucune", "Sonde hygro seule",
                                        "Hygro béton", "Hygro + sonde"})
        ' --- TTP GestionPesage ---
        cmbGestionPesage.Items.AddRange({"Bascule", "Compteur"})
        cmbDestMalaxTTP.Items.AddRange({"Malaxeur 1", "Malaxeur 2", "Les deux"})
        ' --- Destination malaxeurs bascules ---
        Dim destMal() As String = {"Malaxeur 1", "Malaxeur 2", "Les deux"}
        For Each cmb In {cmbDestMalax, cmbDestMalaxGR2, cmbDestMalaxL1, cmbDestMalaxL2,
                         cmbDestMalaxE1, cmbDestMalaxE2, cmbDestMalaxAD, cmbDestMalaxAJ}
            cmb.Items.AddRange(destMal)
        Next
        ' --- Types bascules ---
        Dim typesBasc() As String = {"Bascule standard", "Tapis peseur", "Débitmètre"}
        For Each cmb In {cmbTypes, cmbTypesGR2, cmbTypesL1, cmbTypesL2,
                         cmbTypesE1, cmbTypesE2, cmbTypesAD, cmbTypesAJ}
            cmb.Items.AddRange(typesBasc)
        Next
        ' --- Cases : destinations ---
        cmbDestGranulat.Items.AddRange({"Malaxeur 1", "Malaxeur 2", "Les deux"})
        cmbDestEau.Items.AddRange({"Malaxeur 1", "Malaxeur 2", "Les deux"})
        cmbDestAdj.Items.AddRange({"Malaxeur 1", "Malaxeur 2", "Les deux"})
        cmbMateriau.Items.AddRange({"Granulat", "Liant", "Eau", "Adjuvant"})
        cmbTypeMateriau.Items.AddRange({"GR", "L", "E", "ADJ"})
        ' --- Lavage ---
        cmbGoulotteBP.Items.AddRange({"Position 1", "Position 2"})
        cmbDestConvoyeur.Items.AddRange({"Convoyeur 1", "Convoyeur 2"})
    End Sub

    ' ═══════════════════════════════════════════════════════════════
    '  HELPER : Toggle NON ↔ OUI
    ' ═══════════════════════════════════════════════════════════════
    Private Sub ToggleBtn(btn As Button)
        If btn.Text = "NON" Then
            btn.Text = "OUI"
            btn.BackColor = Color.LightGreen
        Else
            btn.Text = "NON"
            btn.BackColor = SystemColors.Control
        End If
    End Sub

    Private Sub SetBtn(btn As Button, valeur As Boolean)
        btn.Text = If(valeur, "OUI", "NON")
        btn.BackColor = If(valeur, Color.LightGreen, SystemColors.Control)
    End Sub

    Private Function GetBtn(btn As Button) As Boolean
        Return btn.Text = "OUI"
    End Function

    ' ═══════════════════════════════════════════════════════════════
    '  HELPER : Toggle PV/GV
    ' ═══════════════════════════════════════════════════════════════
    Private Sub TogglePVGV(btn As Button)
        If btn.Text = "Petite vitesse" Then
            btn.Text = "Grande vitesse"
            btn.BackColor = Color.LightBlue
        Else
            btn.Text = "Petite vitesse"
            btn.BackColor = SystemColors.Control
        End If
    End Sub

    Private Sub SetPVGV(btn As Button, grandeVitesse As Boolean)
        btn.Text = If(grandeVitesse, "Grande vitesse", "Petite vitesse")
        btn.BackColor = If(grandeVitesse, Color.LightBlue, SystemColors.Control)
    End Sub

    Private Function GetPVGV(btn As Button) As Boolean
        Return btn.Text = "Grande vitesse"
    End Function

    ' ═══════════════════════════════════════════════════════════════
    '  CHARGER CONFIGURATION (dbo.Configuration)
    ' ═══════════════════════════════════════════════════════════════
    Private Sub ChargerConfig()
        Try
            OuvrirConnexion()
            Using cmd As New SqlCommand("SELECT * FROM dbo.Configuration WHERE ID=1", cn)
                Using dr As SqlDataReader = cmd.ExecuteReader()
                    If Not dr.Read() Then Exit Sub

                    ' ── COMPTEURS ─────────────────────────────────
                    txtMultiADJ1.Text = dr("MultiADJ1").ToString : txtDivADJ1.Text = dr("DivADJ1").ToString
                    txtMultiADJ2.Text = dr("MultiADJ2").ToString : txtDivADJ2.Text = dr("DivADJ2").ToString
                    txtMultiADJ3.Text = dr("MultiADJ3").ToString : txtDivADJ3.Text = dr("DivADJ3").ToString
                    txtMultiADJ4.Text = dr("MultiADJ4").ToString : txtDivADJ4.Text = dr("DivADJ4").ToString

                    txtMultiEAU1.Text = dr("MultiEAU1").ToString : txtDivEAU1.Text = dr("DivEAU1").ToString
                    txtMultiEAU2.Text = dr("MultiEAU2").ToString : txtDivEAU2.Text = dr("DivEAU2").ToString
                    txtMultiEAU3.Text = dr("MultiEAU3").ToString : txtDivEAU3.Text = dr("DivEAU3").ToString
                    txtMultiEAU4.Text = dr("MultiEAU4").ToString : txtDivEAU4.Text = dr("DivEAU4").ToString

                    txtMultiHYG1.Text = dr("MultiHYG1").ToString : txtDivHYG1.Text = dr("DivHYG1").ToString
                    txtMultiHYG2.Text = dr("MultiHYG2").ToString : txtDivHYG2.Text = dr("DivHYG2").ToString
                    txtMultiHYG3.Text = dr("MultiHYG3").ToString : txtDivHYG3.Text = dr("DivHYG3").ToString
                    txtMultiHYG4.Text = dr("MultiHYG4").ToString : txtDivHYG4.Text = dr("DivHYG4").ToString

                    ' ── MALAXEURS — Général ───────────────────────
                    SetBtn(btnDivisionGachees, CBool(dr("DivisionGachees") <> 1))
                    SetBtn(btnMaxGachees, CBool(dr("MaxGacheesEntieres") > 0))
                    SetBtn(btnAjoutEau, CBool(dr("AjoutEauPostes")))
                    SetBtn(btnGestionPlasticite, CBool(dr("GestionPlasticiteEC")))
                    txtTailleGacheeSec.Text = dr("TailleGacheeSec_m3").ToString
                    txtTailleMin.Text = dr("TailleMin_m3").ToString
                    SetBtn(btnHygroCommun, CBool(dr("HygroCommun2Mal")))
                    SetBtn(btnPresenceHygro, CBool(dr("PresenceHygroBeton")))

                    ' Malaxeur 1
                    txtMalTaille.Text = dr("Mal1Taille_m3").ToString
                    txtMalPuissVide.Text = dr("Mal1PuissVide_pct").ToString
                    SetBtn(btnMalArretAuto, CBool(dr("Mal1ArretAuto")))
                    SetBtn(btnMalMarcheImp, CBool(dr("Mal1MarcheImp")))
                    txtMalTempsVidange.Text = dr("Mal1TempsVidange_s").ToString
                    txtMalTempsKlaxon.Text = dr("Mal1TempsKlaxon_s").ToString

                    ' Durées Malaxeur 1
                    txtDurImp1_0.Text = dr("Mal1DurImp1_0").ToString : txtDurImp1_1.Text = dr("Mal1DurImp1_1").ToString
                    txtDurImp1_2.Text = dr("Mal1DurImp1_2").ToString : txtDurImp2_0.Text = dr("Mal1DurImp2_0").ToString
                    txtDurImp2_1.Text = dr("Mal1DurImp2_1").ToString : txtDurImp2_2.Text = dr("Mal1DurImp2_2").ToString
                    txtDurMort1_0.Text = dr("Mal1DurMort1_0").ToString : txtDurMort1_1.Text = dr("Mal1DurMort1_1").ToString
                    txtDurMort1_2.Text = dr("Mal1DurMort1_2").ToString : txtDurMort2_0.Text = dr("Mal1DurMort2_0").ToString
                    txtDurMort2_1.Text = dr("Mal1DurMort2_1").ToString : txtDurMort2_2.Text = dr("Mal1DurMort2_2").ToString
                    txtDurPlein_0.Text = dr("Mal1DurPlein_0").ToString : txtDurPlein_1.Text = dr("Mal1DurPlein_1").ToString
                    txtDurPlein_2.Text = dr("Mal1DurPlein_2").ToString
                    txtDurDern_0.Text = dr("Mal1DurDern_0").ToString
                    txtDurDern_1.Text = dr("Mal1DurDern_1").ToString
                    txtDurDern_2.Text = dr("Mal1DurDern_2").ToString
                    txtDurImp1_A.Text = dr("Mal1DurImp1_3").ToString
                    txtDurImp2_A.Text = dr("Mal1DurImp2_3").ToString
                    txtDurImp1_L.Text = dr("Mal1DurImp1_L").ToString
                    txtDurImp2_L.Text = dr("Mal1DurImp2_L").ToString
                    txtDurImp3_A.Text = dr("Mal1DurImp1_4").ToString
                    txtDurImp3_L.Text = dr("Mal1DurImp2_4").ToString

                    ' Watt1
                    txtWatt1Proportion.Text = dr("Watt1Proportion").ToString
                    cmbWatt1TypeLiaison.SelectedIndex = CInt(dr("Watt1TypeLiaison"))
                    txtWatt1D0.Text = dr("Watt1D0").ToString : txtWatt1K0.Text = dr("Watt1K0").ToString
                    txtWatt1D1.Text = dr("Watt1D1").ToString : txtWatt1K1.Text = dr("Watt1K1").ToString
                    txtWatt1D2.Text = dr("Watt1D2").ToString : txtWatt1K2.Text = dr("Watt1K2").ToString
                    txtWatt1D3.Text = dr("Watt1D3").ToString : txtWatt1K3.Text = dr("Watt1K3").ToString
                    txtWatt1D4.Text = dr("Watt1D4").ToString : txtWatt1K4.Text = dr("Watt1K4").ToString

                    ' Watt2
                    txtWatt2Proportion.Text = dr("Watt2Proportion").ToString
                    cmbWatt2TypeLiaison.SelectedIndex = CInt(dr("Watt2TypeLiaison"))
                    txtWatt2D0.Text = dr("Watt2D0").ToString : txtWatt2K0.Text = dr("Watt2K0").ToString
                    txtWatt2D1.Text = dr("Watt2D1").ToString : txtWatt2K1.Text = dr("Watt2K1").ToString
                    txtWatt2D2.Text = dr("Watt2D2").ToString : txtWatt2K2.Text = dr("Watt2K2").ToString
                    txtWatt2D3.Text = dr("Watt2D3").ToString : txtWatt2K3.Text = dr("Watt2K3").ToString
                    txtWatt2D4.Text = dr("Watt2D4").ToString : txtWatt2K4.Text = dr("Watt2K4").ToString

                    ' Divers malaxeur
                    SetBtn(btnGestionFiltrage, CBool(dr("GestionFiltrage")))
                    SetBtn(btnTrappeAnalogique, CBool(dr("TrappeAnalogique")))
                    SetBtn(btnGestionGraissage, CBool(dr("GestionGraissage")))

                    ' Plasticité
                    cmbGestionHygro.SelectedIndex = CInt(dr("GestionHygro"))
                    txtTempsCyclePlas.Text = dr("TempsCyclePlas_s").ToString
                    txtTailleMinGachee.Text = dr("TailleMinGachee_m3").ToString
                    txtTempsEntre2Lectures.Text = dr("TempsEntre2Lect_s").ToString
                    txtStabilitePlas.Text = dr("StabilitePlas").ToString
                    txtTolerancePlas.Text = dr("TolerancePlas").ToString
                    txtDosageMaxAjout.Text = dr("DosageMaxAjout_l").ToString
                    txtTempsEntreAjouts.Text = dr("TempsEntreAjouts_s").ToString

                    ' ── SONDES ────────────────────────────────────
                    ChargerSonde(dr, "S1", cmbTypeLiaisonS1, txtProportionS1, txtNumCaseS1,
                                 txtCorrMinS1, txtCorrMaxS1, btnCorrAutoS1, btnCorrDosageS1,
                                 txtSondeD0S1, txtSondeK0S1, txtSondeD1S1, txtSondeK1S1,
                                 txtSondeD2S1, txtSondeK2S1, txtSondeD3S1, txtSondeK3S1,
                                 txtSondeD4S1, txtSondeK4S1)
                    ChargerSonde(dr, "S2", cmbTypeLiaisonS2, txtProportionS2, txtNumCaseS2,
                                 txtCorrMinS2, txtCorrMaxS2, btnCorrAutoS2, btnCorrDosageS2,
                                 txtSondeD0S2, txtSondeK0S2, txtSondeD1S2, txtSondeK1S2,
                                 txtSondeD2S2, txtSondeK2S2, txtSondeD3S2, txtSondeK3S2,
                                 txtSondeD4S2, txtSondeK4S2)
                    ChargerSonde(dr, "S3", cmbTypeLiaisonS3, txtProportionS3, txtNumCaseS3,
                                 txtCorrMinS3, txtCorrMaxS3, btnCorrAutoS3, btnCorrDosageS3,
                                 txtSondeD0S3, txtSondeK0S3, txtSondeD1S3, txtSondeK1S3,
                                 txtSondeD2S3, txtSondeK2S3, txtSondeD3S3, txtSondeK3S3,
                                 txtSondeD4S3, txtSondeK4S3)
                    ChargerSonde(dr, "S4", cmbTypeLiaisonS4, txtProportionS4, txtNumCaseS4,
                                 txtCorrMinS4, txtCorrMaxS4, btnCorrAutoS4, btnCorrDosageS4,
                                 txtSondeD0S4, txtSondeK0S4, txtSondeD1S4, txtSondeK1S4,
                                 txtSondeD2S4, txtSondeK2S4, txtSondeD3S4, txtSondeK3S4,
                                 txtSondeD4S4, txtSondeK4S4)
                    ChargerSonde(dr, "S5", cmbTypeLiaisonS5, txtProportionS5, txtNumCaseS5,
                                 txtCorrMinS5, txtCorrMaxS5, btnCorrAutoS5, btnCorrDosageS5,
                                 txtSondeD0S5, txtSondeK0S5, txtSondeD1S5, txtSondeK1S5,
                                 txtSondeD2S5, txtSondeK2S5, txtSondeD3S5, txtSondeK3S5,
                                 txtSondeD4S5, txtSondeK4S5)
                    ChargerSonde(dr, "S6", cmbTypeLiaisonS6, txtProportionS6, txtNumCaseS6,
                                 txtCorrMinS6, txtCorrMaxS6, btnCorrAutoS6, btnCorrDosageS6,
                                 txtSondeD0S6, txtSondeK0S6, txtSondeD1S6, txtSondeK1S6,
                                 txtSondeD2S6, txtSondeK2S6, txtSondeD3S6, txtSondeK3S6,
                                 txtSondeD4S6, txtSondeK4S6)
                    ChargerSonde(dr, "S7", cmbTypeLiaisonS7, txtProportionS7, txtNumCaseS7,
                                 txtCorrMinS7, txtCorrMaxS7, btnCorrAutoS7, btnCorrDosageS7,
                                 txtSondeD0S7, txtSondeK0S7, txtSondeD1S7, txtSondeK1S7,
                                 txtSondeD2S7, txtSondeK2S7, txtSondeD3S7, txtSondeK3S7,
                                 txtSondeD4S7, txtSondeK4S7)
                    ChargerSonde(dr, "TH", cmbTypeLiaisonTH, txtProportionTH, txtNumCaseTH,
                                 txtCorrMinTH, txtCorrMaxTH, btnCorrAutoTH, btnCorrDosageTH,
                                 txtSondeD0TH, txtSondeK0TH, txtSondeD1TH, txtSondeK1TH,
                                 txtSondeD2TH, txtSondeK2TH, txtSondeD3TH, txtSondeK3TH,
                                 txtSondeD4TH, txtSondeK4TH)
                    ChargerSonde(dr, "DEN", cmbTypeLiaisonDEN, txtProportionDEN, txtNumCaseDEN,
                                 txtCorrMinDEN, txtCorrMaxDEN, btnCorrAutoDEN, btnCorrDosageDEN,
                                 txtSondeD0DEN, txtSondeK0DEN, txtSondeD1DEN, txtSondeK1DEN,
                                 txtSondeD2DEN, txtSondeK2DEN, txtSondeD3DEN, txtSondeK3DEN,
                                 txtSondeD4DEN, txtSondeK4DEN)

                    ' ── REPRISE GRANULATS ─────────────────────────
                    SetBtn(btnTapisReprise, CBool(dr("TapisReprise")))
                    SetBtn(btnArretTapis, CBool(dr("ArretTapis")))
                    SetBtn(btnTapisPeseur, CBool(dr("TapisPeseur")))
                    SetBtn(btnDoubleTapis, CBool(dr("DoubleTapis")))
                    SetBtn(btnVidangeTapis, CBool(dr("VidangeTapis")))
                    txtTempsMontTapis.Text = dr("TempsMontTapis_s").ToString
                    txtTempsMontSec.Text = dr("TempsMontSec_s").ToString
                    txtTempsMontTapis2.Text = dr("TempsMontTapis2_s").ToString
                    txtDecalageVid.Text = dr("DecalageVid_s").ToString
                    ' TTP
                    cmbDestMalaxTTP.SelectedIndex = CInt(dr("TTP_DestMalax"))
                    cmbGestionPesage.SelectedIndex = CInt(dr("TTP_GestionPesage"))
                    txtRetardLiant.Text = dr("TTP_RetardLiant_s").ToString
                    txtTempsVidTTP.Text = dr("TTP_TempsVid_s").ToString
                    txtActivVibreur.Text = dr("TTP_ActivVibreur_s").ToString
                    txtDureeImpTTP.Text = dr("TTP_DureeImp_s").ToString
                    txtTempsMortTTP.Text = dr("TTP_TempsMort_s").ToString
                    SetBtn(btnDoubleCmdTTP, CBool(dr("TTP_DoubleCmd")))
                    ' Skip
                    txtRetardLiantSkip.Text = dr("Skip_RetardLiant_s").ToString
                    txtTempsMontAtt.Text = dr("Skip_TempsMontAtt_s").ToString
                    txtTempsMontSkip.Text = dr("Skip_TempsMont_s").ToString
                    txtTempsDescSkip.Text = dr("Skip_TempsDesc_s").ToString
                    txtTempsVidSkip.Text = dr("Skip_TempsVid_s").ToString
                    txtTempsVidSecSkip.Text = dr("Skip_TempsVidSec_s").ToString
                    txtRetardTrappe.Text = dr("Skip_RetardTrappe_s").ToString
                    txtRetardPuiss.Text = dr("Skip_RetardPuiss_s").ToString
                    txtRetardSec.Text = dr("Skip_RetardSec_s").ToString
                    SetBtn(btnMonteeImmed, CBool(dr("Skip_MonteeImmed")))

                    ' ── CENTRALE ──────────────────────────────────
                    cmbPesageGR.SelectedIndex = CInt(dr("PesageGR"))
                    SetBtn(btnPesageGRMal, CBool(dr("PesageGRMal")))
                    cmbRepriseGR.SelectedIndex = CInt(dr("RepriseGR"))
                    cmbNbCasesGR.SelectedIndex = CInt(dr("NbCasesGR")) - 1
                    cmbPesageADJ.SelectedIndex = CInt(dr("PesageADJ"))
                    cmbNbBolsADJ.SelectedIndex = CInt(dr("NbBolsADJ")) - 1
                    txtTempsVidBol.Text = dr("TempsVidBol_s").ToString
                    cmbNbCasesADJ.SelectedIndex = CInt(dr("NbCasesADJ")) - 1
                    cmbPesageL.SelectedIndex = CInt(dr("PesageL"))
                    cmbNbCasesL.SelectedIndex = CInt(dr("NbCasesL")) - 1
                    cmbPesageEAU.SelectedIndex = CInt(dr("PesageEAU"))
                    cmbNbCasesEAU.SelectedIndex = CInt(dr("NbCasesEAU")) - 1
                    cmbNbMalaxeurs.SelectedIndex = CInt(dr("NbMalaxeurs")) - 1
                    SetBtn(btnVidSimult, CBool(dr("VidSimult")))
                    cmbNbTrappes1.SelectedIndex = CInt(dr("NbTrappes1")) - 1
                    cmbNbTrappes2.SelectedIndex = CInt(dr("NbTrappes2"))
                    SetBtn(btnByPass1, CBool(dr("ByPass1")))
                    SetBtn(btnByPass2, CBool(dr("ByPass2")))
                    SetBtn(btnGoulotte1, CBool(dr("Goulotte1")))
                    SetPVGV(btnPVGVGR, CBool(dr("PVGVGR")))
                    SetPVGV(btnPVGVL, CBool(dr("PVGVL")))
                    SetPVGV(btnPVGVEAU, CBool(dr("PVGVEAU")))
                    SetPVGV(btnPVGVADJ, CBool(dr("PVGVADJ")))
                    SetPVGV(btnPVGVGR2, CBool(dr("PVGVGR2")))
                    SetPVGV(btnPVGVL2, CBool(dr("PVGVL2")))
                    SetPVGV(btnPVGVEAU2, CBool(dr("PVGVEAU2")))
                    SetPVGV(btnPVGVCptEAU, CBool(dr("PVGVCptEAU")))
                    SetPVGV(btnPVGVAdjEAU, CBool(dr("PVGVAdjEAU")))
                    SetPVGV(btnPVGVAdjMAL, CBool(dr("PVGVAdjMAL")))

                    ' ── PARAM VIDANGE ─────────────────────────────
                    txtRetardGR.Text = dr("RetardGR_s").ToString
                    txtRetardL.Text = dr("RetardL_s").ToString
                    txtRetardEAU.Text = dr("RetardEAU_s").ToString
                    txtRetardADJ.Text = dr("RetardADJ_s").ToString
                    txtRetardAJT.Text = dr("RetardAJT_s").ToString
                    SetBtn(btnSynchroAdjMLX1, CBool(dr("SynchroAdjMLX1")))
                    SetBtn(btnSynchroAdjMLX2, CBool(dr("SynchroAdjMLX2")))
                    SetBtn(btnSynchroAjtMLX1, CBool(dr("SynchroAjtMLX1")))
                    SetBtn(btnSynchroAjtMLX2, CBool(dr("SynchroAjtMLX2")))
                    SetBtn(btnPompeLavage, CBool(dr("PompeLavage")))
                    SetBtn(btnPompeLastGach, CBool(dr("PompeLastGach")))
                    SetBtn(btnRincageADJ, CBool(dr("RincageADJ")))
                    SetBtn(btnGestionAjouts, CBool(dr("GestionAjouts")))
                    SetBtn(btnBypassVidange, CBool(dr("BypassVidange")))
                    SetBtn(btnBypassDosage, CBool(dr("BypassDosage")))

                    ' ── GACHÉE SÈCHE ──────────────────────────────
                    SetBtn(btnSansRepriseGR, CBool(dr("SansRepriseGR")))
                    SetBtn(btnSkipInexist, CBool(dr("SkipInexist")))
                    SetBtn(btnRotInversee, CBool(dr("RotInversee")))
                    SetBtn(btnDblCmdBypass, CBool(dr("DblCmdBypass")))
                    SetBtn(btnPresenceGS, CBool(dr("PresenceGS")))
                    SetBtn(btnLiantMelangeur, CBool(dr("LiantMelangeur")))
                    SetBtn(btnArretMelangeur, CBool(dr("ArretMelangeur")))
                    txtRetardOuvert.Text = dr("RetardOuvert_s").ToString
                    txtRetardFermet.Text = dr("RetardFermet_s").ToString
                    SetBtn(btnPassGR1, CBool(dr("PassGR1"))) : txtChargeGR1.Text = dr("ChargeGR1").ToString
                    SetBtn(btnPassGR2, CBool(dr("PassGR2"))) : txtChargeGR2.Text = dr("ChargeGR2").ToString
                    SetBtn(btnPassL1, CBool(dr("PassL1"))) : txtChargeL1.Text = dr("ChargeL1").ToString
                    SetBtn(btnPassL2, CBool(dr("PassL2"))) : txtChargeL2.Text = dr("ChargeL2").ToString
                    SetBtn(btnPassE1, CBool(dr("PassE1"))) : txtChargeE1.Text = dr("ChargeE1").ToString
                    SetBtn(btnPassE2, CBool(dr("PassE2"))) : txtChargeE2.Text = dr("ChargeE2").ToString
                    SetBtn(btnPassADJ1, CBool(dr("PassADJ1"))) : txtChargeADJ1.Text = dr("ChargeADJ1").ToString
                    SetBtn(btnDosageType, CBool(dr("DosageType")))
                    txtOrdreGR.Text = dr("OrdreGR").ToString
                    txtOrdreL.Text = dr("OrdreL").ToString
                    txtOrdreEAU.Text = dr("OrdreEAU").ToString
                    txtOrdreADJ.Text = dr("OrdreADJ").ToString
                    txtOrdreAJT.Text = dr("OrdreAJT").ToString
                    SetBtn(btnDeuxPoints, CBool(dr("DeuxPoints")))
                    SetBtn(btnDepesageParal, CBool(dr("DepesageParal")))
                    SetBtn(btnPreMelange, CBool(dr("PreMelange")))

                    ' ── PARAM LAVAGE ──────────────────────────────
                    txtConsigneEauRM.Text = dr("ConsigneEau_l").ToString
                    txtTempsMalaxRM.Text = dr("TempsMalax_s").ToString
                    SetBtn(btnLavageTrappe, CBool(dr("LavageTrappe")))
                    cmbGoulotteBP.SelectedIndex = CInt(dr("GoulotteBP"))
                    cmbDestConvoyeur.SelectedIndex = CInt(dr("DestConvoyeur"))
                    txtNumCaseEauRM.Text = dr("NumCaseEauRM").ToString
                    txtNbCyclesRM.Text = dr("NbCycles").ToString
                    txtNbRemplM1.Text = dr("NbRemplM1").ToString
                    txtNbRemplM2.Text = dr("NbRemplM2").ToString
                    SetBtn(btnLavageAuto, CBool(dr("LavageAuto")))
                    ' TODO: txtNbGacheesLavage à ajouter dans le Designer
                    ' txtNbGacheesLavage.Visible = CBool(dr("LavageAuto"))
                    SetBtn(btnLavageExterne, CBool(dr("LavageExterne")))

                    ' ── BASCULES ─────────────────────────────────
                    ChargerBascules(dr)

                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Erreur chargement configuration : " & ex.Message,
                            "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            FermerConnexion()
        End Try
    End Sub

    ' ── Helper : charger une sonde ────────────────────────────────
    Private Sub ChargerSonde(dr As SqlDataReader, prefix As String,
                             cmb As ComboBox, txtProp As TextBox, txtCase As TextBox,
                             txtMin As TextBox, txtMax As TextBox,
                             btnAuto As Button, btnDosage As Button,
                             txtD0 As TextBox, txtK0 As TextBox,
                             txtD1 As TextBox, txtK1 As TextBox,
                             txtD2 As TextBox, txtK2 As TextBox,
                             txtD3 As TextBox, txtK3 As TextBox,
                             txtD4 As TextBox, txtK4 As TextBox)
        cmb.SelectedIndex = CInt(dr(prefix & "TypeLiaison"))
        txtProp.Text = dr(prefix & "Proportion").ToString
        txtCase.Text = dr(prefix & "NumCase").ToString
        txtMin.Text = dr(prefix & "CorrMin").ToString
        txtMax.Text = dr(prefix & "CorrMax").ToString
        SetBtn(btnAuto, CBool(dr(prefix & "CorrAuto")))
        SetBtn(btnDosage, CBool(dr(prefix & "CorrDosage")))
        txtD0.Text = dr(prefix & "D0").ToString : txtK0.Text = dr(prefix & "K0").ToString
        txtD1.Text = dr(prefix & "D1").ToString : txtK1.Text = dr(prefix & "K1").ToString
        txtD2.Text = dr(prefix & "D2").ToString : txtK2.Text = dr(prefix & "K2").ToString
        txtD3.Text = dr(prefix & "D3").ToString : txtK3.Text = dr(prefix & "K3").ToString
        txtD4.Text = dr(prefix & "D4").ToString : txtK4.Text = dr(prefix & "K4").ToString
    End Sub

    ' ═══════════════════════════════════════════════════════════════
    '  SAUVEGARDER CONFIGURATION
    ' ═══════════════════════════════════════════════════════════════
    Private Sub SauvegarderConfig()
        Try
            OuvrirConnexion()

            Dim sql As String = "UPDATE dbo.Configuration SET " &
                " MultiADJ1=@MultiADJ1, DivADJ1=@DivADJ1," &
                " MultiADJ2=@MultiADJ2, DivADJ2=@DivADJ2," &
                " MultiADJ3=@MultiADJ3, DivADJ3=@DivADJ3," &
                " MultiADJ4=@MultiADJ4, DivADJ4=@DivADJ4," &
                " MultiEAU1=@MultiEAU1, DivEAU1=@DivEAU1," &
                " MultiEAU2=@MultiEAU2, DivEAU2=@DivEAU2," &
                " MultiEAU3=@MultiEAU3, DivEAU3=@DivEAU3," &
                " MultiEAU4=@MultiEAU4, DivEAU4=@DivEAU4," &
                " MultiHYG1=@MultiHYG1, DivHYG1=@DivHYG1," &
                " MultiHYG2=@MultiHYG2, DivHYG2=@DivHYG2," &
                " MultiHYG3=@MultiHYG3, DivHYG3=@DivHYG3," &
                " MultiHYG4=@MultiHYG4, DivHYG4=@DivHYG4," &
                " DivisionGachees=@DivisionGachees, MaxGacheesEntieres=@MaxGacheesEntieres," &
                " AjoutEauPostes=@AjoutEauPostes, GestionPlasticiteEC=@GestionPlasticiteEC," &
                " TailleGacheeSec_m3=@TailleGacheeSec_m3, TailleMin_m3=@TailleMin_m3," &
                " HygroCommun2Mal=@HygroCommun2Mal, PresenceHygroBeton=@PresenceHygroBeton," &
                " Mal1Taille_m3=@Mal1Taille, Mal1PuissVide_pct=@Mal1Puiss," &
                " Mal1ArretAuto=@Mal1ArretAuto, Mal1MarcheImp=@Mal1MarcheImp," &
                " Mal1TempsVidange_s=@Mal1TempsVid, Mal1TempsKlaxon_s=@Mal1Klaxon," &
                " Mal1DurImp1_0=@Dur1_0, Mal1DurImp1_1=@Dur1_1, Mal1DurImp1_2=@Dur1_2," &
                " Mal1DurImp2_0=@Dur2_0, Mal1DurImp2_1=@Dur2_1, Mal1DurImp2_2=@Dur2_2," &
                " Mal1DurMort1_0=@DurM1_0, Mal1DurMort1_1=@DurM1_1, Mal1DurMort1_2=@DurM1_2," &
                " Mal1DurMort2_0=@DurM2_0, Mal1DurMort2_1=@DurM2_1, Mal1DurMort2_2=@DurM2_2," &
                " Mal1DurPlein_0=@DurP_0, Mal1DurPlein_1=@DurP_1, Mal1DurPlein_2=@DurP_2," &
                " Mal1DurDern_0=@DurD_0, Mal1DurDern_1=@DurD_1, Mal1DurDern_2=@DurD_2," &
                " Watt1Proportion=@W1Prop, Watt1TypeLiaison=@W1Type," &
                " Watt1D0=@W1D0, Watt1K0=@W1K0, Watt1D1=@W1D1, Watt1K1=@W1K1," &
                " Watt1D2=@W1D2, Watt1K2=@W1K2, Watt1D3=@W1D3, Watt1K3=@W1K3," &
                " Watt1D4=@W1D4, Watt1K4=@W1K4," &
                " Watt2Proportion=@W2Prop, Watt2TypeLiaison=@W2Type," &
                " Watt2D0=@W2D0, Watt2K0=@W2K0, Watt2D1=@W2D1, Watt2K1=@W2K1," &
                " Watt2D2=@W2D2, Watt2K2=@W2K2, Watt2D3=@W2D3, Watt2K3=@W2K3," &
                " Watt2D4=@W2D4, Watt2K4=@W2K4," &
                " GestionFiltrage=@GestionFiltrage, TrappeAnalogique=@TrappeAna," &
                " GestionGraissage=@GestionGraissage, GestionHygro=@GestionHygro," &
                " TempsCyclePlas_s=@TempsCycle, TailleMinGachee_m3=@TailleMinG," &
                " TempsEntre2Lect_s=@TpsEntre2, StabilitePlas=@Stabilite," &
                " TolerancePlas=@Tolerance, DosageMaxAjout_l=@DosageMax," &
                " TempsEntreAjouts_s=@TpsAjouts," &
                " S1TypeLiaison=@S1TL, S1Proportion=@S1Prop, S1NumCase=@S1NC," &
                " S1CorrMin=@S1CMin, S1CorrMax=@S1CMax, S1CorrAuto=@S1CA, S1CorrDosage=@S1CD," &
                " S1D0=@S1D0,S1K0=@S1K0,S1D1=@S1D1,S1K1=@S1K1,S1D2=@S1D2,S1K2=@S1K2," &
                " S1D3=@S1D3,S1K3=@S1K3,S1D4=@S1D4,S1K4=@S1K4," &
                " S2TypeLiaison=@S2TL, S2Proportion=@S2Prop, S2NumCase=@S2NC," &
                " S2CorrMin=@S2CMin, S2CorrMax=@S2CMax, S2CorrAuto=@S2CA, S2CorrDosage=@S2CD," &
                " S2D0=@S2D0,S2K0=@S2K0,S2D1=@S2D1,S2K1=@S2K1,S2D2=@S2D2,S2K2=@S2K2," &
                " S2D3=@S2D3,S2K3=@S2K3,S2D4=@S2D4,S2K4=@S2K4," &
                " S3TypeLiaison=@S3TL, S3Proportion=@S3Prop, S3NumCase=@S3NC," &
                " S3CorrMin=@S3CMin, S3CorrMax=@S3CMax, S3CorrAuto=@S3CA, S3CorrDosage=@S3CD," &
                " S3D0=@S3D0,S3K0=@S3K0,S3D1=@S3D1,S3K1=@S3K1,S3D2=@S3D2,S3K2=@S3K2," &
                " S3D3=@S3D3,S3K3=@S3K3,S3D4=@S3D4,S3K4=@S3K4," &
                " THTypeLiaison=@THTL, THProportion=@THProp, THNumCase=@THNC," &
                " THCorrMin=@THCMin, THCorrMax=@THCMax, THCorrAuto=@THCA, THCorrDosage=@THCD," &
                " THD0=@THD0,THK0=@THK0,THD1=@THD1,THK1=@THK1,THD2=@THD2,THK2=@THK2," &
                " THD3=@THD3,THK3=@THK3,THD4=@THD4,THK4=@THK4," &
                " DENTypeLiaison=@DENTL, DENProportion=@DENProp, DENNumCase=@DENC," &
                " DENCorrMin=@DENCMin, DENCorrMax=@DENCMax, DENCorrAuto=@DENCA, DENCorrDosage=@DENCD," &
                " DEND0=@DEND0,DENK0=@DENK0,DEND1=@DEND1,DENK1=@DENK1,DEND2=@DEND2,DENK2=@DENK2," &
                " DEND3=@DEND3,DENK3=@DENK3,DEND4=@DEND4,DENK4=@DENK4," &
                " TapisReprise=@TapisReprise, ArretTapis=@ArretTapis," &
                " TapisPeseur=@TapisPeseur, DoubleTapis=@DoubleTapis, VidangeTapis=@VidangeTapis," &
                " TempsMontTapis_s=@TpsMontTapis, TempsMontSec_s=@TpsMontSec," &
                " TempsMontTapis2_s=@TpsMontTapis2, DecalageVid_s=@DecalageVid," &
                " TTP_DestMalax=@TTPDest, TTP_GestionPesage=@TTPPesage," &
                " TTP_RetardLiant_s=@TTPRetard, TTP_TempsVid_s=@TTPVid," &
                " TTP_ActivVibreur_s=@TTPVib, TTP_DureeImp_s=@TTPDuree," &
                " TTP_TempsMort_s=@TTPMort, TTP_DoubleCmd=@TTPDbl," &
                " Skip_RetardLiant_s=@SkipRetard, Skip_TempsMontAtt_s=@SkipAtt," &
                " Skip_TempsMont_s=@SkipMont, Skip_TempsDesc_s=@SkipDesc," &
                " Skip_TempsVid_s=@SkipVid, Skip_TempsVidSec_s=@SkipVidSec," &
                " Skip_RetardTrappe_s=@SkipTrappe, Skip_RetardPuiss_s=@SkipPuiss," &
                " Skip_RetardSec_s=@SkipSec, Skip_MonteeImmed=@SkipImmed," &
                " PesageGR=@PesageGR, PesageGRMal=@PesageGRMal, RepriseGR=@RepriseGR," &
                " NbCasesGR=@NbCasesGR, PesageADJ=@PesageADJ, NbBolsADJ=@NbBolsADJ," &
                " TempsVidBol_s=@TempsVidBol, NbCasesADJ=@NbCasesADJ," &
                " PesageL=@PesageL, NbCasesL=@NbCasesL," &
                " PesageEAU=@PesageEAU, NbCasesEAU=@NbCasesEAU," &
                " NbMalaxeurs=@NbMalaxeurs, VidSimult=@VidSimult," &
                " NbTrappes1=@NbTrappes1, NbTrappes2=@NbTrappes2," &
                " ByPass1=@ByPass1, ByPass2=@ByPass2, Goulotte1=@Goulotte1," &
                " PVGVGR=@PVGVGR, PVGVL=@PVGVL, PVGVEAU=@PVGVEAU, PVGVADJ=@PVGVADJ," &
                " PVGVGR2=@PVGVGR2, PVGVL2=@PVGVL2, PVGVEAU2=@PVGVEAU2," &
                " PVGVCptEAU=@PVGVCptEAU, PVGVAdjEAU=@PVGVAdjEAU, PVGVAdjMAL=@PVGVAdjMAL," &
                " RetardGR_s=@RetardGR, RetardL_s=@RetardL, RetardEAU_s=@RetardEAU," &
                " RetardADJ_s=@RetardADJ, RetardAJT_s=@RetardAJT," &
                " SynchroAdjMLX1=@SAdjM1, SynchroAdjMLX2=@SAdjM2," &
                " SynchroAjtMLX1=@SAjtM1, SynchroAjtMLX2=@SAjtM2," &
                " PompeLavage=@PompeLav, PompeLastGach=@PompeLast," &
                " RincageADJ=@RincADJ, GestionAjouts=@GestAjouts," &
                " BypassVidange=@BpVid, BypassDosage=@BpDos," &
                " SansRepriseGR=@SansRepGR, SkipInexist=@SkipInex," &
                " RotInversee=@RotInv, DblCmdBypass=@DblByp," &
                " PresenceGS=@PresGS, LiantMelangeur=@LiantMel, ArretMelangeur=@ArretMel," &
                " RetardOuvert_s=@RetOuv, RetardFermet_s=@RetFerm," &
                " PassGR1=@PGR1, ChargeGR1=@CGR1, PassGR2=@PGR2, ChargeGR2=@CGR2," &
                " PassL1=@PL1, ChargeL1=@CL1, PassL2=@PL2, ChargeL2=@CL2," &
                " PassE1=@PE1, ChargeE1=@CE1, PassE2=@PE2, ChargeE2=@CE2," &
                " PassADJ1=@PADJ1, ChargeADJ1=@CADJ1," &
                " DosageType=@DosType, OrdreGR=@OrdGR, OrdreL=@OrdL," &
                " OrdreEAU=@OrdEAU, OrdreADJ=@OrdADJ, OrdreAJT=@OrdAJT," &
                " DeuxPoints=@DeuxPts, DepesageParal=@DepParal, PreMelange=@PreMel," &
                " ConsigneEau_l=@ConsEau, TempsMalax_s=@TpsMalax," &
                " LavageTrappe=@LavTrap, GoulotteBP=@GoulBP, DestConvoyeur=@DestConv," &
                " NumCaseEauRM=@NumCaseEau, NbCycles=@NbCycles," &
                " NbRemplM1=@NbRemplM1, NbRemplM2=@NbRemplM2," &
                " LavageAuto=@LavAuto, NbGacheesLavage=@NbGachLav, LavageExterne=@LavExt," &
                " GR1_Type=@GR1_Type, GR1_DestMal=@GR1_Dest, GR1_Portee=@GR1_Port," &
                " GR1_Echelon=@GR1_Ech, GR1_Zero=@GR1_Zer, GR1_TareMax=@GR1_Tare," &
                " GR1_SeuilGV=@GR1_SGV, GR1_DureeImp=@GR1_DImp, GR1_DoubleCmd=@GR1_Dbl," &
                " GR1_VerifCas=@GR1_Ver, GR1_TempsStab=@GR1_Stab, GR1_NbEchelon=@GR1_NbE," &
                " GR1_TempMaxVid=@GR1_TMaxV, GR1_TempsFerme=@GR1_TFerm," &
                " GR1_SeuilDebit=@GR1_SDebit, GR1_TempsMort=@GR1_TMort," &
                " GR1_D0=@GR1_D0,GR1_K0=@GR1_K0,GR1_D1=@GR1_D1,GR1_K1=@GR1_K1," &
                " GR1_D2=@GR1_D2,GR1_K2=@GR1_K2,GR1_D3=@GR1_D3,GR1_K3=@GR1_K3," &
                " GR1_D4=@GR1_D4,GR1_K4=@GR1_K4," &
                " GR2_Type=@GR2_Type, GR2_DestMal=@GR2_Dest, GR2_Portee=@GR2_Port," &
                " GR2_Echelon=@GR2_Ech, GR2_Zero=@GR2_Zer, GR2_TareMax=@GR2_Tare," &
                " GR2_SeuilGV=@GR2_SGV, GR2_DureeImp=@GR2_DImp, GR2_DoubleCmd=@GR2_Dbl," &
                " GR2_VerifCas=@GR2_Ver, GR2_TempsStab=@GR2_Stab, GR2_NbEchelon=@GR2_NbE," &
                " GR2_TempMaxVid=@GR2_TMaxV, GR2_TempsFerme=@GR2_TFerm," &
                " GR2_SeuilDebit=@GR2_SDebit, GR2_TempsMort=@GR2_TMort," &
                " GR2_D0=@GR2_D0,GR2_K0=@GR2_K0,GR2_D1=@GR2_D1,GR2_K1=@GR2_K1," &
                " GR2_D2=@GR2_D2,GR2_K2=@GR2_K2,GR2_D3=@GR2_D3,GR2_K3=@GR2_K3," &
                " GR2_D4=@GR2_D4,GR2_K4=@GR2_K4," &
                " L1_Type=@L1_Type, L1_DestMal=@L1_Dest, L1_Portee=@L1_Port," &
                " L1_Echelon=@L1_Ech, L1_Zero=@L1_Zer, L1_TareMax=@L1_Tare," &
                " L1_SeuilGV=@L1_SGV, L1_DureeImp=@L1_DImp, L1_DoubleCmd=@L1_Dbl," &
                " L1_VerifCas=@L1_Ver, L1_TempsStab=@L1_Stab, L1_NbEchelon=@L1_NbE," &
                " L1_TempMaxVid=@L1_TMaxV, L1_TempsFerme=@L1_TFerm," &
                " L1_SeuilDebit=@L1_SDebit, L1_TempsMort=@L1_TMort," &
                " L1_D0=@L1_D0,L1_K0=@L1_K0,L1_D1=@L1_D1,L1_K1=@L1_K1," &
                " L1_D2=@L1_D2,L1_K2=@L1_K2,L1_D3=@L1_D3,L1_K3=@L1_K3," &
                " L1_D4=@L1_D4,L1_K4=@L1_K4," &
                " L2_Type=@L2_Type, L2_DestMal=@L2_Dest, L2_Portee=@L2_Port," &
                " L2_Echelon=@L2_Ech, L2_Zero=@L2_Zer, L2_TareMax=@L2_Tare," &
                " L2_SeuilGV=@L2_SGV, L2_DureeImp=@L2_DImp, L2_DoubleCmd=@L2_Dbl," &
                " L2_VerifCas=@L2_Ver, L2_TempsStab=@L2_Stab, L2_NbEchelon=@L2_NbE," &
                " L2_TempMaxVid=@L2_TMaxV, L2_TempsFerme=@L2_TFerm," &
                " L2_SeuilDebit=@L2_SDebit, L2_TempsMort=@L2_TMort," &
                " L2_D0=@L2_D0,L2_K0=@L2_K0,L2_D1=@L2_D1,L2_K1=@L2_K1," &
                " L2_D2=@L2_D2,L2_K2=@L2_K2,L2_D3=@L2_D3,L2_K3=@L2_K3," &
                " L2_D4=@L2_D4,L2_K4=@L2_K4," &
                " E1_Type=@E1_Type, E1_DestMal=@E1_Dest, E1_Portee=@E1_Port," &
                " E1_Echelon=@E1_Ech, E1_Zero=@E1_Zer, E1_TareMax=@E1_Tare," &
                " E1_SeuilGV=@E1_SGV, E1_DureeImp=@E1_DImp, E1_DoubleCmd=@E1_Dbl," &
                " E1_VerifCas=@E1_Ver, E1_TempsStab=@E1_Stab, E1_NbEchelon=@E1_NbE," &
                " E1_TempMaxVid=@E1_TMaxV, E1_TempsFerme=@E1_TFerm," &
                " E1_SeuilDebit=@E1_SDebit, E1_TempsMort=@E1_TMort," &
                " E1_D0=@E1_D0,E1_K0=@E1_K0,E1_D1=@E1_D1,E1_K1=@E1_K1," &
                " E1_D2=@E1_D2,E1_K2=@E1_K2,E1_D3=@E1_D3,E1_K3=@E1_K3," &
                " E1_D4=@E1_D4,E1_K4=@E1_K4," &
                " E2_Type=@E2_Type, E2_DestMal=@E2_Dest, E2_Portee=@E2_Port," &
                " E2_Echelon=@E2_Ech, E2_Zero=@E2_Zer, E2_TareMax=@E2_Tare," &
                " E2_SeuilGV=@E2_SGV, E2_DureeImp=@E2_DImp, E2_DoubleCmd=@E2_Dbl," &
                " E2_VerifCas=@E2_Ver, E2_TempsStab=@E2_Stab, E2_NbEchelon=@E2_NbE," &
                " E2_TempMaxVid=@E2_TMaxV, E2_TempsFerme=@E2_TFerm," &
                " E2_SeuilDebit=@E2_SDebit, E2_TempsMort=@E2_TMort," &
                " E2_D0=@E2_D0,E2_K0=@E2_K0,E2_D1=@E2_D1,E2_K1=@E2_K1," &
                " E2_D2=@E2_D2,E2_K2=@E2_K2,E2_D3=@E2_D3,E2_K3=@E2_K3," &
                " E2_D4=@E2_D4,E2_K4=@E2_K4," &
                " AD_Type=@AD_Type, AD_DestMal=@AD_Dest, AD_Portee=@AD_Port," &
                " AD_Echelon=@AD_Ech, AD_Zero=@AD_Zer, AD_TareMax=@AD_Tare," &
                " AD_SeuilGV=@AD_SGV, AD_DureeImp=@AD_DImp, AD_DoubleCmd=@AD_Dbl," &
                " AD_VerifCas=@AD_Ver, AD_TempsStab=@AD_Stab, AD_NbEchelon=@AD_NbE," &
                " AD_TempMaxVid=@AD_TMaxV, AD_TempsFerme=@AD_TFerm," &
                " AD_SeuilDebit=@AD_SDebit, AD_TempsMort=@AD_TMort," &
                " AD_D0=@AD_D0,AD_K0=@AD_K0,AD_D1=@AD_D1,AD_K1=@AD_K1," &
                " AD_D2=@AD_D2,AD_K2=@AD_K2,AD_D3=@AD_D3,AD_K3=@AD_K3," &
                " AD_D4=@AD_D4,AD_K4=@AD_K4," &
                " AJ_Type=@AJ_Type, AJ_DestMal=@AJ_Dest, AJ_Portee=@AJ_Port," &
                " AJ_Echelon=@AJ_Ech, AJ_Zero=@AJ_Zer, AJ_TareMax=@AJ_Tare," &
                " AJ_SeuilGV=@AJ_SGV, AJ_DureeImp=@AJ_DImp, AJ_DoubleCmd=@AJ_Dbl," &
                " AJ_VerifCas=@AJ_Ver, AJ_TempsStab=@AJ_Stab, AJ_NbEchelon=@AJ_NbE," &
                " AJ_TempMaxVid=@AJ_TMaxV, AJ_TempsFerme=@AJ_TFerm," &
                " AJ_SeuilDebit=@AJ_SDebit, AJ_TempsMort=@AJ_TMort," &
                " AJ_D0=@AJ_D0,AJ_K0=@AJ_K0,AJ_D1=@AJ_D1,AJ_K1=@AJ_K1," &
                " AJ_D2=@AJ_D2,AJ_K2=@AJ_K2,AJ_D3=@AJ_D3,AJ_K3=@AJ_K3," &
                " AJ_D4=@AJ_D4,AJ_K4=@AJ_K4" &
                " WHERE ID=1"

            Using cmd As New SqlCommand(sql, cn)
                ' Compteurs
                cmd.Parameters.AddWithValue("@MultiADJ1", N(txtMultiADJ1.Text))
                cmd.Parameters.AddWithValue("@DivADJ1", N(txtDivADJ1.Text))
                cmd.Parameters.AddWithValue("@MultiADJ2", N(txtMultiADJ2.Text))
                cmd.Parameters.AddWithValue("@DivADJ2", N(txtDivADJ2.Text))
                cmd.Parameters.AddWithValue("@MultiADJ3", N(txtMultiADJ3.Text))
                cmd.Parameters.AddWithValue("@DivADJ3", N(txtDivADJ3.Text))
                cmd.Parameters.AddWithValue("@MultiADJ4", N(txtMultiADJ4.Text))
                cmd.Parameters.AddWithValue("@DivADJ4", N(txtDivADJ4.Text))
                cmd.Parameters.AddWithValue("@MultiEAU1", N(txtMultiEAU1.Text))
                cmd.Parameters.AddWithValue("@DivEAU1", N(txtDivEAU1.Text))
                cmd.Parameters.AddWithValue("@MultiEAU2", N(txtMultiEAU2.Text))
                cmd.Parameters.AddWithValue("@DivEAU2", N(txtDivEAU2.Text))
                cmd.Parameters.AddWithValue("@MultiEAU3", N(txtMultiEAU3.Text))
                cmd.Parameters.AddWithValue("@DivEAU3", N(txtDivEAU3.Text))
                cmd.Parameters.AddWithValue("@MultiEAU4", N(txtMultiEAU4.Text))
                cmd.Parameters.AddWithValue("@DivEAU4", N(txtDivEAU4.Text))
                cmd.Parameters.AddWithValue("@MultiHYG1", N(txtMultiHYG1.Text))
                cmd.Parameters.AddWithValue("@DivHYG1", N(txtDivHYG1.Text))
                cmd.Parameters.AddWithValue("@MultiHYG2", N(txtMultiHYG2.Text))
                cmd.Parameters.AddWithValue("@DivHYG2", N(txtDivHYG2.Text))
                cmd.Parameters.AddWithValue("@MultiHYG3", N(txtMultiHYG3.Text))
                cmd.Parameters.AddWithValue("@DivHYG3", N(txtDivHYG3.Text))
                cmd.Parameters.AddWithValue("@MultiHYG4", N(txtMultiHYG4.Text))
                cmd.Parameters.AddWithValue("@DivHYG4", N(txtDivHYG4.Text))
                ' Malaxeurs
                cmd.Parameters.AddWithValue("@DivisionGachees", N(txtTailleGacheeSec.Text))
                cmd.Parameters.AddWithValue("@MaxGacheesEntieres", N(txtTailleMin.Text))
                cmd.Parameters.AddWithValue("@AjoutEauPostes", B(btnAjoutEau))
                cmd.Parameters.AddWithValue("@GestionPlasticiteEC", B(btnGestionPlasticite))
                cmd.Parameters.AddWithValue("@TailleGacheeSec_m3", D(txtTailleGacheeSec.Text))
                cmd.Parameters.AddWithValue("@TailleMin_m3", D(txtTailleMin.Text))
                cmd.Parameters.AddWithValue("@HygroCommun2Mal", B(btnHygroCommun))
                cmd.Parameters.AddWithValue("@PresenceHygroBeton", B(btnPresenceHygro))
                cmd.Parameters.AddWithValue("@Mal1Taille", D(txtMalTaille.Text))
                cmd.Parameters.AddWithValue("@Mal1Puiss", N(txtMalPuissVide.Text))
                cmd.Parameters.AddWithValue("@Mal1ArretAuto", B(btnMalArretAuto))
                cmd.Parameters.AddWithValue("@Mal1MarcheImp", B(btnMalMarcheImp))
                cmd.Parameters.AddWithValue("@Mal1TempsVid", N(txtMalTempsVidange.Text))
                cmd.Parameters.AddWithValue("@Mal1Klaxon", N(txtMalTempsKlaxon.Text))
                cmd.Parameters.AddWithValue("@Dur1_0", N(txtDurImp1_0.Text))
                cmd.Parameters.AddWithValue("@Dur1_1", N(txtDurImp1_1.Text))
                cmd.Parameters.AddWithValue("@Dur1_2", N(txtDurImp1_2.Text))
                cmd.Parameters.AddWithValue("@Dur2_0", N(txtDurImp2_0.Text))
                cmd.Parameters.AddWithValue("@Dur2_1", N(txtDurImp2_1.Text))
                cmd.Parameters.AddWithValue("@Dur2_2", N(txtDurImp2_2.Text))
                cmd.Parameters.AddWithValue("@DurM1_0", N(txtDurMort1_0.Text))
                cmd.Parameters.AddWithValue("@DurM1_1", N(txtDurMort1_1.Text))
                cmd.Parameters.AddWithValue("@DurM1_2", N(txtDurMort1_2.Text))
                cmd.Parameters.AddWithValue("@DurM2_0", N(txtDurMort2_0.Text))
                cmd.Parameters.AddWithValue("@DurM2_1", N(txtDurMort2_1.Text))
                cmd.Parameters.AddWithValue("@DurM2_2", N(txtDurMort2_2.Text))
                cmd.Parameters.AddWithValue("@DurP_0", N(txtDurPlein_0.Text))
                cmd.Parameters.AddWithValue("@DurP_1", N(txtDurPlein_1.Text))
                cmd.Parameters.AddWithValue("@DurP_2", N(txtDurPlein_2.Text))
                cmd.Parameters.AddWithValue("@DurD_0", N(txtDurDern_0.Text))
                cmd.Parameters.AddWithValue("@DurD_1", N(txtDurDern_1.Text))
                cmd.Parameters.AddWithValue("@DurD_2", N(txtDurDern_2.Text))
                ' Watt1
                cmd.Parameters.AddWithValue("@W1Prop", N(txtWatt1Proportion.Text))
                cmd.Parameters.AddWithValue("@W1Type", cmbWatt1TypeLiaison.SelectedIndex)
                cmd.Parameters.AddWithValue("@W1D0", D(txtWatt1D0.Text)) : cmd.Parameters.AddWithValue("@W1K0", D(txtWatt1K0.Text))
                cmd.Parameters.AddWithValue("@W1D1", D(txtWatt1D1.Text)) : cmd.Parameters.AddWithValue("@W1K1", D(txtWatt1K1.Text))
                cmd.Parameters.AddWithValue("@W1D2", D(txtWatt1D2.Text)) : cmd.Parameters.AddWithValue("@W1K2", D(txtWatt1K2.Text))
                cmd.Parameters.AddWithValue("@W1D3", D(txtWatt1D3.Text)) : cmd.Parameters.AddWithValue("@W1K3", D(txtWatt1K3.Text))
                cmd.Parameters.AddWithValue("@W1D4", D(txtWatt1D4.Text)) : cmd.Parameters.AddWithValue("@W1K4", D(txtWatt1K4.Text))
                ' Watt2
                cmd.Parameters.AddWithValue("@W2Prop", N(txtWatt2Proportion.Text))
                cmd.Parameters.AddWithValue("@W2Type", cmbWatt2TypeLiaison.SelectedIndex)
                cmd.Parameters.AddWithValue("@W2D0", D(txtWatt2D0.Text)) : cmd.Parameters.AddWithValue("@W2K0", D(txtWatt2K0.Text))
                cmd.Parameters.AddWithValue("@W2D1", D(txtWatt2D1.Text)) : cmd.Parameters.AddWithValue("@W2K1", D(txtWatt2K1.Text))
                cmd.Parameters.AddWithValue("@W2D2", D(txtWatt2D2.Text)) : cmd.Parameters.AddWithValue("@W2K2", D(txtWatt2K2.Text))
                cmd.Parameters.AddWithValue("@W2D3", D(txtWatt2D3.Text)) : cmd.Parameters.AddWithValue("@W2K3", D(txtWatt2K3.Text))
                cmd.Parameters.AddWithValue("@W2D4", D(txtWatt2D4.Text)) : cmd.Parameters.AddWithValue("@W2K4", D(txtWatt2K4.Text))
                ' Divers
                cmd.Parameters.AddWithValue("@GestionFiltrage", B(btnGestionFiltrage))
                cmd.Parameters.AddWithValue("@TrappeAna", B(btnTrappeAnalogique))
                cmd.Parameters.AddWithValue("@GestionGraissage", B(btnGestionGraissage))
                cmd.Parameters.AddWithValue("@GestionHygro", cmbGestionHygro.SelectedIndex)
                cmd.Parameters.AddWithValue("@TempsCycle", N(txtTempsCyclePlas.Text))
                cmd.Parameters.AddWithValue("@TailleMinG", D(txtTailleMinGachee.Text))
                cmd.Parameters.AddWithValue("@TpsEntre2", N(txtTempsEntre2Lectures.Text))
                cmd.Parameters.AddWithValue("@Stabilite", N(txtStabilitePlas.Text))
                cmd.Parameters.AddWithValue("@Tolerance", N(txtTolerancePlas.Text))
                cmd.Parameters.AddWithValue("@DosageMax", D(txtDosageMaxAjout.Text))
                cmd.Parameters.AddWithValue("@TpsAjouts", N(txtTempsEntreAjouts.Text))
                ' Sondes S1
                cmd.Parameters.AddWithValue("@S1TL", cmbTypeLiaisonS1.SelectedIndex)
                cmd.Parameters.AddWithValue("@S1Prop", N(txtProportionS1.Text))
                cmd.Parameters.AddWithValue("@S1NC", N(txtNumCaseS1.Text))
                cmd.Parameters.AddWithValue("@S1CMin", D(txtCorrMinS1.Text))
                cmd.Parameters.AddWithValue("@S1CMax", D(txtCorrMaxS1.Text))
                cmd.Parameters.AddWithValue("@S1CA", B(btnCorrAutoS1))
                cmd.Parameters.AddWithValue("@S1CD", B(btnCorrDosageS1))
                cmd.Parameters.AddWithValue("@S1D0", D(txtSondeD0S1.Text)) : cmd.Parameters.AddWithValue("@S1K0", D(txtSondeK0S1.Text))
                cmd.Parameters.AddWithValue("@S1D1", D(txtSondeD1S1.Text)) : cmd.Parameters.AddWithValue("@S1K1", D(txtSondeK1S1.Text))
                cmd.Parameters.AddWithValue("@S1D2", D(txtSondeD2S1.Text)) : cmd.Parameters.AddWithValue("@S1K2", D(txtSondeK2S1.Text))
                cmd.Parameters.AddWithValue("@S1D3", D(txtSondeD3S1.Text)) : cmd.Parameters.AddWithValue("@S1K3", D(txtSondeK3S1.Text))
                cmd.Parameters.AddWithValue("@S1D4", D(txtSondeD4S1.Text)) : cmd.Parameters.AddWithValue("@S1K4", D(txtSondeK4S1.Text))
                ' Sondes S2
                cmd.Parameters.AddWithValue("@S2TL", cmbTypeLiaisonS2.SelectedIndex)
                cmd.Parameters.AddWithValue("@S2Prop", N(txtProportionS2.Text))
                cmd.Parameters.AddWithValue("@S2NC", N(txtNumCaseS2.Text))
                cmd.Parameters.AddWithValue("@S2CMin", D(txtCorrMinS2.Text))
                cmd.Parameters.AddWithValue("@S2CMax", D(txtCorrMaxS2.Text))
                cmd.Parameters.AddWithValue("@S2CA", B(btnCorrAutoS2))
                cmd.Parameters.AddWithValue("@S2CD", B(btnCorrDosageS2))
                cmd.Parameters.AddWithValue("@S2D0", D(txtSondeD0S2.Text)) : cmd.Parameters.AddWithValue("@S2K0", D(txtSondeK0S2.Text))
                cmd.Parameters.AddWithValue("@S2D1", D(txtSondeD1S2.Text)) : cmd.Parameters.AddWithValue("@S2K1", D(txtSondeK1S2.Text))
                cmd.Parameters.AddWithValue("@S2D2", D(txtSondeD2S2.Text)) : cmd.Parameters.AddWithValue("@S2K2", D(txtSondeK2S2.Text))
                cmd.Parameters.AddWithValue("@S2D3", D(txtSondeD3S2.Text)) : cmd.Parameters.AddWithValue("@S2K3", D(txtSondeK3S2.Text))
                cmd.Parameters.AddWithValue("@S2D4", D(txtSondeD4S2.Text)) : cmd.Parameters.AddWithValue("@S2K4", D(txtSondeK4S2.Text))
                ' Sondes S3
                cmd.Parameters.AddWithValue("@S3TL", cmbTypeLiaisonS3.SelectedIndex)
                cmd.Parameters.AddWithValue("@S3Prop", N(txtProportionS3.Text))
                cmd.Parameters.AddWithValue("@S3NC", N(txtNumCaseS3.Text))
                cmd.Parameters.AddWithValue("@S3CMin", D(txtCorrMinS3.Text))
                cmd.Parameters.AddWithValue("@S3CMax", D(txtCorrMaxS3.Text))
                cmd.Parameters.AddWithValue("@S3CA", B(btnCorrAutoS3))
                cmd.Parameters.AddWithValue("@S3CD", B(btnCorrDosageS3))
                cmd.Parameters.AddWithValue("@S3D0", D(txtSondeD0S3.Text)) : cmd.Parameters.AddWithValue("@S3K0", D(txtSondeK0S3.Text))
                cmd.Parameters.AddWithValue("@S3D1", D(txtSondeD1S3.Text)) : cmd.Parameters.AddWithValue("@S3K1", D(txtSondeK1S3.Text))
                cmd.Parameters.AddWithValue("@S3D2", D(txtSondeD2S3.Text)) : cmd.Parameters.AddWithValue("@S3K2", D(txtSondeK2S3.Text))
                cmd.Parameters.AddWithValue("@S3D3", D(txtSondeD3S3.Text)) : cmd.Parameters.AddWithValue("@S3K3", D(txtSondeK3S3.Text))
                cmd.Parameters.AddWithValue("@S3D4", D(txtSondeD4S3.Text)) : cmd.Parameters.AddWithValue("@S3K4", D(txtSondeK4S3.Text))
                ' Thermomètre
                cmd.Parameters.AddWithValue("@THTL", cmbTypeLiaisonTH.SelectedIndex)
                cmd.Parameters.AddWithValue("@THProp", N(txtProportionTH.Text))
                cmd.Parameters.AddWithValue("@THNC", N(txtNumCaseTH.Text))
                cmd.Parameters.AddWithValue("@THCMin", D(txtCorrMinTH.Text))
                cmd.Parameters.AddWithValue("@THCMax", D(txtCorrMaxTH.Text))
                cmd.Parameters.AddWithValue("@THCA", B(btnCorrAutoTH))
                cmd.Parameters.AddWithValue("@THCD", B(btnCorrDosageTH))
                cmd.Parameters.AddWithValue("@THD0", D(txtSondeD0TH.Text)) : cmd.Parameters.AddWithValue("@THK0", D(txtSondeK0TH.Text))
                cmd.Parameters.AddWithValue("@THD1", D(txtSondeD1TH.Text)) : cmd.Parameters.AddWithValue("@THK1", D(txtSondeK1TH.Text))
                cmd.Parameters.AddWithValue("@THD2", D(txtSondeD2TH.Text)) : cmd.Parameters.AddWithValue("@THK2", D(txtSondeK2TH.Text))
                cmd.Parameters.AddWithValue("@THD3", D(txtSondeD3TH.Text)) : cmd.Parameters.AddWithValue("@THK3", D(txtSondeK3TH.Text))
                cmd.Parameters.AddWithValue("@THD4", D(txtSondeD4TH.Text)) : cmd.Parameters.AddWithValue("@THK4", D(txtSondeK4TH.Text))
                ' Densimètre
                cmd.Parameters.AddWithValue("@DENTL", cmbTypeLiaisonDEN.SelectedIndex)
                cmd.Parameters.AddWithValue("@DENProp", N(txtProportionDEN.Text))
                cmd.Parameters.AddWithValue("@DENC", N(txtNumCaseDEN.Text))
                cmd.Parameters.AddWithValue("@DENCMin", D(txtCorrMinDEN.Text))
                cmd.Parameters.AddWithValue("@DENCMax", D(txtCorrMaxDEN.Text))
                cmd.Parameters.AddWithValue("@DENCA", B(btnCorrAutoDEN))
                cmd.Parameters.AddWithValue("@DENCD", B(btnCorrDosageDEN))
                cmd.Parameters.AddWithValue("@DEND0", D(txtSondeD0DEN.Text)) : cmd.Parameters.AddWithValue("@DENK0", D(txtSondeK0DEN.Text))
                cmd.Parameters.AddWithValue("@DEND1", D(txtSondeD1DEN.Text)) : cmd.Parameters.AddWithValue("@DENK1", D(txtSondeK1DEN.Text))
                cmd.Parameters.AddWithValue("@DEND2", D(txtSondeD2DEN.Text)) : cmd.Parameters.AddWithValue("@DENK2", D(txtSondeK2DEN.Text))
                cmd.Parameters.AddWithValue("@DEND3", D(txtSondeD3DEN.Text)) : cmd.Parameters.AddWithValue("@DENK3", D(txtSondeK3DEN.Text))
                cmd.Parameters.AddWithValue("@DEND4", D(txtSondeD4DEN.Text)) : cmd.Parameters.AddWithValue("@DENK4", D(txtSondeK4DEN.Text))
                ' Reprise granulats
                cmd.Parameters.AddWithValue("@TapisReprise", B(btnTapisReprise))
                cmd.Parameters.AddWithValue("@ArretTapis", B(btnArretTapis))
                cmd.Parameters.AddWithValue("@TapisPeseur", B(btnTapisPeseur))
                cmd.Parameters.AddWithValue("@DoubleTapis", B(btnDoubleTapis))
                cmd.Parameters.AddWithValue("@VidangeTapis", B(btnVidangeTapis))
                cmd.Parameters.AddWithValue("@TpsMontTapis", N(txtTempsMontTapis.Text))
                cmd.Parameters.AddWithValue("@TpsMontSec", N(txtTempsMontSec.Text))
                cmd.Parameters.AddWithValue("@TpsMontTapis2", N(txtTempsMontTapis2.Text))
                cmd.Parameters.AddWithValue("@DecalageVid", N(txtDecalageVid.Text))
                cmd.Parameters.AddWithValue("@TTPDest", cmbDestMalaxTTP.SelectedIndex)
                cmd.Parameters.AddWithValue("@TTPPesage", cmbGestionPesage.SelectedIndex)
                cmd.Parameters.AddWithValue("@TTPRetard", N(txtRetardLiant.Text))
                cmd.Parameters.AddWithValue("@TTPVid", N(txtTempsVidTTP.Text))
                cmd.Parameters.AddWithValue("@TTPVib", N(txtActivVibreur.Text))
                cmd.Parameters.AddWithValue("@TTPDuree", N(txtDureeImpTTP.Text))
                cmd.Parameters.AddWithValue("@TTPMort", N(txtTempsMortTTP.Text))
                cmd.Parameters.AddWithValue("@TTPDbl", B(btnDoubleCmdTTP))
                cmd.Parameters.AddWithValue("@SkipRetard", N(txtRetardLiantSkip.Text))
                cmd.Parameters.AddWithValue("@SkipAtt", N(txtTempsMontAtt.Text))
                cmd.Parameters.AddWithValue("@SkipMont", N(txtTempsMontSkip.Text))
                cmd.Parameters.AddWithValue("@SkipDesc", N(txtTempsDescSkip.Text))
                cmd.Parameters.AddWithValue("@SkipVid", N(txtTempsVidSkip.Text))
                cmd.Parameters.AddWithValue("@SkipVidSec", N(txtTempsVidSecSkip.Text))
                cmd.Parameters.AddWithValue("@SkipTrappe", N(txtRetardTrappe.Text))
                cmd.Parameters.AddWithValue("@SkipPuiss", N(txtRetardPuiss.Text))
                cmd.Parameters.AddWithValue("@SkipSec", N(txtRetardSec.Text))
                cmd.Parameters.AddWithValue("@SkipImmed", B(btnMonteeImmed))
                ' Centrale
                cmd.Parameters.AddWithValue("@PesageGR", cmbPesageGR.SelectedIndex)
                cmd.Parameters.AddWithValue("@PesageGRMal", B(btnPesageGRMal))
                cmd.Parameters.AddWithValue("@RepriseGR", cmbRepriseGR.SelectedIndex)
                cmd.Parameters.AddWithValue("@NbCasesGR", cmbNbCasesGR.SelectedIndex + 1)
                cmd.Parameters.AddWithValue("@PesageADJ", cmbPesageADJ.SelectedIndex)
                cmd.Parameters.AddWithValue("@NbBolsADJ", cmbNbBolsADJ.SelectedIndex + 1)
                cmd.Parameters.AddWithValue("@TempsVidBol", N(txtTempsVidBol.Text))
                cmd.Parameters.AddWithValue("@NbCasesADJ", cmbNbCasesADJ.SelectedIndex + 1)
                cmd.Parameters.AddWithValue("@PesageL", cmbPesageL.SelectedIndex)
                cmd.Parameters.AddWithValue("@NbCasesL", cmbNbCasesL.SelectedIndex + 1)
                cmd.Parameters.AddWithValue("@PesageEAU", cmbPesageEAU.SelectedIndex)
                cmd.Parameters.AddWithValue("@NbCasesEAU", cmbNbCasesEAU.SelectedIndex + 1)
                cmd.Parameters.AddWithValue("@NbMalaxeurs", cmbNbMalaxeurs.SelectedIndex + 1)
                cmd.Parameters.AddWithValue("@VidSimult", B(btnVidSimult))
                cmd.Parameters.AddWithValue("@NbTrappes1", cmbNbTrappes1.SelectedIndex + 1)
                cmd.Parameters.AddWithValue("@NbTrappes2", cmbNbTrappes2.SelectedIndex)
                cmd.Parameters.AddWithValue("@ByPass1", B(btnByPass1))
                cmd.Parameters.AddWithValue("@ByPass2", B(btnByPass2))
                cmd.Parameters.AddWithValue("@Goulotte1", B(btnGoulotte1))
                cmd.Parameters.AddWithValue("@PVGVGR", GetPVGV(btnPVGVGR))
                cmd.Parameters.AddWithValue("@PVGVL", GetPVGV(btnPVGVL))
                cmd.Parameters.AddWithValue("@PVGVEAU", GetPVGV(btnPVGVEAU))
                cmd.Parameters.AddWithValue("@PVGVADJ", GetPVGV(btnPVGVADJ))
                cmd.Parameters.AddWithValue("@PVGVGR2", GetPVGV(btnPVGVGR2))
                cmd.Parameters.AddWithValue("@PVGVL2", GetPVGV(btnPVGVL2))
                cmd.Parameters.AddWithValue("@PVGVEAU2", GetPVGV(btnPVGVEAU2))
                cmd.Parameters.AddWithValue("@PVGVCptEAU", GetPVGV(btnPVGVCptEAU))
                cmd.Parameters.AddWithValue("@PVGVAdjEAU", GetPVGV(btnPVGVAdjEAU))
                cmd.Parameters.AddWithValue("@PVGVAdjMAL", GetPVGV(btnPVGVAdjMAL))
                ' Param vidange
                cmd.Parameters.AddWithValue("@RetardGR", N(txtRetardGR.Text))
                cmd.Parameters.AddWithValue("@RetardL", N(txtRetardL.Text))
                cmd.Parameters.AddWithValue("@RetardEAU", N(txtRetardEAU.Text))
                cmd.Parameters.AddWithValue("@RetardADJ", N(txtRetardADJ.Text))
                cmd.Parameters.AddWithValue("@RetardAJT", N(txtRetardAJT.Text))
                cmd.Parameters.AddWithValue("@SAdjM1", B(btnSynchroAdjMLX1))
                cmd.Parameters.AddWithValue("@SAdjM2", B(btnSynchroAdjMLX2))
                cmd.Parameters.AddWithValue("@SAjtM1", B(btnSynchroAjtMLX1))
                cmd.Parameters.AddWithValue("@SAjtM2", B(btnSynchroAjtMLX2))
                cmd.Parameters.AddWithValue("@PompeLav", B(btnPompeLavage))
                cmd.Parameters.AddWithValue("@PompeLast", B(btnPompeLastGach))
                cmd.Parameters.AddWithValue("@RincADJ", B(btnRincageADJ))
                cmd.Parameters.AddWithValue("@GestAjouts", B(btnGestionAjouts))
                cmd.Parameters.AddWithValue("@BpVid", B(btnBypassVidange))
                cmd.Parameters.AddWithValue("@BpDos", B(btnBypassDosage))
                ' Gachée sèche
                cmd.Parameters.AddWithValue("@SansRepGR", B(btnSansRepriseGR))
                cmd.Parameters.AddWithValue("@SkipInex", B(btnSkipInexist))
                cmd.Parameters.AddWithValue("@RotInv", B(btnRotInversee))
                cmd.Parameters.AddWithValue("@DblByp", B(btnDblCmdBypass))
                cmd.Parameters.AddWithValue("@PresGS", B(btnPresenceGS))
                cmd.Parameters.AddWithValue("@LiantMel", B(btnLiantMelangeur))
                cmd.Parameters.AddWithValue("@ArretMel", B(btnArretMelangeur))
                cmd.Parameters.AddWithValue("@RetOuv", N(txtRetardOuvert.Text))
                cmd.Parameters.AddWithValue("@RetFerm", N(txtRetardFermet.Text))
                cmd.Parameters.AddWithValue("@PGR1", B(btnPassGR1)) : cmd.Parameters.AddWithValue("@CGR1", D(txtChargeGR1.Text))
                cmd.Parameters.AddWithValue("@PGR2", B(btnPassGR2)) : cmd.Parameters.AddWithValue("@CGR2", D(txtChargeGR2.Text))
                cmd.Parameters.AddWithValue("@PL1", B(btnPassL1)) : cmd.Parameters.AddWithValue("@CL1", D(txtChargeL1.Text))
                cmd.Parameters.AddWithValue("@PL2", B(btnPassL2)) : cmd.Parameters.AddWithValue("@CL2", D(txtChargeL2.Text))
                cmd.Parameters.AddWithValue("@PE1", B(btnPassE1)) : cmd.Parameters.AddWithValue("@CE1", D(txtChargeE1.Text))
                cmd.Parameters.AddWithValue("@PE2", B(btnPassE2)) : cmd.Parameters.AddWithValue("@CE2", D(txtChargeE2.Text))
                cmd.Parameters.AddWithValue("@PADJ1", B(btnPassADJ1)) : cmd.Parameters.AddWithValue("@CADJ1", D(txtChargeADJ1.Text))
                cmd.Parameters.AddWithValue("@DosType", B(btnDosageType))
                cmd.Parameters.AddWithValue("@OrdGR", N(txtOrdreGR.Text))
                cmd.Parameters.AddWithValue("@OrdL", N(txtOrdreL.Text))
                cmd.Parameters.AddWithValue("@OrdEAU", N(txtOrdreEAU.Text))
                cmd.Parameters.AddWithValue("@OrdADJ", N(txtOrdreADJ.Text))
                cmd.Parameters.AddWithValue("@OrdAJT", N(txtOrdreAJT.Text))
                cmd.Parameters.AddWithValue("@DeuxPts", B(btnDeuxPoints))
                cmd.Parameters.AddWithValue("@DepParal", B(btnDepesageParal))
                cmd.Parameters.AddWithValue("@PreMel", B(btnPreMelange))
                ' Lavage
                cmd.Parameters.AddWithValue("@ConsEau", D(txtConsigneEauRM.Text))
                cmd.Parameters.AddWithValue("@TpsMalax", N(txtTempsMalaxRM.Text))
                cmd.Parameters.AddWithValue("@LavTrap", B(btnLavageTrappe))
                cmd.Parameters.AddWithValue("@GoulBP", cmbGoulotteBP.SelectedIndex)
                cmd.Parameters.AddWithValue("@DestConv", cmbDestConvoyeur.SelectedIndex)
                cmd.Parameters.AddWithValue("@NumCaseEau", N(txtNumCaseEauRM.Text))
                cmd.Parameters.AddWithValue("@NbCycles", N(txtNbCyclesRM.Text))
                cmd.Parameters.AddWithValue("@NbRemplM1", N(txtNbRemplM1.Text))
                cmd.Parameters.AddWithValue("@NbRemplM2", N(txtNbRemplM2.Text))
                cmd.Parameters.AddWithValue("@LavAuto", B(btnLavageAuto))
                cmd.Parameters.AddWithValue("@NbGachLav", 0) 'TODO: ajouter txtNbGacheesLavage au Designer
                cmd.Parameters.AddWithValue("@LavExt", B(btnLavageExterne))

                ' ── Bascules ──────────────────────────────────────
                AjouterParamsBascules(cmd)

                cmd.ExecuteNonQuery()
            End Using

            ' Sauvegarder aussi la case en cours
            If _listeCases.Count > 0 Then SauvegarderCase(_listeCases(_indexCase))

            MessageBox.Show("Configuration sauvegardée.", "OK",
                            MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show("Erreur sauvegarde : " & ex.Message,
                            "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            FermerConnexion()
        End Try
    End Sub

    ' ═══════════════════════════════════════════════════════════════
    '  HELPERS CONVERSION
    ' ═══════════════════════════════════════════════════════════════
    Private Function N(s As String) As Integer
        Dim v As Integer
        Return If(Integer.TryParse(s, v), v, 0)
    End Function

    Private Function D(s As String) As Decimal
        Dim v As Decimal
        Return If(Decimal.TryParse(s.Replace(",", "."),
                  Globalization.NumberStyles.Any,
                  Globalization.CultureInfo.InvariantCulture, v), v, 0)
    End Function

    Private Function B(btn As Button) As Boolean
        Return btn.Text = "OUI"
    End Function

    ' ═══════════════════════════════════════════════════════════════
    '  GESTION DES CASES
    ' ═══════════════════════════════════════════════════════════════
    Private Sub ChargerListeCases()
        Try
            OuvrirConnexion()
            _listeCases.Clear()
            Using cmd As New SqlCommand(
                "SELECT NumeroCase FROM dbo.Cases ORDER BY NumeroCase", cn)
                Using dr As SqlDataReader = cmd.ExecuteReader()
                    While dr.Read()
                        _listeCases.Add(CInt(dr("NumeroCase")))
                    End While
                End Using
            End Using
            If _listeCases.Count = 0 Then
                ' Créer une première case vide
                Using cmd As New SqlCommand(
                    "INSERT INTO dbo.Cases (NumeroCase) VALUES (1)", cn)
                    cmd.ExecuteNonQuery()
                End Using
                _listeCases.Add(1)
            End If
            _indexCase = 0
            ChargerCase(_listeCases(_indexCase))
        Catch ex As Exception
            MessageBox.Show("Erreur chargement cases : " & ex.Message)
        Finally
            FermerConnexion()
        End Try
    End Sub

    Private Sub ChargerCase(numCase As Integer)
        _chargementCase = True
        Try
            OuvrirConnexion()
            Using cmd As New SqlCommand(
                "SELECT * FROM dbo.Cases WHERE NumeroCase=@N", cn)
                cmd.Parameters.AddWithValue("@N", numCase)
                Using dr As SqlDataReader = cmd.ExecuteReader()
                    If Not dr.Read() Then Exit Try

                    TextBox3.Text = numCase.ToString
                    cmbMateriau.SelectedIndex = CInt(dr("TypeMateriau"))
                    txtTempsDosageMax.Text = dr("TempsDosageMax").ToString
                    txtCapaciteCase.Text = dr("CapaciteCase").ToString
                    txtCodeCase.Text = dr("CodeCase").ToString
                    txtSeuilAlerte.Text = dr("SeuilAlerte").ToString
                    txtNiveauCase.Text = dr("NiveauCase").ToString
                    txtOrdreDosage.Text = dr("OrdreDosage").ToString
                    txtDebitRattrapage.Text = dr("DebitRattrapage").ToString
                    txtNumCaseCommune.Text = dr("NumCaseCommune").ToString

                    ' Type spécifique
                    AfficherOngletType(CInt(dr("TypeMateriau")))

                    ' GR
                    cmbDestGranulat.SelectedIndex = CInt(dr("GR_DestBascule"))
                    txtSeuilPV_G.Text = dr("GR_SeuilPV").ToString
                    txtSeuilDV_G.Text = dr("GR_SeuilDV").ToString
                    txtCanonSeuilD.Text = dr("GR_CanonSeuilD").ToString
                    txtCanonSeuil.Text = dr("GR_CanonSeuil").ToString
                    txtCanonTemps.Text = dr("GR_CanonTemps").ToString
                    txtTapisSeuilA.Text = dr("GR_TapisSeuil").ToString
                    txtTapisTempsA.Text = dr("GR_TapisTemps").ToString
                    txtBatDuree.Text = dr("GR_BatDuree").ToString
                    txtBatMort.Text = dr("GR_BatMort").ToString
                    txtHygro.Text = dr("GR_Hygro").ToString
                    SetBtn(btnSensRotation, CBool(dr("GR_SensRotation")))
                    ' L
                    SetBtn(btnTypeLiant, CBool(dr("L_TypeLiant")))
                    SetBtn(btnBascLiants, CBool(dr("L_BascLiants")))
                    SetBtn(btnCompartiment, CBool(dr("L_Compartiment")))
                    txtSeuilPV_L.Text = dr("L_SeuilPV").ToString
                    txtSeuilDV_L.Text = dr("L_SeuilDV").ToString
                    txtCanonSeuilD_L.Text = dr("L_CanonSeuilD").ToString
                    txtCanonTemps_L.Text = dr("L_CanonTemps").ToString
                    ' E
                    cmbDestEau.SelectedIndex = CInt(dr("E_DestBascule"))
                    SetBtn(btnTypeDosageEau, CBool(dr("E_TypeDosage")))
                    SetBtn(btnEauChargee, CBool(dr("E_EauChargee")))
                    SetBtn(btnCompartimentEau, CBool(dr("E_Compartiment")))
                    txtSeuilPV_E.Text = dr("E_SeuilPV").ToString
                    txtChargeMES.Text = dr("E_ChargeMES").ToString
                    ' ADJ
                    cmbDestAdj.SelectedIndex = CInt(dr("A_DestBascule"))
                    SetBtn(btnTypeDosageAdj, CBool(dr("A_TypeDosage")))
                    txtSeuilPV_A.Text = dr("A_SeuilPV").ToString
                    txtDebitPompe.Text = dr("A_DebitPompe").ToString
                End Using
            End Using
            MettreAJourNavigationCase()
        Catch ex As Exception
            MessageBox.Show("Erreur chargement case : " & ex.Message)
        Finally
            _chargementCase = False
            FermerConnexion()
        End Try
    End Sub

    Private Sub SauvegarderCase(numCase As Integer)
        Using cmd As New SqlCommand(
            "IF EXISTS (SELECT 1 FROM dbo.Cases WHERE NumeroCase=@N) " &
            "  UPDATE dbo.Cases SET TypeMateriau=@TM, TempsDosageMax=@TDM, CapaciteCase=@Cap," &
            "    CodeCase=@Code, SeuilAlerte=@SeuAlerte, NiveauCase=@Niv," &
            "    OrdreDosage=@Ord, DebitRattrapage=@Deb, NumCaseCommune=@NCC," &
            "    GR_DestBascule=@GRDest, GR_SeuilPV=@GRPV, GR_SeuilDV=@GRDV," &
            "    GR_CanonSeuilD=@GRCD, GR_CanonSeuil=@GRC, GR_CanonTemps=@GRCT," &
            "    GR_TapisSeuil=@GRTapis, GR_TapisTemps=@GRTapisT," &
            "    GR_BatDuree=@GRBatD, GR_BatMort=@GRBatM, GR_Hygro=@GRHyg," &
            "    GR_SensRotation=@GRRot," &
            "    L_TypeLiant=@LTL, L_BascLiants=@LBL, L_Compartiment=@LComp," &
            "    L_SeuilPV=@LPV, L_SeuilDV=@LDV, L_CanonSeuilD=@LCD, L_CanonTemps=@LCT," &
            "    E_DestBascule=@EDest, E_TypeDosage=@ETD, E_EauChargee=@EEC," &
            "    E_Compartiment=@EComp, E_SeuilPV=@EPV, E_ChargeMES=@EMES," &
            "    A_DestBascule=@ADest, A_TypeDosage=@ATD, A_SeuilPV=@APV, A_DebitPompe=@ADebit" &
            "  WHERE NumeroCase=@N " &
            "ELSE " &
            "  INSERT INTO dbo.Cases (NumeroCase,TypeMateriau,TempsDosageMax,CapaciteCase," &
            "    CodeCase,SeuilAlerte,NiveauCase,OrdreDosage,DebitRattrapage,NumCaseCommune," &
            "    GR_DestBascule,GR_SeuilPV,GR_SeuilDV,GR_CanonSeuilD,GR_CanonSeuil,GR_CanonTemps," &
            "    GR_TapisSeuil,GR_TapisTemps,GR_BatDuree,GR_BatMort,GR_Hygro,GR_SensRotation," &
            "    L_TypeLiant,L_BascLiants,L_Compartiment,L_SeuilPV,L_SeuilDV,L_CanonSeuilD,L_CanonTemps," &
            "    E_DestBascule,E_TypeDosage,E_EauChargee,E_Compartiment,E_SeuilPV,E_ChargeMES," &
            "    A_DestBascule,A_TypeDosage,A_SeuilPV,A_DebitPompe)" &
            "  VALUES(@N,@TM,@TDM,@Cap,@Code,@SeuAlerte,@Niv,@Ord,@Deb,@NCC," &
            "    @GRDest,@GRPV,@GRDV,@GRCD,@GRC,@GRCT,@GRTapis,@GRTapisT,@GRBatD,@GRBatM,@GRHyg,@GRRot," &
            "    @LTL,@LBL,@LComp,@LPV,@LDV,@LCD,@LCT," &
            "    @EDest,@ETD,@EEC,@EComp,@EPV,@EMES," &
            "    @ADest,@ATD,@APV,@ADebit)", cn)

            cmd.Parameters.AddWithValue("@N", numCase)
            cmd.Parameters.AddWithValue("@TM", cmbMateriau.SelectedIndex)
            cmd.Parameters.AddWithValue("@TDM", N(txtTempsDosageMax.Text))
            cmd.Parameters.AddWithValue("@Cap", D(txtCapaciteCase.Text))
            cmd.Parameters.AddWithValue("@Code", txtCodeCase.Text.Trim)
            cmd.Parameters.AddWithValue("@SeuAlerte", D(txtSeuilAlerte.Text))
            cmd.Parameters.AddWithValue("@Niv", D(txtNiveauCase.Text))
            cmd.Parameters.AddWithValue("@Ord", N(txtOrdreDosage.Text))
            cmd.Parameters.AddWithValue("@Deb", D(txtDebitRattrapage.Text))
            cmd.Parameters.AddWithValue("@NCC", N(txtNumCaseCommune.Text))
            ' GR
            cmd.Parameters.AddWithValue("@GRDest", cmbDestGranulat.SelectedIndex)
            cmd.Parameters.AddWithValue("@GRPV", D(txtSeuilPV_G.Text))
            cmd.Parameters.AddWithValue("@GRDV", D(txtSeuilDV_G.Text))
            cmd.Parameters.AddWithValue("@GRCD", D(txtCanonSeuilD.Text))
            cmd.Parameters.AddWithValue("@GRC", D(txtCanonSeuil.Text))
            cmd.Parameters.AddWithValue("@GRCT", N(txtCanonTemps.Text))
            cmd.Parameters.AddWithValue("@GRTapis", D(txtTapisSeuilA.Text))
            cmd.Parameters.AddWithValue("@GRTapisT", N(txtTapisTempsA.Text))
            cmd.Parameters.AddWithValue("@GRBatD", N(txtBatDuree.Text))
            cmd.Parameters.AddWithValue("@GRBatM", N(txtBatMort.Text))
            cmd.Parameters.AddWithValue("@GRHyg", D(txtHygro.Text))
            cmd.Parameters.AddWithValue("@GRRot", B(btnSensRotation))
            ' L
            cmd.Parameters.AddWithValue("@LTL", B(btnTypeLiant))
            cmd.Parameters.AddWithValue("@LBL", B(btnBascLiants))
            cmd.Parameters.AddWithValue("@LComp", B(btnCompartiment))
            cmd.Parameters.AddWithValue("@LPV", D(txtSeuilPV_L.Text))
            cmd.Parameters.AddWithValue("@LDV", D(txtSeuilDV_L.Text))
            cmd.Parameters.AddWithValue("@LCD", D(txtCanonSeuilD_L.Text))
            cmd.Parameters.AddWithValue("@LCT", N(txtCanonTemps_L.Text))
            ' E
            cmd.Parameters.AddWithValue("@EDest", cmbDestEau.SelectedIndex)
            cmd.Parameters.AddWithValue("@ETD", B(btnTypeDosageEau))
            cmd.Parameters.AddWithValue("@EEC", B(btnEauChargee))
            cmd.Parameters.AddWithValue("@EComp", B(btnCompartimentEau))
            cmd.Parameters.AddWithValue("@EPV", D(txtSeuilPV_E.Text))
            cmd.Parameters.AddWithValue("@EMES", D(txtChargeMES.Text))
            ' ADJ
            cmd.Parameters.AddWithValue("@ADest", cmbDestAdj.SelectedIndex)
            cmd.Parameters.AddWithValue("@ATD", B(btnTypeDosageAdj))
            cmd.Parameters.AddWithValue("@APV", D(txtSeuilPV_A.Text))
            cmd.Parameters.AddWithValue("@ADebit", D(txtDebitPompe.Text))

            cmd.ExecuteNonQuery()
        End Using
    End Sub

    Private Sub AfficherOngletType(typeMat As Integer)
        ' Montre l'onglet correspondant au type de matériau
        TabControlTypes.SelectedIndex = Math.Min(typeMat, 3)
    End Sub

    Private Sub MettreAJourNavigationCase()
        Dim total As Integer = _listeCases.Count
        Label4.Text = $"Case {_indexCase + 1} / {total}"
        btnPrecedentNumero.Enabled = (_indexCase > 0)
        btnSuivantNumero.Enabled = (_indexCase < total - 1)
    End Sub

    ' ═══════════════════════════════════════════════════════════════
    '  NAVIGATION CASES
    ' ═══════════════════════════════════════════════════════════════
    Private Sub btnPrecedentNumero_Click(sender As Object, e As EventArgs) _
        Handles btnPrecedentNumero.Click
        If _indexCase > 0 Then
            OuvrirConnexion()
            SauvegarderCase(_listeCases(_indexCase))
            _indexCase -= 1
            ChargerCase(_listeCases(_indexCase))
            FermerConnexion()
        End If
    End Sub

    Private Sub btnSuivantNumero_Click(sender As Object, e As EventArgs) _
        Handles btnSuivantNumero.Click
        If _indexCase < _listeCases.Count - 1 Then
            OuvrirConnexion()
            SauvegarderCase(_listeCases(_indexCase))
            _indexCase += 1
            ChargerCase(_listeCases(_indexCase))
            FermerConnexion()
        End If
    End Sub

    Private Sub btnRAZ_Click(sender As Object, e As EventArgs) Handles btnRAZ.Click
        If MessageBox.Show("Remettre à zéro cette case ?", "RAZ",
                           MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            txtTempsDosageMax.Text = "0" : txtCapaciteCase.Text = "0"
            txtCodeCase.Text = "" : txtSeuilAlerte.Text = "0"
            txtNiveauCase.Text = "0" : txtOrdreDosage.Text = "0"
            txtDebitRattrapage.Text = "0" : txtNumCaseCommune.Text = "0"
        End If
    End Sub

    Private Sub cmbMateriau_SelectedIndexChanged(sender As Object, e As EventArgs) _
        Handles cmbMateriau.SelectedIndexChanged
        If Not _chargementCase Then
            AfficherOngletType(cmbMateriau.SelectedIndex)
        End If
    End Sub

    ' ═══════════════════════════════════════════════════════════════
    '  TOGGLES — MALAXEURS
    ' ═══════════════════════════════════════════════════════════════
    Private Sub btnDivisionGachees_Click(s As Object, e As EventArgs) Handles btnDivisionGachees.Click
        ToggleBtn(btnDivisionGachees)
    End Sub
    Private Sub btnMaxGachees_Click(s As Object, e As EventArgs) Handles btnMaxGachees.Click
        ToggleBtn(btnMaxGachees)
    End Sub
    Private Sub btnAjoutEau_Click(s As Object, e As EventArgs) Handles btnAjoutEau.Click
        ToggleBtn(btnAjoutEau)
    End Sub
    Private Sub btnGestionPlasticite_Click(s As Object, e As EventArgs) Handles btnGestionPlasticite.Click
        ToggleBtn(btnGestionPlasticite)
    End Sub
    Private Sub btnHygroCommun_Click(s As Object, e As EventArgs) Handles btnHygroCommun.Click
        ToggleBtn(btnHygroCommun)
    End Sub
    Private Sub btnPresenceHygro_Click(s As Object, e As EventArgs) Handles btnPresenceHygro.Click
        ToggleBtn(btnPresenceHygro)
    End Sub
    Private Sub btnMalArretAuto_Click(s As Object, e As EventArgs) Handles btnMalArretAuto.Click
        ToggleBtn(btnMalArretAuto)
    End Sub
    Private Sub btnMalMarcheImp_Click(s As Object, e As EventArgs) Handles btnMalMarcheImp.Click
        ToggleBtn(btnMalMarcheImp)
    End Sub
    Private Sub btnGestionFiltrage_Click(s As Object, e As EventArgs) Handles btnGestionFiltrage.Click
        ToggleBtn(btnGestionFiltrage)
    End Sub
    Private Sub btnTrappeAnalogique_Click(s As Object, e As EventArgs) Handles btnTrappeAnalogique.Click
        ToggleBtn(btnTrappeAnalogique)
    End Sub
    Private Sub btnGestionGraissage_Click(s As Object, e As EventArgs) Handles btnGestionGraissage.Click
        ToggleBtn(btnGestionGraissage)
    End Sub

    ' ═══════════════════════════════════════════════════════════════
    '  TOGGLES — SONDES
    ' ═══════════════════════════════════════════════════════════════
    Private Sub btnCorrAutoS1_Click(s As Object, e As EventArgs) Handles btnCorrAutoS1.Click
        ToggleBtn(btnCorrAutoS1)
    End Sub
    Private Sub btnCorrDosageS1_Click(s As Object, e As EventArgs) Handles btnCorrDosageS1.Click
        ToggleBtn(btnCorrDosageS1)
    End Sub
    Private Sub btnCorrAutoS2_Click(s As Object, e As EventArgs) Handles btnCorrAutoS2.Click
        ToggleBtn(btnCorrAutoS2)
    End Sub
    Private Sub btnCorrDosageS2_Click(s As Object, e As EventArgs) Handles btnCorrDosageS2.Click
        ToggleBtn(btnCorrDosageS2)
    End Sub
    Private Sub btnCorrAutoS3_Click(s As Object, e As EventArgs) Handles btnCorrAutoS3.Click
        ToggleBtn(btnCorrAutoS3)
    End Sub
    Private Sub btnCorrDosageS3_Click(s As Object, e As EventArgs) Handles btnCorrDosageS3.Click
        ToggleBtn(btnCorrDosageS3)
    End Sub
    Private Sub btnCorrAutoS4_Click(s As Object, e As EventArgs) Handles btnCorrAutoS4.Click
        ToggleBtn(btnCorrAutoS4)
    End Sub
    Private Sub btnCorrDosageS4_Click(s As Object, e As EventArgs) Handles btnCorrDosageS4.Click
        ToggleBtn(btnCorrDosageS4)
    End Sub
    Private Sub btnCorrAutoS5_Click(s As Object, e As EventArgs) Handles btnCorrAutoS5.Click
        ToggleBtn(btnCorrAutoS5)
    End Sub
    Private Sub btnCorrDosageS5_Click(s As Object, e As EventArgs) Handles btnCorrDosageS5.Click
        ToggleBtn(btnCorrDosageS5)
    End Sub
    Private Sub btnCorrAutoS6_Click(s As Object, e As EventArgs) Handles btnCorrAutoS6.Click
        ToggleBtn(btnCorrAutoS6)
    End Sub
    Private Sub btnCorrDosageS6_Click(s As Object, e As EventArgs) Handles btnCorrDosageS6.Click
        ToggleBtn(btnCorrDosageS6)
    End Sub
    Private Sub btnCorrAutoS7_Click(s As Object, e As EventArgs) Handles btnCorrAutoS7.Click
        ToggleBtn(btnCorrAutoS7)
    End Sub
    Private Sub btnCorrDosageS7_Click(s As Object, e As EventArgs) Handles btnCorrDosageS7.Click
        ToggleBtn(btnCorrDosageS7)
    End Sub
    Private Sub btnCorrAutoTH_Click(s As Object, e As EventArgs) Handles btnCorrAutoTH.Click
        ToggleBtn(btnCorrAutoTH)
    End Sub
    Private Sub btnCorrDosageTH_Click(s As Object, e As EventArgs) Handles btnCorrDosageTH.Click
        ToggleBtn(btnCorrDosageTH)
    End Sub
    Private Sub btnCorrAutoDEN_Click(s As Object, e As EventArgs) Handles btnCorrAutoDEN.Click
        ToggleBtn(btnCorrAutoDEN)
    End Sub
    Private Sub btnCorrDosageDEN_Click(s As Object, e As EventArgs) Handles btnCorrDosageDEN.Click
        ToggleBtn(btnCorrDosageDEN)
    End Sub

    ' ═══════════════════════════════════════════════════════════════
    '  TOGGLES — REPRISE GRANULATS
    ' ═══════════════════════════════════════════════════════════════
    Private Sub btnTapisReprise_Click(s As Object, e As EventArgs) Handles btnTapisReprise.Click
        ToggleBtn(btnTapisReprise)
    End Sub
    Private Sub btnArretTapis_Click(s As Object, e As EventArgs) Handles btnArretTapis.Click
        ToggleBtn(btnArretTapis)
    End Sub
    Private Sub btnTapisPeseur_Click(s As Object, e As EventArgs) Handles btnTapisPeseur.Click
        ToggleBtn(btnTapisPeseur)
    End Sub
    Private Sub btnDoubleTapis_Click(s As Object, e As EventArgs) Handles btnDoubleTapis.Click
        ToggleBtn(btnDoubleTapis)
    End Sub
    Private Sub btnVidangeTapis_Click(s As Object, e As EventArgs) Handles btnVidangeTapis.Click
        ToggleBtn(btnVidangeTapis)
    End Sub
    Private Sub btnDoubleCmdTTP_Click(s As Object, e As EventArgs) Handles btnDoubleCmdTTP.Click
        ToggleBtn(btnDoubleCmdTTP)
    End Sub
    Private Sub btnMonteeImmed_Click(s As Object, e As EventArgs) Handles btnMonteeImmed.Click
        ToggleBtn(btnMonteeImmed)
    End Sub

    ' ═══════════════════════════════════════════════════════════════
    '  TOGGLES — CENTRALE (NON/OUI)
    ' ═══════════════════════════════════════════════════════════════
    Private Sub btnPesageGRMal_Click(s As Object, e As EventArgs) Handles btnPesageGRMal.Click
        ToggleBtn(btnPesageGRMal)
    End Sub
    Private Sub btnVidSimult_Click(s As Object, e As EventArgs) Handles btnVidSimult.Click
        ToggleBtn(btnVidSimult)
    End Sub
    Private Sub btnByPass1_Click(s As Object, e As EventArgs) Handles btnByPass1.Click
        ToggleBtn(btnByPass1)
    End Sub
    Private Sub btnByPass2_Click(s As Object, e As EventArgs) Handles btnByPass2.Click
        ToggleBtn(btnByPass2)
    End Sub
    Private Sub btnGoulotte1_Click(s As Object, e As EventArgs) Handles btnGoulotte1.Click
        ToggleBtn(btnGoulotte1)
    End Sub

    ' ═══════════════════════════════════════════════════════════════
    '  TOGGLES — CENTRALE (PV/GV)
    ' ═══════════════════════════════════════════════════════════════
    Private Sub btnPVGVGR_Click(s As Object, e As EventArgs) Handles btnPVGVGR.Click
        TogglePVGV(btnPVGVGR)
    End Sub
    Private Sub btnPVGVL_Click(s As Object, e As EventArgs) Handles btnPVGVL.Click
        TogglePVGV(btnPVGVL)
    End Sub
    Private Sub btnPVGVEAU_Click(s As Object, e As EventArgs) Handles btnPVGVEAU.Click
        TogglePVGV(btnPVGVEAU)
    End Sub
    Private Sub btnPVGVADJ_Click(s As Object, e As EventArgs) Handles btnPVGVADJ.Click
        TogglePVGV(btnPVGVADJ)
    End Sub
    Private Sub btnPVGVGR2_Click(s As Object, e As EventArgs) Handles btnPVGVGR2.Click
        TogglePVGV(btnPVGVGR2)
    End Sub
    Private Sub btnPVGVL2_Click(s As Object, e As EventArgs) Handles btnPVGVL2.Click
        TogglePVGV(btnPVGVL2)
    End Sub
    Private Sub btnPVGVEAU2_Click(s As Object, e As EventArgs) Handles btnPVGVEAU2.Click
        TogglePVGV(btnPVGVEAU2)
    End Sub
    Private Sub btnPVGVCptEAU_Click(s As Object, e As EventArgs) Handles btnPVGVCptEAU.Click
        TogglePVGV(btnPVGVCptEAU)
    End Sub
    Private Sub btnPVGVAdjEAU_Click(s As Object, e As EventArgs) Handles btnPVGVAdjEAU.Click
        TogglePVGV(btnPVGVAdjEAU)
    End Sub
    Private Sub btnPVGVAdjMAL_Click(s As Object, e As EventArgs) Handles btnPVGVAdjMAL.Click
        TogglePVGV(btnPVGVAdjMAL)
    End Sub

    ' ═══════════════════════════════════════════════════════════════
    '  TOGGLES — PARAM VIDANGE
    ' ═══════════════════════════════════════════════════════════════
    Private Sub btnSynchroAdjMLX1_Click(s As Object, e As EventArgs) Handles btnSynchroAdjMLX1.Click
        ToggleBtn(btnSynchroAdjMLX1)
    End Sub
    Private Sub btnSynchroAdjMLX2_Click(s As Object, e As EventArgs) Handles btnSynchroAdjMLX2.Click
        ToggleBtn(btnSynchroAdjMLX2)
    End Sub
    Private Sub btnSynchroAjtMLX1_Click(s As Object, e As EventArgs) Handles btnSynchroAjtMLX1.Click
        ToggleBtn(btnSynchroAjtMLX1)
    End Sub
    Private Sub btnSynchroAjtMLX2_Click(s As Object, e As EventArgs) Handles btnSynchroAjtMLX2.Click
        ToggleBtn(btnSynchroAjtMLX2)
    End Sub
    Private Sub btnPompeLavage_Click(s As Object, e As EventArgs) Handles btnPompeLavage.Click
        ToggleBtn(btnPompeLavage)
    End Sub
    Private Sub btnPompeLastGach_Click(s As Object, e As EventArgs) Handles btnPompeLastGach.Click
        ToggleBtn(btnPompeLastGach)
    End Sub
    Private Sub btnRincageADJ_Click(s As Object, e As EventArgs) Handles btnRincageADJ.Click
        ToggleBtn(btnRincageADJ)
    End Sub
    Private Sub btnGestionAjouts_Click(s As Object, e As EventArgs) Handles btnGestionAjouts.Click
        ToggleBtn(btnGestionAjouts)
    End Sub
    Private Sub btnBypassVidange_Click(s As Object, e As EventArgs) Handles btnBypassVidange.Click
        ToggleBtn(btnBypassVidange)
    End Sub
    Private Sub btnBypassDosage_Click(s As Object, e As EventArgs) Handles btnBypassDosage.Click
        ToggleBtn(btnBypassDosage)
    End Sub

    ' ═══════════════════════════════════════════════════════════════
    '  TOGGLES — GACHÉE SÈCHE
    ' ═══════════════════════════════════════════════════════════════
    Private Sub btnSansRepriseGR_Click(s As Object, e As EventArgs) Handles btnSansRepriseGR.Click
        ToggleBtn(btnSansRepriseGR)
    End Sub
    Private Sub btnSkipInexist_Click(s As Object, e As EventArgs) Handles btnSkipInexist.Click
        ToggleBtn(btnSkipInexist)
    End Sub
    Private Sub btnRotInversee_Click(s As Object, e As EventArgs) Handles btnRotInversee.Click
        ToggleBtn(btnRotInversee)
    End Sub
    Private Sub btnDblCmdBypass_Click(s As Object, e As EventArgs) Handles btnDblCmdBypass.Click
        ToggleBtn(btnDblCmdBypass)
    End Sub
    Private Sub btnPresenceGS_Click(s As Object, e As EventArgs) Handles btnPresenceGS.Click
        ToggleBtn(btnPresenceGS)
    End Sub
    Private Sub btnLiantMelangeur_Click(s As Object, e As EventArgs) Handles btnLiantMelangeur.Click
        ToggleBtn(btnLiantMelangeur)
    End Sub
    Private Sub btnArretMelangeur_Click(s As Object, e As EventArgs) Handles btnArretMelangeur.Click
        ToggleBtn(btnArretMelangeur)
    End Sub
    Private Sub btnPassGR1_Click(s As Object, e As EventArgs) Handles btnPassGR1.Click
        ToggleBtn(btnPassGR1)
    End Sub
    Private Sub btnPassGR2_Click(s As Object, e As EventArgs) Handles btnPassGR2.Click
        ToggleBtn(btnPassGR2)
    End Sub
    Private Sub btnPassL1_Click(s As Object, e As EventArgs) Handles btnPassL1.Click
        ToggleBtn(btnPassL1)
    End Sub
    Private Sub btnPassL2_Click(s As Object, e As EventArgs) Handles btnPassL2.Click
        ToggleBtn(btnPassL2)
    End Sub
    Private Sub btnPassE1_Click(s As Object, e As EventArgs) Handles btnPassE1.Click
        ToggleBtn(btnPassE1)
    End Sub
    Private Sub btnPassE2_Click(s As Object, e As EventArgs) Handles btnPassE2.Click
        ToggleBtn(btnPassE2)
    End Sub
    Private Sub btnPassADJ1_Click(s As Object, e As EventArgs) Handles btnPassADJ1.Click
        ToggleBtn(btnPassADJ1)
    End Sub
    Private Sub btnDosageType_Click(s As Object, e As EventArgs) Handles btnDosageType.Click
        ToggleBtn(btnDosageType)
    End Sub
    Private Sub btnDeuxPoints_Click(s As Object, e As EventArgs) Handles btnDeuxPoints.Click
        ToggleBtn(btnDeuxPoints)
    End Sub
    Private Sub btnDepesageParal_Click(s As Object, e As EventArgs) Handles btnDepesageParal.Click
        ToggleBtn(btnDepesageParal)
    End Sub
    Private Sub btnPreMelange_Click(s As Object, e As EventArgs) Handles btnPreMelange.Click
        ToggleBtn(btnPreMelange)
    End Sub

    ' ═══════════════════════════════════════════════════════════════
    '  TOGGLES — PARAM LAVAGE
    ' ═══════════════════════════════════════════════════════════════
    Private Sub btnLavageTrappe_Click(s As Object, e As EventArgs) Handles btnLavageTrappe.Click
        ToggleBtn(btnLavageTrappe)
    End Sub
    Private Sub btnLavageExterne_Click(s As Object, e As EventArgs) Handles btnLavageExterne.Click
        ToggleBtn(btnLavageExterne)
    End Sub

    Private Sub btnLavageAuto_Click(sender As Object, e As EventArgs) Handles btnLavageAuto.Click
        ToggleBtn(btnLavageAuto)
        ' TODO: txtNbGacheesLavage à ajouter dans le Designer (PARAMETRAGELAVAGE)
        ' txtNbGacheesLavage.Visible = (btnLavageAuto.Text = "OUI")
    End Sub

    ' ═══════════════════════════════════════════════════════════════
    '  TOGGLES — CASES
    ' ═══════════════════════════════════════════════════════════════
    Private Sub btnSensRotation_Click(s As Object, e As EventArgs) Handles btnSensRotation.Click
        ToggleBtn(btnSensRotation)
    End Sub
    Private Sub btnTypeLiant_Click(s As Object, e As EventArgs) Handles btnTypeLiant.Click
        ToggleBtn(btnTypeLiant)
    End Sub
    Private Sub btnBascLiants_Click(s As Object, e As EventArgs) Handles btnBascLiants.Click
        ToggleBtn(btnBascLiants)
    End Sub
    Private Sub btnCompartiment_Click(s As Object, e As EventArgs) Handles btnCompartiment.Click
        ToggleBtn(btnCompartiment)
    End Sub
    Private Sub btnTypeDosageEau_Click(s As Object, e As EventArgs) Handles btnTypeDosageEau.Click
        ToggleBtn(btnTypeDosageEau)
    End Sub
    Private Sub btnEauChargee_Click(s As Object, e As EventArgs) Handles btnEauChargee.Click
        ToggleBtn(btnEauChargee)
    End Sub
    Private Sub btnCompartimentEau_Click(s As Object, e As EventArgs) Handles btnCompartimentEau.Click
        ToggleBtn(btnCompartimentEau)
    End Sub
    Private Sub btnTypeDosageAdj_Click(s As Object, e As EventArgs) Handles btnTypeDosageAdj.Click
        ToggleBtn(btnTypeDosageAdj)
    End Sub

    ' ═══════════════════════════════════════════════════════════════
    '  BOUTONS PRINCIPAUX
    ' ═══════════════════════════════════════════════════════════════
    Private Sub BtnValider_Click(sender As Object, e As EventArgs) Handles BtnValider.Click
        SauvegarderConfig()
    End Sub

    Private Sub BtnAbandonner_Click(sender As Object, e As EventArgs) Handles BtnAbandonner.Click
        If MessageBox.Show("Abandonner les modifications ?", "Confirmation",
                           MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            Me.Close()
        End If
    End Sub

    Private Sub BtnAide_Click(sender As Object, e As EventArgs) Handles BtnAide.Click
        MessageBox.Show("Configuration Automate — BETONIA" & vbCrLf &
                        "Remplir tous les paramètres puis cliquer sur Valider.",
                        "Aide", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    ' ═══════════════════════════════════════════════════════════════
    '  CHARGER BASCULES (depuis dbo.Configuration)
    ' ═══════════════════════════════════════════════════════════════
    Private Sub ChargerBascules(dr As SqlDataReader)
        ' GR1
        cmbTypes.SelectedIndex = CInt(dr("GR1_Type"))
        cmbDestMalax.SelectedIndex = CInt(dr("GR1_DestMal"))
        txtPortee.Text = dr("GR1_Portee").ToString
        txtEchelon.Text = dr("GR1_Echelon").ToString
        txtZero.Text = dr("GR1_Zero").ToString
        txtTareMax.Text = dr("GR1_TareMax").ToString
        txtSeuilGV.Text = dr("GR1_SeuilGV").ToString
        txtDureeImp.Text = dr("GR1_DureeImp").ToString
        SetBtn(btnDoubleCmd, CBool(dr("GR1_DoubleCmd")))
        SetBtn(btnVerifCas, CBool(dr("GR1_VerifCas")))
        txtTempsStab.Text = dr("GR1_TempsStab").ToString
        txtNbEchelon.Text = dr("GR1_NbEchelon").ToString
        txtTempMaxVid.Text = dr("GR1_TempMaxVid").ToString
        txtTempsFerme.Text = dr("GR1_TempsFerme").ToString
        txtSeuilDebit.Text = dr("GR1_SeuilDebit").ToString
        txtTempsMort.Text = dr("GR1_TempsMort").ToString
        txtCalD0.Text = dr("GR1_D0").ToString : txtCalK0.Text = dr("GR1_K0").ToString
        txtCalD1.Text = dr("GR1_D1").ToString : txtCalK1.Text = dr("GR1_K1").ToString
        txtCalD2.Text = dr("GR1_D2").ToString : txtCalK2.Text = dr("GR1_K2").ToString
        txtCalD3.Text = dr("GR1_D3").ToString : txtCalK3.Text = dr("GR1_K3").ToString
        txtCalD4.Text = dr("GR1_D4").ToString : txtCalK4.Text = dr("GR1_K4").ToString

        ' GR2
        cmbTypesGR2.SelectedIndex = CInt(dr("GR2_Type"))
        cmbDestMalaxGR2.SelectedIndex = CInt(dr("GR2_DestMal"))
        txtPorteeGR2.Text = dr("GR2_Portee").ToString
        txtEchelonGR2.Text = dr("GR2_Echelon").ToString
        txtZeroGR2.Text = dr("GR2_Zero").ToString
        txtTareMaxGR2.Text = dr("GR2_TareMax").ToString
        txtSeuilGVGR2.Text = dr("GR2_SeuilGV").ToString
        txtDureeImpGR2.Text = dr("GR2_DureeImp").ToString
        SetBtn(btnDoubleCmdGR2, CBool(dr("GR2_DoubleCmd")))
        SetBtn(btnVerifCasGR2, CBool(dr("GR2_VerifCas")))
        txtTempsStabGR2.Text = dr("GR2_TempsStab").ToString
        txtNbEchelonGR2.Text = dr("GR2_NbEchelon").ToString
        txtTempMaxVidGR2.Text = dr("GR2_TempMaxVid").ToString
        txtTempsFermeGR2.Text = dr("GR2_TempsFerme").ToString
        txtSeuilDebitGR2.Text = dr("GR2_SeuilDebit").ToString
        txtTempsMortGR2.Text = dr("GR2_TempsMort").ToString
        txtCalD0GR2.Text = dr("GR2_D0").ToString : txtCalK0GR2.Text = dr("GR2_K0").ToString
        txtCalD1GR2.Text = dr("GR2_D1").ToString : txtCalK1GR2.Text = dr("GR2_K1").ToString
        txtCalD2GR2.Text = dr("GR2_D2").ToString : txtCalK2GR2.Text = dr("GR2_K2").ToString
        txtCalD3GR2.Text = dr("GR2_D3").ToString : txtCalK3GR2.Text = dr("GR2_K3").ToString
        txtCalD4GR2.Text = dr("GR2_D4").ToString : txtCalK4GR2.Text = dr("GR2_K4").ToString

        ' L1
        cmbTypesL1.SelectedIndex = CInt(dr("L1_Type"))
        cmbDestMalaxL1.SelectedIndex = CInt(dr("L1_DestMal"))
        txtPorteeL1.Text = dr("L1_Portee").ToString
        txtEchelonL1.Text = dr("L1_Echelon").ToString
        txtZeroL1.Text = dr("L1_Zero").ToString
        txtTareMaxL1.Text = dr("L1_TareMax").ToString
        txtSeuilGVL1.Text = dr("L1_SeuilGV").ToString
        txtDureeImpL1.Text = dr("L1_DureeImp").ToString
        SetBtn(btnDoubleCmdL1, CBool(dr("L1_DoubleCmd")))
        SetBtn(btnVerifCasL1, CBool(dr("L1_VerifCas")))
        txtTempsStabL1.Text = dr("L1_TempsStab").ToString
        txtNbEchelonL1.Text = dr("L1_NbEchelon").ToString
        txtTempMaxVidL1.Text = dr("L1_TempMaxVid").ToString
        txtTempsFermeL1.Text = dr("L1_TempsFerme").ToString
        txtSeuilDebitL1.Text = dr("L1_SeuilDebit").ToString
        txtTempsMortL1.Text = dr("L1_TempsMort").ToString
        txtCalD0L1.Text = dr("L1_D0").ToString : txtCalK0L1.Text = dr("L1_K0").ToString
        txtCalD1L1.Text = dr("L1_D1").ToString : txtCalK1L1.Text = dr("L1_K1").ToString
        txtCalD2L1.Text = dr("L1_D2").ToString : txtCalK2L1.Text = dr("L1_K2").ToString
        txtCalD3L1.Text = dr("L1_D3").ToString : txtCalK3L1.Text = dr("L1_K3").ToString
        txtCalD4L1.Text = dr("L1_D4").ToString : txtCalK4L1.Text = dr("L1_K4").ToString

        ' L2
        cmbTypesL2.SelectedIndex = CInt(dr("L2_Type"))
        cmbDestMalaxL2.SelectedIndex = CInt(dr("L2_DestMal"))
        txtPorteeL2.Text = dr("L2_Portee").ToString
        txtEchelonL2.Text = dr("L2_Echelon").ToString
        txtZeroL2.Text = dr("L2_Zero").ToString
        txtTareMaxL2.Text = dr("L2_TareMax").ToString
        txtSeuilGVL2.Text = dr("L2_SeuilGV").ToString
        txtDureeImpL2.Text = dr("L2_DureeImp").ToString
        SetBtn(btnDoubleCmdL2, CBool(dr("L2_DoubleCmd")))
        SetBtn(btnVerifCasL2, CBool(dr("L2_VerifCas")))
        txtTempsStabL2.Text = dr("L2_TempsStab").ToString
        txtNbEchelonL2.Text = dr("L2_NbEchelon").ToString
        txtTempMaxVidL2.Text = dr("L2_TempMaxVid").ToString
        txtTempsFermeL2.Text = dr("L2_TempsFerme").ToString
        txtSeuilDebitL2.Text = dr("L2_SeuilDebit").ToString
        txtTempsMortL2.Text = dr("L2_TempsMort").ToString
        txtCalD0L2.Text = dr("L2_D0").ToString : txtCalK0L2.Text = dr("L2_K0").ToString
        txtCalD1L2.Text = dr("L2_D1").ToString : txtCalK1L2.Text = dr("L2_K1").ToString
        txtCalD2L2.Text = dr("L2_D2").ToString : txtCalK2L2.Text = dr("L2_K2").ToString
        txtCalD3L2.Text = dr("L2_D3").ToString : txtCalK3L2.Text = dr("L2_K3").ToString
        txtCalD4L2.Text = dr("L2_D4").ToString : txtCalK4L2.Text = dr("L2_K4").ToString

        ' E1
        cmbTypesE1.SelectedIndex = CInt(dr("E1_Type"))
        cmbDestMalaxE1.SelectedIndex = CInt(dr("E1_DestMal"))
        txtPorteeE1.Text = dr("E1_Portee").ToString
        txtEchelonE1.Text = dr("E1_Echelon").ToString
        txtZeroE1.Text = dr("E1_Zero").ToString
        txtTareMaxE1.Text = dr("E1_TareMax").ToString
        txtSeuilGVE1.Text = dr("E1_SeuilGV").ToString
        txtDureeImpE1.Text = dr("E1_DureeImp").ToString
        SetBtn(btnDoubleCmdE1, CBool(dr("E1_DoubleCmd")))
        SetBtn(btnVerifCasLE1, CBool(dr("E1_VerifCas")))
        txtTempsStabE1.Text = dr("E1_TempsStab").ToString
        txtNbEchelonE1.Text = dr("E1_NbEchelon").ToString
        txtTempMaxVidE1.Text = dr("E1_TempMaxVid").ToString
        txtTempsFermeE1.Text = dr("E1_TempsFerme").ToString
        txtSeuilDebitE1.Text = dr("E1_SeuilDebit").ToString
        txtTempsMortE1.Text = dr("E1_TempsMort").ToString
        txtCalD0E1.Text = dr("E1_D0").ToString : txtCalK0E1.Text = dr("E1_K0").ToString
        txtCalD1E1.Text = dr("E1_D1").ToString : txtCalK1E1.Text = dr("E1_K1").ToString
        txtCalD2E1.Text = dr("E1_D2").ToString : txtCalK2E1.Text = dr("E1_K2").ToString
        txtCalD3E1.Text = dr("E1_D3").ToString : txtCalK3E1.Text = dr("E1_K3").ToString
        txtCalD4E1.Text = dr("E1_D4").ToString : txtCalK4E1.Text = dr("E1_K4").ToString

        ' E2
        cmbTypesE2.SelectedIndex = CInt(dr("E2_Type"))
        cmbDestMalaxE2.SelectedIndex = CInt(dr("E2_DestMal"))
        txtPorteeE2.Text = dr("E2_Portee").ToString
        txtEchelonE2.Text = dr("E2_Echelon").ToString
        TextBox1.Text = dr("E2_Zero").ToString
        txtTareMaxE2.Text = dr("E2_TareMax").ToString
        txtSeuilGVE2.Text = dr("E2_SeuilGV").ToString
        txtDureeImpE2.Text = dr("E2_DureeImp").ToString
        SetBtn(btnDoubleCmdE2, CBool(dr("E2_DoubleCmd")))
        SetBtn(btnVerifCasLE2, CBool(dr("E2_VerifCas")))
        txtTempsStabE2.Text = dr("E2_TempsStab").ToString
        txtNbEchelonE2.Text = dr("E2_NbEchelon").ToString
        txtTempMaxVidE2.Text = dr("E2_TempMaxVid").ToString
        txtTempsFermeE2.Text = dr("E2_TempsFerme").ToString
        txtSeuilDebitE2.Text = dr("E2_SeuilDebit").ToString
        txtTempsMortE2.Text = dr("E2_TempsMort").ToString
        txtCalD0E2.Text = dr("E2_D0").ToString : txtCalK0E2.Text = dr("E2_K0").ToString
        txtCalD1E2.Text = dr("E2_D1").ToString : txtCalK1E2.Text = dr("E2_K1").ToString
        txtCalD2E2.Text = dr("E2_D2").ToString : txtCalK2E2.Text = dr("E2_K2").ToString
        txtCalD3E2.Text = dr("E2_D3").ToString : txtCalK3E2.Text = dr("E2_K3").ToString
        txtCalD4E2.Text = dr("E2_D4").ToString : txtCalK4E2.Text = dr("E2_K4").ToString

        ' AD (Adjuvant)
        cmbTypesAD.SelectedIndex = CInt(dr("AD_Type"))
        cmbDestMalaxAD.SelectedIndex = CInt(dr("AD_DestMal"))
        txtPorteeAD.Text = dr("AD_Portee").ToString
        txtEchelonAD.Text = dr("AD_Echelon").ToString
        txtZeroAD.Text = dr("AD_Zero").ToString
        txtTareMaxAD.Text = dr("AD_TareMax").ToString
        txtSeuilGVAD.Text = dr("AD_SeuilGV").ToString
        txtDureeImpAD.Text = dr("AD_DureeImp").ToString
        SetBtn(btnDoubleCmdAD, CBool(dr("AD_DoubleCmd")))
        SetBtn(btnVerifCasLAD, CBool(dr("AD_VerifCas")))
        txtTempsStabAD.Text = dr("AD_TempsStab").ToString
        txtNbEchelonAD.Text = dr("AD_NbEchelon").ToString
        txtTempMaxVidAD.Text = dr("AD_TempMaxVid").ToString
        txtTempsFermeAD.Text = dr("AD_TempsFerme").ToString
        txtSeuilDebitAD.Text = dr("AD_SeuilDebit").ToString
        txtTempsMortAD.Text = dr("AD_TempsMort").ToString
        txtCalD0AD.Text = dr("AD_D0").ToString : txtCalK0AD.Text = dr("AD_K0").ToString
        txtCalD1AD.Text = dr("AD_D1").ToString : txtCalK1AD.Text = dr("AD_K1").ToString
        txtCalD2AD.Text = dr("AD_D2").ToString : txtCalK2AD.Text = dr("AD_K2").ToString
        txtCalD3AD.Text = dr("AD_D3").ToString : txtCalK3AD.Text = dr("AD_K3").ToString
        txtCalD4AD.Text = dr("AD_D4").ToString : txtCalK4AD.Text = dr("AD_K4").ToString

        ' AJ (Ajout)
        cmbTypesAJ.SelectedIndex = CInt(dr("AJ_Type"))
        cmbDestMalaxAJ.SelectedIndex = CInt(dr("AJ_DestMal"))
        txtPorteeAJ.Text = dr("AJ_Portee").ToString
        txtEchelonAJ.Text = dr("AJ_Echelon").ToString
        txtZeroAJ.Text = dr("AJ_Zero").ToString
        txtTareMaxAJ.Text = dr("AJ_TareMax").ToString
        txtSeuilGVAJ.Text = dr("AJ_SeuilGV").ToString
        txtDureeImpAJ.Text = dr("AJ_DureeImp").ToString
        SetBtn(Button2, CBool(dr("AJ_DoubleCmd")))
        SetBtn(Button1, CBool(dr("AJ_VerifCas")))
        txtTempsStabAJ.Text = dr("AJ_TempsStab").ToString
        txtNbEchelonAJ.Text = dr("AJ_NbEchelon").ToString
        txtTempMaxVidAJ.Text = dr("AJ_TempMaxVid").ToString
        txtTempsFermeAJ.Text = dr("AJ_TempsFerme").ToString
        txtSeuilDebitAJ.Text = dr("AJ_SeuilDebit").ToString
        txtTempsMortAJ.Text = dr("AJ_TempsMort").ToString
        txtCalD0AJ.Text = dr("AJ_D0").ToString : txtCalK0AJ.Text = dr("AJ_K0").ToString
        txtCalD1AJ.Text = dr("AJ_D1").ToString : txtCalK1AJ.Text = dr("AJ_K1").ToString
        txtCalD2AJ.Text = dr("AJ_D2").ToString : txtCalK2AJ.Text = dr("AJ_K2").ToString
        txtCalD3AJ.Text = dr("AJ_D3").ToString : txtCalK3AJ.Text = dr("AJ_K3").ToString
        txtCalD4AJ.Text = dr("AJ_D4").ToString : txtCalK4AJ.Text = dr("AJ_K4").ToString
    End Sub

    ' ═══════════════════════════════════════════════════════════════
    '  PARAMÈTRES BASCULES POUR UPDATE SQL
    ' ═══════════════════════════════════════════════════════════════
    Private Sub AjouterParamsBascules(cmd As SqlCommand)
        ' GR1
        cmd.Parameters.AddWithValue("@GR1_Type", cmbTypes.SelectedIndex)
        cmd.Parameters.AddWithValue("@GR1_Dest", cmbDestMalax.SelectedIndex)
        cmd.Parameters.AddWithValue("@GR1_Port", D(txtPortee.Text))
        cmd.Parameters.AddWithValue("@GR1_Ech", D(txtEchelon.Text))
        cmd.Parameters.AddWithValue("@GR1_Zer", D(txtZero.Text))
        cmd.Parameters.AddWithValue("@GR1_Tare", D(txtTareMax.Text))
        cmd.Parameters.AddWithValue("@GR1_SGV", D(txtSeuilGV.Text))
        cmd.Parameters.AddWithValue("@GR1_DImp", N(txtDureeImp.Text))
        cmd.Parameters.AddWithValue("@GR1_Dbl", B(btnDoubleCmd))
        cmd.Parameters.AddWithValue("@GR1_Ver", B(btnVerifCas))
        cmd.Parameters.AddWithValue("@GR1_Stab", N(txtTempsStab.Text))
        cmd.Parameters.AddWithValue("@GR1_NbE", N(txtNbEchelon.Text))
        cmd.Parameters.AddWithValue("@GR1_TMaxV", N(txtTempMaxVid.Text))
        cmd.Parameters.AddWithValue("@GR1_TFerm", N(txtTempsFerme.Text))
        cmd.Parameters.AddWithValue("@GR1_SDebit", D(txtSeuilDebit.Text))
        cmd.Parameters.AddWithValue("@GR1_TMort", N(txtTempsMort.Text))
        cmd.Parameters.AddWithValue("@GR1_D0", D(txtCalD0.Text)) : cmd.Parameters.AddWithValue("@GR1_K0", D(txtCalK0.Text))
        cmd.Parameters.AddWithValue("@GR1_D1", D(txtCalD1.Text)) : cmd.Parameters.AddWithValue("@GR1_K1", D(txtCalK1.Text))
        cmd.Parameters.AddWithValue("@GR1_D2", D(txtCalD2.Text)) : cmd.Parameters.AddWithValue("@GR1_K2", D(txtCalK2.Text))
        cmd.Parameters.AddWithValue("@GR1_D3", D(txtCalD3.Text)) : cmd.Parameters.AddWithValue("@GR1_K3", D(txtCalK3.Text))
        cmd.Parameters.AddWithValue("@GR1_D4", D(txtCalD4.Text)) : cmd.Parameters.AddWithValue("@GR1_K4", D(txtCalK4.Text))
        ' GR2
        cmd.Parameters.AddWithValue("@GR2_Type", cmbTypesGR2.SelectedIndex)
        cmd.Parameters.AddWithValue("@GR2_Dest", cmbDestMalaxGR2.SelectedIndex)
        cmd.Parameters.AddWithValue("@GR2_Port", D(txtPorteeGR2.Text))
        cmd.Parameters.AddWithValue("@GR2_Ech", D(txtEchelonGR2.Text))
        cmd.Parameters.AddWithValue("@GR2_Zer", D(txtZeroGR2.Text))
        cmd.Parameters.AddWithValue("@GR2_Tare", D(txtTareMaxGR2.Text))
        cmd.Parameters.AddWithValue("@GR2_SGV", D(txtSeuilGVGR2.Text))
        cmd.Parameters.AddWithValue("@GR2_DImp", N(txtDureeImpGR2.Text))
        cmd.Parameters.AddWithValue("@GR2_Dbl", B(btnDoubleCmdGR2))
        cmd.Parameters.AddWithValue("@GR2_Ver", B(btnVerifCasGR2))
        cmd.Parameters.AddWithValue("@GR2_Stab", N(txtTempsStabGR2.Text))
        cmd.Parameters.AddWithValue("@GR2_NbE", N(txtNbEchelonGR2.Text))
        cmd.Parameters.AddWithValue("@GR2_TMaxV", N(txtTempMaxVidGR2.Text))
        cmd.Parameters.AddWithValue("@GR2_TFerm", N(txtTempsFermeGR2.Text))
        cmd.Parameters.AddWithValue("@GR2_SDebit", D(txtSeuilDebitGR2.Text))
        cmd.Parameters.AddWithValue("@GR2_TMort", N(txtTempsMortGR2.Text))
        cmd.Parameters.AddWithValue("@GR2_D0", D(txtCalD0GR2.Text)) : cmd.Parameters.AddWithValue("@GR2_K0", D(txtCalK0GR2.Text))
        cmd.Parameters.AddWithValue("@GR2_D1", D(txtCalD1GR2.Text)) : cmd.Parameters.AddWithValue("@GR2_K1", D(txtCalK1GR2.Text))
        cmd.Parameters.AddWithValue("@GR2_D2", D(txtCalD2GR2.Text)) : cmd.Parameters.AddWithValue("@GR2_K2", D(txtCalK2GR2.Text))
        cmd.Parameters.AddWithValue("@GR2_D3", D(txtCalD3GR2.Text)) : cmd.Parameters.AddWithValue("@GR2_K3", D(txtCalK3GR2.Text))
        cmd.Parameters.AddWithValue("@GR2_D4", D(txtCalD4GR2.Text)) : cmd.Parameters.AddWithValue("@GR2_K4", D(txtCalK4GR2.Text))
        ' L1
        cmd.Parameters.AddWithValue("@L1_Type", cmbTypesL1.SelectedIndex)
        cmd.Parameters.AddWithValue("@L1_Dest", cmbDestMalaxL1.SelectedIndex)
        cmd.Parameters.AddWithValue("@L1_Port", D(txtPorteeL1.Text))
        cmd.Parameters.AddWithValue("@L1_Ech", D(txtEchelonL1.Text))
        cmd.Parameters.AddWithValue("@L1_Zer", D(txtZeroL1.Text))
        cmd.Parameters.AddWithValue("@L1_Tare", D(txtTareMaxL1.Text))
        cmd.Parameters.AddWithValue("@L1_SGV", D(txtSeuilGVL1.Text))
        cmd.Parameters.AddWithValue("@L1_DImp", N(txtDureeImpL1.Text))
        cmd.Parameters.AddWithValue("@L1_Dbl", B(btnDoubleCmdL1))
        cmd.Parameters.AddWithValue("@L1_Ver", B(btnVerifCasL1))
        cmd.Parameters.AddWithValue("@L1_Stab", N(txtTempsStabL1.Text))
        cmd.Parameters.AddWithValue("@L1_NbE", N(txtNbEchelonL1.Text))
        cmd.Parameters.AddWithValue("@L1_TMaxV", N(txtTempMaxVidL1.Text))
        cmd.Parameters.AddWithValue("@L1_TFerm", N(txtTempsFermeL1.Text))
        cmd.Parameters.AddWithValue("@L1_SDebit", D(txtSeuilDebitL1.Text))
        cmd.Parameters.AddWithValue("@L1_TMort", N(txtTempsMortL1.Text))
        cmd.Parameters.AddWithValue("@L1_D0", D(txtCalD0L1.Text)) : cmd.Parameters.AddWithValue("@L1_K0", D(txtCalK0L1.Text))
        cmd.Parameters.AddWithValue("@L1_D1", D(txtCalD1L1.Text)) : cmd.Parameters.AddWithValue("@L1_K1", D(txtCalK1L1.Text))
        cmd.Parameters.AddWithValue("@L1_D2", D(txtCalD2L1.Text)) : cmd.Parameters.AddWithValue("@L1_K2", D(txtCalK2L1.Text))
        cmd.Parameters.AddWithValue("@L1_D3", D(txtCalD3L1.Text)) : cmd.Parameters.AddWithValue("@L1_K3", D(txtCalK3L1.Text))
        cmd.Parameters.AddWithValue("@L1_D4", D(txtCalD4L1.Text)) : cmd.Parameters.AddWithValue("@L1_K4", D(txtCalK4L1.Text))
        ' L2
        cmd.Parameters.AddWithValue("@L2_Type", cmbTypesL2.SelectedIndex)
        cmd.Parameters.AddWithValue("@L2_Dest", cmbDestMalaxL2.SelectedIndex)
        cmd.Parameters.AddWithValue("@L2_Port", D(txtPorteeL2.Text))
        cmd.Parameters.AddWithValue("@L2_Ech", D(txtEchelonL2.Text))
        cmd.Parameters.AddWithValue("@L2_Zer", D(txtZeroL2.Text))
        cmd.Parameters.AddWithValue("@L2_Tare", D(txtTareMaxL2.Text))
        cmd.Parameters.AddWithValue("@L2_SGV", D(txtSeuilGVL2.Text))
        cmd.Parameters.AddWithValue("@L2_DImp", N(txtDureeImpL2.Text))
        cmd.Parameters.AddWithValue("@L2_Dbl", B(btnDoubleCmdL2))
        cmd.Parameters.AddWithValue("@L2_Ver", B(btnVerifCasL2))
        cmd.Parameters.AddWithValue("@L2_Stab", N(txtTempsStabL2.Text))
        cmd.Parameters.AddWithValue("@L2_NbE", N(txtNbEchelonL2.Text))
        cmd.Parameters.AddWithValue("@L2_TMaxV", N(txtTempMaxVidL2.Text))
        cmd.Parameters.AddWithValue("@L2_TFerm", N(txtTempsFermeL2.Text))
        cmd.Parameters.AddWithValue("@L2_SDebit", D(txtSeuilDebitL2.Text))
        cmd.Parameters.AddWithValue("@L2_TMort", N(txtTempsMortL2.Text))
        cmd.Parameters.AddWithValue("@L2_D0", D(txtCalD0L2.Text)) : cmd.Parameters.AddWithValue("@L2_K0", D(txtCalK0L2.Text))
        cmd.Parameters.AddWithValue("@L2_D1", D(txtCalD1L2.Text)) : cmd.Parameters.AddWithValue("@L2_K1", D(txtCalK1L2.Text))
        cmd.Parameters.AddWithValue("@L2_D2", D(txtCalD2L2.Text)) : cmd.Parameters.AddWithValue("@L2_K2", D(txtCalK2L2.Text))
        cmd.Parameters.AddWithValue("@L2_D3", D(txtCalD3L2.Text)) : cmd.Parameters.AddWithValue("@L2_K3", D(txtCalK3L2.Text))
        cmd.Parameters.AddWithValue("@L2_D4", D(txtCalD4L2.Text)) : cmd.Parameters.AddWithValue("@L2_K4", D(txtCalK4L2.Text))
        ' E1
        cmd.Parameters.AddWithValue("@E1_Type", cmbTypesE1.SelectedIndex)
        cmd.Parameters.AddWithValue("@E1_Dest", cmbDestMalaxE1.SelectedIndex)
        cmd.Parameters.AddWithValue("@E1_Port", D(txtPorteeE1.Text))
        cmd.Parameters.AddWithValue("@E1_Ech", D(txtEchelonE1.Text))
        cmd.Parameters.AddWithValue("@E1_Zer", D(txtZeroE1.Text))
        cmd.Parameters.AddWithValue("@E1_Tare", D(txtTareMaxE1.Text))
        cmd.Parameters.AddWithValue("@E1_SGV", D(txtSeuilGVE1.Text))
        cmd.Parameters.AddWithValue("@E1_DImp", N(txtDureeImpE1.Text))
        cmd.Parameters.AddWithValue("@E1_Dbl", B(btnDoubleCmdE1))
        cmd.Parameters.AddWithValue("@E1_Ver", B(btnVerifCasLE1))
        cmd.Parameters.AddWithValue("@E1_Stab", N(txtTempsStabE1.Text))
        cmd.Parameters.AddWithValue("@E1_NbE", N(txtNbEchelonE1.Text))
        cmd.Parameters.AddWithValue("@E1_TMaxV", N(txtTempMaxVidE1.Text))
        cmd.Parameters.AddWithValue("@E1_TFerm", N(txtTempsFermeE1.Text))
        cmd.Parameters.AddWithValue("@E1_SDebit", D(txtSeuilDebitE1.Text))
        cmd.Parameters.AddWithValue("@E1_TMort", N(txtTempsMortE1.Text))
        cmd.Parameters.AddWithValue("@E1_D0", D(txtCalD0E1.Text)) : cmd.Parameters.AddWithValue("@E1_K0", D(txtCalK0E1.Text))
        cmd.Parameters.AddWithValue("@E1_D1", D(txtCalD1E1.Text)) : cmd.Parameters.AddWithValue("@E1_K1", D(txtCalK1E1.Text))
        cmd.Parameters.AddWithValue("@E1_D2", D(txtCalD2E1.Text)) : cmd.Parameters.AddWithValue("@E1_K2", D(txtCalK2E1.Text))
        cmd.Parameters.AddWithValue("@E1_D3", D(txtCalD3E1.Text)) : cmd.Parameters.AddWithValue("@E1_K3", D(txtCalK3E1.Text))
        cmd.Parameters.AddWithValue("@E1_D4", D(txtCalD4E1.Text)) : cmd.Parameters.AddWithValue("@E1_K4", D(txtCalK4E1.Text))
        ' E2
        cmd.Parameters.AddWithValue("@E2_Type", cmbTypesE2.SelectedIndex)
        cmd.Parameters.AddWithValue("@E2_Dest", cmbDestMalaxE2.SelectedIndex)
        cmd.Parameters.AddWithValue("@E2_Port", D(txtPorteeE2.Text))
        cmd.Parameters.AddWithValue("@E2_Ech", D(txtEchelonE2.Text))
        cmd.Parameters.AddWithValue("@E2_Zer", D(TextBox1.Text))
        cmd.Parameters.AddWithValue("@E2_Tare", D(txtTareMaxE2.Text))
        cmd.Parameters.AddWithValue("@E2_SGV", D(txtSeuilGVE2.Text))
        cmd.Parameters.AddWithValue("@E2_DImp", N(txtDureeImpE2.Text))
        cmd.Parameters.AddWithValue("@E2_Dbl", B(btnDoubleCmdE2))
        cmd.Parameters.AddWithValue("@E2_Ver", B(btnVerifCasLE2))
        cmd.Parameters.AddWithValue("@E2_Stab", N(txtTempsStabE2.Text))
        cmd.Parameters.AddWithValue("@E2_NbE", N(txtNbEchelonE2.Text))
        cmd.Parameters.AddWithValue("@E2_TMaxV", N(txtTempMaxVidE2.Text))
        cmd.Parameters.AddWithValue("@E2_TFerm", N(txtTempsFermeE2.Text))
        cmd.Parameters.AddWithValue("@E2_SDebit", D(txtSeuilDebitE2.Text))
        cmd.Parameters.AddWithValue("@E2_TMort", N(txtTempsMortE2.Text))
        cmd.Parameters.AddWithValue("@E2_D0", D(txtCalD0E2.Text)) : cmd.Parameters.AddWithValue("@E2_K0", D(txtCalK0E2.Text))
        cmd.Parameters.AddWithValue("@E2_D1", D(txtCalD1E2.Text)) : cmd.Parameters.AddWithValue("@E2_K1", D(txtCalK1E2.Text))
        cmd.Parameters.AddWithValue("@E2_D2", D(txtCalD2E2.Text)) : cmd.Parameters.AddWithValue("@E2_K2", D(txtCalK2E2.Text))
        cmd.Parameters.AddWithValue("@E2_D3", D(txtCalD3E2.Text)) : cmd.Parameters.AddWithValue("@E2_K3", D(txtCalK3E2.Text))
        cmd.Parameters.AddWithValue("@E2_D4", D(txtCalD4E2.Text)) : cmd.Parameters.AddWithValue("@E2_K4", D(txtCalK4E2.Text))
        ' AD
        cmd.Parameters.AddWithValue("@AD_Type", cmbTypesAD.SelectedIndex)
        cmd.Parameters.AddWithValue("@AD_Dest", cmbDestMalaxAD.SelectedIndex)
        cmd.Parameters.AddWithValue("@AD_Port", D(txtPorteeAD.Text))
        cmd.Parameters.AddWithValue("@AD_Ech", D(txtEchelonAD.Text))
        cmd.Parameters.AddWithValue("@AD_Zer", D(txtZeroAD.Text))
        cmd.Parameters.AddWithValue("@AD_Tare", D(txtTareMaxAD.Text))
        cmd.Parameters.AddWithValue("@AD_SGV", D(txtSeuilGVAD.Text))
        cmd.Parameters.AddWithValue("@AD_DImp", N(txtDureeImpAD.Text))
        cmd.Parameters.AddWithValue("@AD_Dbl", B(btnDoubleCmdAD))
        cmd.Parameters.AddWithValue("@AD_Ver", B(btnVerifCasLAD))
        cmd.Parameters.AddWithValue("@AD_Stab", N(txtTempsStabAD.Text))
        cmd.Parameters.AddWithValue("@AD_NbE", N(txtNbEchelonAD.Text))
        cmd.Parameters.AddWithValue("@AD_TMaxV", N(txtTempMaxVidAD.Text))
        cmd.Parameters.AddWithValue("@AD_TFerm", N(txtTempsFermeAD.Text))
        cmd.Parameters.AddWithValue("@AD_SDebit", D(txtSeuilDebitAD.Text))
        cmd.Parameters.AddWithValue("@AD_TMort", N(txtTempsMortAD.Text))
        cmd.Parameters.AddWithValue("@AD_D0", D(txtCalD0AD.Text)) : cmd.Parameters.AddWithValue("@AD_K0", D(txtCalK0AD.Text))
        cmd.Parameters.AddWithValue("@AD_D1", D(txtCalD1AD.Text)) : cmd.Parameters.AddWithValue("@AD_K1", D(txtCalK1AD.Text))
        cmd.Parameters.AddWithValue("@AD_D2", D(txtCalD2AD.Text)) : cmd.Parameters.AddWithValue("@AD_K2", D(txtCalK2AD.Text))
        cmd.Parameters.AddWithValue("@AD_D3", D(txtCalD3AD.Text)) : cmd.Parameters.AddWithValue("@AD_K3", D(txtCalK3AD.Text))
        cmd.Parameters.AddWithValue("@AD_D4", D(txtCalD4AD.Text)) : cmd.Parameters.AddWithValue("@AD_K4", D(txtCalK4AD.Text))
        ' AJ
        cmd.Parameters.AddWithValue("@AJ_Type", cmbTypesAJ.SelectedIndex)
        cmd.Parameters.AddWithValue("@AJ_Dest", cmbDestMalaxAJ.SelectedIndex)
        cmd.Parameters.AddWithValue("@AJ_Port", D(txtPorteeAJ.Text))
        cmd.Parameters.AddWithValue("@AJ_Ech", D(txtEchelonAJ.Text))
        cmd.Parameters.AddWithValue("@AJ_Zer", D(txtZeroAJ.Text))
        cmd.Parameters.AddWithValue("@AJ_Tare", D(txtTareMaxAJ.Text))
        cmd.Parameters.AddWithValue("@AJ_SGV", D(txtSeuilGVAJ.Text))
        cmd.Parameters.AddWithValue("@AJ_DImp", N(txtDureeImpAJ.Text))
        cmd.Parameters.AddWithValue("@AJ_Dbl", B(Button2))
        cmd.Parameters.AddWithValue("@AJ_Ver", B(Button1))
        cmd.Parameters.AddWithValue("@AJ_Stab", N(txtTempsStabAJ.Text))
        cmd.Parameters.AddWithValue("@AJ_NbE", N(txtNbEchelonAJ.Text))
        cmd.Parameters.AddWithValue("@AJ_TMaxV", N(txtTempMaxVidAJ.Text))
        cmd.Parameters.AddWithValue("@AJ_TFerm", N(txtTempsFermeAJ.Text))
        cmd.Parameters.AddWithValue("@AJ_SDebit", D(txtSeuilDebitAJ.Text))
        cmd.Parameters.AddWithValue("@AJ_TMort", N(txtTempsMortAJ.Text))
        cmd.Parameters.AddWithValue("@AJ_D0", D(txtCalD0AJ.Text)) : cmd.Parameters.AddWithValue("@AJ_K0", D(txtCalK0AJ.Text))
        cmd.Parameters.AddWithValue("@AJ_D1", D(txtCalD1AJ.Text)) : cmd.Parameters.AddWithValue("@AJ_K1", D(txtCalK1AJ.Text))
        cmd.Parameters.AddWithValue("@AJ_D2", D(txtCalD2AJ.Text)) : cmd.Parameters.AddWithValue("@AJ_K2", D(txtCalK2AJ.Text))
        cmd.Parameters.AddWithValue("@AJ_D3", D(txtCalD3AJ.Text)) : cmd.Parameters.AddWithValue("@AJ_K3", D(txtCalK3AJ.Text))
        cmd.Parameters.AddWithValue("@AJ_D4", D(txtCalD4AJ.Text)) : cmd.Parameters.AddWithValue("@AJ_K4", D(txtCalK4AJ.Text))
    End Sub

    ' ═══════════════════════════════════════════════════════════════
    '  TOGGLES — BASCULES
    ' ═══════════════════════════════════════════════════════════════
    Private Sub btnDoubleCmd_Click(s As Object, e As EventArgs) Handles btnDoubleCmd.Click
        ToggleBtn(btnDoubleCmd)
    End Sub
    Private Sub btnVerifCas_Click(s As Object, e As EventArgs) Handles btnVerifCas.Click
        ToggleBtn(btnVerifCas)
    End Sub
    Private Sub btnDoubleCmdGR2_Click(s As Object, e As EventArgs) Handles btnDoubleCmdGR2.Click
        ToggleBtn(btnDoubleCmdGR2)
    End Sub
    Private Sub btnVerifCasGR2_Click(s As Object, e As EventArgs) Handles btnVerifCasGR2.Click
        ToggleBtn(btnVerifCasGR2)
    End Sub
    Private Sub btnDoubleCmdL1_Click(s As Object, e As EventArgs) Handles btnDoubleCmdL1.Click
        ToggleBtn(btnDoubleCmdL1)
    End Sub
    Private Sub btnVerifCasL1_Click(s As Object, e As EventArgs) Handles btnVerifCasL1.Click
        ToggleBtn(btnVerifCasL1)
    End Sub
    Private Sub btnDoubleCmdL2_Click(s As Object, e As EventArgs) Handles btnDoubleCmdL2.Click
        ToggleBtn(btnDoubleCmdL2)
    End Sub
    Private Sub btnVerifCasL2_Click(s As Object, e As EventArgs) Handles btnVerifCasL2.Click
        ToggleBtn(btnVerifCasL2)
    End Sub
    Private Sub btnDoubleCmdE1_Click(s As Object, e As EventArgs) Handles btnDoubleCmdE1.Click
        ToggleBtn(btnDoubleCmdE1)
    End Sub
    Private Sub btnVerifCasLE1_Click(s As Object, e As EventArgs) Handles btnVerifCasLE1.Click
        ToggleBtn(btnVerifCasLE1)
    End Sub
    Private Sub btnDoubleCmdE2_Click(s As Object, e As EventArgs) Handles btnDoubleCmdE2.Click
        ToggleBtn(btnDoubleCmdE2)
    End Sub
    Private Sub btnVerifCasLE2_Click(s As Object, e As EventArgs) Handles btnVerifCasLE2.Click
        ToggleBtn(btnVerifCasLE2)
    End Sub
    Private Sub btnDoubleCmdAD_Click(s As Object, e As EventArgs) Handles btnDoubleCmdAD.Click
        ToggleBtn(btnDoubleCmdAD)
    End Sub
    Private Sub btnVerifCasLAD_Click(s As Object, e As EventArgs) Handles btnVerifCasLAD.Click
        ToggleBtn(btnVerifCasLAD)
    End Sub
    Private Sub Button1_Click(s As Object, e As EventArgs) Handles Button1.Click
        ToggleBtn(Button1)
    End Sub
    Private Sub Button2_Click(s As Object, e As EventArgs) Handles Button2.Click
        ToggleBtn(Button2)
    End Sub

End Class