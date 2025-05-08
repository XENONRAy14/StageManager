using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using FireSharp.Response;
using OfficeOpenXml;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace StageManager
{
    public partial class StageListForm : Form
    {
        // Mode de fonctionnement (normal ou contact)
        private bool contactMode;
        
        // Utilisateur connecté
        private User currentUser;

        public StageListForm(bool contactMode = false, User user = null)
        {
            this.contactMode = contactMode;
            this.currentUser = user;
            InitializeComponent();
            
            // Configuration supplémentaire après InitializeComponent
            this.Text = contactMode ? "Stage Manager - Contacter un élève" : "Stage Manager - Liste des élèves";
            
            this.Load += (s, e) =>
            {
                // Récupération et configuration des contrôles UI
                if (Controls.Find("lblTitle", true).Length > 0)
                {
                    Label lblTitle = (Label)Controls.Find("lblTitle", true)[0];
                    lblTitle.Text = contactMode ? "Sélectionnez un élève à contacter" : "Liste des élèves";
                    lblTitle.ForeColor = ModernTheme.PrimaryColor;
                }
                
                // Configurer les couleurs des boutons
                if (Controls.Find("btnSearch", true).Length > 0)
                    ((Button)Controls.Find("btnSearch", true)[0]).BackColor = ModernTheme.PrimaryColor;
                    
                if (Controls.Find("btnViewDetails", true).Length > 0)
                    ((Button)Controls.Find("btnViewDetails", true)[0]).BackColor = ModernTheme.AccentColor;
                    
                if (Controls.Find("btnContact", true).Length > 0)
                    ((Button)Controls.Find("btnContact", true)[0]).BackColor = ModernTheme.SuccessColor;
                    
                if (Controls.Find("btnExport", true).Length > 0)
                    ((Button)Controls.Find("btnExport", true)[0]).BackColor = ModernTheme.InfoColor;
                    
                if (Controls.Find("btnBack", true).Length > 0)
                {
                    Button btnBack = (Button)Controls.Find("btnBack", true)[0];
                    btnBack.BackColor = ModernTheme.DangerColor;
                    btnBack.Click += (sender, args) => this.Close();
                }
                
                // Détecter et configurer le DataGridView
                if (Controls.Find("dgvStages", true).Length > 0)
                {
                    DataGridView dgvStages = (DataGridView)Controls.Find("dgvStages", true)[0];
                    dgvStages.SelectionChanged += DgvStages_SelectionChanged;
                }
                
                // Configuration des événements pour les boutons
                if (Controls.Find("btnSearch", true).Length > 0)
                    ((Button)Controls.Find("btnSearch", true)[0]).Click += BtnSearch_Click;
                    
                if (Controls.Find("btnViewDetails", true).Length > 0)
                    ((Button)Controls.Find("btnViewDetails", true)[0]).Click += BtnViewDetails_Click;
                    
                if (Controls.Find("btnContact", true).Length > 0)
                    ((Button)Controls.Find("btnContact", true)[0]).Click += BtnContact_Click;
                    
                if (Controls.Find("btnExport", true).Length > 0)
                    ((Button)Controls.Find("btnExport", true)[0]).Click += BtnExport_Click;
                
                // Charger les données des stages
                LoadStages();
            };
        }

        private async void LoadStages()
        {
            try
            {
                // Récupération du DataGridView par son nom
                DataGridView dgvStages = null;
                if (Controls.Find("dgvStages", true).Length > 0)
                    dgvStages = (DataGridView)Controls.Find("dgvStages", true)[0];
                else
                {
                    MessageBox.Show("Le contrôle DataGridView 'dgvStages' est introuvable.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                
                // Configurer la grille des stages avec des colonnes appropriées
                dgvStages.Columns.Clear();
                dgvStages.Columns.Add("NomEtudiant", "Nom");
                dgvStages.Columns.Add("PrenomEtudiant", "Prénom");
                dgvStages.Columns.Add("Classe", "Classe");
                dgvStages.Columns.Add("RaisonSociale", "Entreprise");
                dgvStages.Columns.Add("Ville", "Ville");
                dgvStages.Columns.Add("Contact", "Contact");
                dgvStages.Columns.Add("TelephoneContact", "Téléphone");
                dgvStages.Columns.Add("EmailContact", "Email");
                dgvStages.Columns.Add("Periode", "Période");
                dgvStages.Columns.Add("Annee", "Année");

                // Styliser les colonnes
                dgvStages.ColumnHeadersDefaultCellStyle.BackColor = ModernTheme.PrimaryColor;
                dgvStages.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dgvStages.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);

                // Mise à jour des contrôles d'interface
                Button btnViewDetails = null;
                Button btnContact = null;
                
                if (Controls.Find("btnViewDetails", true).Length > 0)
                    btnViewDetails = (Button)Controls.Find("btnViewDetails", true)[0];
                
                if (Controls.Find("btnContact", true).Length > 0)
                    btnContact = (Button)Controls.Find("btnContact", true)[0];
                    
                if (btnViewDetails != null) btnViewDetails.Enabled = false;
                if (btnContact != null) btnContact.Enabled = false;

                // Récupérer les données de stages depuis Firebase
                var students = await LoadStagesFromFirebase();
                if (students != null && students.Count > 0)
                {
                    foreach (var student in students)
                    {
                        var rowIndex = dgvStages.Rows.Add();
                        var row = dgvStages.Rows[rowIndex];

                        row.Cells["NomEtudiant"].Value = student.NomEtudiant;
                        row.Cells["PrenomEtudiant"].Value = student.PrenomEtudiant;
                        row.Cells["Classe"].Value = student.Classe;
                        row.Cells["RaisonSociale"].Value = student.RaisonSociale;
                        row.Cells["Ville"].Value = student.Ville;
                        row.Cells["Contact"].Value = student.PrenomContact;
                        row.Cells["TelephoneContact"].Value = student.TelephoneContact;
                        row.Cells["EmailContact"].Value = student.EmailContact;
                        row.Cells["Periode"].Value = student.Periode;
                        row.Cells["Annee"].Value = student.Annee;

                        // Stocker l'objet Stage pour une utilisation ultérieure
                        row.Tag = student;
                    }
                }
                else
                {
                    MessageBox.Show("Aucun stage trouvé dans la base de données.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des stages : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task<List<Stage>> LoadStagesFromFirebase()
        {
            try
            {
                var stages = new List<Stage>();
                
                // Accès direct au nœud "students" qui contient tous les étudiants
                var response = await FirebaseManager.Client.GetAsync("students");
                
                if (response == null || string.IsNullOrEmpty(response.Body))
                {
                    MessageBox.Show("Aucune donnée reçue de Firebase", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return stages;
                }
                
                // Désérialisation des données JSON en dictionnaire d'étudiants
                var data = JsonConvert.DeserializeObject<Dictionary<string, Student>>(response.Body);
                
                if (data != null)
                {
                    foreach (var pair in data)
                    {
                        var student = pair.Value;
                        if (student != null)
                        {
                            // Conversion de Student en Stage pour la compatibilité
                            Stage stage = new Stage
                            {
                                Id = student.Id ?? Guid.NewGuid().ToString(),
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
                            
                            stages.Add(stage);
                        }
                    }
                }

                return stages;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur de connexion à Firebase : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new List<Stage>();
            }
        }

        private void DgvStages_SelectionChanged(object sender, EventArgs e)
        {
            bool hasSelection = dgvStages.SelectedRows.Count > 0;
            btnViewDetails.Enabled = hasSelection;
            btnContact.Enabled = hasSelection;
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                // Récupération des contrôles
                TextBox txtSearch = null;
                DataGridView dgvStages = null;
                ComboBox cmbFilter = null;
                
                if (Controls.Find("txtSearch", true).Length > 0)
                    txtSearch = (TextBox)Controls.Find("txtSearch", true)[0];
                
                if (Controls.Find("dgvStages", true).Length > 0)
                    dgvStages = (DataGridView)Controls.Find("dgvStages", true)[0];
                
                if (Controls.Find("cmbFilter", true).Length > 0)
                    cmbFilter = (ComboBox)Controls.Find("cmbFilter", true)[0];
                
                if (txtSearch == null || dgvStages == null)
                {
                    MessageBox.Show("Certains contrôles nécessaires sont introuvables.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                
                string searchText = txtSearch.Text.ToLower();
                if (searchText == "rechercher...") searchText = "";

                foreach (DataGridViewRow row in dgvStages.Rows)
                {
                    bool visible = true; // Par défaut, montrer toutes les lignes si aucun texte de recherche

                    if (!string.IsNullOrEmpty(searchText))
                    {
                        visible = false; // Cacher par défaut si un texte de recherche est spécifié
                        
                        // Si aucun filtre spécifique n'est sélectionné, rechercher dans toutes les colonnes
                        if (cmbFilter.SelectedIndex == 0)
                        {
                            for (int i = 0; i < row.Cells.Count; i++)
                            {
                                if (row.Cells[i].Value?.ToString().ToLower().Contains(searchText) ?? false)
                                {
                                    visible = true;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            // Pour les filtres spécifiques, utiliser les valeurs des cellules plutôt que l'objet Stage
                            switch (cmbFilter.SelectedIndex)
                            {
                                case 1: // Par nom
                                    string nom = row.Cells["NomEtudiant"].Value?.ToString() ?? "";
                                    string prenom = row.Cells["PrenomEtudiant"].Value?.ToString() ?? "";
                                    visible = (nom + " " + prenom).ToLower().Contains(searchText);
                                    break;
                                case 2: // Par entreprise
                                    visible = (row.Cells["RaisonSociale"].Value?.ToString() ?? "").ToLower().Contains(searchText);
                                    break;
                                case 3: // Par ville
                                    visible = (row.Cells["Ville"].Value?.ToString() ?? "").ToLower().Contains(searchText);
                                    break;
                                case 4: // Par contact
                                    visible = (row.Cells["PrenomContact"].Value?.ToString() ?? "").ToLower().Contains(searchText);
                                    break;
                            }
                        }
                    }

                    row.Visible = visible;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la recherche : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnViewDetails_Click(object sender, EventArgs e)
        {
            try
            {
                // Récupération du DataGridView
                DataGridView dgvStages = null;
                if (Controls.Find("dgvStages", true).Length > 0)
                    dgvStages = (DataGridView)Controls.Find("dgvStages", true)[0];
                else
                {
                    MessageBox.Show("Le contrôle DataGridView 'dgvStages' est introuvable.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                
                if (dgvStages.SelectedRows.Count > 0)
                {
                    DataGridViewRow selectedRow = dgvStages.SelectedRows[0];
                    if (selectedRow.Tag == null)
                    {
                        MessageBox.Show("Impossible de récupérer les informations de l'étudiant sélectionné.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    
                    Stage stage = (Stage)selectedRow.Tag;
                    var detailsForm = new StageDetailsForm(stage);
                    detailsForm.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Veuillez sélectionner un élève.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'affichage des détails : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void BtnContact_Click(object sender, EventArgs e)
        {
            try
            {
                // Récupération du DataGridView
                DataGridView dgvStages = null;
                if (Controls.Find("dgvStages", true).Length > 0)
                    dgvStages = (DataGridView)Controls.Find("dgvStages", true)[0];
                else
                {
                    MessageBox.Show("Le contrôle DataGridView 'dgvStages' est introuvable.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                
                if (dgvStages.SelectedRows.Count > 0)
                {
                    // Vérifier si la ligne sélectionnée a un objet Stage associé
                    DataGridViewRow selectedRow = dgvStages.SelectedRows[0];
                    
                    // Si la ligne n'a pas d'objet Stage associé, créer un objet Stage à partir des données de la ligne
                    Stage stage;
                    if (selectedRow.Tag == null)
                    {
                        // Créer un nouvel objet Stage à partir des données de la ligne
                        stage = new Stage
                        {
                            Id = Guid.NewGuid().ToString(),
                            NomEtudiant = selectedRow.Cells["NomEtudiant"].Value?.ToString(),
                            PrenomEtudiant = selectedRow.Cells["PrenomEtudiant"].Value?.ToString(),
                            Classe = selectedRow.Cells["Classe"].Value?.ToString(),
                            RaisonSociale = selectedRow.Cells["RaisonSociale"].Value?.ToString(),
                            Ville = selectedRow.Cells["Ville"].Value?.ToString(),
                            PrenomContact = selectedRow.Cells["Contact"].Value?.ToString(),
                            TelephoneContact = selectedRow.Cells["TelephoneContact"].Value?.ToString(),
                            EmailContact = selectedRow.Cells["EmailContact"].Value?.ToString() ?? "email@example.com" // Valeur par défaut
                        };
                        
                        // Associer l'objet Stage à la ligne pour les futures utilisations
                        selectedRow.Tag = stage;
                    }
                    else
                    {
                        stage = (Stage)selectedRow.Tag;
                    }
                    
                    // On utilise directement l'objet Stage au lieu de créer un Student
                    
                    // Utiliser l'utilisateur connecté ou créer un utilisateur par défaut si null
                    var company = currentUser ?? new User
                    {
                        Email = "entreprise@example.com",
                        CompanyName = "Entreprise Example"
                    };
                    
                    var contactForm = new ContactForm(stage, company);
                    if (contactMode)
                    {
                        contactForm.FormClosed += (s, args) => this.Close(); // Fermer la liste après contact en mode contact
                    }
                    contactForm.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Veuillez sélectionner un élève à contacter.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'ouverture du formulaire de contact : {ex.Message}\n\nDétails : {ex.StackTrace}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            // Récupération du DataGridView
            DataGridView dgvStages = null;
            if (Controls.Find("dgvStages", true).Length > 0)
                dgvStages = (DataGridView)Controls.Find("dgvStages", true)[0];
            else
            {
                MessageBox.Show("Le contrôle DataGridView 'dgvStages' est introuvable.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "Fichier Excel|*.xlsx";
                saveDialog.Title = "Exporter les stages";
                saveDialog.FileName = $"Stages_Export_{DateTime.Now:yyyy-MM-dd}";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (var package = new ExcelPackage())
                        {
                            var worksheet = package.Workbook.Worksheets.Add("Stages");

                            // En-têtes
                            for (int i = 0; i < dgvStages.Columns.Count; i++)
                            {
                                worksheet.Cells[1, i + 1].Value = dgvStages.Columns[i].HeaderText;
                            }

                            // Données
                            for (int i = 0; i < dgvStages.Rows.Count; i++)
                            {
                                if (dgvStages.Rows[i].Visible)
                                {
                                    for (int j = 0; j < dgvStages.Columns.Count; j++)
                                    {
                                        worksheet.Cells[i + 2, j + 1].Value = dgvStages.Rows[i].Cells[j].Value;
                                    }
                                }
                            }

                            // Formatage
                            worksheet.Cells.AutoFitColumns();

                            // Sauvegarde
                            package.SaveAs(new System.IO.FileInfo(saveDialog.FileName));
                        }

                        MessageBox.Show("Export terminé avec succès!", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Erreur lors de l'export : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
