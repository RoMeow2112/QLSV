using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class PrintForm : Form
    {
        Student student = new Student();
        public PrintForm()
        {
            InitializeComponent();
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            string query = "SELECT * FROM student WHERE 1=1";

            if (radioMale.Checked)
                query += " AND gender = 'Male'";
            else if (radioFemale.Checked)
                query += " AND gender = 'Female'";

            if (chkUseDateRange.Checked)
            {
                query += " AND bdate BETWEEN @start AND @end";
            }

            SqlCommand command = new SqlCommand(query);
            if (chkUseDateRange.Checked)
            {
                command.Parameters.Add("@start", SqlDbType.Date).Value = dtpStart.Value;
                command.Parameters.Add("@end", SqlDbType.Date).Value = dtpEnd.Value;
            }

            dataGridView1.DataSource = student.getStudents(command);
        }

        private void btnSaveToFile_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFile = new SaveFileDialog())
            {
                saveFile.Filter = "Text Files |*.txt";
                if (saveFile.ShowDialog() == DialogResult.OK)
                {
                    using (StreamWriter writer = new StreamWriter(saveFile.FileName))
                    {
                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            foreach (DataGridViewCell cell in row.Cells)
                            {
                                writer.Write(cell.Value?.ToString() + "\t");
                            }
                            writer.WriteLine();
                        }
                    }
                    MessageBox.Show("File saved successfully!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            PrintDocument printDoc = new PrintDocument();
            printDoc.PrintPage += (s, ev) =>
            {
                Bitmap bm = new Bitmap(dataGridView1.Width, dataGridView1.Height);
                dataGridView1.DrawToBitmap(bm, new Rectangle(0, 0, dataGridView1.Width, dataGridView1.Height));
                ev.Graphics.DrawImage(bm, 0, 0);
            };

            printDialog.Document = printDoc;
            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                printDoc.Print();
            }
        }

        private void PrintForm_Load(object sender, EventArgs e)
        {
            LoadStudentList();
        }

        private void LoadStudentList()
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
        }
    }
}
