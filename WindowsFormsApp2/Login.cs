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

        private bool VerifyLogin(string username, string password, bool isHumanResource)
        {
            MY_DB db = new MY_DB();
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();

            // Nếu đăng nhập Human Resource, kiểm tra thêm role = 'HR'
            string query = isHumanResource
                ? "SELECT * FROM hr WHERE uname = @User AND pwd = @Pass AND role = 'HR'"
                : "SELECT * FROM hr WHERE uname = @User AND pwd = @Pass";

            SqlCommand command = new SqlCommand(query, db.getConnection);
            command.Parameters.Add("@User", SqlDbType.VarChar).Value = username;
            command.Parameters.Add("@Pass", SqlDbType.VarChar).Value = password;

            adapter.SelectCommand = command;
            adapter.Fill(table);

            return table.Rows.Count > 0;
        }

        public static string CurrentUsername;

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtUsername.Text) || string.IsNullOrEmpty(txtPassword.Text))
            {
                MessageBox.Show("Please enter both Username and Password.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool isHR = radioButtonHumanResource.Checked;
            bool isStudent = radioButtonStudent.Checked;

            if (!isHR && !isStudent)
            {
                MessageBox.Show("Please select a role (Student or Human Resource).", "Role Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (VerifyLogin(txtUsername.Text, txtPassword.Text, isHR))
            {
                CurrentUsername = txtUsername.Text;
                if (isStudent)
                {
                    MainForm mainForm = new MainForm();
                    mainForm.FormClosed += (s, args) => Application.Exit();
                    mainForm.Show(this);
                }
                else if (isHR)
                {
                    HRForm hrForm = new HRForm();
                    hrForm.FormClosed += (s, args) => Application.Exit();
                    hrForm.Show(this);
                }

                this.Hide();
            }
            else
            {
                MessageBox.Show("Invalid Username Or Password or Role.", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
