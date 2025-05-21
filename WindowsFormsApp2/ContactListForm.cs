// ContactListForm.cs
using QLSV;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class ContactListForm : Form
    {
        public ContactListForm()
        {
            InitializeComponent();
            // DataGridView settings
            dataGridViewContacts.ReadOnly = true;
            dataGridViewContacts.AllowUserToAddRows = false;
            dataGridViewContacts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewContacts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Event handlers
            this.Load += ContactListForm_Load;
            listBoxGroups.SelectedIndexChanged += listBoxGroups_SelectedIndexChanged;
            dataGridViewContacts.CellFormatting += dataGridViewContacts_CellFormatting;
        }

        private void ContactListForm_Load(object sender, EventArgs e)
        {
            LoadGroups();
            LoadAllContacts();
        }

        private string CurrentUsername; // giả sử bạn đã gán username hiện tại ở đây

        // Hàm lấy user_id từ bảng HR theo CurrentUsername
        public int GetUidCurrentUser()
        {
            var db = new MY_DB();
            string sql = "SELECT uid FROM HR WHERE uname = @uname";
            using (var cmd = new SqlCommand(sql, db.getConnection))
            {
                cmd.Parameters.AddWithValue("@uname", Login.CurrentUsername);
                db.openConnection();
                var result = cmd.ExecuteScalar();
                db.closeConnection();
                return result != null ? Convert.ToInt32(result) : 0;
            }
        }

        private void LoadGroups()
        {
            int userId = GetUidCurrentUser();
            var db = new MY_DB();
            string sql = "SELECT id, name FROM mygroups WHERE userid = @uid";
            using (var cmd = new SqlCommand(sql, db.getConnection))
            {
                cmd.Parameters.AddWithValue("@uid", userId);
                using (var da = new SqlDataAdapter(cmd))
                {
                    var dt = new DataTable();
                    da.Fill(dt);
                    listBoxGroups.DisplayMember = "name";
                    listBoxGroups.ValueMember = "id";
                    listBoxGroups.DataSource = dt;
                }
            }
        }

        private void LoadAllContacts()
        {
            int userId = GetUidCurrentUser();
            var db = new MY_DB();
            string sql = @"
        SELECT c.fname AS [First Name],
               c.lname AS [Last Name],
               g.name AS [Group],
               c.phone,
               c.email,
               c.address,
               c.pic
        FROM mycontact c
        LEFT JOIN mygroups g ON c.group_id = g.id
        WHERE c.userid = @uid";

            using (var cmd = new SqlCommand(sql, db.getConnection))
            {
                cmd.Parameters.AddWithValue("@uid", userId);
                using (var da = new SqlDataAdapter(cmd))
                {
                    var dt = new DataTable();
                    da.Fill(dt);
                    dataGridViewContacts.DataSource = dt;
                }
            }
        }

        private void LoadContactsByGroup(int groupId)
        {
            int userId = GetUidCurrentUser();
            var db = new MY_DB();
            string sql = @"
        SELECT c.fname AS [First Name],
               c.lname AS [Last Name],
               g.name AS [Group],
               c.phone,
               c.email,
               c.address,
               c.pic
        FROM mycontact c
        LEFT JOIN mygroups g ON c.group_id = g.id
        WHERE c.group_id = @gid AND c.userid = @uid";

            using (var cmd = new SqlCommand(sql, db.getConnection))
            {
                cmd.Parameters.AddWithValue("@gid", groupId);
                cmd.Parameters.AddWithValue("@uid", userId);
                using (var da = new SqlDataAdapter(cmd))
                {
                    var dt = new DataTable();
                    da.Fill(dt);
                    dataGridViewContacts.DataSource = dt;
                }
            }
        }

        private void listBoxGroups_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxGroups.SelectedValue is int groupId)
                LoadContactsByGroup(groupId);
        }



        private void dataGridViewContacts_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridViewContacts.Columns[e.ColumnIndex].Name == "pic" && e.Value != DBNull.Value)
            {
                var bytes = (byte[])e.Value;
                using (var ms = new MemoryStream(bytes))
                    e.Value = Image.FromStream(ms);
            }
        }
    }
}
