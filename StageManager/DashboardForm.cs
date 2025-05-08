using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using System.ComponentModel;

namespace StageManager
{
    public partial class DashboardForm : Form
    {
        // Données utilisateur
        private User currentUser;

        public DashboardForm(User user)
        {
            currentUser = user;
            InitializeComponent();
            
            // Mettre à jour les informations utilisateur
            lblUserInfo.Text = $"{currentUser.CompanyName}\n{currentUser.Email}";
            lblWelcome.Text = $"Bienvenue, {currentUser.CompanyName}!";
        }

        private void BtnViewStudents_Click(object sender, EventArgs e)
        {
            var stageListForm = new StageListForm(false, currentUser);
            this.Hide();
            stageListForm.ShowDialog();
            this.Close();
        }

        private async void BtnImportExcel_Click(object sender, EventArgs e)
        {
            // Créer une école par défaut si nécessaire
            var school = new School
            {
                Id = "default_school",
                Name = "École par défaut",
                City = "Ville par défaut"
            };

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Fichiers Excel|*.xlsx;*.xls";
                openFileDialog.Title = "Sélectionner un fichier Excel";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        await ExcelImporter.ImportFromExcel(openFileDialog.FileName, school);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Erreur lors de l'importation : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void BtnContact_Click(object sender, EventArgs e)
        {
            // Ouvrir la liste des élèves avec le mode contact activé et l'utilisateur actuel
            var stageListForm = new StageListForm(true, currentUser);
            stageListForm.ShowDialog();
        }

        private void BtnLogout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Êtes-vous sûr de vouloir vous déconnecter ?", "Confirmation", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Close();
                var loginForm = new LoginForm();
                loginForm.Show();
            }
        }


    }
}
