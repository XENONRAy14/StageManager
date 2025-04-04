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
        private readonly Student student;
        private readonly User company;

        // Contrôles de l'interface
        private TextBox txtSubject;
        private TextBox txtMessage;
        private Button btnSend;
        private Button btnCancel;
        private Label lblSubject;
        private Label lblMessage;

        /// <summary>
        /// Constructeur du formulaire de contact
        /// </summary>
        /// <param name="student">L'étudiant à contacter</param>
        /// <param name="company">L'entreprise qui envoie le message</param>
        public ContactForm(Student student, User company)
        {
            this.student = student;
            this.company = company;
            InitializeComponent();
            InitializeForm();
        }

        /// <summary>
        /// Initialise les composants de l'interface utilisateur
        /// </summary>
        private void InitializeComponent()
        {
            // Création des contrôles
            txtSubject = new TextBox();
            txtMessage = new TextBox();
            btnSend = new Button();
            btnCancel = new Button();
            lblSubject = new Label();
            lblMessage = new Label();

            // Configuration des labels
            lblSubject.AutoSize = true;
            lblSubject.Location = new System.Drawing.Point(12, 9);
            lblSubject.Text = "Objet :";

            lblMessage.AutoSize = true;
            lblMessage.Location = new System.Drawing.Point(12, 48);
            lblMessage.Text = "Message :";

            // Configuration du champ Objet
            txtSubject.Location = new System.Drawing.Point(12, 25);
            txtSubject.Size = new System.Drawing.Size(360, 20);

            // Configuration du champ Message
            txtMessage.Location = new System.Drawing.Point(12, 64);
            txtMessage.Multiline = true;
            txtMessage.Size = new System.Drawing.Size(360, 200);
            txtMessage.ScrollBars = ScrollBars.Vertical;

            // Configuration du bouton Envoyer
            btnSend.Location = new System.Drawing.Point(12, 270);
            btnSend.Size = new System.Drawing.Size(170, 30);
            btnSend.Text = "Envoyer";
            btnSend.Click += btnSend_Click;

            // Configuration du bouton Annuler
            btnCancel.Location = new System.Drawing.Point(202, 270);
            btnCancel.Size = new System.Drawing.Size(170, 30);
            btnCancel.Text = "Annuler";
            btnCancel.Click += btnCancel_Click;

            // Configuration du formulaire
            this.ClientSize = new System.Drawing.Size(384, 311);
            this.Controls.AddRange(new Control[] { 
                lblSubject, txtSubject, 
                lblMessage, txtMessage, 
                btnSend, btnCancel 
            });
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
        }

        /// <summary>
        /// Initialise le contenu des champs du formulaire
        /// </summary>
        private void InitializeForm()
        {
            // Définit le titre du formulaire
            this.Text = $"Contacter {student.PrenomEtudiant} {student.NomEtudiant}";

            // Prérempli l'objet du message
            txtSubject.Text = $"Stage - {company.CompanyName}";

            // Prérempli le message avec un template
            txtMessage.Text = $"Bonjour {student.PrenomEtudiant},\n\n" +
                          $"Je suis {company.CompanyName} et je souhaiterais vous proposer une opportunité de stage/alternance.\n\n" +
                          "Cordialement,\n" +
                          $"{company.CompanyName}";
        }

        /// <summary>
        /// Gère le clic sur le bouton Annuler
        /// </summary>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Gère le clic sur le bouton Envoyer
        /// Envoie le message à Firebase
        /// </summary>
        private async void btnSend_Click(object sender, EventArgs e)
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
                btnCancel.Enabled = false;
                this.Cursor = Cursors.WaitCursor;

                // Crée l'objet message
                var message = new
                {
                    // Informations sur l'entreprise
                    CompanyId = company.Email.Replace(".", ","),
                    CompanyName = company.CompanyName,

                    // Informations sur l'étudiant
                    StudentId = student.Id,
                    StudentName = $"{student.PrenomEtudiant} {student.NomEtudiant}",

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
                btnCancel.Enabled = true;
                this.Cursor = Cursors.Default;
            }
        }
    }
}
