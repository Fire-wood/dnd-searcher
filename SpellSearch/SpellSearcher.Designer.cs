namespace SpellSearch
{
    partial class SpellSearcher
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SpellSearcher));
            this.spellSearchBox = new System.Windows.Forms.TextBox();
            this.spellsPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // spellSearchBox
            // 
            this.spellSearchBox.AcceptsReturn = true;
            this.spellSearchBox.BackColor = System.Drawing.Color.Black;
            this.spellSearchBox.Font = new System.Drawing.Font("Papyrus", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.spellSearchBox.ForeColor = System.Drawing.SystemColors.Window;
            this.spellSearchBox.Location = new System.Drawing.Point(150, 40);
            this.spellSearchBox.Name = "spellSearchBox";
            this.spellSearchBox.Size = new System.Drawing.Size(472, 41);
            this.spellSearchBox.TabIndex = 0;
            this.spellSearchBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.spellSearchBox.TextChanged += new System.EventHandler(this.spellSearchBox_TextChanged);
            // 
            // spellsPanel
            // 
            this.spellsPanel.AutoScroll = true;
            this.spellsPanel.Location = new System.Drawing.Point(40, 117);
            this.spellsPanel.Name = "spellsPanel";
            this.spellsPanel.Size = new System.Drawing.Size(715, 297);
            this.spellsPanel.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.label1.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(56, 96);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(108, 18);
            this.label1.TabIndex = 3;
            this.label1.Text = "Spell Name";
            // 
            // SpellSearcher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.spellsPanel);
            this.Controls.Add(this.spellSearchBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SpellSearcher";
            this.Text = "Spell Searcher";
            this.Load += new System.EventHandler(this.SpellSearcher_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox spellSearchBox;
        private System.Windows.Forms.FlowLayoutPanel spellsPanel;
        private System.Windows.Forms.Label label1;
    }
}

