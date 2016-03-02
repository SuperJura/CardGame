namespace GameServer
{
    partial class MainForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.btnStartServer = new System.Windows.Forms.Button();
            this.lbPlayers = new System.Windows.Forms.ListBox();
            this.lbEvents = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtUkupnoIgraca = new System.Windows.Forms.TextBox();
            this.txtUkupnoIgracaCekaju = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(407, 184);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(134, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Zavrsni karte Game Server";
            // 
            // btnStartServer
            // 
            this.btnStartServer.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnStartServer.Location = new System.Drawing.Point(434, 228);
            this.btnStartServer.Name = "btnStartServer";
            this.btnStartServer.Size = new System.Drawing.Size(75, 23);
            this.btnStartServer.TabIndex = 1;
            this.btnStartServer.Text = "Start Server";
            this.btnStartServer.UseVisualStyleBackColor = true;
            this.btnStartServer.Click += new System.EventHandler(this.btnStartServer_Click);
            // 
            // lbPlayers
            // 
            this.lbPlayers.Dock = System.Windows.Forms.DockStyle.Left;
            this.lbPlayers.FormattingEnabled = true;
            this.lbPlayers.Location = new System.Drawing.Point(0, 0);
            this.lbPlayers.MultiColumn = true;
            this.lbPlayers.Name = "lbPlayers";
            this.lbPlayers.Size = new System.Drawing.Size(382, 625);
            this.lbPlayers.TabIndex = 2;
            // 
            // lbEvents
            // 
            this.lbEvents.Dock = System.Windows.Forms.DockStyle.Right;
            this.lbEvents.FormattingEnabled = true;
            this.lbEvents.Location = new System.Drawing.Point(566, 0);
            this.lbEvents.MultiColumn = true;
            this.lbEvents.Name = "lbEvents";
            this.lbEvents.Size = new System.Drawing.Size(382, 625);
            this.lbEvents.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(389, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(108, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Ukupno online igraca";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(392, 82);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(161, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "ukupno igraca koji cekaju match";
            // 
            // txtUkupnoIgraca
            // 
            this.txtUkupnoIgraca.Location = new System.Drawing.Point(395, 29);
            this.txtUkupnoIgraca.Name = "txtUkupnoIgraca";
            this.txtUkupnoIgraca.ReadOnly = true;
            this.txtUkupnoIgraca.Size = new System.Drawing.Size(100, 20);
            this.txtUkupnoIgraca.TabIndex = 6;
            // 
            // txtUkupnoIgracaCekaju
            // 
            this.txtUkupnoIgracaCekaju.Location = new System.Drawing.Point(395, 99);
            this.txtUkupnoIgracaCekaju.Name = "txtUkupnoIgracaCekaju";
            this.txtUkupnoIgracaCekaju.ReadOnly = true;
            this.txtUkupnoIgracaCekaju.Size = new System.Drawing.Size(100, 20);
            this.txtUkupnoIgracaCekaju.TabIndex = 7;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(948, 625);
            this.Controls.Add(this.txtUkupnoIgracaCekaju);
            this.Controls.Add(this.txtUkupnoIgraca);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lbEvents);
            this.Controls.Add(this.lbPlayers);
            this.Controls.Add(this.btnStartServer);
            this.Controls.Add(this.label1);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnStartServer;
        private System.Windows.Forms.ListBox lbPlayers;
        private System.Windows.Forms.ListBox lbEvents;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtUkupnoIgraca;
        private System.Windows.Forms.TextBox txtUkupnoIgracaCekaju;
    }
}

