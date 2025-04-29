using QLSV;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace WindowsFormsApp2
{
    public class COURSE
    {
        MY_DB mydb = new MY_DB();

        // Insert Course
        public bool insertCourse(int id, string label, int period, string description)
        {
            SqlCommand cmd = new SqlCommand("INSERT INTO course (id, label, period, description) VALUES (@id, @label, @period, @desc)", mydb.getConnection);
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
            cmd.Parameters.Add("@label", SqlDbType.VarChar).Value = label;
            cmd.Parameters.Add("@period", SqlDbType.Int).Value = period;
            cmd.Parameters.Add("@desc", SqlDbType.Text).Value = description;

            mydb.openConnection();
            bool success = cmd.ExecuteNonQuery() == 1;
            mydb.closeConnection();
            return success;
        }

        public DataTable getCourseByLabel(string label)
        {
            SqlCommand command = new SqlCommand("SELECT * FROM course WHERE label = @label", mydb.getConnection);
            command.Parameters.Add("@label", SqlDbType.VarChar).Value = label;

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);

            return table;
        }


        // Update Course
        public bool updateCourse(int id, string label, int period, string description)
        {
            SqlCommand cmd = new SqlCommand("UPDATE course SET label=@label, period=@period, description=@desc WHERE id=@id", mydb.getConnection);
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
            cmd.Parameters.Add("@label", SqlDbType.VarChar).Value = label;
            cmd.Parameters.Add("@period", SqlDbType.Int).Value = period;
            cmd.Parameters.Add("@desc", SqlDbType.Text).Value = description;

            mydb.openConnection();
            bool success = cmd.ExecuteNonQuery() == 1;
            mydb.closeConnection();
            return success;
        }

        // Delete Course
        public bool deleteCourse(int id)
        {
            SqlCommand cmd = new SqlCommand("DELETE FROM course WHERE id=@id", mydb.getConnection);
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;

            mydb.openConnection();
            bool success = cmd.ExecuteNonQuery() == 1;
            mydb.closeConnection();
            return success;
        }

        // Get All Courses
        public DataTable getAllCourses()
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM course", mydb.getConnection);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable table = new DataTable();
            adapter.Fill(table);
            return table;
        }

        // Get Course By ID
        public DataTable getCourseById(int id)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM course WHERE id=@id", mydb.getConnection);
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;

            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable table = new DataTable();
            adapter.Fill(table);
            return table;
        }

        // Check Course Name trùng không
        public bool checkCourseName(string name, int courseID = 0)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM course WHERE label=@name AND id <> @id", mydb.getConnection);
            cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = name;
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = courseID;

            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable table = new DataTable();
            adapter.Fill(table);

            return table.Rows.Count > 0;
        }

        // Tổng số khóa học
        public int totalCourses()
        {
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM course", mydb.getConnection);
            mydb.openConnection();
            int count = (int)cmd.ExecuteScalar();
            mydb.closeConnection();
            return count;
        }

        // Hàm thi hành lệnh SQL đếm theo điều kiện
        public string execCount(string query)
        {
            SqlCommand cmd = new SqlCommand(query, mydb.getConnection);
            mydb.openConnection();
            string result = cmd.ExecuteScalar().ToString();
            mydb.closeConnection();
            return result;
        }
    }
}
