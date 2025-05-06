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
    class Contact
    {
        MY_DB mydb = new MY_DB();

        // Insert new contact into the database
        public bool insertContact(int id, string fname, string lname, string group_id, string phone, string email, string address, byte[] pic, int userid)
        {
            SqlCommand cmd = new SqlCommand("INSERT INTO mycontact (id, fname, lname, group_id, phone, email, address, pic, userid) " +
                                           "VALUES (@id, @fname, @lname, @group_id, @phone, @email, @address, @pic, @userid)", mydb.getConnection);

            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
            cmd.Parameters.Add("@fname", SqlDbType.VarChar, 50).Value = fname;
            cmd.Parameters.Add("@lname", SqlDbType.VarChar, 50).Value = lname;
            cmd.Parameters.Add("@group_id", SqlDbType.NChar, 10).Value = group_id;
            cmd.Parameters.Add("@phone", SqlDbType.NChar, 10).Value = phone;
            cmd.Parameters.Add("@email", SqlDbType.NChar, 10).Value = email;
            cmd.Parameters.Add("@address", SqlDbType.Text).Value = address;
            cmd.Parameters.Add("@pic", SqlDbType.Image).Value = pic;
            cmd.Parameters.Add("@userid", SqlDbType.Int).Value = userid;

            mydb.openConnection();
            bool result = cmd.ExecuteNonQuery() == 1;
            mydb.closeConnection();

            return result;
        }

        // Update contact in the database
        public bool updateContact(int id, string fname, string lname, string group_id, string phone, string email, string address, byte[] pic)
        {
            SqlCommand cmd = new SqlCommand("UPDATE mycontact SET fname=@fname, lname=@lname, group_id=@group_id, phone=@phone, email=@email, " +
                                           "address=@address, pic=@pic WHERE id=@id", mydb.getConnection);

            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
            cmd.Parameters.Add("@fname", SqlDbType.VarChar, 50).Value = fname;
            cmd.Parameters.Add("@lname", SqlDbType.VarChar, 50).Value = lname;
            cmd.Parameters.Add("@group_id", SqlDbType.NChar, 10).Value = group_id;
            cmd.Parameters.Add("@phone", SqlDbType.NChar, 10).Value = phone;
            cmd.Parameters.Add("@email", SqlDbType.NChar, 10).Value = email;
            cmd.Parameters.Add("@address", SqlDbType.Text).Value = address;
            cmd.Parameters.Add("@pic", SqlDbType.Image).Value = pic;

            mydb.openConnection();
            bool result = cmd.ExecuteNonQuery() == 1;
            mydb.closeConnection();

            return result;
        }

        // Delete contact from the database
        public bool deleteContact(int id)
        {
            SqlCommand cmd = new SqlCommand("DELETE FROM mycontact WHERE id=@id", mydb.getConnection);
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;

            mydb.openConnection();
            bool result = cmd.ExecuteNonQuery() == 1;
            mydb.closeConnection();

            return result;
        }

        // Get all contacts
        public DataTable getAllContacts()
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM mycontact", mydb.getConnection);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            return dt;
        }

        // Get contact by id
        public DataRow getContactById(int id)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM mycontact WHERE id=@id", mydb.getConnection);
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }
    }
}
