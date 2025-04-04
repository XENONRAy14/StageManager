using System;
using System.Windows.Forms;
using System.Drawing;
using FireSharp.Response;

namespace StageManager
{
    public partial class RegisterForm : Form
    {
        private TextBox txtEmail;
        private TextBox txtPassword;
        private TextBox txtConfirmPassword;
        private TextBox txtCompanyName;
        private Button btnRegister;

        private Button btnLogin;
        private Label lblTitle;
        private Panel leftPanel;
        private Panel rightPanel;
        private Panel inputPanel;

        public RegisterForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialise les composants de l'interface utilisateur
        /// </summary>
        private void InitializeComponent()
        {
            // Configuration de base du formulaire
            this.ClientSize = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Text = "Stage Manager - Inscription";
            this.BackColor = Color.White;
            
            // Création du panel gauche (couleur de fond)
            leftPanel = new Panel
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
            
            // Panel droit (formulaire d'inscription)
            rightPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(40),
                AutoScroll = true
            };
            
            // Titre du formulaire
            lblTitle = new Label
            {
                Text = "Créer un compte",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = ModernTheme.PrimaryColor,
                Size = new Size(300, 40),
                Location = new Point(50, 40)
            };
            
            // Sous-titre du formulaire
            Label lblFormSubtitle = new Label
            {
                Text = "Inscrivez-vous pour accéder à la plateforme",
                Font = new Font("Segoe UI", 10),
                ForeColor = ModernTheme.TextLightColor,
                Size = new Size(300, 20),
                Location = new Point(50, 80)
            };
            
            // Conteneur pour les champs de saisie
            inputPanel = new Panel
            {
                Size = new Size(320, 400),
                Location = new Point(40, 120),
                BackColor = Color.White
            };
            
            // Champ nom de l'entreprise
            Label lblCompanyName = new Label
            {
                Text = "Nom de l'entreprise",
                Font = new Font("Segoe UI", 9),
                ForeColor = ModernTheme.TextColor,
                Size = new Size(300, 20),
                Location = new Point(0, 0)
            };
            
            txtCompanyName = new TextBox
            {
                Size = new Size(320, 40),
                Location = new Point(0, 25),
                Text = "Nom de l'entreprise",
                Font = ModernTheme.DefaultFont,
                BorderStyle = BorderStyle.FixedSingle
            };
            txtCompanyName.GotFocus += (s, e) => { if (txtCompanyName.Text == "Nom de l'entreprise") txtCompanyName.Text = ""; };
            txtCompanyName.LostFocus += (s, e) => { if (string.IsNullOrWhiteSpace(txtCompanyName.Text)) txtCompanyName.Text = "Nom de l'entreprise"; };
            
            // Champ email
            Label lblEmail = new Label
            {
                Text = "Email",
                Font = new Font("Segoe UI", 9),
                ForeColor = ModernTheme.TextColor,
                Size = new Size(300, 20),
                Location = new Point(0, 80)
            };
            
            txtEmail = new TextBox
            {
                Size = new Size(320, 40),
                Location = new Point(0, 105),
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
                Location = new Point(0, 160)
            };
            
            txtPassword = new TextBox
            {
                Size = new Size(320, 40),
                Location = new Point(0, 185),
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
            
            // Champ confirmation mot de passe
            Label lblConfirmPassword = new Label
            {
                Text = "Confirmer le mot de passe",
                Font = new Font("Segoe UI", 9),
                ForeColor = ModernTheme.TextColor,
                Size = new Size(300, 20),
                Location = new Point(0, 240)
            };
            
            txtConfirmPassword = new TextBox
            {
                Size = new Size(320, 40),
                Location = new Point(0, 265),
                Text = "Confirmer le mot de passe",
                Font = ModernTheme.DefaultFont,
                BorderStyle = BorderStyle.FixedSingle,
                PasswordChar = '\0'
            };
            txtConfirmPassword.GotFocus += (s, e) => 
            { 
                if (txtConfirmPassword.Text == "Confirmer le mot de passe") 
                {
                    txtConfirmPassword.Text = "";
                    txtConfirmPassword.PasswordChar = '•';
                }
            };
            txtConfirmPassword.LostFocus += (s, e) => 
            { 
                if (string.IsNullOrWhiteSpace(txtConfirmPassword.Text)) 
                {
                    txtConfirmPassword.Text = "Confirmer le mot de passe";
                    txtConfirmPassword.PasswordChar = '\0';
                }
            };
            
            // Bouton d'inscription
            btnRegister = new Button
            {
                Size = new Size(320, 45),
                Location = new Point(0, 330),
                Text = "S'INSCRIRE",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 },
                BackColor = ModernTheme.PrimaryColor,
                ForeColor = Color.White,
                Cursor = Cursors.Hand
            };
            btnRegister.Click += BtnRegister_Click;
            
            // Lien pour se connecter
            btnLogin = new Button
            {
                Size = new Size(320, 30),
                Location = new Point(0, 380),
                Text = "Déjà inscrit ? Se connecter",
                Font = new Font("Segoe UI", 9),
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 },
                BackColor = Color.Transparent,
                ForeColor = ModernTheme.PrimaryColor,
                Cursor = Cursors.Hand,
                TextAlign = ContentAlignment.MiddleCenter
            };
            btnLogin.Click += BtnLogin_Click;
            
            // Ajout des contrôles au panel des champs
            inputPanel.Controls.Add(lblCompanyName);
            inputPanel.Controls.Add(txtCompanyName);
            inputPanel.Controls.Add(lblEmail);
            inputPanel.Controls.Add(txtEmail);
            inputPanel.Controls.Add(lblPassword);
            inputPanel.Controls.Add(txtPassword);
            inputPanel.Controls.Add(lblConfirmPassword);
            inputPanel.Controls.Add(txtConfirmPassword);
            inputPanel.Controls.Add(btnRegister);
            inputPanel.Controls.Add(btnLogin);
            
            // Ajout des contrôles au panel droit
            rightPanel.Controls.Add(lblTitle);
            rightPanel.Controls.Add(lblFormSubtitle);
            rightPanel.Controls.Add(inputPanel);
            
            // Ajout des panels au formulaire
            this.Controls.Add(rightPanel);
            this.Controls.Add(leftPanel);
        }

        private async void BtnRegister_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEmail.Text.Replace("Email", "")) || 
                string.IsNullOrWhiteSpace(txtPassword.Text.Replace("Mot de passe", "")) || 
                string.IsNullOrWhiteSpace(txtConfirmPassword.Text.Replace("Confirmer le mot de passe", "")) || 
                string.IsNullOrWhiteSpace(txtCompanyName.Text.Replace("Nom de l'entreprise", "")))
            {
                MessageBox.Show("Veuillez remplir tous les champs.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (txtPassword.Text.Replace("Mot de passe", "") != txtConfirmPassword.Text.Replace("Confirmer le mot de passe", ""))
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
                    Email = txtEmail.Text.Replace("Email", ""),
                    Password = BCrypt.Net.BCrypt.HashPassword(txtPassword.Text.Replace("Mot de passe", "")),
                    CompanyName = txtCompanyName.Text.Replace("Nom de l'entreprise", ""),
                    Role = "company"
                };

                var response = await FirebaseManager.Client.SetAsync(
                    "users/" + txtEmail.Text.Replace("Email", "").Replace(".", ","), 
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
