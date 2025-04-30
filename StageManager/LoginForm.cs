using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using FireSharp.Response;
using BCrypt.Net;

namespace StageManager
{
    /// <summary>
    /// Formulaire de connexion qui permet aux utilisateurs de s'authentifier
    /// C'est le premier écran que voit l'utilisateur au lancement de l'application
    /// </summary>
    public partial class LoginForm : Form
    {
        // Déclaration des contrôles principaux du formulaire
        private TextBox txtEmail;        // Champ de saisie pour l'email
        private TextBox txtPassword;     // Champ de saisie pour le mot de passe
        private Button btnLogin;         // Bouton de connexion
        private Button btnRegister;      // Bouton pour accéder au formulaire d'inscription

        /// <summary>
        /// Constructeur du formulaire de connexion
        /// </summary>
        public LoginForm()
        {
            // Initialise tous les composants graphiques du formulaire
            InitializeComponent();
        }

        /// <summary>
        /// Initialise les composants de l'interface utilisateur
        /// Cette méthode crée et configure tous les éléments visuels du formulaire
        /// </summary>
        private void InitializeComponent()
        {
            // Configuration de base du formulaire
            this.ClientSize = new Size(800, 500);                  // Définit la taille du formulaire
            this.StartPosition = FormStartPosition.CenterScreen;    // Centre le formulaire sur l'écran
            this.FormBorderStyle = FormBorderStyle.FixedSingle;    // Empêche le redimensionnement
            this.MaximizeBox = false;                              // Désactive le bouton maximiser
            this.Text = "Stage Manager - Connexion";                // Titre de la fenêtre
            this.BackColor = Color.White;                          // Fond blanc pour le formulaire

            // Création du panel gauche (bande colorée à gauche)
            // Ce panel contient le logo et le slogan de l'application
            Panel leftPanel = new Panel
            {
                Dock = DockStyle.Left,                   // Ancré sur le côté gauche
                Width = 400,                            // Largeur fixe de 400 pixels
                BackColor = ModernTheme.PrimaryColor    // Couleur principale définie dans ModernTheme
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

        /// <summary>
        /// Gestionnaire d'événement pour le clic sur le bouton de connexion
        /// Vérifie les identifiants et connecte l'utilisateur si valides
        /// </summary>
        /// <param name="sender">L'objet qui a déclenché l'événement</param>
        /// <param name="e">Arguments de l'événement</param>
        private async void BtnLogin_Click(object sender, EventArgs e)
        {
            // Vérifie que les champs ne sont pas vides ou avec leurs valeurs par défaut
            if (txtEmail.Text == "Email" || txtPassword.Text == "Mot de passe" ||
                string.IsNullOrWhiteSpace(txtEmail.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                // Affiche un message d'erreur si les champs ne sont pas remplis correctement
                MessageBox.Show("Veuillez remplir tous les champs.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                // Désactive les boutons et change le curseur pour indiquer le chargement
                btnLogin.Enabled = false;
                btnRegister.Enabled = false;
                this.Cursor = Cursors.WaitCursor;

                // Prépare l'email pour la recherche dans Firebase
                // Convertit en minuscules et supprime les espaces pour éviter les problèmes de casse
                string originalEmail = txtEmail.Text.Trim().ToLower();
                string safeEmail = SafeFirebaseKey(originalEmail);  // Convertit l'email en format compatible avec Firebase
                
                // Affiche des informations de débogage dans la console
                Console.WriteLine($"Email original: {originalEmail}");
                Console.WriteLine($"Email encodé: {safeEmail}");

                // Récupère tous les utilisateurs depuis Firebase
                var response = await FirebaseManager.Client.GetAsync($"users");
                if (response == null)
                {
                    // Affiche une erreur si la connexion à Firebase a échoué
                    MessageBox.Show("Erreur de connexion à Firebase.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Convertit la réponse en dictionnaire d'utilisateurs
                var allUsers = response.ResultAs<Dictionary<string, User>>();
                if (allUsers == null)
                {
                    // Affiche une erreur si la conversion a échoué
                    MessageBox.Show("Erreur lors de la récupération des utilisateurs.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Affiche des informations de débogage sur les utilisateurs trouvés
                Console.WriteLine("Utilisateurs trouvés dans Firebase:");
                foreach (var kvp in allUsers)
                {
                    Console.WriteLine($"Clé: {kvp.Key}, Email: {kvp.Value.Email}");
                }

                // Recherche l'utilisateur dans la liste par son email
                User user = null;
                foreach (var kvp in allUsers)
                {
                    // Compare les emails en ignorant la casse
                    if (kvp.Value.Email.ToLower() == originalEmail)
                    {
                        user = kvp.Value;  // Utilisateur trouvé
                        break;
                    }
                }

                // Vérifie si l'utilisateur existe et si le mot de passe est correct
                // BCrypt.Verify compare le mot de passe saisi avec le hash stocké dans la base de données
                if (user != null && BCrypt.Net.BCrypt.Verify(txtPassword.Text, user.Password))
                {
                    // Authentification réussie : ouvre le tableau de bord
                    var dashboardForm = new DashboardForm(user);  // Crée le formulaire du tableau de bord
                    this.Hide();                                  // Cache le formulaire de connexion
                    dashboardForm.ShowDialog();                   // Affiche le tableau de bord
                    this.Close();                                 // Ferme le formulaire de connexion quand le tableau de bord est fermé
                }
                else
                {
                    // Authentification échouée : affiche un message d'erreur
                    // Le message ne précise pas si c'est l'email ou le mot de passe qui est incorrect
                    // pour des raisons de sécurité
                    MessageBox.Show("Email ou mot de passe incorrect.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                // Capture et affiche toute exception qui pourrait se produire pendant la connexion
                // Affiche le message d'erreur et la pile d'appel pour faciliter le débogage
                MessageBox.Show($"Erreur lors de la connexion : {ex.Message}\n\nDétails: {ex.StackTrace}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Réactive les boutons et restaure le curseur normal, même en cas d'erreur
                // Ce bloc s'exécute toujours, que la connexion réussisse ou échoue
                btnLogin.Enabled = true;
                btnRegister.Enabled = true;
                this.Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// Gestionnaire d'événement pour le clic sur le bouton d'inscription
        /// Ouvre le formulaire d'inscription pour créer un nouveau compte
        /// </summary>
        /// <param name="sender">L'objet qui a déclenché l'événement</param>
        /// <param name="e">Arguments de l'événement</param>
        private void BtnRegister_Click(object sender, EventArgs e)
        {
            // Crée une nouvelle instance du formulaire d'inscription
            var registerForm = new RegisterForm();
            // Affiche le formulaire d'inscription en mode modal
            // L'utilisateur doit fermer ce formulaire avant de revenir au formulaire de connexion
            registerForm.ShowDialog();
        }

        /// <summary>
        /// Convertit un email en une clé Firebase valide en remplaçant les caractères interdits
        /// </summary>
        /// <param name="email">L'adresse email à convertir</param>
        /// <returns>Une chaîne de caractères utilisable comme clé dans Firebase</returns>
        /// <remarks>
        /// Firebase n'autorise pas certains caractères dans les clés, notamment le point (.)
        /// Cette méthode remplace les points par des virgules pour créer une clé valide
        /// tout en préservant l'unicité de l'email
        /// </remarks>
        private string SafeFirebaseKey(string email)
        {
            // Remplace les points par des virgules car Firebase n'accepte pas les points dans les clés
            return email.Replace(".", ",");
        }
    }
}
