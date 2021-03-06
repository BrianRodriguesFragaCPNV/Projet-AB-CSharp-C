﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Gestion_alarme
{
    public partial class InterfaceGraphique : Form
    {
        #region Variables
        private bool erreur = false;
        private bool alerte = false;
        int index = -1;
        int indexOfX = -1;
        int nombreV;
        string nbrX;
        string newNbrX;
        string rqSQL = "";
        string connString = "Server=localhost; Database=alarme; Uid=root; Pwd=;";
        Connection_db connection = new Connection_db();
        List<String> statusInter = new List<String>();
        List<String> SDIS = new List<String>();
        List<String> Personnel = new List<String>();
        List<String> Status = new List<String>();
        List<String> TypeInter = new List<String>();
        #endregion Variables

        public InterfaceGraphique()
        {
            InitializeComponent();

            #region Hover
            //met le texte en gris du champ texte SiteSinistre (comme pour un hover en html)
            SiteSinistre.ForeColor = Color.Gray;
            #endregion Hover

            #region Liste intervention
            //ajout des item dans la liste des intervention

            rqSQL = "SELECT Nature, Ampleur from type_sinistre";
            MySqlConnection typeSinistre = new MySqlConnection(connString);
            MySqlCommand rqSinistre = new MySqlCommand(rqSQL, typeSinistre);
            typeSinistre.Open();
            MySqlDataReader readSinistre;
            readSinistre = rqSinistre.ExecuteReader();
            // Appeler le reader avant d'accéder aux données.
            while (readSinistre.Read())
            {
                TypeInter.Add(readSinistre.GetString(0) + " " + readSinistre.GetString(1));
            }
            // Fermer après lecture.
            readSinistre.Close();
            // Fermer la connection après utilisation.
            typeSinistre.Close();


            //foreach pour entrer les valeurs pour les champs depuis la liste
            foreach (string sinistre in TypeInter)
            {
                lstTypeInter.Items.Add(sinistre);
            }

            #endregion Liste intervention

            #region Status d'intervention

            rqSQL = "SELECT Status from alarme_status";
            MySqlConnection statusI = new MySqlConnection(connString);
            MySqlCommand rqStatus = new MySqlCommand(rqSQL, statusI);
            statusI.Open();
            MySqlDataReader readStatus;
            readStatus = rqStatus.ExecuteReader();
            // Always call Read before accessing data.
            while (readStatus.Read())
            {
                statusInter.Add(readStatus.GetString(0));
            }
            // always call Close when done reading.
            readStatus.Close();
            // Close the connection when done with it.
            statusI.Close();

            //foreach pour entrer les valeurs pour les champs depuis la liste
            foreach(string status in statusInter)
            {
                lstInterCourantes.Items.Add(status);
            }

            #endregion Status d'intervention

            #region Liste SDIS
            //ajout des item dans la liste des SDIS
            rqSQL = "SELECT NomCaserne from caserne";
            MySqlConnection caserne = new MySqlConnection(connString);
            MySqlCommand rqCaserne = new MySqlCommand(rqSQL, caserne);
            caserne.Open();
            MySqlDataReader readCaserne;
            readCaserne = rqCaserne.ExecuteReader();
            // Appeler le reader avant d'accéder aux données.
            while (readCaserne.Read())
            {
                SDIS.Add(readCaserne.GetString(0));
            }
            // Fermer après lecture.
            readCaserne.Close();
            // Fermer la connection après utilisation.
            caserne.Close();


            //foreach pour entrer les valeurs pour les champs depuis la liste
            foreach (string nomCaserne in SDIS)
            {
                lstSDIS.Items.Add(nomCaserne);
            }


            #endregion Liste SDIS
        }

        #region Liste des personnes des SDIS
        private void lstSDIS_SelectedIndexChanged(object sender, EventArgs e)
        {
            //afficher la liste des personnes engagées selon le SDIS sélectionné
            lstEngagement.Items.Clear();
            Personnel.Clear();
            string selectionCaserne = lstSDIS.Text;

            //ajout des item dans la liste des SDIS
            rqSQL = "SELECT SPNom, SPPrenom, SPStatus from personnels inner join sp_status on personnels.SP_status_idSP_status = sp_status.idSP_status where Caserne_idCaserne =  (select idCaserne from caserne where NomCaserne = '" + selectionCaserne + "');";
            MySqlConnection personnels = new MySqlConnection(connString);
            MySqlCommand rqPersonnel = new MySqlCommand(rqSQL, personnels);
            personnels.Open();
            MySqlDataReader readPersonnels;
            readPersonnels = rqPersonnel.ExecuteReader();
            // Appeler le reader avant d'accéder aux données.
            while (readPersonnels.Read())
            {
                Personnel.Add(readPersonnels.GetString(1) + " " + readPersonnels.GetString(0) + "\t       (" + readPersonnels.GetString(2) + ")");
                Status.Add(readPersonnels.GetString(2));
            }
            // Fermer après lecture.
            readPersonnels.Close();
            // Fermer la connection après utilisation.
            personnels.Close();
            //foreach pour entrer les valeurs pour les champs depuis la liste
            foreach (string nomPersonnels in Personnel)
            {
                lstEngagement.Items.Add(nomPersonnels);
            }
        }
        #endregion Liste des personnes des SDIS

        #region Véhicule selon type d'intervention
        private void lstTypeInter_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Permet d'afficher les boutons des véhicules selon le type d'intervention séléctionnée
            int idTypeIntervention = -1;
            idTypeIntervention = lstTypeInter.SelectedIndex;
            switch (idTypeIntervention)
            {
                //Feu
                case 0:
                    btnVTonneP1000.Visible = true;
                    btnVTonneP2000.Visible = true;
                    btnVTonneP6000.Visible = true;
                    btnVTransportPM.Visible = true;
                    btnVEchelle25S.Visible = true;
                    btnVEchelle30D.Visible = true;
                    btnVOfficier.Visible = true;
                    btnVMSR.Visible = true;
                    break;
                //INO
                case 1:
                    btnVTonneP1000.Visible = true;
                    btnVTonneP2000.Visible = true;
                    btnVTonneP6000.Visible = true;
                    btnVTransportPM.Visible = true;
                    btnVEchelle25S.Visible = true;
                    btnVEchelle30D.Visible = true;
                    btnVOfficier.Visible = true;
                    btnVMSR.Visible = true;
                    break;
                //POL
                case 2:
                    btnVTonneP1000.Visible = true;
                    btnVTonneP2000.Visible = true;
                    btnVTonneP6000.Visible = true;
                    btnVTransportPM.Visible = true;
                    btnVEchelle25S.Visible = false;
                    btnVEchelle30D.Visible = false;
                    btnVOfficier.Visible = true;
                    btnVMSR.Visible = true;
                    break;
                //REN
                case 3:
                    btnVTonneP1000.Visible = true;
                    btnVTonneP2000.Visible = true;
                    btnVTonneP6000.Visible = true;
                    btnVTransportPM.Visible = true;
                    btnVEchelle25S.Visible = false;
                    btnVEchelle30D.Visible = false;
                    btnVOfficier.Visible = false;
                    btnVMSR.Visible = true;
                    break;
                //SAU
                case 4:
                    btnVTonneP1000.Visible = false;
                    btnVTonneP2000.Visible = false;
                    btnVTonneP6000.Visible = false;
                    btnVTransportPM.Visible = false;
                    btnVEchelle25S.Visible = true;
                    btnVEchelle30D.Visible = true;
                    btnVOfficier.Visible = false;
                    btnVMSR.Visible = false;
                    break;
                //TEC
                case 5:
                    btnVTonneP1000.Visible = true;
                    btnVTonneP2000.Visible = true;
                    btnVTonneP6000.Visible = true;
                    btnVTransportPM.Visible = false;
                    btnVEchelle25S.Visible = false;
                    btnVEchelle30D.Visible = false;
                    btnVOfficier.Visible = false;
                    btnVMSR.Visible = false;
                    break;
                //PIO
                case 6:
                    btnVTonneP1000.Visible = false;
                    btnVTonneP2000.Visible = true;
                    btnVTonneP6000.Visible = true;
                    btnVTransportPM.Visible = true;
                    btnVEchelle25S.Visible = false;
                    btnVEchelle30D.Visible = false;
                    btnVOfficier.Visible = false;
                    btnVMSR.Visible = true;
                    break;
                //CHI
                case 7:
                    btnVTonneP1000.Visible = false;
                    btnVTonneP2000.Visible = true;
                    btnVTonneP6000.Visible = true;
                    btnVTransportPM.Visible = true;
                    btnVEchelle25S.Visible = false;
                    btnVEchelle30D.Visible = false;
                    btnVOfficier.Visible = true;
                    btnVMSR.Visible = true;
                    break;
                //RAD
                case 8:
                    btnVTonneP1000.Visible = true;
                    btnVTonneP2000.Visible = true;
                    btnVTonneP6000.Visible = false;
                    btnVTransportPM.Visible = true;
                    btnVEchelle25S.Visible = false;
                    btnVEchelle30D.Visible = false;
                    btnVOfficier.Visible = true;
                    btnVMSR.Visible = true;
                    break;
                //par défaut
                default:
                    btnVTonneP1000.Visible = false;
                    btnVTonneP2000.Visible = false;
                    btnVTonneP6000.Visible = false;
                    btnVTransportPM.Visible = false;
                    btnVEchelle25S.Visible = false;
                    btnVEchelle30D.Visible = false;
                    btnVMSR.Visible = false;
                    btnVOfficier.Visible = false;
                    break;
            }
        }
        #endregion Véhicule selon type d'intervention

        #region Focus hover
        //Si le focus est sur SiteSinistre
        private void SiteSinistre_Enter(object sender, EventArgs e)
        {
            if (SiteSinistre.Text == "Zone touchée")
            {
                SiteSinistre.Text = "";
            }
            SiteSinistre.ForeColor = Color.Black;
        }

        //Si le focus n'est plus sur SiteSinistre
        private void SiteSinistre_Leave(object sender, EventArgs e)
        {
            //Si il n'y a rien :
            //Met du texte en gris du champ texte SiteSinistre (comme pour un hover en html)
            if (SiteSinistre.Text == "")
            {
                SiteSinistre.Text = "Zone touchée";
                SiteSinistre.ForeColor = Color.Gray;
            }
        }

        private void lstInterCourantes_Enter(object sender, EventArgs e)
        {
            if (lstInterCourantes.Text == "Status de l'intervention")
            {
                lstInterCourantes.Text = "";
            }
            lstInterCourantes.ForeColor = Color.Black;
        }
        #endregion Focus hover

        #region Reset status séléctionné
        private void btnChangeStatus_Click(object sender, EventArgs e)
        {
            if (lstInterCourantes.SelectedIndex >= 0)
            {
                txtStatus.Text = lstInterCourantes.SelectedItem.ToString();
                lstInterCourantes.SelectedIndex = -1;
            }
        }
        #endregion Reset status séléctionné

        #region Clic véhicules

        private void btnVTonneP1000_Click(object sender, EventArgs e)
        {
            VehicleGestion("Tonne Pompe 1000L");
        }

        private void btnVTonneP2000_Click(object sender, EventArgs e)
        {
            VehicleGestion("Tonne Pompe 2000L");
        }

        private void btnVTonneP6000_Click(object sender, EventArgs e)
        {
            VehicleGestion("Tonne Pompe 6000L");
        }

        private void btnVTransportPM_Click(object sender, EventArgs e)
        {
            VehicleGestion("Transport PM");
        }

        private void btnVEchelle25S_Click(object sender, EventArgs e)
        {
            VehicleGestion("Echelle automobile 25M");
        }

        private void btnVEchelle30D_Click(object sender, EventArgs e)
        {
            VehicleGestion("Echelle automobile 30M");
        }

        private void btnVOfficier_Click(object sender, EventArgs e)
        {
            VehicleGestion("Officier");
        }

        private void btnVMSR_Click(object sender, EventArgs e)
        {
            VehicleGestion("Modulaire de secours");
        }
        #endregion Clic véhicules

        #region Quittancer
        private void btnQuittance_Click(object sender, EventArgs e)
        {
            #region Variables local
            //met erreur à false pour vérifier par la suite si il y a des erreurs dans les champs
            erreur = false;
            string rqSQL = "";
            #endregion Variables local

            #region Verification des champs
            //processus de vérification de tout les champs
            if (txtQui.TextLength <= 0) { MessageBox.Show("Le champ 'Qui ?' est vide !", "Erreur ! Champ vide.", MessageBoxButtons.OK, MessageBoxIcon.Error); erreur = true; }
            else if (lstTypeInter.SelectedIndex < 0) { MessageBox.Show("Aucun index n'a été selectionner dans 'Type Intervention' !", "Erreur ! Index non selectionné.", MessageBoxButtons.OK, MessageBoxIcon.Error); erreur = true; }
            else if (txtLieu.TextLength <= 0) { MessageBox.Show("Le champ 'Lieu' est vide !", "Erreur ! Champ vide.", MessageBoxButtons.OK, MessageBoxIcon.Error); erreur = true; }
            else if (txtStatus.Text == "") { MessageBox.Show("Le champ 'Status de l'intervention' est vide ! \nSelectionnez le type de status", "Erreur ! Champ vide.", MessageBoxButtons.OK, MessageBoxIcon.Error); erreur = true; }
            //si un champ est vide, demande à l'utilisateur une confirmation
            if ((SiteSinistre.TextLength <= 0 || SiteSinistre.Text == "Zone touchée") && (erreur == false && alerte == false))
            {
                var SiteSinistreRep = MessageBox.Show("Le champ 'Zone touchée ?' est vide ! Voulez-vous continuez ?", "Champ vide ?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (SiteSinistreRep != DialogResult.Yes) { erreur = true; }
            }
            if (rtxtRemarques.TextLength <= 0 && (erreur == false && alerte == false))
            {
                var rtxtRemarquesRep = MessageBox.Show("Le champ 'Remarque' est vide ! Voulez-vous continuez ?", "Champ vide ?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (rtxtRemarquesRep != DialogResult.Yes) { erreur = true; }
            }
            #endregion Verification des champs

            #region Validation de l'alerte

            #region Si 0 erreur
            //Si il y a aucune erreur, on lance l'alerte
            if (erreur == false && alerte == false)
            {
                //Préparation de la requête voulue
                int idCaserne = lstSDIS.SelectedIndex + 1;
                int idSinistre = lstTypeInter.SelectedIndex + 1;
                int idStatus = GetSelectedStatusId();
                string Qui = txtQui.Text;
                string SQLqui = RemoveSpecialChars(Qui);
                string Zone_touchee = SiteSinistre.Text;
                string SQLZone_touchee = RemoveSpecialChars(Zone_touchee);
                string Lieu = txtLieu.Text;
                string SQLLieu = RemoveSpecialChars(Lieu);
                string remarque = rtxtRemarques.Text;
                string SQLremarque = RemoveSpecialChars(remarque);

                //Sert à envoyer la requête voulue à la classe qui s'en occupe
                rqSQL = "INSERT INTO Alarme (Caserne_idCaserne, Type_sinistre_idType_sinistre, Alarme_status_idAlarme_status, Qui, Zone_touchee, Lieu, Description) VALUES ('" + idCaserne + "','" + idSinistre + "','" + idStatus + "','" + SQLqui + "','" + SQLZone_touchee + "','" + SQLLieu + "','" + SQLremarque + "');";
                MySqlDataAdapter data = new MySqlDataAdapter(rqSQL, connection.conn);
                DataSet Ds = new DataSet();
                Ds.Reset();
                data.Fill(Ds, rqSQL);

                //on verrouille tous les champs necessaire qui ne doivent pas être modifier
                txtQui.Enabled = false;
                lstTypeInter.Enabled = false;
                SiteSinistre.Enabled = false;
                txtLieu.Enabled = false;
                rtxtRemarques.Enabled = false;
                lstSDIS.Enabled = false;
                btnVEchelle25S.Enabled = false;
                btnVEchelle30D.Enabled = false;
                btnVMSR.Enabled = false;
                btnVOfficier.Enabled = false;
                btnVTonneP1000.Enabled = false;
                btnVTonneP2000.Enabled = false;
                btnVTonneP6000.Enabled = false;
                btnVTransportPM.Enabled = false;
                rdbtnAdd.Enabled = false;
                rdbtnRemove.Enabled = false;
                btnQuittance.Text = "Terminer l'intervention";
                alerte = true;

                //Affiche le message que l'alerte à bien été envoyé
                MessageBox.Show("L'alerte a été envoyée !", "Envoyée !", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            #endregion Si 0 erreur

            #region Si alerte actif
            //Si l'alerte est déjà envoyé et qu'il y a aucune erreur dans les champs
            else if (erreur == false && alerte == true)
            {
                //Et si le status est défini sur Terminée ou Annulée, on reactive tout les champs et les remets à zéro pour être prêt à lancer une nouvelle alerte
                if (txtStatus.Text == "Terminée" || txtStatus.Text == "Annulée")
                {
                    //Met à jour dans la base de données le status de l'alerte
                    int idStatus = GetSelectedStatusId();
                    int idAlarme = GetTopIdAlarme();
                    rqSQL = "UPDATE alarme SET Alarme_status_idAlarme_status = "+ idStatus + " WHERE idAlarme = " + idAlarme + ";";
                    MySqlDataAdapter data = new MySqlDataAdapter(rqSQL, connection.conn);
                    DataSet Ds = new DataSet();
                    Ds.Reset();
                    data.Fill(Ds, rqSQL);
                    //Fait apparaitre une fenêtre d'information
                    MessageBox.Show("L'intervention sur les lieux est terminer.", "Terminer !", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //Restore tous les champs
                    txtQui.Enabled = true;
                    txtQui.Text = "";
                    lstTypeInter.Enabled = true;
                    lstTypeInter.Text = "";
                    SiteSinistre.Enabled = true;
                    SiteSinistre.Text = "";
                    txtLieu.Enabled = true;
                    txtLieu.Text = "";
                    rtxtRemarques.Enabled = true;
                    rtxtRemarques.Text = "";
                    lstSDIS.Enabled = true;
                    rtxtRemarques.Text = "";
                    lstTypeInter.SelectedIndex = -1;
                    lstSDIS.SelectedIndex = -1;
                    lstEngagement.Items.Clear();
                    lstVSelection.Items.Clear();
                    txtStatus.Text = "";
                    rdbtnAdd.Enabled = true;
                    rdbtnRemove.Enabled = true;
                    SiteSinistre.Text = "Zone touchée";
                    SiteSinistre.ForeColor = Color.Gray;
                    btnQuittance.Text = "Quittancer";
                    alerte = false;
                }
                //sinon, renvoie un message d'erreur
                else
                {
                    MessageBox.Show("Le status de l'intervention n'est pas terminée ou annulée.", "Erreur !", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            #endregion Si alerte actif

            #endregion Validation de l'alerte
        }
        #endregion Quittancer

        #region Fonctions
        int FindMyStringInList(ListBox lb, string searchString, int startIndex)
        {
            for (int i = startIndex; i < lb.Items.Count; ++i)
            {
                string lbString = lb.Items[i].ToString();
                if (lbString.Contains(searchString))
                    return i;
            }
            return -1;
        }
        void VehicleGestion(string Vehicle)
        {
            index = FindMyStringInList(lstVSelection, Vehicle, 0);
            if (index >= 0)
            {
                nbrX = lstVSelection.Items[index].ToString();
                if (nbrX.Contains("x"))
                {
                    if (rdbtnAdd.Checked)
                    {
                        indexOfX = nbrX.IndexOf("x") + 1;
                        nombreV = Int32.Parse(nbrX.Substring(indexOfX));
                        nombreV++;
                    }
                    if (rdbtnRemove.Checked)
                    {
                        int indexOfX = nbrX.IndexOf("x") + 1;
                        nombreV = Int32.Parse(nbrX.Substring(indexOfX));
                        nombreV--;

                    }
                    if (nombreV > 1)
                    {
                        newNbrX = Vehicle + " x" + nombreV;
                    }
                    else
                    {
                        newNbrX = Vehicle;
                    }
                    lstVSelection.Items[index] = newNbrX;
                }
                else
                {
                    if (rdbtnAdd.Checked)
                    {
                        lstVSelection.Items[index] = (Vehicle + " x2");
                    }
                    if (rdbtnRemove.Checked)
                    {
                        lstVSelection.Items.RemoveAt(index);
                    }
                }
            }
            else if (rdbtnAdd.Checked)
            {
                lstVSelection.Items.Add(Vehicle);
            }
        }
        int GetSelectedStatusId()
        {
            int idStatus = 1;
            foreach (string status in statusInter)
            {
                if (status == txtStatus.Text)
                {
                    break;
                }
                else
                {
                    idStatus++;
                }
            }
            return idStatus;
        }
        int GetTopIdAlarme()
        {
            int TopID = -1;
            rqSQL = "SELECT idAlarme FROM alarme LIMIT 1;";
            MySqlConnection AlerteI = new MySqlConnection(connString);
            MySqlCommand rqStatus = new MySqlCommand(rqSQL, AlerteI);
            AlerteI.Open();
            MySqlDataReader readIdAlerte;
            readIdAlerte = rqStatus.ExecuteReader();
            // Always call Read before accessing data.
            while (readIdAlerte.Read())
            {
                TopID = Int32.Parse(readIdAlerte.GetString(0));
            }
            // always call Close when done reading.
            readIdAlerte.Close();
            // Close the connection when done with it.
            AlerteI.Close();

            return TopID;
        }
        public static string RemoveSpecialChars(string str)
        {
            // Create  a string array and add the special characters you want to remove
            string[] chars = new string[] { "/", "@", "#", "$", "%", "^", "&", "*", "'", "\"", ";", "|", "[", "]" };
            //Iterate the number of times based on the String array length.
            for (int i = 0; i < chars.Length; i++)
            {
                if (str.Contains(chars[i]))
                {
                    str = str.Replace(chars[i], " ");
                }
            }
            return str;
        }
        #endregion Fonctions
    }
}























//ajout des item dans la liste du status d'intervention
//TO DO : à adapter avec la DB
/*rqSQL = "SELECT * from alarme_status";
  //Sert à envoyer la requête voulue à la classe qui s'en occupe
  MySqlDataAdapter data = new MySqlDataAdapter(rqSQL, connection.conn);
  DataSet Ds = new DataSet();
  Ds.Reset();
  data.Fill(Ds, rqSQL);*/










/* Seul link peut vaincre Ganon
'''''''''''''''''''''''''''''''''''''''''''''''#@##';;;;;;;;;;;;;;;;;'####++''''''''''''''''''''''''''''''''''''''''''+'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'''''''''''''''''''''''''''''''''''''''''''''+##;;;;;;;;;;;;;;;;;;;;;;''''##+'+'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'''''''''''''''''''''''''''''''''''''''''''+##;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;+###++''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'''''''''''''''''''''''''''''''''''''''''++++;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;'++#++''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
''''''''''''''''''''''''''''''''''''''''+#';;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;#+++++'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'''''''''''''''''''''''''''''''''''''''#+;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;''##+'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'''''''''''''''''''''''''''''''''''''++';;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;###+'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
''''''''''''''''''''''''''''''''''''+#';;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;##++'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
''''''''''''''''''''''''''''''''''''##;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;##++'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
''''''''''''''''''''''''''''''''''+++;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;'+#+''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
''''''''''''''''''''''''''''''''''##;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;''##''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'''''''''''''''''''''''''''''''''+#;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;'##'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'''''''''''''''''''''''''''''''''#;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;##+'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
''''''';''''''''''''''''''''''''++;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;##+''''''''''''';;;''''''''''''''''''''''''''''''''''''''''''''''''''''''';''''''''''''''''''''''''''
'''''''::::::::::::::::::::::::;#;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;'#+;::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::''''''''''''
'''''''::::::::::::::::::::::::#';;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;#;:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::''''''''''''
''''''':::::::::::::::::::::::;#;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;'##::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::''''''''''''
''''''':::::::::::::::::::::::#;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;'#:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::''''''''''''
'''''''::::::::::::::::::::::;';;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;+;:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::''''''''''''
'''''''::::::::::::::::::::::+;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;+'::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::''''''''''''
'''''''::::::,,,,,,,,,,,,,,,'#;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;''',,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,::::::''''''''''''
'''''''::::::,,,,,,,,,,,,,,,+;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;'#+,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,::::::''''''''''''
''''''':::::::''''''''''''''#;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;+#+''''''''''''''''''''##+++++.,++++++++'';;;'''''''''''''''''''''';,,:::::'''''''''''''
''''''':::::::'''''''''''''+';;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;+#+''''''''''''''''''+'',..,,.,,,,.....''';;;;;;;;'''''''''''''''':,,:::::'''''''''''''
''''''':::::::'''''''''''''#';;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;'++''''''''''''''++''..,,,,,.,,..,,..,..;'';;;;;;;;;;;;;;;;'''''':,,:::::'''''''''''''
''''''':::::::'''''''''''''#;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;'+#+'''''''''''++;..,,,,,,,.,..,,....,.,,;#;;;;;;;;;;;;;;;;;;;;':,::::::'''''''''''''
''''''':::::::'''''''''''''';;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;'#+''''''''+#',,,.,,,,,,,.,,,,,,,,,,,.,,:++;;;;;;;;;;;;;;;;;;;:.,,,:::'''''''''''''
''''''':::::::''''''''''''#';;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;##++''''++,,,,..,,,,,,,.,,,,,,,,,,,,,,..,+;;;;;;;;;;;;;;;;;;,`.,,,,,''';;;'''''''
''''''':::::::''''''''''''#;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;#+#++'++,.,.,..,,,..,,.,,,..,,,,,,,,,..,,';;;;;;;;;;;;;;;;;,`.,,,,,'''::::;;;;;'
''''''':::::::''''''''''''#;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;++@##:,,:;;;;;;;;;;,.,.,,,,,.,,,,,,,..,'';;;;;;;;;;;;;;;;,`.,,,,,''':::::::::;
''''''':::::::''''''''''''';;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;''#++#;;;:#++#++#+++'.,,,,,,,,,,,,...,:+;;;;;;;;;;;;;;;,`.,,,,,'''::::::::::
''''''':::::::'''''''''''+';;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;#',,,,,,##+++++++###,,,,,.,,.,,...,,,+;;;;;;;;;;;;;;,`.,,,,,'''::::::::::
''''''':::::::'''''''''''#';;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;#:,,,,,,,++++++++++###:,.,..,,,.,.,.,';;;;;;;;;;;;;,`.,,,,,'''::::::::::
''''''':::::::''''''''''+#';;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;+#,,,,,,,,,++++++++++##;,:.,,,,......;';;;;;;;;;;;;,`.,,,,,'''::::::::::
''''''':::::::'''''''''+,#;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;'#;,,,,,,,,;+#++++++++###;;;,.,,.,,..'';;;;;;;;;;;,`.,,,,,'''::::::::::
''''''':::::::''''''''',,#;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;#+,,,,,,,,,##++++++++++++###.,.,.,,,;;;;;;;;;;;;,`.,,,,,'''::::::::::
''''''':::::::''''''''#..#;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;'#,,,,,,,,,,;#++++++++++++#+#+:,....,+;;;;;;;;;;,`.,,,,,'''::::::::::
''''''':::::::'''''''+,,,#;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;'+,,,,,,,,,,,#+++++++++++++++##;:.,.:;;;;;;;;;;,`.,,,,,'''::::::::::
''''''':::::::'''''''',.,#;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;#;,,,,,,,,,,+++++++++++++++++##::,,';;;;;;;;;,`.,,,,,'''::::::::::
''''''':::::::''''''+,..,#;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;'#,,,,,,,,,,,'#++++++++++++++++##'.,+;;;;;;;;,`.,,,,,'''::::::::::
''''''':::::::''''''+.,,,#;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;'+,,,,,,,,,,,:#+++++++++++++++++##';;;;;;;;;,`.,,,,,'''::::::::::
''''''':::::::'''''+,.,,,#;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;'+,,,,,,,,,,,:#++++++++++++++++++#++;;;;;;;,`.,,,,,'''::::::::::
''''''':::::::'''''+.,,,,#;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;+;,,,,,,,,,,,+#+++++++++++++++++++##;;;;;;,`.,,,,,'''::::::::::
''''''':::::::''''';...,,#;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;+',,,,,,,,,,,'##++++++++++++++++++##';;;;,`.,,,,,'''::::::::::
''''''':::::::''''+,..,,,:;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;#',,,,,,,,,,,;##++++++++++++++++++##';;;,`.,,,,,'''::::::::::
''''''':::::::''''',,,,,,.;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;'#;,,,,,,,,,,,:'#+++++++++++++++++++#+;;,`.,,,,,'''::::::::::
''''''':::::::'''+,,,,,,,.;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;#:,,,,,,,,,,,,#++++++++++++++++++++#+;,`.,,,,,'''::::::::::
''''''':::::::''''.,,,,,,.#;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;+#:,,,,,,,,,,,;#++++++++++++++++++++#+,`.,,,,,'''::::::::::
''''''':::::::''+;.,,,,,,,#;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;'+,,,,,,,,,,,,++++++++++++++++++++++#'`.,,,,,'''::::::::::
''''''':::::::''#,,,,,,,,,+;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;+;,,,,,,,,,,,:@+++++++++++++++++++++#;.,,,,,''';;;;;;;;;;
''''''':::::::''',,,,,,,,.+';;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;'#,,,,,,,,,,,,'#++++++++++++++++++++++;,,,,,''':;;;;;;;;;
''''''':::::::'';,,,,,,,,,,';;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;'+,,,,,,,,,,,,#++++++++++++++++++++++#+,,,,'''::::::::::
''''''':::::::'+;.,,,,,,,,,';;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;'''''';;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;:,,,,,,,,,,,,#++++++++++++++++++++++@:,,,'''::::::::::
''''''':::::::'#,,,,,,,,,,.#;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;''''''''''';;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;+:,,,,,,,,,,,++++++++++++++++++++++++#;,,'''::::::::::
''''''':::::::'',.,,,,,,,..+;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;'''''''''''''''';;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;#,,,,,,,,,,,,#+++++++++++++++++++++++@,,'''::::::::::
''''''':::::::':,.,,,,,,,,,:';;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;''''''''''''''''''';;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;+,,,,,,,,,,,;+++++++++++++++++++++++#+,'''::::::::::
''''''':::::::':,.,,,,,,,,,,+;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;'''''''''''''''''''''''';;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;#,,,,,,,,,,,,#++++++++++++++++++++++++#'''::::::::::
''''''':::::::#,..,,,,,,,,.,#;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;''''''''''''''''''''''''';;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;,,,,,,,,,,,+#+++++++++++++++++++++++#+''::::::::::
''''''':::::::#,,,,,,,,,,,,,';;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;''''''''''''''''''''''''''';;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;+,,,,,,,,,,,,#++++++++++++++++++++++++#''::::::::::
''''''':::::::#,,,,,,,,,,,,,;';;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;''''''''''''''''''''''''''''';;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;+;;;;;;;;;;;;;',,,,,,,,,,,'++++++++++++++++++++++++##'::::::::::
''''''':::::::;,,,,,,,,,,,,,.';;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;''''''''''''''''''''''''''''''';;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;#####';;;;;;;;;;;#:,,,,,,,,,,,#++++++++++++++++++++++++#+::::::::::
'''''''::::::::,,,,,,,,,,,,,,';;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;'''''''''''''''''''''''''''''''';;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;++;;''';;;;;;;;;;;;;#,,,,,,,,,,,++++++++++++++++++++++++++#::::::::::
'''''''::::::::,,,,,,,,,,,,,,;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;'''''''''''''''''''''''''''''''''';;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;#;;;;;;;;;;;;;;;;;;;;+;,,,,,,,,,,:+++++++++++++++++++++++++#;:::::::::
'''''''::::::::,,,,,,,,,,,..,,+;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;''''''''''''''''''''''''''''''''''';;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;#;;;;;;;;;;;;;;;;;;;;;;;,,,,,,,,,,,++++++++++++++++++++++++++#:::::::::
'''''''::::::::,,,,,...,....,,';;;;;;;;;;;;;;;;;;;;;;;;;;;;;;'''''''''''''''''''''''''''''''''''''';;;;;;;;;;;;;;;;;;;;;;;;;;;;'';;;;;;;;;;;;;;;;;;;;;;;':,,,,,,,,,,;+++++++++++++++++++++++++++,:::::::
'''''''::::::+,,,,,,,,,,,.,,.,:;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;''''''''''''''''''''''''''''''''''''''';;;;;;;;;;;;;;;;;;;;;;;;;;+';;;;;;;;;;;;;;;;;;;;;;;;;+,,,,,,,,,,,#+++++++++++++++++++++++++#':::::::
'''''''::::::#,,,,,,,,,,,,,,,,,+;;;;;;;;;;;;;;;;;;;;;;;;;;;;;'''''''''''''''''''''''''''''''''''''''';;;;;;;;;;;;;;;;;;;;;;;;;#;;;;;;;;;;;;;;;;;;;;;;;;;;#,,,,,,,,,,,+++++++++++++++++++++++++++#:::::::
'''''''::::::@,,,,,,,,,,,,,,,,,+;;;;;;;;;;;;;;;;;;;;;;;;;;;;'''''''''''''''''''''''''''''''''''''''''';;;;;;;;;;;;;;;;;;;;;;;#';;;;;;;;;;;;;;;;;;;;;;;;;;;',,,,,,,,,,'++++++++++++++++++++++++++#+::::::
'''''''::::::+,,,,,,,,,,,,,,,,,;';;;;;;;;;;;;;;;;;;;;;;;;;;;'''''''''''''''''''++++++++++''''''''''''';;;;;;;;;;;;;;;;;;;;;;;+;;;;;;;;;;;;;;;;;;;;;;;;;;;;#,,,,,,,,,,,#++++++++++++++++++++++++++#;:::::
'''''''::::::;;;;;;;;;;;;;;;;;;'#;;;;;;;;;;;;;;;;;;;;;;;;;;;'''''''''''''''''+';;'+#;;;';++'''''''''''';;;;;;;;;;;;;;;;;;;;;+;;;;;;;;;;;;;;;;;;;;;;;;;;;;;';,,,,,,,,,,'++++++++++++++++++++++++++++:::::
'''''''::::::;;;;;;;;;;;;;;;;;;;';;;;;;;;;;;;;;;;;;;;;;;;;;;'''''''''''''''##::,,,#;.,,,,;;++''''''''''';;;;;;;;;;;;;;;;;;;;#;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;',,,,,,,,,,'+++++++++++++++++++++++++++#:::::
''''''':::::::,::::::::::::::::::+;;;;;;;;;;;;;;;;;;;;;;;;;;'''''''''''''''#:.,,,,#,,,,,,,,::##+''''''''';;;;;;;;;;;;;;;;;;;#;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;',,,,,,,,,,:#++++++++++++++++++++++++++#+::::
'''''''::::::::::::::::::::::::::+;;;;;;;;;;;;;;;;;;;;;;;;;'''''''''''''''+...,,,;:,,,,,,,,,,,,:#+'++''''';;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;'+,,,,,,,,,,++++++++++++++++++++++++++++#;:::
''''''':::::::::::::::::::::::::::';;;;;;;;;;;;;;;;;;;;;;;;''''''''''''''#,...,,:+,,,,,,,,,,,,,,,,++#++'''';;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;+,,,,,,,,,,;++++++++++++++++++++++++++++#:;:
''''''':::::::::::::::::::::::::::#;;;;;;;;;;;;;;;;;;;;;;;;'''''''''''''+;,...,,;;,,,,,,,,,,,,,,,,,,#::''++++;;;;;;;;;;;;+++;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;+,,,,,,,,,,;++++++++++++++++++++++++++++#+;;
''''''':::::::::::::::::::::::::::';;;;;;;;;;;;;;;;;;;;;;;;''''''''''''#;.,...,,+,,,,,,,,,,,,,,,,,,,#,,,:;;;@++++++++++++#+#;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;',,,,,,,,,:#++++++++++++++++++++++++++++#::
'''''''::::::::::::::::::::::::::::#;;;;;;;;;;;;;;;;;;;;;;;''''''''''''#,,,...,,+,,,,,,,,,,,,,,,,,,,:,,,,,,,::::::::,,@#'''#;;;;;;;;;;;;####;;;;;;;;;;;;;;;;;+,,,,,,,,,,#+++++++++++++++++++++++++++++;:
'''''''::::::::::::::::::::::::::::+';;;;;;;;;;;;;;;;;;;;;;'''''''''''#,,,,...,';,,,,:::::::,,,,,,,,,,,,,,,,,,,,,,,,:#+''++#;;;;;;;;''#+,,,,'##;;;;;;;;;;;;;;#,,,,,,,,,,++++++++++++++++++++++++++++++#:
''''''':::::::::::::::::::::::::::::#;;;;;;;;;;;;;;;;;;;;;;''''''''''+'.,,,...,+,,,;;##+###':,,,,,,,,,,,,,,,,,,,,,,:#++++++;;;;;;;;''+#;,,,,;+';;;;;;;;;;;;;;;:,,,,,,,,,;+++++++++++++++++++++++++++++#'
''''''':::::::::::::::::::::::::::::';;;;;;;;;;;;;;;;;;;;;;''''''''''#.,.,,...,+,,;#'@++'++#+:,,,,,,,,,,,,,,,,,,,,;##++'''#;;;;;;;+++''#',,:+;;;;;;;;;;;;;;;;;#,,,,,,,,,:++++++++++++++++++++++++++++++#
'''''''::::::::::::::::::::::::::::::+;;;;;;;;;;;;;;;;;;;;;'''''''''#,,,,,,...'',:#,`@++#;+++#,,,,,,,,,,,,,,,,,,,+#;::,,,,:;;;;;;+#+''''#,,#;;;;;;;;;;;;;;;;;;#,,,,,,,,,;++++++++++++++++++++++++++++++@
'''''''::::::::::::::::::::::::::::::+';;;;;;;;;;;;;;;;;;;;''''''''''.,,,,,...',,+.``@++#,#++++,,,,,,,,,,,,,,,,,+#;,,,,,,,#;;;;;#'#+''''':+;;;;;;;;;;;;;;;;;;;#,,,,,,,,,,#+++++++++++++++++++++#####++++
''''''':::::::::::::::::::::::::::::::#;;;;;;;;;;;;;;;;;;;;''''''''+,.,,,,,.,,;,,#```@++###+++#,,,,,,,,,,,,,,,,,+:,,,,,,,,#;;;'+;,;#'''''#;;;;;;;;;;;;;;;;;;;;',,,,,,,,,,#+++++++++++++++++++##+;,;#++++
''''''':::::::::::::::::::::::::::::::+';;;;;;;;;;;;;;;;;;;''''''''+,.,,,,,..+;,,:```#++'##++'#,,,,,,,,,,,,,,,,,,,,,,,,,,:;;;++,,,,#+''''#;;;;;;;;;;;;;;;;;;;;;:,,,,,,,,,#+++++++++++++++++##:,,,,,:#+++
'''''''::::::::::::::::::::::::::::::::#;;;;;;;;;;;;;;;;;;;;''''''';,,,,,,,.;',,,:````#''+'+'#;#,,,,,,,,,,,,,,,,,,,,,,,,,#''',,,,,,,#'''+;;;;;;;;;;;;;;;;;;;;;;#,,,,,,,,,#+++++++++++++###',,,,,,,,'++++
''''''':::::::::::::::::::::::::::::::::#;;;;;;;;;;;;;;;;;;;''''''#,,,,,,,,,#,,,,:'```,#+'+'#'`#,,,,,,,,,,,,,,,,,,,,,,,,,::,,,,,,,,,#'''#;;;;;;;;;;;;;;;;;;;;;;#,,,,,,,,,#++++++++++++##::,,,,,,,,:#++++
''''''':::::::::::::::::::::::::::::::::++;;;;;;;;;;;;;;;;;;''''''',.,,,,,,,;,,,,,#````.#+++;.`,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,;#'#';;;;;;;;;;;;;;;;;;;;;;#,,,,,,,,,;+++++++++###::,,,,,,,,,,#+++++
'''''''::::::::::::::::::::::::::::::::::#;;;;;;;;;;;;;;;;;;'''''';,,,,,,,:+,,,,,,#`````....```.,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,#+@;;;;;;;;;;;;;;;;;;;;;;;#,,,,,,,,,:+++++++###::,,,,,,,,,,,;#+++++
'''''''::::::::::::::::::::::::::::::::::'+;;;;;;;;;;;;;;;;;'''''#,,,,,,,.#,,,,,,,:#```````````.,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,##;;;;;;;;;;;;;;;;;;;;;;;+,,,,,,,,,:+++#+@++,,,,,,,,,,,,,,:#++++++
'''''''::::::::::::::::::::::::::::::::::::+;;;;;;;;;;;;;;;;''''''..,,,,.';,,,,,,,,;'``````````.',,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,';;;;;;;;;;;;;;;;;;;;;;;;;,,,,,,,,,:+##;;;,,,,,,,,,,,',,,:#+++++++
'''''''::::::::::::::::::::::::::::::::::::';;;;;;;;;;;;;;;;''''';..,,,,,#,,,,,,,,,,#``````````.:,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,#;;;;;;;;;;;;;;;;;;;;;;;;;,,,,,,,,::##:,,,,,,,,,,,,,,#,,,#++++++++
''''''':::::::::::::::::::::::::::::::::::::+;;;;;;;;;;;;;;;;'''+,,,,,,,+:,,,,,,,,,,:'..```````.,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,;;;;;;;;;;;;;;;;;;;;;;;;;;,,,,,,,,++::,,,,,,,,,,,,,,+;,,##++++++++
'''''''::::::::::::::::::::::::::::::::::::,,#;;;;;;;;;;;;;;;'''#,,,,,.:+,,,,,,,,,,,,,##,.....,+,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,:;;;;;;;;;;;;;;;;;;;;;;;;;;:,,,,,,#',,,,,,,,,,,,,,,##,,,+++++++++++
'''''''::::::::::::::::::::::::::::::::::::,.+';;;;;;;;;;;;;;'''+,,,,,,#,,,,,,,,,,,,,,..'+''''',,,,,,,,,,,,,,,,:;;;;;;:,,,,,,,,,,,,,,#;;;;;;;;;;;;;;;;;;;;;;;;;;#,,,;'+,,,,,,,,,,,,,,,;#+,,,:#++++++++++
'''''''::::::::::::::::::::::::::::::::::::,.,#;;;;;;;;;;;;;;''':...,.,;,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,'#+++++++;,,,,,,,,,,,,,#;;;;;;;;;;;;;;;;;;;;;;;;;;#,,'':,,,,,,,,,,,,,,:''#,,,,#+++++++++++
'''''''::::::::::::::::::::::::::::::::::::,.,;+;;;;;;;;;;;;;''':...,,+,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,:':#++''##++;,,,,,,,,,,,:';;;;;;;;;;;;;;;;;;;;;;;;;;#,+;,,,,,,,,,,,,,,,++;:;,,,'#+++++++++++
'''''''::::::::::::::::::::::::::::::::::::,.,,;;;;;;;;;;;;;;''':...,:#,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,:+,`#'+++#@#'#+,,,,,,,,,,:;;;;;;;;;;;;;;;;;;;;;;;;;;;#+:,,,,,,,,,,,,,,,+':,+,,,++++++++++++++
'''''''::::::::::::::::::::::::::::::::::::,.,,.#;;;;;;;;;;;;''#....,;,,,,,,,::':::,,,,,,,,,,,,,,,,,,,,,,,;+```'+''#;;#''#:,,,,,,,,,#;;;;;;;;;;;;;;;;;;;;;;;;;;'+,,,,,,,,,,,,,,:+++,,,#,,:#+++++++++++++
'''''''::::::::::::::::::::::::::::::::::::,.,.,.';;;;;;;;;;;''#.,,,,#,,,,,,,#';++';;,,,,,,,,,,,,,,,,,,,,,#````+'++#'@#'++@,,,,,,,,,#;;;;;;;;;;;;;;;;;;;;;;;;;'',,,,,,,,,,,,,;;+:,+,,:',,@++++++++++++++
'''''''::::::::::::::::::::::::::::::::::::,.,.,.;;;;;;;;;;;;''#,,,,,+,,,,,,;',,,,:'';;;,,;;,,,,,,,,,,,,,,#````#++++##@'+++,,,,,,,,,#;;;;;;;;;;;;;;;;;;;;;;;;+',,,,,,,,,,,,,'#',,:;,,,,,;#++++++++++++++
'''''''::::::::::::::::::::::::::::::::::::,.,.,,,+;;;;;;;;;;;';,.,,;:,,,,,,;,,,,,,,,;;;++';,,,,,,,,,,,,,,'````#+++'++++++'+,,,,,,,:';;;;;;;;;;;;;;;;;;;;;;;+;,,,,,,,,,,,,''@;,,,+;,,,,:#+++++++++++++++
'''''''::::::::::::::::::::::::::::::::::::,.,.,.,,';;;;;;;;;;',,,,,;,,,,,,+,,,,,,,,,,,,::,,,,,,,,,,,,,,,,:````.+++++++++++#,,,,,,,:;;;;;;;;;;;;;;;;;;;;;;;#:,,,,,,,,,,,:#:;#,,,,#,,,,,#++++++++++++++++
'''''''::::::::::::::::::::::::::::::::::::,.,.,.,,';;;;;;;;;;#.,.,,#,,,,,,#,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,:,````'+''''++++#',,,,,,,;;;;;;;;;;;;;;;;;;;;;;;#,,,,,,,,,,::+',,#,,,,,;,,,,#+++++++++++++++++
'''''''::::::::::::::::::::::::::::::::::::,.,,,,,,,';;;;;;;;;#,,,.,+,,,,,,',,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,+`````'+'''++++'`,,,,,,,#;;;;;;;;;;;;;;;;;;;;;+;,,,,,,,,,:'+,,,,',,,,':,,,'#+++++++++++++++++
'''''''::::::::::::::::::::::::::::::::::::,.,,,,,,.+;;;;;;;;;#,,..,+,,,,,,;,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,'.`````'+++++#'``,,,,,,,+;;;;;;;;;;;;;;;;;;;;':,,,,,,,,,,#+,,,,',,,,,+,,,,#++++++++++++++++++
'''''''::::::::::::::::::::::::::::::::::::,.,,,,,,,:#;;;;;;;;;,,,..+,,,,,,#,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,:'``````,,'+,,``.+,,,,,,+;;;;;;;;;;;;;;;;;;;+',,,,,,,,,,+;,,,,,#,,,,+',,,#+++++++++++++++++++
'''''''::::::::::::::::::::::::::::::::::::,.,,,,,,,.'+;;;;;;;,,,,.;,,,,,,,#,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,#.``````````````#,,,,,,+;;;;;;;;;;;;;;;;;;;:,,,,,,,,,:+;,,,,,;,.,,:#,,,'#+++++++++++++++++++
'''''''::::::::::::::::::::::::::::::::::::,.,,,,,,,,,#;;;;;;#,,,,,;,,,,,,,,:,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,:',`````````````#,,,,,,+;;;;;;;;;;;;;;;;;;+,,,,,,,,,,#:,,,,,,#,,,,':,,,#++++++++++++++++++++
'''''''::::::::::::::::::::::::::::::::::::,.,,,,,,,,,;+;;;;;#.,,.,;,,,,,,,,',,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,#`````````````#,,,,,,+;;;;;;;;;;;;;;;;;;,,,,,,,,,,:#,,,,,,:+,,,,,,,,+#++++++++++++++++++++
'''''''::::::::::::::::::::::::::::::::::::,.,.,,,,,,,,+';;;;;,,,.,;,,,,,,,,,':,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,';;.`````````;,,,,,,;';;;;;;;;;;;;;;;;;',,,,,,,,,,#,,,,,,,;;,,,,,,,;#+++++++++++++++++++++
'''''''::::::::::::::::::::::::::::::::::::,.,.,,,,,,,,,#;;;#,.,,..;,,,,,,,,,:#,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,:#+.``````.+:,,,,,,;;;;;;;;;;;;;;;;;;#,,,,,,,,,,,:,,,,,,,#,,,,,,,,#++++++++++++++++++++++
'''''''::::::::::::::::::::::::::::::::::::,.,.,,,,,,,,.;;;;@,,,,..;,,,,,,,,,,:+:,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,:++++++++:,,,,,,,;;;;;;;;;;;;;;;;;;#,,,,,,,,,,#,,,,,,,:+,,,,,,,++++++++++++++++++++++++
'''''''::::::::::::::::::::::::::::::::::::,`,.,,,,,,,,,,+;;#,,,,..;,,,,,,,,,,,,++,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,;;;;;;;;;;;;;;;;;;:,,,,,,,,,,#,,,,,,,;,,,,,,,,#+++++++++++++++++++++++
'''''''::::::::::::::::::::::::::::::::::::,`,.,,,,,,,,.,:''..,,,..;,,,,,,,,,,,,,#+:,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,;;;;;;;;;;;;;;;;;#,,,,,,,,,,,#,,,,,,,#,,,,,,,#++++++++++++++++++++++++
'''''''::::::::::::::::::::::::::::::::::::,`,.,,,,,,,,,,,##.,,,,..;,,,,,,,,,,,,,,;;',,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,;;;;;;;;;;;;;;;;;#,,,,,,,,,,,;,,,,,,;+,,,,,,'#++++++++++++++++++++++++
'''''''::::::::::::::::::::::::::::::::::::,`,.,,,,,,,,,.,#:.,.,,..;,,,,,,,,,,,,,,,,#,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,;;;;;;;;;;;;;;;;;;,,,,,,,,,,,,,,,,,,':,,,,,,#+++++++++++++++++++++++++
''''''':::::::::::::::::::::::::::::::::::::.,.,,,,,,,,,,,:.,,,,,..;,,,,,,,,,,,,,,,,:,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,;;;;;;;;;;;;;;;;#,,,,,,,,,,,:,,,,,,,',,,,,,+#+++++++++++++++++++++++++
''''''':::::::::::::::::::::::::::::::::::::.,.,,,,,,,,,,,.,,,,,,..;,,,,,,,,,,,,,,,,,#,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,;;;;;;;;;;;;;;;;#,,,,,,,,,,,:,,,,,,+;,,,,,:#++++++++++++++++++++++++++
''''''':::::::::::::::::::::::::::::::::::::`,.,,,,,,,,,,,,,,,,,,..;,,,,,,,,,,,',,,,,+,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,;';;;;;;;;;;;;;;',,,,,,,,,,,#,,,,,;+,,,,,,#+++++++++++++++++++++++++++
''''''':::::::::::::::::::::::::::::::::::::`,.,,,,,,,,,,,,,,,,,,..;,,,,,,,,,,,;,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,+;;;;;;;;;;;;;',,,,,,,,,,,+;,,,'';,,,,,,'#+++++++++++++++++++++++++++
''''''':::::::::::::::::::::::::::::::::::::`,.,,,,,,,,,,,,,,,,,,,.;;,,,,,,,,,,',,,,,,,,,,,,,,,,,,,,',,,,,,,,,,,,,,,,,,,,,,,,,,,,,,+;;;;;;;;;;;;;;,,,,,,,,,,,+,,+';;,,,,,,,@++++++++++++++++++++++++++++
''''''':::::::::::::::::::::::::::::::::::::`,.,,,,,,,,,,,,,,,,,,,.,+,,,,,,,,,,',,,,,,,,,,,,,,,,,,,,#,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,+;;;;;;;;;;;;;+,,,,,,,,,:';,+::,,,,,,,,:+++++++++++++++++++++++++++++
''''''':::::::::::::::::::::::::::::::::::::`,.,,,,,,,,,,,,,,,,,,,.,+,,,,,,,,,,,,:,,,,,,,,,,,,,,,,,,#,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,+;;;;;;;;;;;;;#,,,,,,:'+',,#,,,,,,,,,,:#+++++++++++++++++++++++++++++
''''''':::::::::::::::::::::::::::::::::::::`,.,,,,,,,,,,,,,,,,,,,.,#,,,,,,,,,,,::;,,,,,,,,,,,,,,,,,#,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,+;;;;;;;;;;;;;#,,,,,:+:,,,'',,,,,,,,,,@++++++++++++++++++++++++++++++
''''''':::::::::::::::::::::::::::::::::::::`,.,,,,,,,,,,,,,.,...,.,',,,,,,,,,,,,:::,,,,,,,,,,,,,,,,#,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,';;;;;;;;;;;;;#,,,,,+:,,,,#,,,,,,,,,:'#++++++++++++++++++++++++++++++
''''''':::::::::::::::::::::::::::::::::::::.,.,,,,,,,,,,,,,..,,,,,,:,,,,,,,,,,,,:`;:,,,,,,,,,,,,,,,;:,,,,,,,,,,,,,,,,,,,,,,,,,,,,,:;;;;;;;;;;;;;',,,,'',,,,:#,,,,,,,,;+#+++++++++++++++++++++++++++++++
''''''':::::::::::::::::::::::::::::::::::::.,.,,,,,,,,,,,,,,,,,,,,,,+,,,,,,,,,,,:`.;:,,,,,,,,,,,,,,,:,,,,,,,,,,,,,,,,,,,,,,,,,,,,,:;;;;;;;;;;;;;;,,,,#,,,,,;:,,,,,,'##+++++++++++++++++++++++++++++++++
'''''''::::::::::::::::::::::::::::::::::::,.,,,,,,,,,,,,,,,,,,,,,,,,+,,,,,,,,,,::```:;,,,,,,,,,,,,,,#,,,,,,,,,,,,,,,,,,,,,,,,,,,,,:';;;;;;;;;;;;;,,,::,,,,:+,,,,:###+++++++++++++++++++++++++++++++++++
''''''':::::::,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,.,,,,,,,,,,,,,,,,,,,,,,,,#,,,,,,,,,,::```.,;:,,,,,,,,,,,,#,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,#;;;;;;;;;;;;;,,,#,,,,,;;,,,;###++++++++++++++++++++++++++++++++++++
''''''':::::::,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,.,,,,,,,,,,,,,,,,,,,,,,,,+,,,,,,,,,,::`````,:::,,,,,,,,,,;:,,,,,,,,,,,,,,,,,,,,,,,,,,,,,#;;;;;;;;;;;;;,,,#,,,,,',,,,@+'#++++++++++++++++++++++++++++++++++++
'''''''::::::'''''''''''''''''''''''''''''''.'''''''''''''''''''''''''.,,,,,,,,,::.``````.;::,,,,,,,,,',,,,,,,,,,,,,,,,,,,,,,,,,,,,,#;;;;;;;;;;;;;,,,#,,,,;+,,,,+'''++++++++++++++++++++++++++++++++++++
'''''''::::::;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;:.:;;;;;;;;;;;;;;;;;;;:;;;;+,,,,,,,,,::.```````..::;:,,,,,,,;,,,,,,,,,,,,,,,,,,,,,,,,,,,,#;;;;;;;;;;;;;,,,,:,,:':,,,#''''++++++++++++++++++++++++++++++++++++
'''''''::::::;::::::::::::::::::::::::::::::.,::::::::::::::::::::::::#,,,,,,,,,:''`````````..::::;,,,:;:,,,,,,,,,,,,,,,,,,,,,,,,,,,:;;;;;;;;;;;;;,,,,#,:+;,,,,#''''++++++++++++++++++++++++++++++++++++
'''''''::::::;::::::::::::::::::::::::::::::.,::::::::::::::::::::::::#,,,,,,,,,,'+.``````````````::::;,;,,,,,,,,,,,,,,,,,,,,,,,,,,,,;;;;;;;;;;;;;,,,,+;'#,,,,:#''''++++++++++++++++++++++++++++++++++++
'''''''::::::;::::::::::::::::::::::::::::::`,::::::::::::::::::::::::',,,,,,,,,,+++:````````````````.,,',,,,,,,,,,,,,,,,,,,,,,,,,,,,#;;;;;;;;;;;;#,,,,;:,,,,,''''''++++++++++++++++++++++++++++++++++++
'''''''::::::;::::::::::::::::::::::::::::::`,:::::::::::::::::::::::::,,,,,,,,,,++++;``````````````.:,,'',,,,,,,,,,,,,,,,,,,,,,,,,,,#;;;;;;;;;;;;#,,,,,,,,,,:#'''''++++++++++++++++++++++++++++++++++++
'''''''::::::;::::::::::::::::::::::::::::::`,:::::::::::::::::::::::::+,,,,,,,,:+++++'..````````````:,,,',,,,,,,,,,,,,,,,,,,,,,,,,,,#;;;;;;;;;;;;#,,,,,,,,,,:'''''+++++++++++++++++++++++++++++++++++++
'''''''::::::;::::::::::::::::::::::::::::::`,:::::::::::::::::::::::::#,,,,,,,,:++++++''..``````````;,,,',,,,,,,,,,,,,,,,,,,,,,,,,,,:;;;;;;;;;;;;#,,,,,,,,,,+'''''#++++++++++++++++++++++++++++++++++++
'''''''::::::;::::::::::::::::::::::::::::::`,:::::::::::::::::::::::::#,,,,,,,,;++++++++++,..``````.:,,,',,,,,,,,,,,,,,,,,,,,,,,,,,,,;;;;;;;;;;;;#,,,,,,,,,;+'''''#++++++++++++++++++++++++++++++++++++
'''''''::::::;::::::::::::::::::::::::::::::`,:::::::::::::::::::::::::#,,,,,,,,;+++++++++++++;;;;;;;,,,,:,,,,,,,,,,,,,,,,,,,,,,,,,,,,+;;;;;;;;;;;',,,,,,,,:#;'''''#++++++++++++++++++++++++++++++++++++
'''''''::::::;::::::::::::::::::::::::::::::`,:::::::::::::::::::::::::;,,,,,,,,'+++++++++++++++++++;,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,#;;;;;;;;;;;;,,,,,,,,+'''''''#++++++++++++++++++++++++++++++++++++
'''''''::::::;::::::::::::::::::::::::::::::`,:::::::::::::::::::::::::;,,,,,,,:'+++++++++++++++++++:,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,#;;;;;;;;;;;;+,,,,,,;+'''''''#++++++++++++++++++++++++++++++++++++
'''''''::::::;::::::::::::::::::::::::::::::`,:::::::::::::::::::::::::;,,,,,,,:'++++++++++++++++++;,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,#;;;;;;;;;;;;#,,,,,,+''''''''#++++++++++++++++++++++++++++++++++++
'''''''::::::;::::::::::::::::::::::::::::::`,:::::::::::::::::::::::::;;,,,,,,:+++++++++++++++++++:,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,#;;;;;;;;;;;;#,,,,:#'''''''''+#+++++++++++++++++++++++++++++++++++
'''''''::::::;::::::::::::::::::::::::::::::`,::::::::::::::::::::::::::#,,,,,,:++++++++++++++++++',,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,#;;;;;;;;;;;;#,,,;#'''''''''''#+++++++++++++++++++++++++++++++++++
'''''''::::::;::::::::::::::::::::::::::::::`,::::::::::::::::::::::::::@,,,,,,;++++++++++++++++++;,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,#;;;;;;;;;;;;',,'#''''''''''''++++++++++++++++++++++++++++++++++++
'''''''::::::;::::::::::::::::::::::::::::::`,::::::::::::::::::::::::::@,,,,,,;++:::::+++++++++++,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,#;;;;;;;;;;;;;++#'''''''''''''++++++++++++++++++++++++++++++++++++
'''''''::::::;::::::::::::::::::::::::::::::.,::::::::::::::::::::::::::#,,,,,,';,,,,,,:'++++++++;,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,#;;;;;;;;;;;;;#'''''''''''''''#+++++++++++++++++++++++++++++++++++
'''''''::::::;::::::::::::::::::::::::::::::.,:::::::::::::::::::::::::::,,,,,::,,,,,;;;,'''''++':,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,#;;;;;;;;;;;;;@'''''''''''''''#+++++++++++++++++++++++++++++++++++
'''''''::::::;::::::::::::::::::::::::::::::.,:::::::::::::::::::::::::::,,,,,::,,,,;;;,,,,,,:;+;,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,#;;;;;;;;;;;;;#'''''''''''''''#+++++++++++++++++++++++++++++++++++
'''''''::::::;::::::::::::::::::::::::::::::.,:::::::::::::::::::::::::::',,,,::,,,,:,,,,,,,,,,',,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,';;;;;;;;;;;;;#'''''''''''''''+#++++++++++++++++++++++++++++++++++
'''''''::::::;::::::::::::::::::::::::::::::.,:::::::::::::::::::::::::::#,,,,::,,,,,,,,,,,,,,::,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,:;;;;;;;;;;;;;;'''''''''''''''''#++++++++++++++++++++++++++++++++++
'''''''::::::;::::::::::::::::::::::::::::::.,:::::::::::::::::::::::::::#,,,,::,,,,,,,,,,,,,::,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,#;;;;;;;;;;;;;;;''''''''''''''''+#+++++++++++++++++++++++++++++++++
'''''''::::::;::::::::::::::::::::::::::::::.,:::::::::::::::::::::::::::#,,,,,;,,,,,,,,,,,,::,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,#;;;;;;;;;;;;;;;'''''''''''''''''#+++++++++++++++++++++++++++++++++
'''''''::::::;::::::::::::::::::::::::::::::.,:::::::::::::::::::::::::::#,,,,,::,,,,,,,,,,:;,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,#;;;;;;;;;;;;;;;+''''''''''''''''+#++++++++++++++++++++++++++++++++
'''''''::::::;::::::::::::::::::::::::::::::.,:::::::::::::::::::::::::::#,,,,,,::,,,,,,,,::,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,:';;;;;;;;;;;;;;;#'''''''''''''''''##++++++++++++++++++++++++++++++'
'''''''::::::;::::::::::::::::::::::::::::::.,::::::::;;;;;;;;::::::::::::,,,,,,,;;;:,,,;:,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,#;;;;;;;;;;;;;;;;#''''''''''''''''''#++++++++++++++++++++++++++++++:
'''''''::::::;::::::::::::::::::::::::::::::.,:::::;;;########';;:::::::::,,,,,,,,,::;;::,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,#;;;;;;;;;;;;;;;;#''''''''''''''''''+#+++++++++++++++++++++++++++++:
'''''''::::::;::::::::::::::::::::::::::::::.,:::;;+###+++++++###;:::::::;,,,,,:,,,,,;;,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,',,,,,,,,,,,,,:+;;;;;;;;;;;;;;;;#'''''''''''''''''''#+++++++++++++++++++++++++++++:
'''''''::::::;::::::::::::::::::::::::::::::.,:''###++++++++++++##+':::::#,,,,,:,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,:+,,,,,,,,,,,,,+;;;;;;;;;;;;;;;;;;''''''''''''''''''''+#++++++++++#++++++++++++++++:
'''''''::::::;::::::::::::::::::::::::::::::.,#+#++++++++++++++++++#';::#;:,,,,,::,,,,::,,,,,,,,,,,,,,,,,,,,,,,,,,,';,,,,,,,,,,,,,,#;;;;;;;;;;;;;;;;;;''''''''''''''''''''''##++++++###++++++++++++++++:
'''''''::::::;::::::::::::::::::::::::::::::.:##+++++++++++++++++++++':;#;+,,,,,,,:::::,,,,,,,,,,,,,,,,,,,,,,,,,,,,#,,,,,,,,,,,,,,;+;;;;;;;;;;;;;;;;;;'''''''''''''''''''''''@#######++++++++++++++++++:
'''''''::::::;:::::::::::::::::::::::::::::::++#+++++++++++++++++++++++#;;#,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,';,,,,,,,,,,,,,,;';;;;;;;;;;;;;;;;;;#''''''''''''''''''''''##++++++++++++++++++++++++:
'''''''::::::;:::::::::::::::::::::::::::::'#+#+++++++++++++++++++++++#+;;#,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,:#:,,,,,,,,,,,,,,,#;;;;;;;;;;;;;;;;;;;#'''''''''''''''''''''''#++++++++++++++++++++++++,
'''''''::::::;::::::::::::::::::::::::::::'#++++++++++++++++++++++++++#;;;',,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,'#;,,,,,,,,,,,,,,,;';;;;;;;;;;;;;;;;;;;#''''''''''''''''''''''''+#+++++++++++++++++++++',
'''''''::::::;:::::::::::::::::::::::::::'#++#+++++++++++++++++++++++#';;;;+,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,##;,,,,,,,,,,,,,,,,#;;;;;;;;;;;;;;;;;;;;#'''''''''''''''''''''''''##++++++++++++++++++++::
'''''''::::::;::::::::::::::::::::::::::'+++##+++++++++++++++++++++++#;;;;;#,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,+#;,,,,,,,,,,,,,,,,,+;;;;;;;;;;;;;;;;;;;;#''''''''''''''''''''''''''##+++++++++++++++++++::
'''''''::::::;:::::::::::::::::::::::::;#+++#+++++++++++++++++++++++#;;;;;;#:,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,:++,,,,,,,,,,,,,,,,,,'';;;;;;;;;;;;;;;;;;;;;'''''''''''''''''''''''''''#+++++++++++++++++++::
'''''''::::::;::::::::::::::::::::::::;+++++#++++++++++++++++++++++#';;;;;;;;,,,,,,,,,,,,,,,,,,,,,,,,,,,,:'+,,,,,,,,,,,,,,,,,,,,#;;;;;;;;;;;;;;;;;;;;;;'''''''''''''''''''''''''''+#++++++++++++++++++::
'''''''::::::;::::::::::::::::::::::::#++++##+++++++++++++++++++++#';;;;;;;;#:,,,,,,,,,,,,,,,,,,,,,,,,,,'+',,,,,,,,,,,,,,,,,,,,;';;;;;;;;;;;;;;;;;;;;;;+'''''''''''''''''''''''''''#++++++++++++++++++::
'''''''::::::;:::::::::::::::::::::::+#++++#+++++++++++++++++++++#+;;;;;;;;;'+,,,,,,,,,,,,,,,,,,,,,,,,,'';,,,,,,,,,,,,,,,,,,,,,#';;;;;;;;;;;;;;;;;;;;;;#'''''''''''''''''''''''''''+++++++++++++++++++;:
'''''''::::::;:::::::::::::::::::::::#++++##++++++++++++++++++++#+;;;;;;;;;;;#;,,,,,,,,,,,,,,,,,,,,,,'+;,,,,,,,,,,,,,,,,,,,,,,,';;;;;;;;;;;;;;;;;;;;;;;#''''''''''''''''''''''''''''#++++++++++++++++++:
'''''''::::::;::::::::::::::::::::::#+++++#+++++++++++++++++++++#;;;;;;;;;;;;;+:,,,,,,,,,,,,,,,,,,::##,,,,,,,,,,,,,,,,,,,,,,,,+';;;;;;;;;;;;;;;;;;;;;;;#''''''''''''''''''''''''''''++++++++++++++++++#:
'''''''::::::;:::::::::::::::::::::;#+++++#++++++++++++++++++++#;;;;;;;;;;;;;;;+;:,,,,,,,,,,,,,,;;++,,,,,,,,,,,,,,,,,,,,,,,,,,#;;;;;;;;;;;;;;;;;;;;;;;;#++'''''''''''''''''''''''''''#++++++++++++++++#:
''''''':;;;;;;,,,,,,,,,,,,,,,,,,,,,#+++++#++++++++++++++++++++#+;;;;;;;;;;;;;;;;+#',,,,,,,,,,'';'',,,,,,,,,,,,,,,,,,,,,,,,,,,;';;;;;;;;;;;;;;;;;;;;;;;;'##+++''''''''''''''''''''''''#++++++++++++++++#;
''''''++'''''+,...................'#+++++#+++++++++++++++++++++;;;;;;;;;;;;;;;;;;'+'+,,,,;'+'+;;,,,,,,,,,,,,,,,,,,,,,,,,,,,,,#';;;;;;;;;;;;;;;;;;;;;;;;;++###+##+++++++''''''''''''''+++++++++++++++++@;
''''''+;:;'''':,,,,,,,,,,,,,,,,,,:#+++++##+++++++++++++++++++#;;;;;;;;;;;;;;;;;;;;;''+###++++,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,';;;;;;;;;;;;;;;;;;;;;;;;;;#+++++++++++++##########''''''#+++++++++++++++#:
''''''+::::::::,,,,,,,,,,,,,,,,,,#++++++#+++++++++++++++++++#;;;;;;;;;;;;;;;;;;;;;;;;;;;++++#;,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,#;;;;;;;;;;;;;;;;;;;;;;;;;;;+++++++++++++++++++++++#####''#++++++++++++++++;
''''''+:::::::::,,,,,,,,,,,,,,,,,#++++++#++++++++++++++++++++;;;;;;;;;;;;;;;;;;;;;;;;;;;+++++#;;,,,,,,,,,,,,,,,,,,,,,,,,,,,,#;;;;;;;;;;;;;;;;;;;;;;;;;;;#++++++++++++++++++++###+++#####+++++++++++++++'
''''''+::::::::::,,,,,,,,,,,,,,,'#++++++#++++++++++++++++++#;;;;;;;;;;;;;;;;;;;;;;;;;;;'++++++##:,,,,,,,,,,,,,,,,,,,,,,,,,,,+;;;;;;;;;;;;;;;;;;;;;;;;;;;'#++++++++++++++++++###++++++++###+++++++++++++#
''''''+::::::::::,,,,,,,,,,,,,,:#++++++++++++++++++++++++++#;;;;;;;;;;;;;;;;;;;;;;;;;;;#++++++++#+,,,,,,,,,,,,,,,,,,,,,,,,,+;;;;;;;;;;;;;;;;;;;;;;;;;;;;;#++++++++++++++++++#+++++++++++###++++++++++++@
''''''+:::::::::::,,,,,,,,,,,,,;+++++++#++++++++++++++++++#;;;;;;;;;;;;;;;;;;;;;;;;;;;;#++++++++++#:,,,,,,,,,,,,,,,,,,,,,,,#;;;;;;;;;;;;;;;;;;;;;;;;;;;;;#+++++++++++++++++##+++++++++++++##+++++++++++#
''''''+::::::::::::,,,,,,,,,,,,#+++++++#+++++++++++++++++##;;;;;;;;;;;;;;;;;;;;;;;;;;;;#+++++++++++#:,,,,,,,,,,,,,,,,,,:::##;;;;;;;;;;;;;;;;;;;;;;;;;;;;;#++++++++++++++++##++++++++++++++++##+++++++++#
''''''+::::::::::::,,,,,,,,,,,;#+++++++#+++++++++++++++++#;;;;;;;;;;;;;;;;;;;;;;;;;;;;;#++++++++++++#,,,:::::::,,,,::::###+;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;#+++++++++++++++##++++++++++++++++++##+++++++++
''''''+:::::::::::::,,,,,,,,,,+++++++++#++++++++++++++++#';;;;;;;;;;;;;;;;;;;;;;;;;;;;;#++++++++++++@,,,#######''''#####++#;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;++++++++++++++++#++++++++++++++++++++##++++++++
''''''+++++++++++###++#++##++##+++++++++++++++++++++++++#;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;+++++++++++++@,,,#+++++++++++++++++#;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;+++++++++++++++#++++++++++++++++++++++###++++++
'''''''++++++++++++++++++++++#+++++++++++++++++++++++++#+;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;+++++++++++++@,,;++++++++++++++++++;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;++++++++++++++##++++++++++++++++++++++++#++++++
''''''''''''''''''''''''''''+#++++++++#++++++++++++++++#;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;'+++++++++++++#,,'+++++++++++++++++#;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;+#++++++++++++##++++++++++++++++++++++++##+++++
''''''''''''''''''''''''''''#+++++++++#+++++++++++++++#+;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;#+++++++++++++,,,'+++++++++++++++++#;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;#++++++++++++#+++++++++++++++++++++++++++##+++
'''''''''''''''''''''''';;;;#+++++++++#+++++++++++++++#;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;#+++++++++++++,,,#+++++++++++++++++#;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;#+++++++++++#++++++++++++++++++++++++++++##+++
'''''''''''''''''''''''':::'#+++++++++#++++++++++++++#+;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;#+++++++++++++,,,#+++++++++++++++++';;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;#+++++++++++#+++++++++++++++++++++++++++++##++
'''''''''''''''''''''''':::#++++++++++#++++++++++++++#';;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;++++++#+++++++,,,+++++++++++++++++#;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;#++++++++++##++++++++++++++++++++++++++++++#++
''''''''''''''''''''''''::;++++++++++++++++++++++++++#;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;'++++++#+++++++,,+#++++++++++++++++#;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;+++++++++++#+++++++++++++++++++++++++++++++##+
'''''''''''''''''''''''';:+++++++++++#++++++++++++++#';;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;#++++++++++++++,,+++++++++++++++++++;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;+++++++++++#++++++++++++++++++++++++++++++++##
'''''''''''''''''''''''';;#++++++++++#++++++++++++++#;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;@+++++++#+++++#,,++++++++++++++++++;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;++++++++++#++++++++++++++++++++++++++++++++++#
'''''''''''''''''''''''';;+++++++++++@+++++++++++++++;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;@+++++++#+++++@,,#++++++++++++++++#;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;+#++++++++#+++++++++++++++++++++++++++++++++++
'''''''''''''''''''''''';++++++++++++#+++++++++++++#';;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;#+++++++##++++@,,#+++++++#++++++++#;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;#+++++++##+++++++++++++++++++++++++++++++++++
'''''''''''''''''''''''';#+++++++++++@+++++++++++++#;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;#+++++++++#++++@,,+++++++##++++++++#;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;#+++++++#++++++++++++++++++++++++++++++++++++
'''''''''''''''''''''''';#+++++++++++#+++++++++++++#;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;@+++++++++@++++@,:++++++#@++++++++#+;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;#+++++++#++++++++++++++++++++++++++++++++++++
'''''''''''''''''''''''';+++++++++++++++++++++++++++;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;#+++++++++#++++@,+++++++@#++++++++#;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;#+++++++#++++++++++++++++++++++++++++++++++++
*/
