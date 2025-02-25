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
    }
}
