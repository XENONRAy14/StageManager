namespace StageManager
{
    partial class StageDetailsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panelHeader = new System.Windows.Forms.Panel();
            this.btnBack = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panelContainer = new System.Windows.Forms.Panel();
            this.groupBoxEntreprise = new System.Windows.Forms.GroupBox();
            this.lblEmailContact = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.lblTelephoneContact = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblContact = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblVille = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblRaisonSociale = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBoxEtudiant = new System.Windows.Forms.GroupBox();
            this.lblClasse = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.lblNomPrenom = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.btnContact = new System.Windows.Forms.Button();
            this.groupBoxPeriode = new System.Windows.Forms.GroupBox();
            this.lblAnnee = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.lblPeriode = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.panelHeader.SuspendLayout();
            this.panelContainer.SuspendLayout();
            this.groupBoxEntreprise.SuspendLayout();
            this.groupBoxEtudiant.SuspendLayout();
            this.groupBoxPeriode.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.panelHeader.Controls.Add(this.btnBack);
            this.panelHeader.Controls.Add(this.lblTitle);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(800, 60);
            this.panelHeader.TabIndex = 0;
            // 
            // btnBack
            // 
            this.btnBack.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
            this.btnBack.FlatAppearance.BorderSize = 0;
            this.btnBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBack.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBack.ForeColor = System.Drawing.Color.White;
            this.btnBack.Location = new System.Drawing.Point(20, 15);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(100, 30);
            this.btnBack.TabIndex = 1;
            this.btnBack.Text = "< RETOUR";
            this.btnBack.UseVisualStyleBackColor = false;
            // 
            // lblTitle
            // 
            this.lblTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(0, 15);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(800, 30);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "DÉTAILS DU STAGE";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelContainer
            // 
            this.panelContainer.BackColor = System.Drawing.Color.White;
            this.panelContainer.Controls.Add(this.groupBoxEntreprise);
            this.panelContainer.Controls.Add(this.groupBoxEtudiant);
            this.panelContainer.Controls.Add(this.btnContact);
            this.panelContainer.Controls.Add(this.groupBoxPeriode);
            this.panelContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContainer.Location = new System.Drawing.Point(0, 60);
            this.panelContainer.Name = "panelContainer";
            this.panelContainer.Size = new System.Drawing.Size(800, 490);
            this.panelContainer.TabIndex = 1;
            // 
            // groupBoxEntreprise
            // 
            this.groupBoxEntreprise.Controls.Add(this.lblEmailContact);
            this.groupBoxEntreprise.Controls.Add(this.label9);
            this.groupBoxEntreprise.Controls.Add(this.lblTelephoneContact);
            this.groupBoxEntreprise.Controls.Add(this.label7);
            this.groupBoxEntreprise.Controls.Add(this.lblContact);
            this.groupBoxEntreprise.Controls.Add(this.label5);
            this.groupBoxEntreprise.Controls.Add(this.lblVille);
            this.groupBoxEntreprise.Controls.Add(this.label3);
            this.groupBoxEntreprise.Controls.Add(this.lblRaisonSociale);
            this.groupBoxEntreprise.Controls.Add(this.label1);
            this.groupBoxEntreprise.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.groupBoxEntreprise.Location = new System.Drawing.Point(40, 140);
            this.groupBoxEntreprise.Name = "groupBoxEntreprise";
            this.groupBoxEntreprise.Size = new System.Drawing.Size(400, 250);
            this.groupBoxEntreprise.TabIndex = 0;
            this.groupBoxEntreprise.TabStop = false;
            this.groupBoxEntreprise.Text = "Entreprise";
            // 
            // lblEmailContact
            // 
            this.lblEmailContact.AutoSize = true;
            this.lblEmailContact.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblEmailContact.Location = new System.Drawing.Point(180, 200);
            this.lblEmailContact.Name = "lblEmailContact";
            this.lblEmailContact.Size = new System.Drawing.Size(107, 19);
            this.lblEmailContact.TabIndex = 9;
            this.lblEmailContact.Text = "email@test.com";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.label9.Location = new System.Drawing.Point(20, 200);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(104, 19);
            this.label9.TabIndex = 8;
            this.label9.Text = "Email du tuteur:";
            // 
            // lblTelephoneContact
            // 
            this.lblTelephoneContact.AutoSize = true;
            this.lblTelephoneContact.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblTelephoneContact.Location = new System.Drawing.Point(180, 160);
            this.lblTelephoneContact.Name = "lblTelephoneContact";
            this.lblTelephoneContact.Size = new System.Drawing.Size(81, 19);
            this.lblTelephoneContact.TabIndex = 7;
            this.lblTelephoneContact.Text = "0123456789";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.label7.Location = new System.Drawing.Point(20, 160);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(133, 19);
            this.label7.TabIndex = 6;
            this.label7.Text = "Téléphone du tuteur:";
            // 
            // lblContact
            // 
            this.lblContact.AutoSize = true;
            this.lblContact.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblContact.Location = new System.Drawing.Point(180, 120);
            this.lblContact.Name = "lblContact";
            this.lblContact.Size = new System.Drawing.Size(69, 19);
            this.lblContact.TabIndex = 5;
            this.lblContact.Text = "John DOE";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.label5.Location = new System.Drawing.Point(20, 120);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(112, 19);
            this.label5.TabIndex = 4;
            this.label5.Text = "Tuteur de stage :";
            // 
            // lblVille
            // 
            this.lblVille.AutoSize = true;
            this.lblVille.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblVille.Location = new System.Drawing.Point(180, 80);
            this.lblVille.Name = "lblVille";
            this.lblVille.Size = new System.Drawing.Size(40, 19);
            this.lblVille.TabIndex = 3;
            this.lblVille.Text = "Paris";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.label3.Location = new System.Drawing.Point(20, 80);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 19);
            this.label3.TabIndex = 2;
            this.label3.Text = "Ville :";
            // 
            // lblRaisonSociale
            // 
            this.lblRaisonSociale.AutoSize = true;
            this.lblRaisonSociale.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblRaisonSociale.Location = new System.Drawing.Point(180, 40);
            this.lblRaisonSociale.Name = "lblRaisonSociale";
            this.lblRaisonSociale.Size = new System.Drawing.Size(101, 19);
            this.lblRaisonSociale.TabIndex = 1;
            this.lblRaisonSociale.Text = "ENTREPRISE SA";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.label1.Location = new System.Drawing.Point(20, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "Nom:";
            // 
            // groupBoxEtudiant
            // 
            this.groupBoxEtudiant.Controls.Add(this.lblClasse);
            this.groupBoxEtudiant.Controls.Add(this.label12);
            this.groupBoxEtudiant.Controls.Add(this.lblNomPrenom);
            this.groupBoxEtudiant.Controls.Add(this.label10);
            this.groupBoxEtudiant.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.groupBoxEtudiant.Location = new System.Drawing.Point(40, 30);
            this.groupBoxEtudiant.Name = "groupBoxEtudiant";
            this.groupBoxEtudiant.Size = new System.Drawing.Size(400, 100);
            this.groupBoxEtudiant.TabIndex = 1;
            this.groupBoxEtudiant.TabStop = false;
            this.groupBoxEtudiant.Text = "Étudiant";
            // 
            // lblClasse
            // 
            this.lblClasse.AutoSize = true;
            this.lblClasse.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblClasse.Location = new System.Drawing.Point(180, 60);
            this.lblClasse.Name = "lblClasse";
            this.lblClasse.Size = new System.Drawing.Size(38, 19);
            this.lblClasse.TabIndex = 3;
            this.lblClasse.Text = "BTS2";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.label12.Location = new System.Drawing.Point(20, 60);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(53, 19);
            this.label12.TabIndex = 2;
            this.label12.Text = "Classe :";
            // 
            // lblNomPrenom
            // 
            this.lblNomPrenom.AutoSize = true;
            this.lblNomPrenom.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblNomPrenom.Location = new System.Drawing.Point(180, 30);
            this.lblNomPrenom.Name = "lblNomPrenom";
            this.lblNomPrenom.Size = new System.Drawing.Size(91, 19);
            this.lblNomPrenom.TabIndex = 1;
            this.lblNomPrenom.Text = "Pierre DUPUY";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.label10.Location = new System.Drawing.Point(20, 30);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(114, 19);
            this.label10.TabIndex = 0;
            this.label10.Text = "Nom et Prénom :";
            // 
            // btnContact
            // 
            this.btnContact.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.btnContact.FlatAppearance.BorderSize = 0;
            this.btnContact.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnContact.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnContact.ForeColor = System.Drawing.Color.White;
            this.btnContact.Location = new System.Drawing.Point(40, 410);
            this.btnContact.Name = "btnContact";
            this.btnContact.Size = new System.Drawing.Size(200, 40);
            this.btnContact.TabIndex = 3;
            this.btnContact.Text = "CONTACTER L\'ÉTUDIANT";
            this.btnContact.UseVisualStyleBackColor = false;
            // 
            // groupBoxPeriode
            // 
            this.groupBoxPeriode.Controls.Add(this.lblAnnee);
            this.groupBoxPeriode.Controls.Add(this.label14);
            this.groupBoxPeriode.Controls.Add(this.lblPeriode);
            this.groupBoxPeriode.Controls.Add(this.label8);
            this.groupBoxPeriode.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.groupBoxPeriode.Location = new System.Drawing.Point(460, 30);
            this.groupBoxPeriode.Name = "groupBoxPeriode";
            this.groupBoxPeriode.Size = new System.Drawing.Size(300, 100);
            this.groupBoxPeriode.TabIndex = 2;
            this.groupBoxPeriode.TabStop = false;
            this.groupBoxPeriode.Text = "Période";
            // 
            // lblAnnee
            // 
            this.lblAnnee.AutoSize = true;
            this.lblAnnee.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblAnnee.Location = new System.Drawing.Point(120, 60);
            this.lblAnnee.Name = "lblAnnee";
            this.lblAnnee.Size = new System.Drawing.Size(43, 19);
            this.lblAnnee.TabIndex = 3;
            this.lblAnnee.Text = "2024";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.label14.Location = new System.Drawing.Point(20, 60);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(52, 19);
            this.label14.TabIndex = 2;
            this.label14.Text = "Année:";
            // 
            // lblPeriode
            // 
            this.lblPeriode.AutoSize = true;
            this.lblPeriode.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblPeriode.Location = new System.Drawing.Point(120, 30);
            this.lblPeriode.Name = "lblPeriode";
            this.lblPeriode.Size = new System.Drawing.Size(126, 19);
            this.lblPeriode.TabIndex = 1;
            this.lblPeriode.Text = "Janvier - Février";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.label8.Location = new System.Drawing.Point(20, 30);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(60, 19);
            this.label8.TabIndex = 0;
            this.label8.Text = "Période:";
            // 
            // StageDetailsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 550);
            this.Controls.Add(this.panelContainer);
            this.Controls.Add(this.panelHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "StageDetailsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Stage Manager - Détails du stage";
            this.panelHeader.ResumeLayout(false);
            this.panelContainer.ResumeLayout(false);
            this.groupBoxEntreprise.ResumeLayout(false);
            this.groupBoxEntreprise.PerformLayout();
            this.groupBoxEtudiant.ResumeLayout(false);
            this.groupBoxEtudiant.PerformLayout();
            this.groupBoxPeriode.ResumeLayout(false);
            this.groupBoxPeriode.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Panel panelContainer;
        private System.Windows.Forms.GroupBox groupBoxEntreprise;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblRaisonSociale;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblVille;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblContact;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblTelephoneContact;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label lblEmailContact;
        private System.Windows.Forms.GroupBox groupBoxEtudiant;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label lblNomPrenom;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label lblClasse;
        private System.Windows.Forms.GroupBox groupBoxPeriode;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lblPeriode;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label lblAnnee;
        private System.Windows.Forms.Button btnContact;
    }
}
