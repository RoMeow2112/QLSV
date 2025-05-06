using QLSV;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private bool VerifyLogin(string username, string password)
        {
            MY_DB db = new MY_DB();

            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();

            // Sửa lại câu truy vấn để sử dụng bảng hr thay vì account
            SqlCommand command = new SqlCommand("SELECT * FROM hr WHERE uname = @User AND pwd = @Pass", db.getConnection);

            command.Parameters.Add("@User", SqlDbType.VarChar).Value = username;
            command.Parameters.Add("@Pass", SqlDbType.VarChar).Value = password;

            adapter.SelectCommand = command;
            adapter.Fill(table);

            return table.Rows.Count > 0;
        }


        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtUsername.Text) || string.IsNullOrEmpty(txtPassword.Text))
            {
                MessageBox.Show("Please enter both Username and Password.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (VerifyLogin(txtUsername.Text, txtPassword.Text))
            {
                // Kiểm tra xem người dùng chọn "Student" hay "Human Resource"
                if (radioButtonStudent.Checked)
                {
                    // Nếu chọn "Student", hiển thị MainForm
                    MainForm mainForm = new MainForm();
                    mainForm.FormClosed += (s, args) => Application.Exit();
                    mainForm.Show(this);
                }
                else if (radioButtonHumanResource.Checked)
                {
                    // Nếu chọn "Human Resource", hiển thị HRForm
                    HRForm hrForm = new HRForm();
                    hrForm.FormClosed += (s, args) => Application.Exit();
                    hrForm.Show(this);
                }

                this.Hide(); // Ẩn form Login sau khi đăng nhập thành công
            }
            else
            {
                MessageBox.Show("Invalid Username Or Password", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            RegisterForm register = new RegisterForm();
            register.Show(this);
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(txtUsername, "Nhập tên đăng nhập");
            toolTip1.SetToolTip(txtPassword, "Nhập mật khẩu của bạn");
            toolTip1.SetToolTip(btnLogin, "Nhấn để đăng nhập");
        }

    }
}
