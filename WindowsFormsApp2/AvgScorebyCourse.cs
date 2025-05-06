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
    public partial class AvgScorebyCourse : Form
    {
        public AvgScorebyCourse()
        {
            InitializeComponent();
        }

        private void Form_Load(object sender, EventArgs e)
        {
            // Tạo đối tượng lớp SCORE
            SCORE scoreObj = new SCORE();

            // Lấy điểm trung bình theo môn học
            DataTable dtAverageScores = scoreObj.getAverageScoreByCourse();

            // Điền dữ liệu vào DataGridView
            dataGridView1.DataSource = dtAverageScores;

            // Tùy chỉnh các cột trong DataGridView nếu cần
            dataGridView1.Columns["CourseName"].HeaderText = "Course Name";  // Đổi tên cột label thành "Course Name"
            dataGridView1.Columns["AverageGrade"].HeaderText = "Average Grade"; // Đổi tên cột AverageGrade thành "Average Grade"
        }

    }
}
