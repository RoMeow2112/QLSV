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
    public partial class HRForm: Form
    {
        public HRForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddContact addcontact = new AddContact();
            addcontact.Show(this);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            EditContact editcontact = new EditContact();
            editcontact.Show(this);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string groupName = txtgroupname.Text.Trim();

            if (string.IsNullOrEmpty(groupName))
            {
                MessageBox.Show("Vui lòng nhập tên nhóm.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            MY_DB db = new MY_DB();

            string query = "INSERT INTO mygroups (name) VALUES (@name)";
            SqlCommand cmd = new SqlCommand(query, db.getConnection);
            cmd.Parameters.AddWithValue("@name", groupName);

            try
            {
                db.openConnection();
                int result = cmd.ExecuteNonQuery();
                db.closeConnection();

                if (result > 0)
                {
                    MessageBox.Show("Tạo nhóm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Không thể tạo nhóm.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                db.closeConnection();
                MessageBox.Show("Lỗi khi tạo nhóm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            RefreshAllGroupComboboxes();
            ClearAllTextboxes();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (cbSelectGroupEdit.SelectedItem == null || string.IsNullOrWhiteSpace(txtEditGroupName.Text))
            {
                MessageBox.Show("Vui lòng chọn nhóm và nhập tên mới.");
                return;
            }

            string newName = txtEditGroupName.Text.Trim();
            int groupId = Convert.ToInt32(cbSelectGroupEdit.SelectedValue); // ID nhóm

            MY_DB db = new MY_DB();
            string query = "UPDATE mygroups SET name = @name WHERE id = @id";

            SqlCommand cmd = new SqlCommand(query, db.getConnection);
            cmd.Parameters.AddWithValue("@name", newName);
            cmd.Parameters.AddWithValue("@id", groupId);

            try
            {
                db.openConnection();
                if (cmd.ExecuteNonQuery() > 0)
                    MessageBox.Show("Đổi tên nhóm thành công.");
                else
                    MessageBox.Show("Không tìm thấy nhóm cần đổi.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            finally
            {
                db.closeConnection();
            }

            RefreshAllGroupComboboxes();
            ClearAllTextboxes();
        }

        private void LoadGroupList(ComboBox cb)
        {
            MY_DB db = new MY_DB();
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT id, name FROM mygroups", db.getConnection);
            DataTable table = new DataTable();
            adapter.Fill(table);

            cb.DataSource = table;
            cb.DisplayMember = "name";
            cb.ValueMember = "id";
        }

        private void GroupForm_Load(object sender, EventArgs e)
        {
            LoadGroupList(cbSelectGroupEdit);
            LoadGroupList(cbSelectGroupRemove);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (cbSelectGroupRemove.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn nhóm cần xóa.");
                return;
            }

            int groupId = Convert.ToInt32(cbSelectGroupRemove.SelectedValue);

            MY_DB db = new MY_DB();
            string query = "DELETE FROM mygroups WHERE id = @id";

            SqlCommand cmd = new SqlCommand(query, db.getConnection);
            cmd.Parameters.AddWithValue("@id", groupId);

            try
            {
                db.openConnection();
                if (cmd.ExecuteNonQuery() > 0)
                    MessageBox.Show("Xóa nhóm thành công.");
                else
                    MessageBox.Show("Không tìm thấy nhóm cần xóa.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            finally
            {
                db.closeConnection();
            }

            RefreshAllGroupComboboxes();
            ClearAllTextboxes();

        }

        private void RefreshAllGroupComboboxes()
        {
            LoadGroupList(cbSelectGroupEdit);
            LoadGroupList(cbSelectGroupRemove);
        }

        private void ClearAllTextboxes()
        {
            txtgroupname.Clear();
            txtEditGroupName.Clear();
        }

    }
}
