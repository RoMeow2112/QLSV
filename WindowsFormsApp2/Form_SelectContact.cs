using QLSV;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form_SelectContact : Form
    {
        public delegate void SelectContactDelegate(DataRow row);
        public event SelectContactDelegate OnContactSelected;

        public Form_SelectContact()
        {
            InitializeComponent();
            // Grid settings
            dataGridViewContacts.ReadOnly = true;
            dataGridViewContacts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewContacts.MultiSelect = false;
            dataGridViewContacts.AllowUserToAddRows = false;
            dataGridViewContacts.EditMode = DataGridViewEditMode.EditProgrammatically;
        }

        private void Form_SelectContact_Load(object sender, EventArgs e)
        {
            var db = new MY_DB();
            using (var adapter = new SqlDataAdapter(
                "SELECT id, fname, lname, group_id, phone, email, address, pic FROM mycontact",
                db.getConnection))
            {
                var table = new DataTable();
                adapter.Fill(table);
                dataGridViewContacts.DataSource = table;
            }
        }

        private void Select_Click(object sender, EventArgs e)
        {
            if (dataGridViewContacts.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn một contact.", "Chưa chọn", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var drv = dataGridViewContacts.CurrentRow.DataBoundItem as DataRowView;
            if (drv != null)
            {
                // Invoke the subscribed event
                OnContactSelected?.Invoke(drv.Row);
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
