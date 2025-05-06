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
    public partial class StaticsResultForm : Form
    {
        public StaticsResultForm()
        {
            InitializeComponent();
        }

        private void Form_Load(object sender, EventArgs e)
        {
            // Tạo đối tượng lớp SCORE
            SCORE scoreObj = new SCORE();

            // Lấy số lượng Pass cho mỗi môn học
            DataTable dtPassFailCount = scoreObj.getPassCountByCourse();

            // Kiểm tra nếu DataTable không có dữ liệu
            if (dtPassFailCount == null || dtPassFailCount.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để hiển thị.");
                return;
            }

            // Biến để lưu tổng số Pass và Fail
            int totalPass = 0;
            int totalFail = 0;
            int totalStudents = 0; // Biến để lưu tổng số sinh viên

            // Lặp qua các môn học và hiển thị kết quả
            for (int i = 0; i < dtPassFailCount.Rows.Count; i++)
            {
                string courseName = dtPassFailCount.Rows[i]["CourseName"].ToString();
                int passCount = Convert.ToInt32(dtPassFailCount.Rows[i]["PassCount"]);
                int studentsCount = Convert.ToInt32(dtPassFailCount.Rows[i]["TotalStudents"]); // Tổng số sinh viên tham gia môn học
                int failCount = studentsCount - passCount; // Tính số sinh viên Fail

                // Cộng dồn tổng Pass, Fail và Total Students
                totalPass += passCount;
                totalFail += failCount;
                totalStudents += studentsCount;

                // Tạo Label để hiển thị số lượng Pass và Fail cho môn học
                Label courseLabel = new Label();
                courseLabel.Text = $"{courseName}: Pass = {passCount}, Fail = {failCount}";
                courseLabel.Font = new Font("Times New Roman", 12, FontStyle.Regular); // Font chữ
                courseLabel.ForeColor = Color.White; // Màu chữ trắng
                courseLabel.AutoSize = true;
                courseLabel.MaximumSize = new Size(300, 0); // Giới hạn chiều rộng của Label
                courseLabel.Location = new Point(10, 30 + i * 40); // Vị trí động theo số môn học

                // Thêm Label vào form
                this.Controls.Add(courseLabel);
            }

            // Kiểm tra giá trị tổng sau khi lặp
            Console.WriteLine($"Total Pass: {totalPass}, Total Fail: {totalFail}, Total Students: {totalStudents}");

            // Tính tỷ lệ tổng Pass và Fail
            decimal totalPassPercentage = (totalStudents > 0) ? ((decimal)totalPass / totalStudents) * 100 : 0;
            decimal totalFailPercentage = (totalStudents > 0) ? ((decimal)totalFail / totalStudents) * 100 : 0;

            // Hiển thị tổng số Pass và Fail dưới dạng phần trăm
            Label passLabel = new Label();
            passLabel.Text = $"Pass: {totalPassPercentage:F2}%"; // Hiển thị phần trăm với 2 chữ số thập phân
            passLabel.Font = new Font("Times New Roman", 14, FontStyle.Bold); // Font chữ
            passLabel.AutoSize = true;
            passLabel.ForeColor = Color.Blue; // Màu chữ xanh cho Pass
            passLabel.Location = new Point(10, 30 + dtPassFailCount.Rows.Count * 40); // Vị trí dưới cùng của danh sách môn học

            Label failLabel = new Label();
            failLabel.Text = $"Fail: {totalFailPercentage:F2}%"; // Hiển thị phần trăm với 2 chữ số thập phân
            failLabel.Font = new Font("Times New Roman", 14, FontStyle.Bold); // Font chữ
            failLabel.AutoSize = true;
            failLabel.ForeColor = Color.Red; // Màu chữ đỏ cho Fail
            failLabel.Location = new Point(200, 30 + dtPassFailCount.Rows.Count * 40); // Vị trí bên cạnh Pass

            this.Controls.Add(passLabel);
            this.Controls.Add(failLabel);
        }


    }
}
