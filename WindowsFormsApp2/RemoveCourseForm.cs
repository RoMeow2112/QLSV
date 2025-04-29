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
    public partial class RemoveCourseForm : Form
    {
        public RemoveCourseForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int id;
            if (int.TryParse(textBoxCourseID.Text, out id))
            {
                COURSE course = new COURSE();
                if (course.deleteCourse(id))
                {
                    MessageBox.Show("Xóa thành công.");
                }
                else
                {
                    MessageBox.Show("Không tìm thấy hoặc lỗi khi xóa.");
                }
            }
            else
            {
                MessageBox.Show("Vui lòng nhập ID hợp lệ.");
            }
        }
    }
}
