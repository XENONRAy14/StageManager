using System;
using System.Windows.Forms;
using System.Drawing;

namespace StageManager
{
    public partial class StageDetailsForm : Form
    {
        private Stage stage;

        public StageDetailsForm(Stage stage)
        {
            this.stage = stage;
            InitializeComponent();
            
            // Configuration des événements
            btnBack.Click += (s, e) => this.Close();
            btnContact.Click += (s, e) => new ContactForm(stage).ShowDialog();
            
            // Remplir les champs avec les informations du stage
            PopulateFields();
        }

        private void PopulateFields()
        {
            // Données de l'étudiant
            lblNomPrenom.Text = $"{stage.PrenomEtudiant} {stage.NomEtudiant}".ToUpper();
            lblClasse.Text = stage.Classe;
            
            // Données de l'entreprise
            lblRaisonSociale.Text = stage.RaisonSociale;
            lblVille.Text = stage.Ville;
            lblContact.Text = $"{stage.PrenomContact} {stage.NomContact}";
            lblTelephoneContact.Text = stage.TelephoneContact;
            lblEmailContact.Text = stage.EmailContact;
            
            // Données de période
            lblPeriode.Text = stage.Periode;
            lblAnnee.Text = stage.Annee;
            
            // Mise à jour du titre
            this.Text = $"Stage Manager - Détails du stage de {stage.PrenomEtudiant} {stage.NomEtudiant}";
        }
    }
}
