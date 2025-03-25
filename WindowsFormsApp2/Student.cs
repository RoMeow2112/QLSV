using QLSV;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp2
{
    internal class Student
    {
        MY_DB mydb = new MY_DB();

        // function to insert a new student
        public bool insertStudent(int Id, string fname, string lname, DateTime bdate, string gender, string phone, string address, MemoryStream picture)
        {

            SqlCommand command = new SqlCommand("INSERT INTO student (id, fname, lname, bdate, gender, phone, address, picture)" +
                " VALUES (@id, @fn, @ln, @bdt, @gen, @phn, @adrs, @pic)", mydb.getConnection);

            command.Parameters.Add("@id", SqlDbType.Int).Value = Convert.ToInt32(Id);
            command.Parameters.Add("@fn", SqlDbType.VarChar, 50).Value = fname.Trim();
            command.Parameters.Add("@ln", SqlDbType.VarChar, 50).Value = lname.Trim();
            command.Parameters.Add("@bdt", SqlDbType.Date).Value = bdate;
            command.Parameters.Add("@gen", SqlDbType.VarChar, 10).Value = gender;
            command.Parameters.Add("@phn", SqlDbType.VarChar, 15).Value = phone.Trim();
            command.Parameters.Add("@adrs", SqlDbType.VarChar, 255).Value = address.Trim();
            command.Parameters.Add("@pic", SqlDbType.Image).Value = picture.ToArray();

            mydb.openConnection();

            if ((command.ExecuteNonQuery() == 1))
            {
                mydb.closeConnection();
                return true;
            }
            else
            {
                mydb.closeConnection();
                return false;
            }
        }


        public DataTable getStudents(SqlCommand command)
        {
            command.Connection = mydb.getConnection;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);
            return table;
        }

        public bool checkID(int id)
        {
            SqlCommand command = new SqlCommand("SELECT * FROM student WHERE id = @id", mydb.getConnection);
            command.Parameters.Add("@id", SqlDbType.Int).Value = id;

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);

            return (table.Rows.Count > 0); // true nếu ID đã tồn tại
        }

        // UPDATE
        public bool updateStudent(int id, string fname, string lname, DateTime bdate, string gender, string phone, string address, MemoryStream picture)
        {
            SqlCommand command = new SqlCommand("UPDATE student SET fname=@fn, lname=@ln, bdate=@bd, gender=@gen, phone=@ph, address=@ad, picture=@pic WHERE id=@id", mydb.getConnection);
            command.Parameters.Add("@id", SqlDbType.Int).Value = id;
            command.Parameters.Add("@fn", SqlDbType.VarChar).Value = fname;
            command.Parameters.Add("@ln", SqlDbType.VarChar).Value = lname;
            command.Parameters.Add("@bd", SqlDbType.Date).Value = bdate;
            command.Parameters.Add("@gen", SqlDbType.VarChar).Value = gender;
            command.Parameters.Add("@ph", SqlDbType.VarChar).Value = phone;
            command.Parameters.Add("@ad", SqlDbType.Text).Value = address;
            command.Parameters.Add("@pic", SqlDbType.Image).Value = picture.ToArray();

            mydb.openConnection();
            bool result = command.ExecuteNonQuery() == 1;
            mydb.closeConnection();
            return result;
        }

        // DELETE
        public bool deleteStudent(int id)
        {
            SqlCommand command = new SqlCommand("DELETE FROM student WHERE id=@id", mydb.getConnection);
            command.Parameters.Add("@id", SqlDbType.Int).Value = id;

            mydb.openConnection();
            bool result = command.ExecuteNonQuery() == 1;
            mydb.closeConnection();
            return result;
        }

        public int totalStudent()
        {
            SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM student", mydb.getConnection);
            mydb.openConnection();
            int count = (int)command.ExecuteScalar();
            mydb.closeConnection();
            return count;
        }

        // Lấy tổng số sinh viên nam
        public int totalMaleStudent()
        {
            SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM student WHERE gender = 'Male'", mydb.getConnection);
            mydb.openConnection();
            int count = (int)command.ExecuteScalar();
            mydb.closeConnection();
            return count;
        }

        // Lấy tổng số sinh viên nữ
        public int totalFemaleStudent()
        {
            SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM student WHERE gender = 'Female'", mydb.getConnection);
            mydb.openConnection();
            int count = (int)command.ExecuteScalar();
            mydb.closeConnection();
            return count;
        }
    }
}
