using QLSV;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WindowsFormsApp2
{
    public class SCORE
    {
        MY_DB mydb = new MY_DB();

        public bool insertScore(int studentId, int courseId, float score, string description)
        {
            SqlCommand cmd = new SqlCommand("INSERT INTO score (student_id, course_id, student_score, description) VALUES (@sid, @cid, @scr, @desc)", mydb.getConnection);
            cmd.Parameters.Add("@sid", SqlDbType.Int).Value = studentId;
            cmd.Parameters.Add("@cid", SqlDbType.Int).Value = courseId;
            cmd.Parameters.Add("@scr", SqlDbType.Float).Value = score;
            cmd.Parameters.Add("@desc", SqlDbType.NVarChar).Value = description;

            mydb.openConnection();
            bool result = cmd.ExecuteNonQuery() == 1;
            mydb.closeConnection();
            return result;
        }

        public bool updateScore(int studentId, int courseId, float score, string description)
        {
            SqlCommand cmd = new SqlCommand("UPDATE score SET student_score=@scr, description=@desc WHERE student_id=@sid AND course_id=@cid", mydb.getConnection);
            cmd.Parameters.Add("@sid", SqlDbType.Int).Value = studentId;
            cmd.Parameters.Add("@cid", SqlDbType.Int).Value = courseId;
            cmd.Parameters.Add("@scr", SqlDbType.Float).Value = score;
            cmd.Parameters.Add("@desc", SqlDbType.NVarChar).Value = description;

            mydb.openConnection();
            bool result = cmd.ExecuteNonQuery() == 1;
            mydb.closeConnection();
            return result;
        }

        public bool deleteScore(int studentId, int courseId)
        {
            SqlCommand cmd = new SqlCommand("DELETE FROM score WHERE student_id=@sid AND course_id=@cid", mydb.getConnection);
            cmd.Parameters.Add("@sid", SqlDbType.Int).Value = studentId;
            cmd.Parameters.Add("@cid", SqlDbType.Int).Value = courseId;

            mydb.openConnection();
            bool result = cmd.ExecuteNonQuery() == 1;
            mydb.closeConnection();
            return result;
        }

        public DataTable getScores()
        {
            // Create the SQL command to fetch all scores from the "score" table
            SqlCommand command = new SqlCommand("SELECT * FROM score", mydb.getConnection);

            // Use SqlDataAdapter to execute the command and fill the result into a DataTable
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable table = new DataTable();

            // Fill the DataTable with the query result
            adapter.Fill(table);

            // Return the DataTable containing the scores
            return table;
        }


        public DataTable getStudentScoreByCourse(int courseId)
        {
            SqlCommand command = new SqlCommand("SELECT * FROM score WHERE course_id=@cid", mydb.getConnection);
            command.Parameters.Add("@cid", SqlDbType.Int).Value = courseId;

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);
            return table;
        }

        public DataTable getAverageScore()
        {
            SqlCommand command = new SqlCommand(@"
            SELECT s.student_id, st.fname, st.lname, 
                   AVG(s.student_score) AS avg_score
            FROM score s
            INNER JOIN student st ON st.id = s.student_id
            GROUP BY s.student_id, st.fname, st.lname", mydb.getConnection);

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);
            return table;
        }

        public bool studentScoreExists(int studentId, int courseId)
        {
            SqlCommand command = new SqlCommand("SELECT * FROM score WHERE student_id=@sid AND course_id=@cid", mydb.getConnection);
            command.Parameters.Add("@sid", SqlDbType.Int).Value = studentId;
            command.Parameters.Add("@cid", SqlDbType.Int).Value = courseId;

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);

            return (table.Rows.Count > 0);
        }

        public DataTable getAverageScoreByCourse()
        {
            // Tạo câu lệnh SQL để tính điểm trung bình theo course_id, nhưng hiển thị tên môn học (label)
            SqlCommand command = new SqlCommand(@"
        SELECT c.label AS CourseName, AVG(s.student_score) AS AverageGrade
        FROM score s
        INNER JOIN course c ON s.course_id = c.id
        GROUP BY c.id, c.label", mydb.getConnection);

            // Sử dụng SqlDataAdapter để thực thi câu lệnh và điền kết quả vào DataTable
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);

            // Trả về DataTable chứa thông tin tên môn học và điểm trung bình
            return table;
        }

        public DataTable getStudentResultBySearch(string studentId, string firstName, string lastName)
        {
            // Lấy danh sách các môn học từ bảng course
            SqlCommand commandCourses = new SqlCommand("SELECT id, label FROM course", mydb.getConnection);
            SqlDataAdapter adapterCourses = new SqlDataAdapter(commandCourses);
            DataTable tableCourses = new DataTable();
            adapterCourses.Fill(tableCourses);

            // Tạo câu lệnh SQL để lấy kết quả của sinh viên từ bảng score, và tính điểm trung bình
            StringBuilder queryBuilder = new StringBuilder();
            queryBuilder.Append(@"
        SELECT 
            st.id AS StudentID,
            st.fname AS FirstName, 
            st.lname AS LastName,
    ");

            // Thêm các môn học động vào câu truy vấn SQL
            foreach (DataRow row in tableCourses.Rows)
            {
                string courseId = row["id"].ToString();
                string courseLabel = row["label"].ToString();

                queryBuilder.Append($@"
            AVG(CASE WHEN c.id = {courseId} AND s.student_score IS NOT NULL THEN s.student_score ELSE NULL END) AS [{courseLabel}], 
        ");
            }

            // Loại bỏ dấu phẩy dư thừa ở cuối câu lệnh
            queryBuilder.Length--;

            // Tiếp tục câu truy vấn để lọc theo sinh viên và nhóm theo sinh viên
            queryBuilder.Append(@"
        AVG(s.student_score) AS AverageScore
        FROM score s
        INNER JOIN student st ON s.student_id = st.id
        INNER JOIN course c ON s.course_id = c.id
        WHERE (st.id = @studentId OR @studentId = '') 
        AND (st.fname LIKE @firstName OR @firstName = '') 
        AND (st.lname LIKE @lastName OR @lastName = '')
        GROUP BY st.id, st.fname, st.lname
        ORDER BY st.id
    ");

            // Tạo câu lệnh SQL
            SqlCommand command = new SqlCommand(queryBuilder.ToString(), mydb.getConnection);
            command.Parameters.AddWithValue("@studentId", studentId);
            command.Parameters.AddWithValue("@firstName", "%" + firstName + "%");
            command.Parameters.AddWithValue("@lastName", "%" + lastName + "%");

            // Sử dụng SqlDataAdapter để thực thi câu lệnh và điền kết quả vào DataTable
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);

            // Trả về DataTable chứa kết quả tìm kiếm
            return table;
        }

        public DataTable getPassCountByCourse()
        {
            // Tạo câu lệnh SQL để đếm số lượng sinh viên Pass và tính tổng số sinh viên cho từng môn học
            SqlCommand command = new SqlCommand(@"
        SELECT 
            c.label AS CourseName, 
            COUNT(CASE WHEN s.student_score > 5 THEN 1 END) AS PassCount,
            COUNT(s.student_id) AS TotalStudents
        FROM 
            score s
        INNER JOIN course c ON s.course_id = c.id
        GROUP BY c.label", mydb.getConnection);

            // Sử dụng SqlDataAdapter để thực thi câu lệnh và điền kết quả vào DataTable
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);

            // Kiểm tra kết quả để xem cột TotalStudents có thực sự có trong bảng trả về không
            foreach (DataColumn column in table.Columns)
            {
                Console.WriteLine(column.ColumnName);  // Debug: In ra tên các cột để xác minh
            }

            // Trả về DataTable chứa số lượng Pass và tổng số sinh viên cho mỗi môn học
            return table;
        }



    }

}
