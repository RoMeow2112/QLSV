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
    public partial class AddScoreForm : Form
    {
        public AddScoreForm()
        {
            InitializeComponent();
        }

        COURSE course = new COURSE();
        SCORE score = new SCORE();
        Student student = new Student(); // thêm dòng này
        private void dataGridViewStudent_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                textBoxStudentID.Text = dataGridViewStudent.Rows[e.RowIndex].Cells["id"].Value.ToString();
            }
        }

        private void AddScoreForm_Load(object sender, EventArgs e)
        {
            comboBoxCourse.DataSource = course.getAllCourses();
            comboBoxCourse.DisplayMember = "label";
            comboBoxCourse.ValueMember = "id";

            Student student = new Student();
            SqlCommand command = new SqlCommand("SELECT id, fname, lname FROM student");
            dataGridViewStudent.DataSource = student.getStudents(command);
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            try
            {
                int studentId = Convert.ToInt32(textBoxStudentID.Text);
                int courseId = Convert.ToInt32(comboBoxCourse.SelectedValue);
                float studentScore = (float)numericUpDownScore.Value;
                string description = textBoxDescription.Text;

                // 1. Kiểm tra ID sinh viên có tồn tại không
                if (!student.checkID(studentId))
                {
                    MessageBox.Show("Mã sinh viên không tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 2. Kiểm tra xem đã có điểm môn đó chưa
                if (score.studentScoreExists(studentId, courseId))
                {
                    MessageBox.Show("Sinh viên này đã có điểm môn học này rồi!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 3. Nếu không lỗi thì Insert
                if (score.insertScore(studentId, courseId, studentScore, description))
                {
                    MessageBox.Show("Thêm điểm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Lỗi khi thêm điểm!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


    }
}
