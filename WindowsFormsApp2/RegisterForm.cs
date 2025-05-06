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
    public partial class RegisterForm: Form
    {
        public RegisterForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog opf = new OpenFileDialog();
            opf.Filter = "Select Image(*.jpg;*.png;*.gif)|*.jpg;*.png;*.gif";
            if ((opf.ShowDialog() == DialogResult.OK))
            {
                PictureBoxStudentImage.Image = Image.FromFile(opf.FileName);
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            // Lấy các giá trị từ các trường trong form
            string firstName = txtFirstName.Text.Trim();
            string lastName = txtLastName.Text.Trim();
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();
            byte[] picture = null;

            // Kiểm tra nếu có ảnh
            if (PictureBoxStudentImage.Image != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    PictureBoxStudentImage.Image.Save(ms, PictureBoxStudentImage.Image.RawFormat);
                    picture = ms.ToArray();
                }
            }

            // Kiểm tra các trường bắt buộc
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) ||
                string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please fill all the fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Kiểm tra xem tên đăng nhập đã tồn tại chưa
            if (CheckUsernameExist(username))
            {
                MessageBox.Show("Username already exists. Please choose a different one.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Tiến hành đăng ký người dùng mới
            if (RegisterUser(firstName, lastName, username, password, picture))
            {
                MessageBox.Show("New user registered successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Error registering user.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Kiểm tra xem tên đăng nhập đã tồn tại trong cơ sở dữ liệu chưa
        public bool CheckUsernameExist(string username)
        {
            MY_DB db = new MY_DB();
            SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM hr WHERE uname = @User", db.getConnection);
            command.Parameters.Add("@User", SqlDbType.VarChar).Value = username;

            db.openConnection();
            int userCount = (int)command.ExecuteScalar();
            db.closeConnection();

            return userCount > 0;
        }

        // Phương thức đăng ký người dùng mới
        public bool RegisterUser(string firstName, string lastName, string username, string password, byte[] picture)
        {
            MY_DB db = new MY_DB();
            SqlCommand command = new SqlCommand("INSERT INTO hr (f_name, l_name, uname, pwd, fig) VALUES (@FirstName, @LastName, @Username, @Password, @Picture)", db.getConnection);
            command.Parameters.Add("@FirstName", SqlDbType.VarChar).Value = firstName;
            command.Parameters.Add("@LastName", SqlDbType.VarChar).Value = lastName;
            command.Parameters.Add("@Username", SqlDbType.VarChar).Value = username;
            command.Parameters.Add("@Password", SqlDbType.VarChar).Value = password; // Lưu mật khẩu, có thể mã hóa nếu cần
            command.Parameters.Add("@Picture", SqlDbType.Image).Value = picture;

            db.openConnection();
            bool result = command.ExecuteNonQuery() == 1;
            db.closeConnection();

            return result;
        }

    }
}
