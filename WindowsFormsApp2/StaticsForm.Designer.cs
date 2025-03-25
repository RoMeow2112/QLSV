namespace WindowsFormsApp2
{
    partial class StaticsForm
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
            this.PanelFemale = new System.Windows.Forms.Panel();
            this.PanelTotal = new System.Windows.Forms.Panel();
            this.LabelTotal = new System.Windows.Forms.Label();
            this.LabelFemale = new System.Windows.Forms.Label();
            this.PanelMale = new System.Windows.Forms.Panel();
            this.LabelMale = new System.Windows.Forms.Label();
            this.PanelFemale.SuspendLayout();
            this.PanelTotal.SuspendLayout();
            this.PanelMale.SuspendLayout();
            this.SuspendLayout();
            // 
            // PanelFemale
            // 
            this.PanelFemale.BackColor = System.Drawing.Color.LightPink;
            this.PanelFemale.Controls.Add(this.LabelFemale);
            this.PanelFemale.Location = new System.Drawing.Point(276, 117);
            this.PanelFemale.Name = "PanelFemale";
            this.PanelFemale.Size = new System.Drawing.Size(260, 146);
            this.PanelFemale.TabIndex = 0;
            // 
            // PanelTotal
            // 
            this.PanelTotal.BackColor = System.Drawing.Color.Blue;
            this.PanelTotal.Controls.Add(this.LabelTotal);
            this.PanelTotal.Location = new System.Drawing.Point(12, 12);
            this.PanelTotal.Name = "PanelTotal";
            this.PanelTotal.Size = new System.Drawing.Size(524, 99);
            this.PanelTotal.TabIndex = 1;
            // 
            // LabelTotal
            // 
            this.LabelTotal.AutoSize = true;
            this.LabelTotal.Dock = System.Windows.Forms.DockStyle.Left;
            this.LabelTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelTotal.ForeColor = System.Drawing.Color.White;
            this.LabelTotal.Location = new System.Drawing.Point(0, 0);
            this.LabelTotal.Name = "LabelTotal";
            this.LabelTotal.Size = new System.Drawing.Size(57, 20);
            this.LabelTotal.TabIndex = 0;
            this.LabelTotal.Text = "label1";
            this.LabelTotal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.LabelTotal.MouseEnter += new System.EventHandler(this.LabelTotal_MouseEnter);
            this.LabelTotal.MouseLeave += new System.EventHandler(this.LabelTotal_MouseLeave);
            // 
            // LabelFemale
            // 
            this.LabelFemale.AutoSize = true;
            this.LabelFemale.Dock = System.Windows.Forms.DockStyle.Left;
            this.LabelFemale.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelFemale.ForeColor = System.Drawing.Color.White;
            this.LabelFemale.Location = new System.Drawing.Point(0, 0);
            this.LabelFemale.Name = "LabelFemale";
            this.LabelFemale.Size = new System.Drawing.Size(57, 20);
            this.LabelFemale.TabIndex = 2;
            this.LabelFemale.Text = "label3";
            this.LabelFemale.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.LabelFemale.MouseEnter += new System.EventHandler(this.LabelFemale_MouseEnter);
            this.LabelFemale.MouseLeave += new System.EventHandler(this.LabelFemale_MouseLeave);
            // 
            // PanelMale
            // 
            this.PanelMale.BackColor = System.Drawing.Color.Green;
            this.PanelMale.Controls.Add(this.LabelMale);
            this.PanelMale.Location = new System.Drawing.Point(12, 117);
            this.PanelMale.Name = "PanelMale";
            this.PanelMale.Size = new System.Drawing.Size(260, 146);
            this.PanelMale.TabIndex = 3;
            // 
            // LabelMale
            // 
            this.LabelMale.AutoSize = true;
            this.LabelMale.Dock = System.Windows.Forms.DockStyle.Left;
            this.LabelMale.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelMale.ForeColor = System.Drawing.Color.White;
            this.LabelMale.Location = new System.Drawing.Point(0, 0);
            this.LabelMale.Name = "LabelMale";
            this.LabelMale.Size = new System.Drawing.Size(57, 20);
            this.LabelMale.TabIndex = 2;
            this.LabelMale.Text = "label2";
            this.LabelMale.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.LabelMale.MouseEnter += new System.EventHandler(this.LabelMale_MouseEnter);
            this.LabelMale.MouseLeave += new System.EventHandler(this.LabelMale_MouseLeave);
            // 
            // StaticsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(548, 275);
            this.Controls.Add(this.PanelMale);
            this.Controls.Add(this.PanelTotal);
            this.Controls.Add(this.PanelFemale);
            this.Name = "StaticsForm";
            this.Text = "StaticsForm";
            this.Load += new System.EventHandler(this.StaticsForm_Load);
            this.PanelFemale.ResumeLayout(false);
            this.PanelFemale.PerformLayout();
            this.PanelTotal.ResumeLayout(false);
            this.PanelTotal.PerformLayout();
            this.PanelMale.ResumeLayout(false);
            this.PanelMale.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel PanelFemale;
        private System.Windows.Forms.Panel PanelTotal;
        private System.Windows.Forms.Label LabelFemale;
        private System.Windows.Forms.Label LabelTotal;
        private System.Windows.Forms.Panel PanelMale;
        private System.Windows.Forms.Label LabelMale;
    }
}