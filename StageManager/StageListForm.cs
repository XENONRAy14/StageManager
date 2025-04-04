using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using FireSharp.Response;
using OfficeOpenXml;
using System.Linq;

namespace StageManager
{
    public partial class StageListForm : Form
    {
        private DataGridView dgvStages;
        private TextBox txtSearch;
        private Button btnSearch;
        private Button btnViewDetails;
        private Button btnContact;
        private Button btnExport;
        private Button btnBack;
        private ComboBox cmbFilter;
        private Panel topPanel;
        private Panel bottomPanel;
        private Label lblTitle;
        
        // Mode de fonctionnement (normal ou contact)
        private bool contactMode;
        
        // Utilisateur connecté
        private User currentUser;

        public StageListForm(bool contactMode = false, User user = null)
        {
            this.contactMode = contactMode;
            this.currentUser = user;
            InitializeComponent();
            LoadStages();
        }

        private void InitializeComponent()
        {
            try 
            {
                // Configuration de base du formulaire
                this.ClientSize = new Size(1000, 600);
                this.StartPosition = FormStartPosition.CenterScreen;
                this.FormBorderStyle = FormBorderStyle.FixedSingle;
                this.MaximizeBox = false;
                this.Text = contactMode ? "Stage Manager - Contacter un élève" : "Stage Manager - Liste des élèves";
                this.BackColor = Color.White;
                
                // Initialisation des contrôles
                this.dgvStages = new DataGridView();
                this.txtSearch = new TextBox();
                this.btnSearch = new Button();
                this.btnViewDetails = new Button();
                this.btnContact = new Button();
                this.btnExport = new Button();
                this.btnBack = new Button();
                this.cmbFilter = new ComboBox();
                
                // Panneau supérieur pour les contrôles de recherche
                topPanel = new Panel
                {
                    Dock = DockStyle.Top,
                    Height = 80,
                    BackColor = Color.White,
                    Padding = new Padding(10)
                };
                
                // Panneau inférieur pour les boutons d'action
                bottomPanel = new Panel
                {
                    Dock = DockStyle.Bottom,
                    Height = 60,
                    BackColor = Color.FromArgb(245, 245, 245),
                    Padding = new Padding(10)
                };
                
                // Titre de la page
                lblTitle = new Label
                {
                    Text = contactMode ? "Sélectionnez un élève à contacter" : "Liste des élèves",
                    Font = new Font("Segoe UI", 14, FontStyle.Bold),
                    ForeColor = ModernTheme.PrimaryColor,
                    AutoSize = true,
                    Location = new Point(20, 15)
                };

                // Configuration de la DataGridView
                this.dgvStages.Dock = DockStyle.Fill;
                this.dgvStages.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                this.dgvStages.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                this.dgvStages.MultiSelect = false;
                this.dgvStages.AllowUserToAddRows = false;
                this.dgvStages.AllowUserToDeleteRows = false;
                this.dgvStages.ReadOnly = true;
                this.dgvStages.BorderStyle = BorderStyle.None;
                this.dgvStages.BackgroundColor = Color.White;
                this.dgvStages.RowHeadersVisible = false;
                this.dgvStages.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
                this.dgvStages.ColumnHeadersDefaultCellStyle.BackColor = ModernTheme.PrimaryColor;
                this.dgvStages.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                this.dgvStages.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                this.dgvStages.ColumnHeadersHeight = 40;
                this.dgvStages.RowTemplate.Height = 35;
                this.dgvStages.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);

                // Définition des colonnes
                this.dgvStages.Columns.AddRange(new DataGridViewColumn[]
                {
                    new DataGridViewTextBoxColumn { Name = "NomEtudiant", HeaderText = "Nom de l'étudiant" },
                    new DataGridViewTextBoxColumn { Name = "PrenomEtudiant", HeaderText = "Prénom de l'étudiant" },
                    new DataGridViewTextBoxColumn { Name = "Classe", HeaderText = "Classe" },
                    new DataGridViewTextBoxColumn { Name = "RaisonSociale", HeaderText = "Entreprise" },
                    new DataGridViewTextBoxColumn { Name = "Ville", HeaderText = "Ville" },
                    new DataGridViewTextBoxColumn { Name = "PrenomContact", HeaderText = "Contact" },
                    new DataGridViewTextBoxColumn { Name = "TelephoneContact", HeaderText = "Téléphone" }
                });

                // Configuration de la recherche
                this.txtSearch.Location = new Point(20, 45);
                this.txtSearch.Size = new Size(250, 30);
                this.txtSearch.Text = "Rechercher...";
                this.txtSearch.ForeColor = Color.Gray;
                this.txtSearch.Font = new Font("Segoe UI", 10);
                this.txtSearch.BorderStyle = BorderStyle.FixedSingle;
                this.txtSearch.GotFocus += (s, e) => {
                    if (txtSearch.Text == "Rechercher...")
                    {
                        txtSearch.Text = "";
                        txtSearch.ForeColor = Color.Black;
                    }
                };
                this.txtSearch.LostFocus += (s, e) => {
                    if (string.IsNullOrWhiteSpace(txtSearch.Text))
                    {
                        txtSearch.Text = "Rechercher...";
                        txtSearch.ForeColor = Color.Gray;
                    }
                };

                // Configuration du filtre
                this.cmbFilter.Location = new Point(290, 45);
                this.cmbFilter.Size = new Size(180, 30);
                this.cmbFilter.Font = new Font("Segoe UI", 10);
                this.cmbFilter.DropDownStyle = ComboBoxStyle.DropDownList;
                this.cmbFilter.Items.AddRange(new object[] { 
                    "Tous", 
                    "Par nom",
                    "Par entreprise",
                    "Par ville",
                    "Par contact"
                });
                this.cmbFilter.SelectedIndex = 0;

                // Configuration du bouton de recherche
                this.btnSearch.Location = new Point(490, 45);
                this.btnSearch.Size = new Size(100, 30);
                this.btnSearch.Text = "Rechercher";
                this.btnSearch.Font = new Font("Segoe UI", 9);
                this.btnSearch.FlatStyle = FlatStyle.Flat;
                this.btnSearch.BackColor = ModernTheme.PrimaryColor;
                this.btnSearch.ForeColor = Color.White;
                this.btnSearch.Cursor = Cursors.Hand;
                this.btnSearch.Click += BtnSearch_Click;


                // Configuration du bouton de détails
                this.btnViewDetails.Size = new Size(150, 30);
                this.btnViewDetails.Text = "Voir les détails";
                this.btnViewDetails.Font = new Font("Segoe UI", 9);
                this.btnViewDetails.FlatStyle = FlatStyle.Flat;
                this.btnViewDetails.BackColor = ModernTheme.PrimaryColor;
                this.btnViewDetails.ForeColor = Color.White;
                this.btnViewDetails.Cursor = Cursors.Hand;
                this.btnViewDetails.Enabled = false;
                this.btnViewDetails.Click += BtnViewDetails_Click;

                // Configuration du bouton de contact
                this.btnContact.Size = new Size(150, 30);
                this.btnContact.Text = "Contacter";
                this.btnContact.Font = new Font("Segoe UI", 9);
                this.btnContact.FlatStyle = FlatStyle.Flat;
                this.btnContact.BackColor = Color.FromArgb(40, 167, 69); // Vert
                this.btnContact.ForeColor = Color.White;
                this.btnContact.Cursor = Cursors.Hand;
                this.btnContact.Enabled = false;
                this.btnContact.Click += BtnContact_Click;

                // Configuration du bouton d'export
                this.btnExport.Size = new Size(150, 30);
                this.btnExport.Text = "Exporter";
                this.btnExport.Font = new Font("Segoe UI", 9);
                this.btnExport.FlatStyle = FlatStyle.Flat;
                this.btnExport.BackColor = Color.FromArgb(0, 123, 255); // Bleu
                this.btnExport.ForeColor = Color.White;
                this.btnExport.Cursor = Cursors.Hand;
                this.btnExport.Click += BtnExport_Click;

                // Les boutons seront positionnés dans le panneau fixe plus bas

                // Ajout des contrôles au formulaire
                topPanel.Controls.Add(lblTitle);
                topPanel.Controls.Add(txtSearch);
                topPanel.Controls.Add(cmbFilter);
                topPanel.Controls.Add(btnSearch);

                // Ajouter un message explicatif si en mode contact
                if (contactMode)
                {
                    var lblInstruction = new Label
                    {
                        Text = "Sélectionnez un étudiant dans la liste puis cliquez sur 'Contacter' pour lui envoyer un message",
                        Font = new Font("Segoe UI", 9, FontStyle.Italic),
                        ForeColor = Color.DimGray,
                        AutoSize = true,
                        Location = new Point(20, lblTitle.Bottom + 5)
                    };
                    topPanel.Controls.Add(lblInstruction);
                }

                // Créer un panel fixe en bas pour les boutons
                Panel fixedButtonPanel = new Panel
                {
                    Dock = DockStyle.Bottom,
                    Height = 60,
                    BackColor = Color.FromArgb(245, 245, 245)
                };

                // Positionner les boutons côte à côte au centre du panel
                const int buttonSpacing = 20;
                const int buttonY = 15;
                int totalWidth = btnViewDetails.Width + btnContact.Width + btnExport.Width + 2 * buttonSpacing;
                int startX = (this.ClientSize.Width - totalWidth) / 2;

                btnViewDetails.Location = new Point(startX, buttonY);
                btnContact.Location = new Point(startX + btnViewDetails.Width + buttonSpacing, buttonY);
                btnExport.Location = new Point(startX + btnViewDetails.Width + btnContact.Width + 2 * buttonSpacing, buttonY);

                // Ajouter les boutons au panel fixe
                fixedButtonPanel.Controls.Add(btnViewDetails);
                fixedButtonPanel.Controls.Add(btnContact);
                fixedButtonPanel.Controls.Add(btnExport);

                // Ajouter les panneaux au formulaire
                this.Controls.Add(dgvStages);
                this.Controls.Add(fixedButtonPanel);
                this.Controls.Add(topPanel);
                
                // S'assurer que les boutons sont visibles
                fixedButtonPanel.BringToFront();

                // Ajout de l'événement de sélection
                this.dgvStages.SelectionChanged += DgvStages_SelectionChanged;
                
                // Double-clic pour voir les détails
                this.dgvStages.CellDoubleClick += (s, e) => {
                    if (e.RowIndex >= 0)
                    {
                        BtnViewDetails_Click(s, e);
                    }
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur dans InitializeComponent : {ex.Message}");
            }
        }

        private async void LoadStages()
        {
            try
            {
                dgvStages.Rows.Clear();

                // Charger directement depuis la table students
                var response = await FirebaseManager.Client.GetAsync("students");
                
                if (response == null || string.IsNullOrEmpty(response.Body) || response.Body == "null")
                {
                    MessageBox.Show("Aucun étudiant trouvé dans la base de données.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var students = response.ResultAs<Dictionary<string, Stage>>();
                
                if (students != null)
                {
                    foreach (var student in students.Values)
                    {
                        if (student != null)
                        {
                            dgvStages.Rows.Add(
                                student.NomEtudiant ?? "",
                                student.PrenomEtudiant ?? "",
                                student.Classe ?? "",
                                student.RaisonSociale ?? "",
                                student.Ville ?? "",
                                student.PrenomContact ?? "",
                                student.TelephoneContact ?? ""
                            );
                        }
                    }
                }

                if (dgvStages.Rows.Count == 0)
                {
                    MessageBox.Show("Aucun étudiant trouvé.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show($"{dgvStages.Rows.Count} étudiants chargés avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des étudiants : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            string searchText = txtSearch.Text.ToLower();
            if (searchText == "rechercher...") searchText = "";

            foreach (DataGridViewRow row in dgvStages.Rows)
            {
                bool visible = false;
                Stage stage = (Stage)row.Tag;

                switch (cmbFilter.SelectedIndex)
                {
                    case 0: // Tous
                        visible = false;
                        for (int i = 0; i < row.Cells.Count; i++)
                        {
                            if (row.Cells[i].Value?.ToString().ToLower().Contains(searchText) ?? false)
                            {
                                visible = true;
                                break;
                            }
                        }
                        break;
                    case 1: // Par nom
                        visible = (stage.NomEtudiant + " " + stage.PrenomEtudiant).ToLower().Contains(searchText);
                        break;
                    case 2: // Par entreprise
                        visible = stage.RaisonSociale.ToLower().Contains(searchText);
                        break;
                    case 3: // Par ville
                        visible = stage.Ville.ToLower().Contains(searchText);
                        break;
                    case 4: // Par contact
                        visible = stage.PrenomContact.ToLower().Contains(searchText);
                        break;
                }

                row.Visible = visible;
            }
        }

        private void BtnViewDetails_Click(object sender, EventArgs e)
        {
            if (dgvStages.SelectedRows.Count > 0)
            {
                Stage stage = (Stage)dgvStages.SelectedRows[0].Tag;
                var detailsForm = new StageDetailsForm(stage);
                detailsForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un élève.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        
        private void BtnContact_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvStages.SelectedRows.Count > 0)
                {
                    // Vérifier si la ligne sélectionnée a un objet Stage associé
                    DataGridViewRow selectedRow = dgvStages.SelectedRows[0];
                    if (selectedRow.Tag == null)
                    {
                        MessageBox.Show("Impossible de récupérer les informations de l'étudiant sélectionné.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    Stage stage = (Stage)selectedRow.Tag;
                    
                    // Créer un Student à partir des données de la ligne sélectionnée
                    var student = new Student
                    {
                        Id = stage.Id ?? "unknown",
                        NomEtudiant = stage.NomEtudiant ?? selectedRow.Cells["NomEtudiant"].Value?.ToString() ?? "Nom inconnu",
                        PrenomEtudiant = stage.PrenomEtudiant ?? selectedRow.Cells["PrenomEtudiant"].Value?.ToString() ?? "Prénom inconnu",
                        Classe = stage.Classe ?? selectedRow.Cells["Classe"].Value?.ToString() ?? "Classe inconnue",
                        EmailContact = stage.EmailContact ?? "email@example.com",
                        TelephoneContact = stage.TelephoneContact ?? selectedRow.Cells["TelephoneContact"].Value?.ToString() ?? "Téléphone inconnu",
                        Ville = stage.Ville ?? selectedRow.Cells["Ville"].Value?.ToString() ?? "Ville inconnue",
                        RaisonSociale = stage.RaisonSociale ?? selectedRow.Cells["RaisonSociale"].Value?.ToString() ?? "Entreprise inconnue",
                        PrenomContact = stage.PrenomContact ?? selectedRow.Cells["Contact"].Value?.ToString() ?? "Contact inconnu"
                    };
                    
                    // Utiliser l'utilisateur connecté ou créer un utilisateur par défaut si null
                    var company = currentUser ?? new User
                    {
                        Email = "entreprise@example.com",
                        CompanyName = "Entreprise Example"
                    };
                    
                    var contactForm = new ContactForm(student, company);
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
