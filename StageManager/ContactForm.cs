using System;
using System.Windows.Forms;
using FireSharp.Response;

namespace StageManager
{
    /// <summary>
    /// Formulaire permettant à une entreprise de contacter un étudiant
    /// </summary>
    public partial class ContactForm : Form
    {
        // Données du formulaire
        private readonly Stage stage;
        private readonly User company;

        /// <summary>
        /// Constructeur du formulaire de contact
        /// </summary>
        /// <param name="stage">Le stage contenant les informations de l'étudiant à contacter</param>
        /// <param name="company">L'entreprise qui contacte l'étudiant</param>
        public ContactForm(Stage stage, User company = null)
        {
            this.stage = stage;
            this.company = company;
            InitializeComponent();

            // Configuration des événements
            btnBack.Click += (s, e) => this.Close();
            btnSend.Click += BtnSend_Click;
            
            // Initialisation du formulaire
            InitializeForm();
        }



        /// <summary>
        /// Initialise le contenu des champs du formulaire
        /// </summary>
        private void InitializeForm()
        {
            // Définit le titre du formulaire
            this.Text = $"Stage Manager - Contacter {stage.PrenomEtudiant} {stage.NomEtudiant}";
            
            // Affiche l'email de l'étudiant à contacter
            lblContact.Text = stage.EmailEtudiant;
            
            // Prérempli l'objet du message
            txtSubject.Text = $"Stage - {stage.RaisonSociale}";
            
            // Prérempli le message avec un template
            txtMessage.Text = $"Bonjour {stage.PrenomEtudiant},\n\n" +
                          $"Je suis {stage.NomContact} de {stage.RaisonSociale} et je souhaiterais vous contacter concernant votre stage.\n\n" +
                          "Cordialement,\n" +
                          $"{stage.NomContact}\n{stage.RaisonSociale}";
        }

        /// <summary>
        /// Gère le clic sur le bouton Envoyer
        /// Envoie le message à Firebase
        /// </summary>
        private async void BtnSend_Click(object sender, EventArgs e)
        {
            // Vérifie que les champs sont remplis
            if (string.IsNullOrWhiteSpace(txtSubject.Text))
            {
                MessageBox.Show("Veuillez saisir un objet pour le message.", "Champ requis", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSubject.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtMessage.Text))
            {
                MessageBox.Show("Veuillez saisir un message.", "Champ requis", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMessage.Focus();
                return;
            }

            try
            {
                // Désactive les contrôles pendant l'envoi
                btnSend.Enabled = false;
                btnBack.Enabled = false;
                this.Cursor = Cursors.WaitCursor;

                // Crée l'objet message
                var message = new
                {
                    // Informations sur l'entreprise
                    CompanyId = stage.RaisonSociale.Replace(".", ","),
                    CompanyName = stage.RaisonSociale,

                    // Informations sur l'étudiant
                    StudentId = stage.NomEtudiant + stage.PrenomEtudiant,
                    StudentName = $"{stage.PrenomEtudiant} {stage.NomEtudiant}",

                    // Contenu du message
                    Subject = txtSubject.Text,
                    Content = txtMessage.Text,
                    Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    Status = "sent"
                };

                // Envoie le message à Firebase
                var response = await FirebaseManager.Client.PushAsync("messages", message);

                // Vérifie si l'envoi a réussi
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    MessageBox.Show("Message envoyé avec succès!", "Succès", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Erreur lors de l'envoi du message.", "Erreur", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur: {ex.Message}", "Erreur", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Réactive les contrôles
                btnSend.Enabled = true;
                btnBack.Enabled = true;
                this.Cursor = Cursors.Default;
            }
        }
    }
}
