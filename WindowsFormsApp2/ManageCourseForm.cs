using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsApp2
{
    public partial class ManageCourseForm : Form
    {
        public ManageCourseForm()
        {
            InitializeComponent();
        }

        COURSE course = new COURSE();
        int pos = 0;

        private void ManageCourseForm_Load(object sender, EventArgs e)
        {
            listBoxCourse.DataSource = course.getAllCourses();
            listBoxCourse.DisplayMember = "label";
            listBoxCourse.ValueMember = "id";

            showData(pos);
            labelTotalCourses.Text = "Total Courses: " + course.totalCourses();
        }

        private void listBoxCourse_Click(object sender, EventArgs e)
        {
            if (listBoxCourse.SelectedItem == null) return;

            DataRowView row = listBoxCourse.SelectedItem as DataRowView;
            if (row != null)
            {
                textBoxID.Text = row["id"].ToString();
                textBoxLabel.Text = row["label"].ToString();
                numericUpDownPeriod.Value = Convert.ToInt32(row["period"]);
                textBoxDescription.Text = row["description"].ToString();
            }
        }

        void showData(int index)
        {
            DataTable table = course.getAllCourses();
            if (table.Rows.Count > 0)
            {
                textBoxID.Text = table.Rows[index]["id"].ToString();
                textBoxLabel.Text = table.Rows[index]["label"].ToString();
                numericUpDownPeriod.Value = Convert.ToInt32(table.Rows[index]["period"]);
                textBoxDescription.Text = table.Rows[index]["description"].ToString();
                listBoxCourse.SelectedIndex = index;
            }
        }
        private void buttonFirst_Click(object sender, EventArgs e)
        {
            pos = 0;
            showData(pos);
        }

        private void buttonLast_Click(object sender, EventArgs e)
        {
            pos = course.getAllCourses().Rows.Count - 1;
            showData(pos);
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            if (pos < course.getAllCourses().Rows.Count - 1)
            {
                pos++;
                showData(pos);
            }
        }

        private void buttonPrevious_Click(object sender, EventArgs e)
        {
            if (pos > 0)
            {
                pos--;
                showData(pos);
            }
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(textBoxID.Text);

            DialogResult result = MessageBox.Show("Bạn có chắc muốn xóa không?", "Xác nhận", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                if (course.deleteCourse(id))
                {
                    MessageBox.Show("Xóa thành công");
                    listBoxCourse.DataSource = course.getAllCourses();
                    labelTotalCourses.Text = "Total Courses: " + course.totalCourses();
                    showData(0); // Reset lại form về đầu
                }
                else
                {
                    MessageBox.Show("Xóa thất bại");
                }
            }
        }


        private void buttonAdd_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(textBoxID.Text);
            string label = textBoxLabel.Text;
            int period = (int)numericUpDownPeriod.Value;
            string description = textBoxDescription.Text;

            if (label.Trim() == "")
            {
                MessageBox.Show("Thiếu thông tin tên môn học");
                return;
            }

            if (period <= 10)
            {
                MessageBox.Show("Thời lượng phải lớn hơn 10");
                return;
            }

            if (!course.checkCourseName(label))
            {
                MessageBox.Show("Tên môn học đã tồn tại");
                return;
            }

            if (course.insertCourse(id, label, period, description))
            {
                MessageBox.Show("Thêm môn học thành công");
                listBoxCourse.DataSource = course.getAllCourses();
                labelTotalCourses.Text = "Total Courses: " + course.totalCourses();
            }
            else
            {
                MessageBox.Show("Lỗi khi thêm");
            }
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(textBoxID.Text);
            string label = textBoxLabel.Text;
            int period = (int)numericUpDownPeriod.Value;
            string description = textBoxDescription.Text;

            if (label.Trim() == "")
            {
                MessageBox.Show("Thiếu thông tin tên môn học");
                return;
            }

            if (period <= 10)
            {
                MessageBox.Show("Thời lượng phải lớn hơn 10");
                return;
            }

            // Kiểm tra tên trùng với môn khác
            DataRow selectedCourse = course.getCourseById(id).Rows[0];
            string oldLabel = selectedCourse["label"].ToString();

            if (label != oldLabel && !course.checkCourseName(label))
            {
                MessageBox.Show("Tên môn học đã tồn tại");
                return;
            }

            if (course.updateCourse(id, label, period, description))
            {
                MessageBox.Show("Cập nhật thành công");
                listBoxCourse.DataSource = course.getAllCourses();
            }
            else
            {
                MessageBox.Show("Lỗi khi cập nhật");
            }
        }


    }
}
