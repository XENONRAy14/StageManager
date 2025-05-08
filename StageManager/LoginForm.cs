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
        /// <summary>
        /// Constructeur du formulaire de connexion
        /// </summary>
        public LoginForm()
        {
            // Initialise tous les composants graphiques du formulaire
            InitializeComponent();
            
            // Configuration des événements
            this.Load += (s, e) => {
                if (Controls.Find("btnLogin", true).Length > 0)
                    ((Button)Controls.Find("btnLogin", true)[0]).Click += BtnLogin_Click;
                
                if (Controls.Find("lblRegister", true).Length > 0)
                    ((Label)Controls.Find("lblRegister", true)[0]).Click += BtnRegister_Click;
            };
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
                if (Controls.Find("btnLogin", true).Length > 0)
                    ((Button)Controls.Find("btnLogin", true)[0]).Enabled = false;
                if (Controls.Find("btnRegister", true).Length > 0)
                    ((Button)Controls.Find("btnRegister", true)[0]).Enabled = false;
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
                if (Controls.Find("btnLogin", true).Length > 0)
                    ((Button)Controls.Find("btnLogin", true)[0]).Enabled = true;
                if (Controls.Find("btnRegister", true).Length > 0)
                    ((Button)Controls.Find("btnRegister", true)[0]).Enabled = true;
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
