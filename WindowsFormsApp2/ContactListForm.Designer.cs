namespace WindowsFormsApp2
{
    partial class ContactListForm
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
            this.listBoxGroups = new System.Windows.Forms.ListBox();
            this.dataGridViewContacts = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewContacts)).BeginInit();
            this.SuspendLayout();
            // 
            // listBoxGroups
            // 
            this.listBoxGroups.FormattingEnabled = true;
            this.listBoxGroups.Location = new System.Drawing.Point(12, 12);
            this.listBoxGroups.Name = "listBoxGroups";
            this.listBoxGroups.Size = new System.Drawing.Size(148, 342);
            this.listBoxGroups.TabIndex = 0;
            // 
            // dataGridViewContacts
            // 
            this.dataGridViewContacts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewContacts.Location = new System.Drawing.Point(166, 12);
            this.dataGridViewContacts.Name = "dataGridViewContacts";
            this.dataGridViewContacts.Size = new System.Drawing.Size(653, 345);
            this.dataGridViewContacts.TabIndex = 1;
            // 
            // ContactListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(831, 369);
            this.Controls.Add(this.dataGridViewContacts);
            this.Controls.Add(this.listBoxGroups);
            this.Name = "ContactListForm";
            this.Text = "ContactListForm";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewContacts)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxGroups;
        private System.Windows.Forms.DataGridView dataGridViewContacts;
    }
}