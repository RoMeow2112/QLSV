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
    public partial class AddCourseForm : Form
    {
        public AddCourseForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(textBoxCourseID.Text);
            string label = textBoxLabel.Text.Trim();
            int period = Convert.ToInt32(textBoxPeriod.Text);
            string description = textBoxDescription.Text;

            COURSE course = new COURSE();

            if (label == "")
            {
                MessageBox.Show("Vui lòng nhập tên khóa học.");
                return;
            }

            if (course.checkCourseName(label))
            {
                MessageBox.Show("Tên khóa học đã tồn tại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (period < 10)
            {
                MessageBox.Show("Thời lượng phải từ 10 trở lên.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (course.insertCourse(id, label, period, description))
            {
                MessageBox.Show("Thêm khóa học thành công.");
            }
            else
            {
                MessageBox.Show("Lỗi khi thêm.");
            }
        }
    }
}
