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
            SqlCommand command = new SqlCommand("SELECT * FROM score", mydb.getConnection);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);
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

    }

}
