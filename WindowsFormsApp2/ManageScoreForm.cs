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
    public partial class ManageScoreForm : Form
    {
        public ManageScoreForm()
        {
            InitializeComponent();
        }

        COURSE course = new COURSE();

        private void Form_Load(object sender, EventArgs e)
        {
            // Tạo đối tượng lớp SCORE

            // Lấy danh sách tất cả các khóa học từ cơ sở dữ liệu
            DataTable dtCourses = course.getAllCourses();

            // Điền dữ liệu vào ComboBox môn học
            cmbCourse.DataSource = dtCourses;
            cmbCourse.DisplayMember = "label";  // Hiển thị tên môn học
            cmbCourse.ValueMember = "id";  // Giá trị môn học (ID)
            cmbCourse.SelectedIndex = -1;  // Đặt ComboBox không chọn mặc định
        }



        private void btnShowStudents_Click(object sender, EventArgs e)
        {
            // Create an instance of the Student class
            Student studentObj = new Student();

            // Create the SqlCommand object with the query to fetch all students
            SqlCommand command = new SqlCommand("SELECT * FROM student");

            // Pass the SqlCommand object to getStudents()
            DataTable dtStudents = studentObj.getStudents(command);

            // Bind the result to the DataGridView
            dataGridView1.DataSource = dtStudents;
        }

        // Hiển thị bảng điểm
        private void btnShowScores_Click(object sender, EventArgs e)
        {
            // Create an instance of the SCORE class
            SCORE scoreObj = new SCORE();

            // Call getScores() without any arguments
            DataTable dtScores = scoreObj.getScores();  // This method now doesn't take any parameters

            // Bind the result to the DataGridView
            dataGridView1.DataSource = dtScores;
        }

        // Thêm điểm cho sinh viên
        private void btnAdd_Click(object sender, EventArgs e)
        {
            int studentId = int.Parse(txtStudentId.Text); // Lấy ID sinh viên từ TextBox
            int courseId = int.Parse(cmbCourse.SelectedValue.ToString()); // Lấy ID khóa học từ ComboBox
            float score = float.Parse(txtScore.Text); // Lấy điểm từ TextBox
            string description = txtDescription.Text; // Lấy mô tả từ TextBox

            SCORE scoreObj = new SCORE();
            bool result = scoreObj.insertScore(studentId, courseId, score, description);

            if (result)
            {
                MessageBox.Show("Score added successfully!");
                btnShowScores_Click(sender, e); // Cập nhật lại bảng điểm
            }
            else
            {
                MessageBox.Show("Failed to add score.");
            }
        }

        // Xóa điểm
        private void btnRemove_Click(object sender, EventArgs e)
        {
            int studentId = int.Parse(txtStudentId.Text);
            int courseId = int.Parse(cmbCourse.SelectedValue.ToString());

            SCORE scoreObj = new SCORE();
            bool result = scoreObj.deleteScore(studentId, courseId);

            if (result)
            {
                MessageBox.Show("Score removed successfully!");
                btnShowScores_Click(sender, e); // Cập nhật lại bảng điểm
            }
            else
            {
                MessageBox.Show("Failed to remove score.");
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Kiểm tra nếu có dòng được chọn
            if (e.RowIndex >= 0)
            {
                // Lấy Student ID từ DataGridView (Giả sử cột chứa ID là "id")
                int studentId = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString()); // 0 là chỉ số của cột "Student ID"

                // Gán Student ID vào TextBox
                txtStudentId.Text = studentId.ToString();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AvgScorebyCourse avg = new AvgScorebyCourse();
            avg.Show(this);
        }
    }
}
