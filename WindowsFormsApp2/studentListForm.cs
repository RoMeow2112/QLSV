using ExcelDataReader;
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
    public partial class studentListForm : Form
    {
        public studentListForm()
        {
            InitializeComponent();
        }

        Student student = new Student();

        private void studentListForm_Load(object sender, EventArgs e)
        {
            SqlCommand command = new SqlCommand("SELECT * FROM student");
            dataGridView1.ReadOnly = true;
            DataGridViewImageColumn picCol = new DataGridViewImageColumn();
            dataGridView1.RowTemplate.Height = 80;
            dataGridView1.DataSource = student.getStudents(command);
            picCol = (DataGridViewImageColumn)dataGridView1.Columns[7];
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
        }
        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            UpdateDeleteStudentForm updateDeleteStudentForm = new UpdateDeleteStudentForm();
            updateDeleteStudentForm.textBoxID.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            updateDeleteStudentForm.textBoxFname.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            updateDeleteStudentForm.textBoxLname.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            updateDeleteStudentForm.dateTimePicker1.Value = (DateTime)dataGridView1.CurrentRow.Cells[3].Value;

            if ((dataGridView1.CurrentRow.Cells[4].Value.ToString().Trim() == "Female"))
            {
                updateDeleteStudentForm.radioButtonFemale.Checked = true;
            }
            else
            {
                updateDeleteStudentForm.radioButtonMale.Checked = true;
            }
                updateDeleteStudentForm.textBoxPhone.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
                updateDeleteStudentForm.textBoxAddress.Text = dataGridView1.CurrentRow.Cells[6].Value.ToString();

                byte[] pic;
                pic = (byte[])dataGridView1.CurrentRow.Cells[7].Value;
                MemoryStream picture = new MemoryStream(pic);
                updateDeleteStudentForm.pictureBoxStudentImage.Image = Image.FromStream(picture);
                updateDeleteStudentForm.Show();
            }

        private void button1_Click(object sender, EventArgs e)
        {
            studentListForm_Load(sender, e);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Text Files (*.txt)|*.txt";
            sfd.FileName = "student_list.txt";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter sw = new StreamWriter(sfd.FileName))
                {
                    // Ghi tiêu đề cột
                    string header =
                        "ID".PadRight(10) +
                        "First Name".PadRight(13) +
                        "Last Name".PadRight(13) +
                        "Birthdate".PadRight(22) +
                        "Gender".PadRight(9) +
                        "Phone".PadRight(13) +
                        "Address";
                    sw.WriteLine(header);
                    sw.WriteLine(new string('-', 90));

                    // Ghi từng dòng dữ liệu
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        var row = dataGridView1.Rows[i];
                        if (!row.IsNewRow)
                        {
                            string line =
                                row.Cells[0].Value.ToString().PadRight(10) +
                                row.Cells[1].Value.ToString().PadRight(13) +
                                row.Cells[2].Value.ToString().PadRight(13) +
                                Convert.ToDateTime(row.Cells[3].Value).ToString("M/d/yyyy hh:mm tt").PadRight(22) +
                                row.Cells[4].Value.ToString().PadRight(9) +
                                row.Cells[5].Value.ToString().PadRight(13) +
                                row.Cells[6].Value.ToString();

                            sw.WriteLine(line);
                        }
                    }

                    MessageBox.Show("Lưu file thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Excel Files|*.xlsx;*.xls";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                using (var stream = File.Open(ofd.FileName, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelDataReader.ExcelReaderFactory.CreateReader(stream))
                    {
                        var result = reader.AsDataSet();
                        var table = result.Tables[0]; // Lấy sheet đầu tiên

                        for (int i = 1; i < table.Rows.Count; i++) // bỏ dòng header
                        {
                            try
                            {
                                int id = int.Parse(table.Rows[i][0].ToString());
                                string fname = table.Rows[i][1].ToString();
                                string lname = table.Rows[i][2].ToString();
                                DateTime bdate = DateTime.Parse(table.Rows[i][3].ToString());
                                string gender = table.Rows[i][4].ToString();
                                string phone = table.Rows[i][5].ToString();
                                string address = table.Rows[i][6].ToString();

                                // Tạo email tự động từ MSSV
                                string email = id + "@student.hcmute.edu.vn";

                                

                                // Insert student
                                if (!student.checkID(id))
                                {
                                    string imagePath = Path.Combine(Application.StartupPath, "Images", "default.png");
                                    byte[] imageBytes = File.ReadAllBytes(imagePath); // phải có file default.png cùng thư mục hoặc xử lý khác
                                    MemoryStream picture = new MemoryStream(imageBytes);

                                    bool success = student.insertStudent(id, fname, lname, bdate, gender, phone, address, picture, email);

                                    picture.Dispose();
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Lỗi dòng {i + 1}: {ex.Message}");
                            }
                        }

                        MessageBox.Show("Import danh sách thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        studentListForm_Load(sender, e); // reload lại danh sách
                    }
                }
            }
        }
    }
    }

