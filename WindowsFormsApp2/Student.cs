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
    }
}
