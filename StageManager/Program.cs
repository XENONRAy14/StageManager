using System;
using System.Windows.Forms;

namespace StageManager
{
    // Classe principale du programme qui sert de point d'entrée à l'application
    static class Program
    {
        // Attribut nécessaire pour les applications Windows Forms
        // STA = Single-Threaded Apartment, mode d'exécution requis pour les interfaces graphiques
        [STAThread]
        static void Main()
        {
            // Active les styles visuels modernes de Windows
            Application.EnableVisualStyles();
            // Configure le rendu du texte pour une meilleure compatibilité
            Application.SetCompatibleTextRenderingDefault(false);

            // Vérifie si un compte administrateur existe déjà, sinon en crée un
            CreateAdminAccount();

            // Démarre l'application en affichant le formulaire de connexion
            Application.Run(new LoginForm());
        }

        // Méthode qui vérifie si un compte administrateur existe déjà dans Firebase
        // Si non, crée un compte admin par défaut avec des identifiants prédéfinis
        private static async void CreateAdminAccount()
        {
            try
            {
                // Email de l'administrateur par défaut
                var adminEmail = "admin@stagemanager.com";
                // Firebase ne permet pas les points dans les clés, donc on les remplace par des virgules
                var response = await FirebaseManager.Client.GetAsync("users/" + adminEmail.Replace(".", ","));
                // Convertit la réponse en objet User
                var admin = response.ResultAs<User>();

                // Si aucun admin n'existe, on en crée un
                if (admin == null)
                {
                    // Hashage du mot de passe pour sécuriser le stockage
                    var hashedPassword = BCrypt.Net.BCrypt.HashPassword("admin123");
                    // Création d'un nouvel objet User avec les droits d'administrateur
                    var adminUser = new User
                    {
                        Email = adminEmail,
                        Password = hashedPassword,
                        CompanyName = "Stage Manager Admin",
                        Role = "admin" // Définit le rôle comme administrateur
                    };

                    // Enregistre l'utilisateur admin dans Firebase
                    await FirebaseManager.Client.SetAsync("users/" + adminEmail.Replace(".", ","), adminUser);
                    // Affiche un message de confirmation
                    MessageBox.Show(
                        "Un compte administrateur a été créé.\nEmail: admin@stagemanager.com\nMot de passe: admin123", 
                        "Information", 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Information
                    );
                }
            }
            catch (Exception ex)
            {
                // En cas d'erreur, affiche un message d'erreur
                MessageBox.Show(
                    "Erreur lors de la création du compte admin: " + ex.Message,
                    "Erreur",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
    }
}
