using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using FireSharp.Response;
using Newtonsoft.Json;
using System.Threading.Tasks;

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

        /// <summary>
        /// Constructeur du formulaire principal
        /// </summary>
        /// <param name="user">L'utilisateur connecté (entreprise)</param>
        public MainForm(User user)
        {
            currentUser = user;
            InitializeComponent();
            
            // Configuration après chargement pour s'assurer que les contrôles sont créés
            this.Load += async (s, e) =>
            {
                // Initialisation des événements pour les boutons
                if (Controls.Find("btnContact", true).Length > 0)
                    ((Button)Controls.Find("btnContact", true)[0]).Click += BtnContact_Click;
                    
                if (Controls.Find("btnRefresh", true).Length > 0)
                    ((Button)Controls.Find("btnRefresh", true)[0]).Click += BtnRefresh_Click;
                    
                if (Controls.Find("btnImport", true).Length > 0)
                    ((Button)Controls.Find("btnImport", true)[0]).Click += BtnImport_Click;
                    
                if (Controls.Find("btnDashboard", true).Length > 0)
                    ((Button)Controls.Find("btnDashboard", true)[0]).Click += BtnDashboard_Click;
                    
                if (Controls.Find("btnStageList", true).Length > 0)
                    ((Button)Controls.Find("btnStageList", true)[0]).Click += BtnStageList_Click;
                    
                if (Controls.Find("btnProfile", true).Length > 0)
                    ((Button)Controls.Find("btnProfile", true)[0]).Click += BtnProfile_Click;
                    
                if (Controls.Find("btnSettings", true).Length > 0)
                    ((Button)Controls.Find("btnSettings", true)[0]).Click += BtnSettings_Click;
                    
                if (Controls.Find("btnLogout", true).Length > 0)
                    ((Button)Controls.Find("btnLogout", true)[0]).Click += BtnLogout_Click;
                
                // Configuration de la ListView
                if (Controls.Find("lstStudents", true).Length > 0)
                {
                    ListView lstStudents = (ListView)Controls.Find("lstStudents", true)[0];
                    lstStudents.SelectedIndexChanged += LstStudents_SelectedIndexChanged;
                    
                    // Initialisation de la liste et chargement des données
                    InitializeListView();
                    
                    // Vérification des données dans Firebase
                    bool dataExists = await CheckFirebaseData();
                    if (!dataExists)
                    {
                        DialogResult result = MessageBox.Show(
                            "Aucune donnée trouvée dans Firebase. Voulez-vous ajouter des données de test ?",
                            "Base de données vide",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);
                            
                        if (result == DialogResult.Yes)
                        {
                            await AddTestData();
                        }
                    }
                    
                    // Chargement des données
                    LoadData();
                }
                
                // Affichage des informations utilisateur
                if (Controls.Find("lblUserInfo", true).Length > 0)
                    ((Label)Controls.Find("lblUserInfo", true)[0]).Text = $"Bienvenue\n{currentUser.CompanyName}";
            };
        }

        /// <summary>
        /// Configure la ListView avec les colonnes appropriées
        /// </summary>
        private void InitializeListView()
        {
            // Récupération de la ListView par son nom
            ListView lstStudents = null;
            if (Controls.Find("lstStudents", true).Length > 0)
                lstStudents = (ListView)Controls.Find("lstStudents", true)[0];
            else
            {
                MessageBox.Show("Le contrôle ListView 'lstStudents' est introuvable.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
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
        /// Charge les données des étudiants depuis Firebase
        /// </summary>
        private async void LoadData()
        {
            try
            {
                // Récupération de la ListView
                ListView lstStudents = null;
                Label lblHeader = null;
                
                if (Controls.Find("lstStudents", true).Length > 0)
                    lstStudents = (ListView)Controls.Find("lstStudents", true)[0];
                else
                {
                    MessageBox.Show("Le contrôle ListView 'lstStudents' est introuvable.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                
                if (Controls.Find("lblHeader", true).Length > 0)
                    lblHeader = (Label)Controls.Find("lblHeader", true)[0];
                
                lstStudents.Items.Clear();
                this.Cursor = Cursors.WaitCursor;

                // Récupération des données depuis Firebase
                MessageBox.Show("Tentative de connexion à Firebase...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                try
                {
                    var response = await FirebaseManager.Client.GetAsync("students");
                    
                    if (response == null)
                    {
                        MessageBox.Show("La réponse de Firebase est null", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    
                    if (string.IsNullOrEmpty(response.Body))
                    {
                        MessageBox.Show("Aucune donnée reçue de Firebase (réponse vide)", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    // Affichage pour déboguer
                    // MessageBox.Show($"Données reçues de Firebase: {response.Body.Substring(0, Math.Min(100, response.Body.Length))}...", "Débogage", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    // Conversion des données JSON en objets Student
                    var data = JsonConvert.DeserializeObject<Dictionary<string, Student>>(response.Body);
                    
                    if (data == null || data.Count == 0)
                    {
                        MessageBox.Show("Aucun étudiant trouvé dans la base de données", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    
                    // Ajout de chaque étudiant dans la ListView
                    foreach (var pair in data)
                    {
                        var student = pair.Value;
                        if (student != null)
                        {
                            var item = lstStudents.Items.Add(student.NomEtudiant ?? "[Nom inconnu]");
                            item.SubItems.Add(student.PrenomEtudiant ?? "[Prénom inconnu]");
                            item.SubItems.Add(student.Classe ?? "[Classe inconnue]");
                            item.SubItems.Add(student.RaisonSociale ?? "[Entreprise inconnue]");
                            item.SubItems.Add(student.EmailContact ?? "[Email inconnu]");
                            item.Tag = student; // Stocke l'objet Student pour un accès facile
                        }
                    }

                    if (lblHeader != null)
                        lblHeader.Text = $"Liste des stages ({data.Count} élèves)";
                    
                    MessageBox.Show($"{data.Count} étudiants chargés avec succès", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur lors de la communication avec Firebase: {ex.Message}\n\nStackTrace: {ex.StackTrace.Substring(0, Math.Min(200, ex.StackTrace.Length))}", 
                        "Erreur Firebase", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des données: {ex.Message}\n\nStackTrace: {ex.StackTrace.Substring(0, Math.Min(200, ex.StackTrace.Length))}", 
                    "Erreur générale", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            ListView lstStudents = null;
            
            if (Controls.Find("lstStudents", true).Length > 0)
                lstStudents = (ListView)Controls.Find("lstStudents", true)[0];
            else
            {
                MessageBox.Show("Le contrôle ListView 'lstStudents' est introuvable.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            if (lstStudents.SelectedItems.Count > 0)
            {
                var student = (Student)lstStudents.SelectedItems[0].Tag;
                
                // Création d'un objet Stage à partir du Student pour être compatible avec ContactForm
                var stage = new Stage
                {
                    NomEtudiant = student.NomEtudiant,
                    PrenomEtudiant = student.PrenomEtudiant,
                    Classe = student.Classe,
                    Annee = student.Annee,
                    Periode = student.Periode,
                    RaisonSociale = student.RaisonSociale,
                    Ville = student.Ville,
                    PaysHorsFrance = student.PaysHorsFrance,
                    EmailContact = student.EmailContact,
                    TelephoneContact = student.TelephoneContact,
                    PrenomContact = student.PrenomContact
                };
                
                var contactForm = new ContactForm(stage, currentUser);
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
        /// Vérifie si des données existent déjà dans Firebase
        /// </summary>
        /// <returns>True si des données existent, False sinon</returns>
        private async Task<bool> CheckFirebaseData()
        {
            try
            {
                var response = await FirebaseManager.Client.GetAsync("students");
                if (response == null || string.IsNullOrEmpty(response.Body) || response.Body == "null")
                    return false;
                    
                var data = JsonConvert.DeserializeObject<Dictionary<string, Student>>(response.Body);
                return data != null && data.Count > 0;
            }
            catch
            {
                return false;
            }
        }
        
        /// <summary>
        /// Ajoute des données de test dans Firebase
        /// </summary>
        private async Task AddTestData()
        {
            try
            {
                MessageBox.Show("Ajout de données de test dans Firebase...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                // Créer quelques étudiants de test
                var students = new Dictionary<string, Student>
                {
                    {
                        Guid.NewGuid().ToString(),
                        new Student
                        {
                            Id = Guid.NewGuid().ToString(),
                            NomEtudiant = "Dubois",
                            PrenomEtudiant = "Thomas",
                            Classe = "BTS SIO 2",
                            Annee = "2024",
                            Periode = "Mai-Juin",
                            RaisonSociale = "Dev-Sys",
                            Ville = "Paris",
                            PaysHorsFrance = false,
                            EmailContact = "tdubois@example.com",
                            TelephoneContact = "0123456789",
                            PrenomContact = "Jean"
                        }
                    },
                    {
                        Guid.NewGuid().ToString(),
                        new Student
                        {
                            Id = Guid.NewGuid().ToString(),
                            NomEtudiant = "Martin",
                            PrenomEtudiant = "Sophie",
                            Classe = "BTS SIO 1",
                            Annee = "2024",
                            Periode = "Janvier-Février",
                            RaisonSociale = "InfoTech",
                            Ville = "Lyon",
                            PaysHorsFrance = false,
                            EmailContact = "smartin@example.com",
                            TelephoneContact = "0234567890",
                            PrenomContact = "Marie"
                        }
                    },
                    {
                        Guid.NewGuid().ToString(),
                        new Student
                        {
                            Id = Guid.NewGuid().ToString(),
                            NomEtudiant = "Lefebvre",
                            PrenomEtudiant = "Lucas",
                            Classe = "BTS SIO 2",
                            Annee = "2024",
                            Periode = "Mars-Avril",
                            RaisonSociale = "NetVision",
                            Ville = "Marseille",
                            PaysHorsFrance = false,
                            EmailContact = "llefebvre@example.com",
                            TelephoneContact = "0345678901",
                            PrenomContact = "Pierre"
                        }
                    }
                };
                
                // Ajouter chaque étudiant dans Firebase
                foreach (var pair in students)
                {
                    await FirebaseManager.Client.SetAsync($"students/{pair.Key}", pair.Value);
                }
                
                MessageBox.Show("Données de test ajoutées avec succès !", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'ajout des données de test : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
            Button btnContact = null;
            ListView lstStudents = sender as ListView;
            
            if (lstStudents == null)
            {
                if (Controls.Find("lstStudents", true).Length > 0)
                    lstStudents = (ListView)Controls.Find("lstStudents", true)[0];
            }
            
            if (Controls.Find("btnContact", true).Length > 0)
                btnContact = (Button)Controls.Find("btnContact", true)[0];
            
            if (btnContact != null && lstStudents != null)
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
