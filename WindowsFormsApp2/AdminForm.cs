using System;
using System.Data;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class AdminForm : Form
    {
        Account account = new Account(); // <<< GỌI account class

        public AdminForm()
        {
            InitializeComponent();
        }

        private void AdminForm_Load(object sender, EventArgs e)
        {
            dataGridView1.ReadOnly = true;
            dataGridView1.RowTemplate.Height = 50; // chỉnh cao 1 dòng cho đẹp
            dataGridView1.DataSource = account.getAccounts(); // lấy danh sách account

            dataGridView1.AllowUserToAddRows = false; // không cho thêm dòng trắng

            // Đặt tên cột dễ nhìn
            dataGridView1.Columns["username"].HeaderText = "Tên Đăng Nhập";
            dataGridView1.Columns["password"].HeaderText = "Mật Khẩu";
            dataGridView1.Columns["status"].HeaderText = "Trạng Thái";
            dataGridView1.Columns["role"].HeaderText = "Quyền";

            // Căn giữa dữ liệu ở 1 số cột cho đẹp
            dataGridView1.Columns["status"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.Columns["role"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Tùy chỉnh tự động vừa cột
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }


        private void loadAccountList()
        {
            dataGridView1.DataSource = account.getAccounts();
        }

        private void buttonApprove_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                string username = dataGridView1.CurrentRow.Cells["username"].Value.ToString();
                if (account.updateStatus(username, "Active"))
                {
                    loadAccountList();
                }
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                string username = dataGridView1.CurrentRow.Cells["username"].Value.ToString();
                if (account.deleteAccount(username))
                {
                    loadAccountList();
                }
            }
        }
    }
}
