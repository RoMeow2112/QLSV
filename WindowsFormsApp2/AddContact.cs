using QLSV;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class AddContact: Form
    {
        public AddContact()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string fname = txtFirstName.Text.Trim();
            string lname = txtLastName.Text.Trim();
            int groupId = Convert.ToInt32(cbGroup.SelectedValue);
            string phone = txtPhone.Text.Trim();
            string email = txtEmail.Text.Trim();
            string address = txtAddress.Text.Trim();
            int userId = Convert.ToInt32(txtUID.Text);

            MemoryStream pic = new MemoryStream();
            pictureBoxImage.Image.Save(pic, pictureBoxImage.Image.RawFormat);

            MY_DB db = new MY_DB();
            string query = "INSERT INTO mycontact (fName, lName, group_id, phone, email, address, pic, userid) VALUES " +
                           "(@fn, @ln, @group, @phone, @email, @addr, @pic, @uid)";
            SqlCommand cmd = new SqlCommand(query, db.getConnection);
            cmd.Parameters.AddWithValue("@fn", fname);
            cmd.Parameters.AddWithValue("@ln", lname);
            cmd.Parameters.AddWithValue("@group", groupId);
            cmd.Parameters.AddWithValue("@phone", phone);
            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@addr", address);
            cmd.Parameters.AddWithValue("@pic", pic.ToArray());
            cmd.Parameters.AddWithValue("@uid", userId);

            db.openConnection();
            if (cmd.ExecuteNonQuery() == 1)
                MessageBox.Show("Liên hệ đã được thêm.");
            else
                MessageBox.Show("Thêm liên hệ thất bại.");
            db.closeConnection();

    }
        private void LoadGroupToComboBox(ComboBox comboBox)
        {
            MY_DB db = new MY_DB();
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT id, name FROM mygroups", db.getConnection);
            DataTable table = new DataTable();
            adapter.Fill(table);

            comboBox.DataSource = table;
            comboBox.DisplayMember = "name";  // Hiển thị tên nhóm
            comboBox.ValueMember = "id";      // Lưu giá trị là id
        }

        private void FormAddContact_Load(object sender, EventArgs e)
        {
            LoadGroupToComboBox(cbGroup);
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image Files (*.jpg;*.png)|*.jpg;*.png";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                pictureBoxImage.Image = Image.FromFile(ofd.FileName);
            }
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close(); // Đóng form hiện tại
        }
    }
}
