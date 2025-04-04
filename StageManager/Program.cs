using System;
using System.Windows.Forms;

namespace StageManager
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Créer le compte admin si nécessaire
            CreateAdminAccount();

            Application.Run(new LoginForm());
        }

        private static async void CreateAdminAccount()
        {
            try
            {
                var adminEmail = "admin@stagemanager.com";
                var response = await FirebaseManager.Client.GetAsync("users/" + adminEmail.Replace(".", ","));
                var admin = response.ResultAs<User>();

                if (admin == null)
                {
                    var hashedPassword = BCrypt.Net.BCrypt.HashPassword("admin123");
                    var adminUser = new User
                    {
                        Email = adminEmail,
                        Password = hashedPassword,
                        CompanyName = "Stage Manager Admin",
                        Role = "admin"
                    };

                    await FirebaseManager.Client.SetAsync("users/" + adminEmail.Replace(".", ","), adminUser);
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
