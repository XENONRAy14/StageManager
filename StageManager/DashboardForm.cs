using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;

namespace StageManager
{
    public partial class DashboardForm : Form
    {
        // Contrôles de l'interface
        private Panel sidePanel;
        private Panel contentPanel;
        private Label lblWelcome;
        private Label lblUserInfo;
        private Label lblAppTitle;
        private PictureBox pictureBoxLogo;
        
        // Boutons de navigation
        private Button btnViewStudents;
        private Button btnImportExcel;
        private Button btnContact;
        private Button btnLogout;
        
        // Données utilisateur
        private User currentUser;

        public DashboardForm(User user)
        {
            currentUser = user;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            // Configuration de base du formulaire
            this.ClientSize = new Size(1000, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Text = "Stage Manager - Tableau de bord";
            this.BackColor = Color.White;

            // Création du panneau latéral
            sidePanel = new Panel
            {
                Dock = DockStyle.Left,
                Width = 250,
                BackColor = ModernTheme.PrimaryColor
            };

            // Logo de l'application
            pictureBoxLogo = new PictureBox
            {
                Size = new Size(80, 80),
                Location = new Point((sidePanel.Width - 80) / 2, 30),
                BackColor = Color.White,
                Image = null // Vous pouvez ajouter un logo ici
            };

            // Titre de l'application
            lblAppTitle = new Label
            {
                Text = "STAGE MANAGER",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(sidePanel.Width, 30),
                Location = new Point(0, pictureBoxLogo.Bottom + 20)
            };

            // Informations utilisateur
            lblUserInfo = new Label
            {
                Text = $"{currentUser.CompanyName}\n{currentUser.Email}",
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(sidePanel.Width, 40),
                Location = new Point(0, lblAppTitle.Bottom + 10)
            };

            // Création des boutons de navigation avec style moderne
            btnViewStudents = CreateMenuButton("Voir la liste des élèves", 0);
            btnImportExcel = CreateMenuButton("Importer des élèves (Excel)", 1);
            btnContact = CreateMenuButton("Contacter un élève", 2);
            btnLogout = CreateMenuButton("Déconnexion", 3, Color.FromArgb(220, 53, 69)); // Rouge pour déconnexion

            // Ajout des événements aux boutons
            btnViewStudents.Click += BtnViewStudents_Click;
            btnImportExcel.Click += BtnImportExcel_Click;
            btnContact.Click += BtnContact_Click;
            btnLogout.Click += BtnLogout_Click;

            // Ajout des contrôles au panneau latéral
            sidePanel.Controls.AddRange(new Control[] { 
                pictureBoxLogo, lblAppTitle, lblUserInfo,
                btnViewStudents, btnImportExcel, btnContact, btnLogout
            });

            // Création du panneau de contenu principal
            contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(20)
            };

            // Message de bienvenue dans le panneau de contenu
            lblWelcome = new Label
            {
                Text = $"Bienvenue, {currentUser.CompanyName} !",
                Font = new Font("Segoe UI", 24, FontStyle.Bold),
                ForeColor = ModernTheme.PrimaryColor,
                AutoSize = true,
                Location = new Point(30, 30)
            };

            // Ajout des contrôles au panneau de contenu
            contentPanel.Controls.Add(lblWelcome);
            
            // Ajout d'une description des fonctionnalités
            var lblDescription = new Label
            {
                Text = "Utilisez le menu à gauche pour accéder aux différentes fonctionnalités de l'application.\n\n" +
                      "• Voir la liste des élèves : Consultez tous les étudiants disponibles pour un stage\n" +
                      "• Importer des élèves : Ajoutez de nouveaux étudiants à partir d'un fichier Excel\n" +
                      "• Contacter un élève : Envoyez un message à un étudiant spécifique",
                Font = new Font("Segoe UI", 12),
                ForeColor = Color.DimGray,
                Size = new Size(contentPanel.Width - 60, 200),
                Location = new Point(30, lblWelcome.Bottom + 20)
            };
            contentPanel.Controls.Add(lblDescription);

            // Ajout des panneaux au formulaire
            this.Controls.AddRange(new Control[] { contentPanel, sidePanel });
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

        /// <summary>
        /// Crée un bouton de menu avec un style uniforme
        /// </summary>
        private Button CreateMenuButton(string text, int position, Color? customColor = null)
        {
            Color buttonColor = customColor ?? ModernTheme.PrimaryColor;
            Color hoverColor = Color.FromArgb(
                Math.Max(buttonColor.R - 30, 0),
                Math.Max(buttonColor.G - 30, 0),
                Math.Max(buttonColor.B - 30, 0));

            var button = new Button
            {
                Text = text,
                Size = new Size(220, 45),
                Location = new Point(15, 200 + (position * 60)),
                Font = new Font("Segoe UI", 11),
                FlatStyle = FlatStyle.Flat,
                BackColor = buttonColor,
                ForeColor = Color.White,
                Cursor = Cursors.Hand,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(15, 0, 0, 0),
                FlatAppearance = { BorderSize = 0 }
            };

            // Ajouter des effets de survol
            button.MouseEnter += (s, e) => button.BackColor = hoverColor;
            button.MouseLeave += (s, e) => button.BackColor = buttonColor;

            return button;
        }
    }
}
