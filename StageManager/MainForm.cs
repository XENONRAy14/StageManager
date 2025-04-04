using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using FireSharp.Response;
using Newtonsoft.Json;

namespace StageManager
{
    /// <summary>
    /// Formulaire principal de l'application qui affiche la liste des étudiants
    /// et permet de les contacter
    /// </summary>
    public partial class MainForm : Form
    {
        // L'utilisateur actuellement connecté (une entreprise)
        private readonly User currentUser;

        // Contrôles de l'interface utilisateur
        private ListView lstStudents;
        private Button btnContact;
        private Button btnRefresh;
        private Button btnImport;
        
        // Contrôles pour le menu latéral et le contenu
        private Panel sideMenu;
        private Panel contentPanel;
        private Panel headerPanel;
        private Label lblHeader;
        private Label lblUserInfo;
        private Button btnDashboard;
        private Button btnStageList;
        private Button btnProfile;
        private Button btnSettings;
        private Button btnLogout;

        /// <summary>
        /// Constructeur du formulaire principal
        /// </summary>
        /// <param name="user">L'utilisateur connecté (entreprise)</param>
        public MainForm(User user)
        {
            currentUser = user;
            InitializeComponent();
            InitializeListView();
            LoadData(); // Charger les données immédiatement
        }

        /// <summary>
        /// Configure la ListView avec les colonnes appropriées
        /// </summary>
        private void InitializeListView()
        {
            // Configuration de base de la ListView
            lstStudents.View = View.Details;
            lstStudents.FullRowSelect = true;
            lstStudents.GridLines = true;
            lstStudents.MultiSelect = false;
            lstStudents.HideSelection = false;
            ModernTheme.ApplyListViewStyle(lstStudents);

            // Supprime toutes les colonnes existantes
            lstStudents.Columns.Clear();

            // Ajoute les colonnes avec des largeurs appropriées
            lstStudents.Columns.Add("Nom", 150);
            lstStudents.Columns.Add("Prénom", 150);
            lstStudents.Columns.Add("Classe", 100);
            lstStudents.Columns.Add("Spécialité", 150);
            lstStudents.Columns.Add("Email", 200);
            
            // Désactiver le mode OwnerDraw qui peut causer des problèmes d'affichage
            lstStudents.OwnerDraw = false;
        }

        /// <summary>
        /// Initialise les composants de l'interface utilisateur
        /// </summary>
        private void InitializeComponent()
        {
            // Appliquer le thème moderne au formulaire
            ModernTheme.ApplyFormStyle(this);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.WindowState = FormWindowState.Maximized;
            this.MaximizeBox = true;
            this.MinimumSize = new Size(1000, 600);
            
            // Création du menu latéral
            sideMenu = ModernTheme.CreateSideMenu(220, this.ClientSize.Height);
            
            // Création des boutons du menu
            btnDashboard = ModernTheme.CreateMenuButton("Tableau de bord");
            btnStageList = ModernTheme.CreateMenuButton("Liste des stages");
            btnProfile = ModernTheme.CreateMenuButton("Mon profil");
            btnSettings = ModernTheme.CreateMenuButton("Paramètres");
            
            // Séparateur
            Panel separator = new Panel
            {
                Height = 1,
                Dock = DockStyle.Top,
                BackColor = ModernTheme.PrimaryLightColor
            };
            
            // Bouton de déconnexion en bas du menu
            btnLogout = ModernTheme.CreateMenuButton("Déconnexion");
            btnLogout.Dock = DockStyle.Bottom;
            btnLogout.BackColor = ModernTheme.PrimaryDarkColor;
            btnLogout.Click += BtnLogout_Click;
            
            // Informations utilisateur
            lblUserInfo = new Label
            {
                Text = $"Connecté en tant que\n{currentUser.CompanyName}",
                ForeColor = Color.White,
                Font = new Font(ModernTheme.DefaultFont.FontFamily, 9),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Bottom,
                Height = 50,
                BackColor = ModernTheme.PrimaryDarkColor
            };
            
            // Ajout des contrôles au menu latéral
            sideMenu.Controls.Add(btnLogout);
            sideMenu.Controls.Add(lblUserInfo);
            sideMenu.Controls.Add(btnSettings);
            sideMenu.Controls.Add(btnProfile);
            sideMenu.Controls.Add(btnStageList);
            sideMenu.Controls.Add(btnDashboard);
            
            // Sélection par défaut
            btnStageList.BackColor = ModernTheme.PrimaryColor;
            
            // Événements des boutons du menu
            btnDashboard.Click += BtnDashboard_Click;
            btnStageList.Click += BtnStageList_Click;
            btnProfile.Click += BtnProfile_Click;
            btnSettings.Click += BtnSettings_Click;
            
            // Création du panneau d'en-tête
            headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = ModernTheme.SurfaceColor
            };
            
            // Titre de la page
            lblHeader = new Label
            {
                Text = "Liste des stages",
                Font = ModernTheme.TitleFont,
                ForeColor = ModernTheme.TextColor,
                Location = new Point(20, 15),
                AutoSize = true
            };
            
            headerPanel.Controls.Add(lblHeader);
            
            // Création du panneau de contenu
            contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = ModernTheme.BackgroundColor,
                Padding = new Padding(20)
            };
            
            // Création des contrôles
            lstStudents = new ListView
            {
                Dock = DockStyle.Fill
            };
            
            // Panel pour les boutons
            Panel buttonPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 60,
                BackColor = ModernTheme.SurfaceColor
            };
            
            // Configuration des boutons
            btnContact = new Button
            {
                Size = new System.Drawing.Size(180, 36),
                Location = new Point(20, 12),
                Text = "Contacter l'élève"
            };
            ModernTheme.ApplyButtonStyle(btnContact);
            btnContact.Click += BtnContact_Click;
            btnContact.Enabled = false;
            
            btnRefresh = new Button
            {
                Size = new System.Drawing.Size(180, 36),
                Location = new Point(220, 12),
                Text = "Actualiser"
            };
            ModernTheme.ApplyButtonStyle(btnRefresh, false);
            btnRefresh.Click += BtnRefresh_Click;
            
            btnImport = new Button
            {
                Size = new System.Drawing.Size(180, 36),
                Location = new Point(420, 12),
                Text = "Importer depuis Excel"
            };
            ModernTheme.ApplyButtonStyle(btnImport, false);
            btnImport.Click += BtnImport_Click;
            
            // Ajout des boutons au panel
            buttonPanel.Controls.Add(btnContact);
            buttonPanel.Controls.Add(btnRefresh);
            buttonPanel.Controls.Add(btnImport);
            
            // Ajout des contrôles au panneau de contenu
            contentPanel.Controls.Add(lstStudents);
            contentPanel.Controls.Add(buttonPanel);
            
            // Ajout des panneaux au formulaire
            this.Controls.Add(contentPanel);
            this.Controls.Add(headerPanel);
            this.Controls.Add(sideMenu);
            
            // Titre du formulaire
            this.Text = "Stage Manager";
            
            // Ajout des gestionnaires d'événements
            lstStudents.SelectedIndexChanged += LstStudents_SelectedIndexChanged;
        }

        /// <summary>
        /// Charge les données des étudiants depuis Firebase
        /// </summary>
        private async void LoadData()
        {
            try
            {
                lstStudents.Items.Clear();
                this.Cursor = Cursors.WaitCursor;

                // Récupération des données depuis Firebase
                var response = await FirebaseManager.Client.GetAsync("students");
                if (response == null || string.IsNullOrEmpty(response.Body))
                {
                    MessageBox.Show("Aucune donnée reçue de Firebase", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Conversion des données JSON en objets Student
                var data = JsonConvert.DeserializeObject<Dictionary<string, Student>>(response.Body);
                
                if (data != null)
                {
                    // Ajout de chaque étudiant dans la ListView
                    foreach (var pair in data)
                    {
                        var student = pair.Value;
                        if (student != null)
                        {
                            var item = lstStudents.Items.Add(student.NomEtudiant ?? "");
                            item.SubItems.Add(student.PrenomEtudiant ?? "");
                            item.SubItems.Add(student.Classe ?? "");
                            item.SubItems.Add(student.RaisonSociale ?? "");
                            item.SubItems.Add(student.EmailContact ?? "");
                            item.Tag = student; // Stocke l'objet Student pour un accès facile
                        }
                    }

                    lblHeader.Text = $"Liste des stages ({data.Count} élèves)";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des données: {ex.Message}", 
                    "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// Gestionnaire d'événement pour le bouton Contact
        /// </summary>
        private void BtnContact_Click(object sender, EventArgs e)
        {
            if (lstStudents.SelectedItems.Count > 0)
            {
                var student = (Student)lstStudents.SelectedItems[0].Tag;
                var contactForm = new ContactForm(student, currentUser);
                contactForm.ShowDialog();
            }
        }

        /// <summary>
        /// Gestionnaire d'événement pour le bouton Actualiser
        /// </summary>
        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        /// <summary>
        /// Gestionnaire d'événement pour le bouton Import
        /// </summary>
        private void BtnImport_Click(object sender, EventArgs e)
        {
            // TODO: Implémenter l'import depuis Excel
            MessageBox.Show("Fonctionnalité à venir", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Active/désactive le bouton Contact en fonction de la sélection
        /// </summary>
        private void LstStudents_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnContact.Enabled = lstStudents.SelectedItems.Count > 0;
        }
        
        /// <summary>
        /// Gestionnaire d'événement pour le bouton Tableau de bord
        /// </summary>
        private void BtnDashboard_Click(object sender, EventArgs e)
        {
            // Réinitialiser les couleurs des boutons
            ResetMenuButtonColors();
            btnDashboard.BackColor = ModernTheme.PrimaryColor;
            
            // Changer le titre
            lblHeader.Text = "Tableau de bord";
            
            // TODO: Afficher le tableau de bord
            MessageBox.Show("Fonctionnalité à venir : Tableau de bord", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        
        /// <summary>
        /// Gestionnaire d'événement pour le bouton Liste des stages
        /// </summary>
        private void BtnStageList_Click(object sender, EventArgs e)
        {
            // Réinitialiser les couleurs des boutons
            ResetMenuButtonColors();
            btnStageList.BackColor = ModernTheme.PrimaryColor;
            
            // Changer le titre
            lblHeader.Text = "Liste des stages";
            
            // Afficher la liste des stages (déjà visible)
        }
        
        /// <summary>
        /// Gestionnaire d'événement pour le bouton Profil
        /// </summary>
        private void BtnProfile_Click(object sender, EventArgs e)
        {
            // Réinitialiser les couleurs des boutons
            ResetMenuButtonColors();
            btnProfile.BackColor = ModernTheme.PrimaryColor;
            
            // Changer le titre
            lblHeader.Text = "Mon profil";
            
            // TODO: Afficher le profil
            MessageBox.Show("Fonctionnalité à venir : Profil", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        
        /// <summary>
        /// Gestionnaire d'événement pour le bouton Paramètres
        /// </summary>
        private void BtnSettings_Click(object sender, EventArgs e)
        {
            // Réinitialiser les couleurs des boutons
            ResetMenuButtonColors();
            btnSettings.BackColor = ModernTheme.PrimaryColor;
            
            // Changer le titre
            lblHeader.Text = "Paramètres";
            
            // TODO: Afficher les paramètres
            MessageBox.Show("Fonctionnalité à venir : Paramètres", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        
        /// <summary>
        /// Gestionnaire d'événement pour le bouton Déconnexion
        /// </summary>
        private void BtnLogout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Êtes-vous sûr de vouloir vous déconnecter ?", "Confirmation", 
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Close();
            }
        }
        
        /// <summary>
        /// Réinitialise les couleurs des boutons du menu
        /// </summary>
        private void ResetMenuButtonColors()
        {
            btnDashboard.BackColor = Color.Transparent;
            btnStageList.BackColor = Color.Transparent;
            btnProfile.BackColor = Color.Transparent;
            btnSettings.BackColor = Color.Transparent;
        }
    }
}
