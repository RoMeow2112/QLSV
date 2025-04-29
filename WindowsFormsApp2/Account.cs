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
    internal class Account
    {
        MY_DB db = new MY_DB();
        public bool RegisterUser(string username, string password)
        {
            using (SqlCommand command = new SqlCommand("INSERT INTO account (username, password) VALUES (@usr, @pwd)", db.getConnection))
            {
                command.Parameters.Add("@usr", SqlDbType.VarChar, 50).Value = username;
                command.Parameters.Add("@pwd", SqlDbType.VarChar, 255).Value = password;

                db.openConnection();
                if ((command.ExecuteNonQuery() == 1))
                {
                    db.closeConnection();
                    return true;
                }
                else
                {
                    db.closeConnection();
                    return false;
                }
            }
        }

        public DataTable getAccounts()
        {
            SqlCommand command = new SqlCommand("SELECT username, password, status, role FROM account", db.getConnection);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);
            return table;
        }


        public bool updateStatus(string username, string status)
        {
            SqlCommand command = new SqlCommand("UPDATE account SET status=@status WHERE username=@username", db.getConnection);
            command.Parameters.Add("@status", SqlDbType.VarChar).Value = status;
            command.Parameters.Add("@username", SqlDbType.VarChar).Value = username;

            db.openConnection();
            bool success = command.ExecuteNonQuery() == 1;
            db.closeConnection();
            return success;
        }

        // Xóa account
        public bool deleteAccount(string username)
        {
            SqlCommand command = new SqlCommand("DELETE FROM account WHERE username=@username", db.getConnection);
            command.Parameters.Add("@username", SqlDbType.VarChar).Value = username;

            db.openConnection();
            bool success = command.ExecuteNonQuery() == 1;
            db.closeConnection();
            return success;
        }
    }
}
