using System;
using System.Windows.Forms;
using System.Drawing;
using FireSharp.Response;

namespace StageManager
{
    public partial class RegisterForm : Form
    {
        public RegisterForm()
        {
            InitializeComponent();
            // Configuration des événements
            btnRegister.Click += BtnRegister_Click;
            lblLogin.Click += BtnLogin_Click;
        }

        private async void BtnRegister_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEmail.Text) || 
                string.IsNullOrWhiteSpace(txtPassword.Text) || 
                string.IsNullOrWhiteSpace(txtPasswordConfirm.Text) || 
                string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                MessageBox.Show("Veuillez remplir tous les champs.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (txtPassword.Text != txtPasswordConfirm.Text)
            {
                MessageBox.Show("Les mots de passe ne correspondent pas.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                btnRegister.Enabled = false;
                btnRegister.Text = "Inscription en cours...";
                Application.DoEvents();

                var user = new User
                {
                    Email = txtEmail.Text,
                    Password = BCrypt.Net.BCrypt.HashPassword(txtPassword.Text),
                    CompanyName = txtFullName.Text, // Utilise le champ FullName comme nom d'entreprise
                    Role = "company"
                };

                var response = await FirebaseManager.Client.SetAsync(
                    "users/" + txtEmail.Text.Replace(".", ","), 
                    user
                );

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    MessageBox.Show("Inscription réussie! Vous pouvez maintenant vous connecter.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de l'inscription: " + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnRegister.Enabled = true;
                btnRegister.Text = "S'INSCRIRE";
            }
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            // Fermer le formulaire d'inscription et ouvrir le formulaire de connexion
            var loginForm = new LoginForm();
            this.Hide();
            loginForm.ShowDialog();
            this.Close();
        }
    }
}
