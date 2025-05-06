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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsApp2
{
    public partial class AvgResultbyCourseForm : Form
    {
        public AvgResultbyCourseForm()
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
                    textBoxID.Text = table.Rows[0]["id"].ToString();
                    textBoxFname.Text = table.Rows[0]["fname"].ToString();
                    textBoxLname.Text = table.Rows[0]["lname"].ToString();

                    string studentId = textBoxID.Text.Trim();
                    string firstName = textBoxFname.Text.Trim();
                    string lastName = textBoxLname.Text.Trim();

                    SCORE scoreObj = new SCORE();

                    // Lấy kết quả tìm kiếm
                    DataTable dtResults = scoreObj.getStudentResultBySearch(studentId, firstName, lastName);

                    // Hiển thị kết quả vào DataGridView
                    dataGridView1.DataSource = dtResults;

                    foreach (DataGridViewColumn column in dataGridView1.Columns)
                    {
                        if (column.Name != "StudentID" && column.Name != "FirstName" && column.Name != "LastName")
                        {
                            // Kiểm tra nếu điểm trung bình của môn học là NULL hoặc 0
                            if (dtResults.Rows[0][column.Name] == DBNull.Value)
                            {
                                column.Visible = false; // Ẩn cột nếu không có điểm
                            }
                            else
                            {
                                column.Visible = true;  // Hiển thị cột nếu có điểm
                            }
                        }
                    }
                }

                else
                {
                    MessageBox.Show("No student found", "Find student", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }
    }
}
