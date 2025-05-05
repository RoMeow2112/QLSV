using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class RemoveScoreForm : Form
    {
        public RemoveScoreForm()
        {
            InitializeComponent();
        }

        SCORE score = new SCORE();
        COURSE course = new COURSE();

        private void RemoveScoreForm_Load(object sender, EventArgs e)
        {
            // Load danh sách môn học
            comboBoxCourse.DataSource = course.getAllCourses();
            comboBoxCourse.DisplayMember = "label";
            comboBoxCourse.ValueMember = "id";
            comboBoxCourse.SelectedIndex = -1;
            comboBoxStudent.DataSource = null;
        }

        private void comboBoxCourse_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxCourse.SelectedValue == null || comboBoxCourse.SelectedValue.GetType() == typeof(DataRowView))
                return; // Nếu chưa Binding xong thì bỏ qua

            int courseId = Convert.ToInt32(comboBoxCourse.SelectedValue);
            DataTable table = score.getStudentScoreByCourse(courseId);

            comboBoxStudent.DataSource = table;
            comboBoxStudent.DisplayMember = "student_id";
            comboBoxStudent.ValueMember = "student_id";
            comboBoxStudent.SelectedIndex = -1;
        }


        private void buttonRemove_Click(object sender, EventArgs e)
        {
            if (comboBoxCourse.SelectedValue == null || comboBoxStudent.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn đầy đủ Course và Student", "Thiếu dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int studentId = Convert.ToInt32(comboBoxStudent.SelectedValue);
            int courseId = Convert.ToInt32(comboBoxCourse.SelectedValue);

            if (score.deleteScore(studentId, courseId))
            {
                MessageBox.Show("Xóa điểm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // Reload lại student list
                comboBoxCourse_SelectedIndexChanged(null, null);
            }
            else
            {
                MessageBox.Show("Không thể xóa điểm!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
