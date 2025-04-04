using System;
using System.Windows.Forms;
using System.Drawing;

namespace StageManager
{
    public partial class StageDetailsForm : Form
    {
        private Stage stage;
        private TableLayoutPanel tableLayout;

        public StageDetailsForm(Stage stage)
        {
            this.stage = stage;
            InitializeComponent();
            PopulateFields();
        }

        private void InitializeComponent()
        {
            this.tableLayout = new TableLayoutPanel();
            
            // Configuration du TableLayoutPanel
            this.tableLayout.Dock = DockStyle.Fill;
            this.tableLayout.AutoScroll = true;
            this.tableLayout.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            this.tableLayout.ColumnCount = 2;
            this.tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            this.tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));
            
            // Configuration du formulaire
            this.Text = "Détails du Stage";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterParent;
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            
            this.Controls.Add(this.tableLayout);
        }

        private void PopulateFields()
        {
            AddField("Informations de l'étudiant", "", true);
            AddField("Nom", stage.NomEtudiant);
            AddField("Prénom", stage.PrenomEtudiant);
            AddField("Classe", stage.Classe);
            AddField("Téléphone", stage.TelephoneEtudiant);
            AddField("Email", stage.EmailEtudiant);

            AddField("Informations du stage", "", true);
            AddField("Année", stage.Annee);
            AddField("Période", stage.Periode);
            AddField("Suivi par", stage.SuiviPar);

            AddField("Informations de l'entreprise", "", true);
            AddField("Raison sociale", stage.RaisonSociale);
            AddField("Adresse", stage.AdresseOrganisation);
            AddField("Code postal", stage.CodePostal);
            AddField("Ville", stage.Ville);
            AddField("Pays hors France", stage.PaysHorsFrance ? "Oui" : "Non");
            AddField("Téléphone", stage.TelephoneOrganisation);
            AddField("Site web", stage.SiteWeb);

            AddField("Contact principal", "", true);
            AddField("Civilité", stage.CiviliteContact);
            AddField("Nom", stage.NomContact);
            AddField("Prénom", stage.PrenomContact);
            AddField("Téléphone", stage.TelephoneContact);
            AddField("Email", stage.EmailContact);

            AddField("Tuteur", "", true);
            AddField("Civilité", stage.CiviliteTuteur);
            AddField("Nom", stage.NomTuteur);
            AddField("Prénom", stage.PrenomTuteur);
            AddField("Téléphone", stage.TelephoneTuteur);
            AddField("Email", stage.EmailTuteur);

            AddField("Responsable", "", true);
            AddField("Civilité", stage.CiviliteResponsable);
            AddField("Nom", stage.NomResponsable);
            AddField("Prénom", stage.PrenomResponsable);
            AddField("Téléphone", stage.TelephoneResponsable);
            AddField("Email", stage.EmailResponsable);

            AddField("Description des activités", "", true);
            AddField("", stage.DescriptionActivites, false, true);
        }

        private void AddField(string label, string value, bool isHeader = false, bool isMultiline = false)
        {
            int rowIndex = tableLayout.RowCount++;
            tableLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            if (isHeader)
            {
                var headerLabel = new Label
                {
                    Text = label,
                    Font = new Font(Font.FontFamily, 10, FontStyle.Bold),
                    AutoSize = true,
                    Dock = DockStyle.Fill,
                    Padding = new Padding(5)
                };

                tableLayout.Controls.Add(headerLabel, 0, rowIndex);
                tableLayout.SetColumnSpan(headerLabel, 2);
                return;
            }

            var lblField = new Label
            {
                Text = label,
                AutoSize = true,
                Dock = DockStyle.Fill,
                Padding = new Padding(5)
            };

            Control valueControl;
            if (isMultiline)
            {
                valueControl = new TextBox
                {
                    Text = value,
                    Multiline = true,
                    ReadOnly = true,
                    ScrollBars = ScrollBars.Vertical,
                    Height = 100,
                    Dock = DockStyle.Fill
                };
            }
            else
            {
                valueControl = new Label
                {
                    Text = value,
                    AutoSize = true,
                    Dock = DockStyle.Fill,
                    Padding = new Padding(5)
                };
            }

            tableLayout.Controls.Add(lblField, 0, rowIndex);
            tableLayout.Controls.Add(valueControl, 1, rowIndex);
        }
    }
}
