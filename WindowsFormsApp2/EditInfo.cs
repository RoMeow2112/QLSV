using QLSV;
using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class EditInfo : Form
    {
        private string originalUsername;

        public EditInfo(string username)
        {
            InitializeComponent();
            originalUsername = username;
            txtUsername.Text = username;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string newUsername = txtUsername.Text.Trim();
            string newPassword = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(newUsername) || string.IsNullOrEmpty(newPassword))
            {
                MessageBox.Show("Username và Password không được để trống.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            MY_DB db = new MY_DB();
            string query = "UPDATE hr SET uname = @newUname, pwd = @newPwd WHERE uname = @oldUname";

            using (SqlCommand cmd = new SqlCommand(query, db.getConnection))
            {
                cmd.Parameters.AddWithValue("@newUname", newUsername);
                cmd.Parameters.AddWithValue("@newPwd", newPassword);
                cmd.Parameters.AddWithValue("@oldUname", originalUsername);

                try
                {
                    db.openConnection();
                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        MessageBox.Show("Cập nhật thông tin thành công.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        originalUsername = newUsername; // cập nhật username mới
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Cập nhật thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    db.closeConnection();
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
