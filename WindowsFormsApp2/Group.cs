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
    class Group
    {
        MY_DB db = new MY_DB();  // Kết nối đến cơ sở dữ liệu

        // Lấy tất cả nhóm từ cơ sở dữ liệu
        public DataTable GetAllGroups()
        {
            SqlCommand command = new SqlCommand("SELECT * FROM mygroups", db.getConnection); // Lấy tất cả nhóm
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);
            return table;
        }


        // Thêm nhóm mới vào cơ sở dữ liệu
        public bool AddGroup(string groupName)
        {
            MY_DB db = new MY_DB();

            // Câu lệnh SQL chỉ chèn vào cột `name`, không cần phải cung cấp giá trị cho `id` (cột tự động tăng)
            SqlCommand command = new SqlCommand("INSERT INTO mygroups (name) VALUES (@GroupName)", db.getConnection);

            // Thêm tham số vào câu lệnh SQL
            command.Parameters.Add("@GroupName", SqlDbType.VarChar).Value = groupName;

            db.openConnection();
            bool result = command.ExecuteNonQuery() == 1;  // Kiểm tra nếu có 1 dòng bị ảnh hưởng
            db.closeConnection();

            return result;  // Trả về true nếu thêm thành công
        }







        // Sửa tên nhóm
        public bool EditGroup(int groupId, string newGroupName)
        {
            MY_DB db = new MY_DB();

            SqlCommand command = new SqlCommand("UPDATE mygroups SET name = @NewGroupName WHERE id = @GroupId", db.getConnection);
            command.Parameters.Add("@NewGroupName", SqlDbType.VarChar).Value = newGroupName;
            command.Parameters.Add("@GroupId", SqlDbType.Int).Value = groupId;  // Cập nhật theo ID nhóm

            db.openConnection();
            bool result = command.ExecuteNonQuery() == 1;
            db.closeConnection();

            return result;
        }



        // Xóa nhóm khỏi cơ sở dữ liệu
        public bool RemoveGroup(int groupId)
        {
            MY_DB db = new MY_DB();

            SqlCommand command = new SqlCommand("DELETE FROM mygroups WHERE id = @GroupId", db.getConnection);
            command.Parameters.Add("@GroupId", SqlDbType.Int).Value = groupId;  // Xóa theo ID nhóm

            db.openConnection();
            bool result = command.ExecuteNonQuery() == 1;
            db.closeConnection();

            return result;
        }


    }
}
