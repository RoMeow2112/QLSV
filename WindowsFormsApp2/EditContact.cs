using QLSV;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class EditContact : Form
    {
        public EditContact()
        {
            InitializeComponent();
        }

        private void LoadGroupList()
        {
            MY_DB db = new MY_DB();
            using (SqlDataAdapter adapter = new SqlDataAdapter("SELECT id, name FROM mygroups", db.getConnection))
            {
                DataTable table = new DataTable();
                adapter.Fill(table);
                cbGroup.DataSource = table;
                cbGroup.DisplayMember = "name";
                cbGroup.ValueMember = "id";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var selectForm = new Form_SelectContact())
            {
                // Subscribe to the selection event
                selectForm.OnContactSelected += row =>
                {
                    txtID.Text = row["id"].ToString();
                    txtFirstName.Text = row["fname"].ToString();
                    txtLastName.Text = row["lname"].ToString();

                    if (row["group_id"] != DBNull.Value)
                        cbGroup.SelectedValue = Convert.ToInt32(row["group_id"]);

                    txtPhone.Text = row["phone"].ToString();
                    txtEmail.Text = row["email"].ToString();
                    txtAddress.Text = row["address"].ToString();

                    if (row["pic"] != DBNull.Value)
                    {
                        byte[] picBytes = (byte[])row["pic"];
                        using (var ms = new MemoryStream(picBytes))
                            pictureBoxContact.Image = Image.FromStream(ms);
                    }
                };

                // Show as modal dialog to wait for selection
                selectForm.ShowDialog(this);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtID.Text, out int contactId))
            {
                MessageBox.Show("Vui lòng chọn một contact hợp lệ.", "Lỗi",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string fName = txtFirstName.Text.Trim();
            string lName = txtLastName.Text.Trim();
            int groupId = Convert.ToInt32(cbGroup.SelectedValue);
            string phone = txtPhone.Text.Trim();
            string email = txtEmail.Text.Trim();
            string address = txtAddress.Text.Trim();

            byte[] picBytes = null;
            if (pictureBoxContact.Image != null)
            {
                using (var ms = new MemoryStream())
                {
                    pictureBoxContact.Image.Save(ms, pictureBoxContact.Image.RawFormat);
                    picBytes = ms.ToArray();
                }
            }

            string query = @"
                UPDATE mycontact
                   SET fname    = @fn,
                       lname    = @ln,
                       group_id = @gid,
                       phone    = @ph,
                       email    = @em,
                       address  = @ad,
                       pic      = @pic
                 WHERE id = @id";

            MY_DB db = new MY_DB();
            using (SqlCommand cmd = new SqlCommand(query, db.getConnection))
            {
                cmd.Parameters.AddWithValue("@fn", (object)fName ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ln", (object)lName ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@gid", groupId);
                cmd.Parameters.AddWithValue("@ph", (object)phone ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@em", (object)email ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ad", (object)address ?? DBNull.Value);
                cmd.Parameters.Add("@pic", SqlDbType.Image).Value = picBytes ?? (object)DBNull.Value;
                cmd.Parameters.AddWithValue("@id", contactId);

                try
                {
                    db.openConnection();
                    int rows = cmd.ExecuteNonQuery();
                    MessageBox.Show(rows > 0 ? "Cập nhật thành công." : "Không tìm thấy contact.",
                                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi cập nhật:\n" + ex.Message,
                                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    db.closeConnection();
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Upload_Click(object sender, EventArgs e)
        {
            using (var dlg = new OpenFileDialog() { Filter = "Image Files|*.jpg;*.png;*.bmp" })
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    pictureBoxContact.Image = Image.FromFile(dlg.FileName);
                }
            }
        }

        private void EditContact_Load(object sender, EventArgs e)
        {
            LoadGroupList();
        }
    }
}