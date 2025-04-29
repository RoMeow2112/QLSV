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
    public partial class EditCourseForm : Form
    {
        public EditCourseForm()
        {
            InitializeComponent();
        }

        private void EditCourseForm_Load(object sender, EventArgs e)
        {
            COURSE course = new COURSE();
            comboBoxSelectCourse.DataSource = course.getAllCourses();
            comboBoxSelectCourse.DisplayMember = "label";
            comboBoxSelectCourse.ValueMember = "id";
        }

        private void comboBoxSelectCourse_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(comboBoxSelectCourse.SelectedValue);
                DataTable dt = new COURSE().getCourseById(id);
                if (dt.Rows.Count > 0)
                {
                    textBoxLabel.Text = dt.Rows[0]["label"].ToString();
                    numericUpDownPeriod.Value = Convert.ToInt32(dt.Rows[0]["period"]);
                    textBoxDescription.Text = dt.Rows[0]["description"].ToString();
                }
            }
            catch { }
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(comboBoxSelectCourse.SelectedValue);
            string label = textBoxLabel.Text.Trim();
            int period = (int)numericUpDownPeriod.Value;
            string description = textBoxDescription.Text;

            COURSE course = new COURSE();

            if (label == "")
            {
                MessageBox.Show("Vui lòng nhập tên khóa học.");
                return;
            }

            if (period < 10)
            {
                MessageBox.Show("Thời lượng phải từ 10 trở lên.");
                return;
            }

            if (course.updateCourse(id, label, period, description))
            {
                MessageBox.Show("Cập nhật thành công.");
            }
            else
            {
                MessageBox.Show("Cập nhật thất bại.");
            }
        }
    }
}
