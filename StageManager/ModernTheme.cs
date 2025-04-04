using System;
using System.Drawing;
using System.Windows.Forms;

namespace StageManager
{
    /// <summary>
    /// Fournit des couleurs, polices et styles pour un thème moderne
    /// </summary>
    public static class ModernTheme
    {
        // Couleurs principales
        public static Color PrimaryColor = Color.FromArgb(63, 81, 181);       // Bleu indigo
        public static Color PrimaryDarkColor = Color.FromArgb(48, 63, 159);   // Bleu indigo foncé
        public static Color PrimaryLightColor = Color.FromArgb(121, 134, 203); // Bleu indigo clair
        public static Color AccentColor = Color.FromArgb(255, 64, 129);       // Rose
        
        // Couleurs neutres
        public static Color TextColor = Color.FromArgb(33, 33, 33);           // Presque noir
        public static Color TextLightColor = Color.FromArgb(158, 158, 158);   // Gris
        public static Color BackgroundColor = Color.FromArgb(250, 250, 250);  // Presque blanc
        public static Color SurfaceColor = Color.White;                       // Blanc
        
        // Couleurs d'état
        public static Color SuccessColor = Color.FromArgb(76, 175, 80);       // Vert
        public static Color WarningColor = Color.FromArgb(255, 152, 0);       // Orange
        public static Color ErrorColor = Color.FromArgb(244, 67, 54);         // Rouge
        
        // Police par défaut
        public static Font DefaultFont = new Font("Segoe UI", 9F);
        public static Font HeaderFont = new Font("Segoe UI Semibold", 12F);
        public static Font TitleFont = new Font("Segoe UI", 14F, FontStyle.Bold);
        
        /// <summary>
        /// Applique le thème moderne à un bouton
        /// </summary>
        public static void ApplyButtonStyle(Button button, bool isPrimary = true)
        {
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Font = new Font(DefaultFont.FontFamily, DefaultFont.Size, FontStyle.Regular);
            button.Cursor = Cursors.Hand;
            button.BackColor = isPrimary ? PrimaryColor : Color.White;
            button.ForeColor = isPrimary ? Color.White : PrimaryColor;
            
            if (!isPrimary)
            {
                button.FlatAppearance.BorderSize = 1;
                button.FlatAppearance.BorderColor = PrimaryColor;
            }
        }
        
        /// <summary>
        /// Applique le thème moderne à un TextBox
        /// </summary>
        public static void ApplyTextBoxStyle(TextBox textBox)
        {
            textBox.BorderStyle = BorderStyle.FixedSingle;
            textBox.Font = DefaultFont;
            textBox.BackColor = SurfaceColor;
            textBox.ForeColor = TextColor;
        }
        
        /// <summary>
        /// Applique le thème moderne à un Label
        /// </summary>
        public static void ApplyLabelStyle(Label label, bool isHeader = false)
        {
            label.Font = isHeader ? HeaderFont : DefaultFont;
            label.ForeColor = isHeader ? PrimaryColor : TextColor;
            label.BackColor = Color.Transparent;
        }
        
        /// <summary>
        /// Applique le thème moderne à une ListView
        /// </summary>
        public static void ApplyListViewStyle(ListView listView)
        {
            listView.BorderStyle = BorderStyle.None;
            listView.Font = DefaultFont;
            listView.BackColor = SurfaceColor;
            listView.ForeColor = TextColor;
            listView.FullRowSelect = true;
            listView.GridLines = true;
            listView.View = View.Details;
            
            // Désactivation du mode OwnerDraw qui cause des problèmes d'affichage
            listView.OwnerDraw = false;
            
            // Style pour les en-têtes de colonne
            foreach (ColumnHeader column in listView.Columns)
            {
                column.Width = column.Width; // Force le rafraîchissement
            }
        }
        
        /// <summary>
        /// Applique le thème moderne à un formulaire
        /// </summary>
        public static void ApplyFormStyle(Form form)
        {
            form.BackColor = BackgroundColor;
            form.Font = DefaultFont;
            form.ForeColor = TextColor;
            form.FormBorderStyle = FormBorderStyle.FixedSingle;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MaximizeBox = false;
        }
        
        /// <summary>
        /// Crée un panel de navigation latéral
        /// </summary>
        public static Panel CreateSideMenu(int width, int height)
        {
            Panel sideMenu = new Panel
            {
                Width = width,
                Height = height,
                BackColor = PrimaryDarkColor,
                Dock = DockStyle.Left
            };
            
            // Logo ou titre en haut du menu
            Label lblTitle = new Label
            {
                Text = "Stage Manager",
                Font = TitleFont,
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 60,
                Padding = new Padding(0, 20, 0, 0)
            };
            
            sideMenu.Controls.Add(lblTitle);
            
            return sideMenu;
        }
        
        /// <summary>
        /// Crée un bouton de menu pour le panel latéral
        /// </summary>
        public static Button CreateMenuButton(string text, Image icon = null)
        {
            Button button = new Button
            {
                Text = text,
                TextAlign = ContentAlignment.MiddleLeft,
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 },
                Font = DefaultFont,
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                Height = 45,
                Dock = DockStyle.Top,
                Padding = new Padding(20, 0, 0, 0),
                Cursor = Cursors.Hand,
                ImageAlign = ContentAlignment.MiddleLeft,
                TextImageRelation = TextImageRelation.ImageBeforeText
            };
            
            if (icon != null)
            {
                button.Image = icon;
                button.ImageAlign = ContentAlignment.MiddleLeft;
                button.Padding = new Padding(30, 0, 0, 0);
            }
            
            button.MouseEnter += (sender, e) => button.BackColor = PrimaryColor;
            button.MouseLeave += (sender, e) => button.BackColor = Color.Transparent;
            
            return button;
        }
    }
}
