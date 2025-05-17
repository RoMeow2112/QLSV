namespace WindowsFormsApp2
{
    partial class Form_SelectContact
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
            this.dataGridViewContacts = new System.Windows.Forms.DataGridView();
            this.Select = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewContacts)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewContacts
            // 
            this.dataGridViewContacts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewContacts.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridViewContacts.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewContacts.Name = "dataGridViewContacts";
            this.dataGridViewContacts.Size = new System.Drawing.Size(800, 450);
            this.dataGridViewContacts.TabIndex = 0;
            // 
            // Select
            // 
            this.Select.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Select.Location = new System.Drawing.Point(367, 458);
            this.Select.Name = "Select";
            this.Select.Size = new System.Drawing.Size(75, 39);
            this.Select.TabIndex = 1;
            this.Select.Text = "Select";
            this.Select.UseVisualStyleBackColor = true;
            this.Select.Click += new System.EventHandler(this.Select_Click);
            // 
            // Form_SelectContact
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 509);
            this.Controls.Add(this.Select);
            this.Controls.Add(this.dataGridViewContacts);
            this.Name = "Form_SelectContact";
            this.Text = "Form_SelectContact";
            this.Load += new System.EventHandler(this.Form_SelectContact_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewContacts)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewContacts;
        private System.Windows.Forms.Button Select;
    }
}