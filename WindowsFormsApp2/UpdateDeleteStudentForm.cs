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
    public partial class UpdateDeleteStudentForm : Form
    {
        public UpdateDeleteStudentForm()
        {
            InitializeComponent();
            comboBoxSearchType.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            {
                string searchType = comboBoxSearchType.SelectedItem.ToString(); // Lấy kiểu tìm kiếm
                string query = "";
                SqlCommand command = new SqlCommand();

                if (searchType == "ID")
                {
                    if (!int.TryParse(textBoxID.Text, out int id))
                    {
                        MessageBox.Show("ID phải là số!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    query = "SELECT id, fname, lname, bdate, gender, phone, address, picture FROM student WHERE id = @value";
                    command.Parameters.Add("@value", SqlDbType.Int).Value = id;
                }
                else if (searchType == "Phone")
                {
                    query = "SELECT id, fname, lname, bdate, gender, phone, address, picture FROM student WHERE phone = @value";
                    command.Parameters.Add("@value", SqlDbType.VarChar).Value = textBoxPhone.Text.Trim();
                }
                else if (searchType == "Name")
                {
                    query = "SELECT id, fname, lname, bdate, gender, phone, address, picture " +
                            "FROM student WHERE CONCAT(fname, ' ', lname) LIKE @value";
                    command.Parameters.Add("@value", SqlDbType.VarChar).Value = "%" + textBoxFname.Text.Trim() + " " + textBoxLname.Text.Trim() + "%";
                }

                command.CommandText = query;
                DataTable table = new Student().getStudents(command);

                if (table.Rows.Count > 0)
                {
                    textBoxFname.Text = table.Rows[0]["fname"].ToString();
                    textBoxLname.Text = table.Rows[0]["lname"].ToString();
                    dateTimePicker1.Value = (DateTime)table.Rows[0]["bdate"];
                    if (table.Rows[0]["gender"].ToString().Trim() == "Female")
                    {
                        radioButtonFemale.Checked = true;
                    }
                    else
                    {
                        radioButtonMale.Checked = true;
                    }
                    textBoxPhone.Text = table.Rows[0]["phone"].ToString();
                    textBoxAddress.Text = table.Rows[0]["address"].ToString();

                    byte[] pic = (byte[])table.Rows[0]["picture"];
                    MemoryStream picture = new MemoryStream(pic);
                    pictureBoxStudentImage.Image = Image.FromStream(picture);
                }

                else
                {
                    MessageBox.Show("No student found", "Find student", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        private void textBoxID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }   
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog opf = new OpenFileDialog();
            opf.Filter = "Select Image(*.jpg;*.png;*.gif)|*.jpg;*.png;*.gif";
            if ((opf.ShowDialog() == DialogResult.OK))
            {
                pictureBoxStudentImage.Image = Image.FromFile(opf.FileName);
            }
        }

        private void textBoxFname_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBoxLname_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBoxPhone_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int id;
            if (!int.TryParse(textBoxID.Text, out id))
            {
                MessageBox.Show("Not found ID", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string fname = textBoxFname.Text;
            string lname = textBoxLname.Text;
            DateTime bdate = dateTimePicker1.Value;
            string phone = textBoxPhone.Text;
            string address = textBoxAddress.Text;
            string gender = radioButtonMale.Checked ? "Male" : "Female";

            MemoryStream picture = new MemoryStream();
            pictureBoxStudentImage.Image.Save(picture, pictureBoxStudentImage.Image.RawFormat);

            if (new Student().updateStudent(id, fname, lname, bdate, gender, phone, address, picture))
            {
                MessageBox.Show("Student information updated successfully", "Edit Student", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Error while updating student", "Edit Student", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int id;
            if (!int.TryParse(textBoxID.Text, out id))
            {
                MessageBox.Show("Not found ID", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult result = MessageBox.Show("Are you sure to delete this student?", "Delete Student", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                if (new Student().deleteStudent(id))
                {
                    MessageBox.Show("Delete", "Delete Student", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    clearFields();
                }
                else
                {
                    MessageBox.Show("Error", "Delete Student", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void clearFields()
        {
            textBoxID.Clear();
            textBoxFname.Clear();
            textBoxLname.Clear();
            textBoxPhone.Clear();
            textBoxAddress.Clear();
            dateTimePicker1.Value = DateTime.Now;
            radioButtonMale.Checked = true;
            pictureBoxStudentImage.Image = null;
        }
    }
}
