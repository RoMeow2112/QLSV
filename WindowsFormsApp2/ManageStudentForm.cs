using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsApp2
{
    public partial class ManageStudentForm : Form
    {
        Student student = new Student();

        public ManageStudentForm()
        {
            InitializeComponent();
            LoadStudentList();
        }

        private void LoadStudentList(string searchQuery = "")
        {
            SqlCommand command;
            if (string.IsNullOrWhiteSpace(searchQuery))
            {
                command = new SqlCommand("SELECT * FROM student");
            }
            else
            {
                string query = "SELECT * FROM student WHERE " +
                               "REPLACE(CONCAT(fname, ' ', lname, ' ', address), ' ', '') LIKE @search";

                command = new SqlCommand(query);
                command.Parameters.AddWithValue("@search", "%" + searchQuery.Replace(" ", "") + "%");
            }


            dataGridView1.ReadOnly = true;
            dataGridView1.RowTemplate.Height = 80;
            dataGridView1.DataSource = student.getStudents(command);

            DataGridViewImageColumn picCol = (DataGridViewImageColumn)dataGridView1.Columns["picture"];
            picCol.ImageLayout = DataGridViewImageCellLayout.Stretch;

            dataGridView1.AllowUserToAddRows = false;

            dataGridView1.Columns["id"].HeaderText = "Mã SV";
            dataGridView1.Columns["fname"].HeaderText = "Họ";
            dataGridView1.Columns["lname"].HeaderText = "Tên";
            dataGridView1.Columns["bdate"].HeaderText = "Ngày Sinh";
            dataGridView1.Columns["gender"].HeaderText = "Giới Tính";
            dataGridView1.Columns["phone"].HeaderText = "Số Điện Thoại";
            dataGridView1.Columns["address"].HeaderText = "Địa Chỉ";
            dataGridView1.Columns["picture"].HeaderText = "Hình Ảnh";
            dataGridView1.Columns["email"].HeaderText = "Email";

            LabelTotalStudents.Text = "Total Students: " + dataGridView1.Rows.Count;
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                // Gán dữ liệu từ DataGridView vào các ô nhập trên form
                txtStudentID.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                TextBoxFname.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                TextBoxLname.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                DateTimePicker1.Value = (DateTime)dataGridView1.CurrentRow.Cells[3].Value;

                // Xử lý giới tính
                if (dataGridView1.CurrentRow.Cells[4].Value.ToString().Trim() == "Female")
                {
                    RadioButtonFemale.Checked = true;
                }
                else
                {
                    RadioButtonMale.Checked = true;
                }

                TextBoxPhone.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
                TextBoxAddress.Text = dataGridView1.CurrentRow.Cells[6].Value.ToString();

                // Hiển thị ảnh
                if (dataGridView1.CurrentRow.Cells[7].Value != DBNull.Value)
                {
                    byte[] pic = (byte[])dataGridView1.CurrentRow.Cells[7].Value;
                    MemoryStream picture = new MemoryStream(pic);
                    PictureBoxStudentImage.Image = Image.FromStream(picture);
                }
                else
                {
                    PictureBoxStudentImage.Image = null; // Nếu không có ảnh thì để trống
                }
            }
        }

        private void ButtonAddStudent_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txtStudentID.Text);
            string fname = TextBoxFname.Text;
            string lname = TextBoxLname.Text;
            DateTime bdate = DateTimePicker1.Value;
            string phone = TextBoxPhone.Text;
            string adrs = TextBoxAddress.Text;
            string gender = "Male";

            if (RadioButtonMale.Checked)
            {
                gender = "Female";
            }
            
            string email = id.ToString() + "@student.hcmute.edu.vn";

            MemoryStream pic = new MemoryStream();
            int born_year = DateTimePicker1.Value.Year;
            int this_year = DateTime.Now.Year;

            if ((this_year - born_year) < 10 || (this_year - born_year) > 100)
            {
                MessageBox.Show("The Student Age Must Be Between 10 and 100 year", "Invalid Birth Date", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (verif())
            {
                if (student.checkID(id))
                {
                    MessageBox.Show("This ID already exists. Please enter a different ID!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    return;
                }
                PictureBoxStudentImage.Image.Save(pic, PictureBoxStudentImage.Image.RawFormat);
                if (student.insertStudent(id, fname, lname, bdate, gender, phone, adrs, pic, email))
                {
                    MessageBox.Show("New Student Added", "Add Student", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Error", "Add Student", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Empty Fields", "Add Student", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            LoadStudentList();
        }

        bool verif()
        {
            if ((TextBoxFname.Text.Trim() == "")
                || (TextBoxLname.Text.Trim() == "")
                || (TextBoxAddress.Text.Trim() == "")
                || (TextBoxPhone.Text.Trim() == "")
                || (PictureBoxStudentImage.Image == null))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void ButtonUploadImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog opf = new OpenFileDialog();
            opf.Filter = "Select Image(*.jpg;*.png;*.gif)|*.jpg;*.png;*.gif";
            if ((opf.ShowDialog() == DialogResult.OK))
            {
                PictureBoxStudentImage.Image = Image.FromFile(opf.FileName);
            }
        }

        private void txtStudentID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void TextBoxFname_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void TextBoxLname_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void TextBoxPhone_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            int id;
            if (!int.TryParse(txtStudentID.Text, out id))
            {
                MessageBox.Show("Not found ID", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string fname = TextBoxFname.Text;
            string lname = TextBoxLname.Text;
            DateTime bdate = DateTimePicker1.Value;
            string phone = TextBoxPhone.Text;
            string address = TextBoxAddress.Text;
            string gender = RadioButtonMale.Checked ? "Male" : "Female";

            MemoryStream picture = new MemoryStream();
            PictureBoxStudentImage.Image.Save(picture, PictureBoxStudentImage.Image.RawFormat);

            if (new Student().updateStudent(id, fname, lname, bdate, gender, phone, address, picture))
            {
                MessageBox.Show("Student information updated successfully", "Edit Student", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Error while updating student", "Edit Student", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            LoadStudentList();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            int id;
            if (!int.TryParse(txtStudentID.Text, out id))
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
            LoadStudentList();
        }

        private void clearFields()
        {
            txtStudentID.Clear();
            TextBoxFname.Clear();
            TextBoxLname.Clear();
            TextBoxPhone.Clear();
            TextBoxAddress.Clear();
            DateTimePicker1.Value = DateTime.Now;
            RadioButtonMale.Checked = true;
            PictureBoxStudentImage.Image = null;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            clearFields();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchQuery = TextBoxSearch.Text;
            LoadStudentList(searchQuery);
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.FileName = "student_" + txtStudentID.Text;

            if (PictureBoxStudentImage.Image == null)
            {
                MessageBox.Show("No Image in the PictureBox", "Download Image", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                if (saveFile.ShowDialog() == DialogResult.OK)
                {
                    ImageFormat format = ImageFormat.Jpeg; // Hoặc format khác nếu muốn
                    string extension = GetImageExtension(format);
                    PictureBoxStudentImage.Image.Save(saveFile.FileName + extension, format);
                }
            }
        }

        // Hàm lấy đuôi file hợp lệ từ ImageFormat
        private string GetImageExtension(ImageFormat format)
        {
            if (format.Equals(ImageFormat.Jpeg)) return ".jpg";
            if (format.Equals(ImageFormat.Png)) return ".png";
            if (format.Equals(ImageFormat.Bmp)) return ".bmp";
            if (format.Equals(ImageFormat.Gif)) return ".gif";
            if (format.Equals(ImageFormat.Tiff)) return ".tiff";
            return ".img"; // Trường hợp không xác định
        }

    }
}
