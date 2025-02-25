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

        // Tạo hàm VerifyLogin để xác thực đăng nhập
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

            // Kiểm tra xem có dữ liệu không
            return table.Rows.Count > 0; // Nếu có dữ liệu, trả về true, nếu không trả về false
        }

        // Sự kiện btnLogin_Click gọi hàm VerifyLogin
        private void btnLogin_Click(object sender, EventArgs e)
        {
            // Kiểm tra nếu tên đăng nhập hoặc mật khẩu trống
            if (string.IsNullOrEmpty(txtUsername.Text) || string.IsNullOrEmpty(txtPassword.Text))
            {
                MessageBox.Show("Please enter both Username and Password.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Dừng phương thức nếu thiếu thông tin
            }

            // Gọi hàm VerifyLogin để kiểm tra đăng nhập
            if (VerifyLogin(txtUsername.Text, txtPassword.Text))
            {
                MainForm mainForm = new MainForm();
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
            Account account = new Account();    
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (username == "" || password == "")
            {
                MessageBox.Show("Username and Password cannot be empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (account.RegisterUser(username,password))
            {
                MessageBox.Show("New user registered successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Error registering user", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
