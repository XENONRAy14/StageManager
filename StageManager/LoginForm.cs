using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using FireSharp.Response;
using BCrypt.Net;

namespace StageManager
{
    public partial class LoginForm : Form
    {
        private TextBox txtEmail;
        private TextBox txtPassword;
        private Button btnLogin;
        private Button btnRegister;

        public LoginForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialise les composants de l'interface utilisateur
        /// </summary>
        private void InitializeComponent()
        {
            // Configuration de base du formulaire
            this.ClientSize = new Size(800, 500);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Text = "Stage Manager - Connexion";
            this.BackColor = Color.White;

            // Création du panel gauche (couleur de fond)
            Panel leftPanel = new Panel
            {
                Dock = DockStyle.Left,
                Width = 400,
                BackColor = ModernTheme.PrimaryColor
            };

            // Logo ou image sur le panel gauche
            Label lblLogo = new Label
            {
                Text = "STAGE MANAGER",
                Font = new Font("Segoe UI", 24, FontStyle.Bold),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(400, 60),
                Location = new Point(0, 150)
            };

            // Sous-titre
            Label lblSubtitle = new Label
            {
                Text = "Gérez vos stages en toute simplicité",
                Font = new Font("Segoe UI", 12),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(400, 30),
                Location = new Point(0, 220)
            };

            // Ajout des contrôles au panel gauche
            leftPanel.Controls.Add(lblLogo);
            leftPanel.Controls.Add(lblSubtitle);

            // Panel droit (formulaire de connexion)
            Panel rightPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(40)
            };

            // Titre du formulaire
            Label lblTitle = new Label
            {
                Text = "Connexion",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = ModernTheme.PrimaryColor,
                Size = new Size(300, 40),
                Location = new Point(50, 80)
            };

            // Sous-titre du formulaire
            Label lblFormSubtitle = new Label
            {
                Text = "Veuillez vous connecter pour continuer",
                Font = new Font("Segoe UI", 10),
                ForeColor = ModernTheme.TextLightColor,
                Size = new Size(300, 20),
                Location = new Point(50, 120)
            };

            // Conteneur pour les champs de saisie
            Panel inputPanel = new Panel
            {
                Size = new Size(320, 250),
                Location = new Point(40, 160),
                BackColor = Color.White
            };

            // Champ email
            Label lblEmail = new Label
            {
                Text = "Email",
                Font = new Font("Segoe UI", 9),
                ForeColor = ModernTheme.TextColor,
                Size = new Size(300, 20),
                Location = new Point(0, 0)
            };

            txtEmail = new TextBox
            {
                Size = new Size(320, 40),
                Location = new Point(0, 25),
                Text = "Email",
                Font = ModernTheme.DefaultFont,
                BorderStyle = BorderStyle.FixedSingle
            };
            txtEmail.GotFocus += (s, e) => { if (txtEmail.Text == "Email") txtEmail.Text = ""; };
            txtEmail.LostFocus += (s, e) => { if (string.IsNullOrWhiteSpace(txtEmail.Text)) txtEmail.Text = "Email"; };

            // Champ mot de passe
            Label lblPassword = new Label
            {
                Text = "Mot de passe",
                Font = new Font("Segoe UI", 9),
                ForeColor = ModernTheme.TextColor,
                Size = new Size(300, 20),
                Location = new Point(0, 80)
            };

            txtPassword = new TextBox
            {
                Size = new Size(320, 40),
                Location = new Point(0, 105),
                Text = "Mot de passe",
                Font = ModernTheme.DefaultFont,
                BorderStyle = BorderStyle.FixedSingle,
                PasswordChar = '\0'
            };
            txtPassword.GotFocus += (s, e) =>
            {
                if (txtPassword.Text == "Mot de passe")
                {
                    txtPassword.Text = "";
                    txtPassword.PasswordChar = '•';
                }
            };
            txtPassword.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtPassword.Text))
                {
                    txtPassword.Text = "Mot de passe";
                    txtPassword.PasswordChar = '\0';
                }
            };

            // Bouton de connexion
            btnLogin = new Button
            {
                Size = new Size(320, 45),
                Location = new Point(0, 170),
                Text = "SE CONNECTER",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 },
                BackColor = ModernTheme.PrimaryColor,
                ForeColor = Color.White,
                Cursor = Cursors.Hand
            };
            btnLogin.Click += BtnLogin_Click;

            // Lien pour s'inscrire
            btnRegister = new Button
            {
                Size = new Size(320, 30),
                Location = new Point(0, 220),
                Text = "Pas encore inscrit ? Créer un compte",
                Font = new Font("Segoe UI", 9),
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 },
                BackColor = Color.Transparent,
                ForeColor = ModernTheme.PrimaryColor,
                Cursor = Cursors.Hand,
                TextAlign = ContentAlignment.MiddleCenter
            };
            btnRegister.Click += BtnRegister_Click;

            // Ajout des contrôles au panel des champs
            inputPanel.Controls.Add(lblEmail);
            inputPanel.Controls.Add(txtEmail);
            inputPanel.Controls.Add(lblPassword);
            inputPanel.Controls.Add(txtPassword);
            inputPanel.Controls.Add(btnLogin);
            inputPanel.Controls.Add(btnRegister);

            // Ajout des contrôles au panel droit
            rightPanel.Controls.Add(lblTitle);
            rightPanel.Controls.Add(lblFormSubtitle);
            rightPanel.Controls.Add(inputPanel);

            // Ajout des panels au formulaire
            this.Controls.Add(rightPanel);
            this.Controls.Add(leftPanel);
        }

        private async void BtnLogin_Click(object sender, EventArgs e)
        {
            if (txtEmail.Text == "Email" || txtPassword.Text == "Mot de passe" ||
                string.IsNullOrWhiteSpace(txtEmail.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Veuillez remplir tous les champs.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                btnLogin.Enabled = false;
                btnRegister.Enabled = false;
                this.Cursor = Cursors.WaitCursor;

                // Pour le débogage
                string originalEmail = txtEmail.Text.Trim().ToLower();
                string safeEmail = SafeFirebaseKey(originalEmail);
                Console.WriteLine($"Email original: {originalEmail}");
                Console.WriteLine($"Email encodé: {safeEmail}");

                var response = await FirebaseManager.Client.GetAsync($"users");
                if (response == null)
                {
                    MessageBox.Show("Erreur de connexion à Firebase.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var allUsers = response.ResultAs<Dictionary<string, User>>();
                if (allUsers == null)
                {
                    MessageBox.Show("Erreur lors de la récupération des utilisateurs.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Pour le débogage
                Console.WriteLine("Utilisateurs trouvés dans Firebase:");
                foreach (var kvp in allUsers)
                {
                    Console.WriteLine($"Clé: {kvp.Key}, Email: {kvp.Value.Email}");
                }

                // Rechercher l'utilisateur par email
                User user = null;
                foreach (var kvp in allUsers)
                {
                    if (kvp.Value.Email.ToLower() == originalEmail)
                    {
                        user = kvp.Value;
                        break;
                    }
                }

                if (user != null && BCrypt.Net.BCrypt.Verify(txtPassword.Text, user.Password))
                {
                    var dashboardForm = new DashboardForm(user);
                    this.Hide();
                    dashboardForm.ShowDialog();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Email ou mot de passe incorrect.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la connexion : {ex.Message}\n\nDétails: {ex.StackTrace}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnLogin.Enabled = true;
                btnRegister.Enabled = true;
                this.Cursor = Cursors.Default;
            }
        }

        private void BtnRegister_Click(object sender, EventArgs e)
        {
            var registerForm = new RegisterForm();
            registerForm.ShowDialog();
        }

        /// <summary>
        /// Convertit un email en une clé Firebase valide en remplaçant les caractères interdits
        /// </summary>
        private string SafeFirebaseKey(string email)
        {
            return email.Replace(".", ",");
        }
    }
}
