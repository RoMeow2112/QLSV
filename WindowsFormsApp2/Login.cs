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

            SqlCommand command = new SqlCommand("SELECT * FROM account WHERE username = @User AND password = @Pass", db.getConnection);

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
                MainForm mainForm = new MainForm();
                mainForm.FormClosed += (s, args) => Application.Exit();
                mainForm.Show(this);
                this.Hide();
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
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();
            string confirmPassword = txtConfirmPassword.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Username and Password cannot be empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (checkBoxRegister.Checked)
            {
                if (string.IsNullOrEmpty(confirmPassword))
                {
                    MessageBox.Show("Please confirm your password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (password != confirmPassword)
                {
                    MessageBox.Show("Passwords do not match", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Account account = new Account();
                if (account.RegisterUser(username, password))
                {
                    MessageBox.Show("New user registered successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Error registering user", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Please check 'Register' to register.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void checkBoxRegister_CheckedChanged(object sender, EventArgs e)
        {
            bool isChecked = checkBoxRegister.Checked;
            label4.Visible = isChecked;
            txtConfirmPassword.Visible = isChecked;
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(txtUsername, "Nhập tên đăng nhập");
            toolTip1.SetToolTip(txtPassword, "Nhập mật khẩu của bạn");
            toolTip1.SetToolTip(btnLogin, "Nhấn để đăng nhập");
        }

    }
}
