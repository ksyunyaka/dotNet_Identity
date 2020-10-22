using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Identity.Entity;

namespace WebApplication1.Identity.Dao
{
    public class SqlDao
    {
        private MySqlConnection connection;

        public SqlDao(MySqlConnection con)
        {
            connection = con;
        }


        public UserRole getRole(string roleName)
        {
            string sql = "select * from identity_role where name = @name";
            try
            {
                connection.Open();
                var cmd = new MySqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@name", roleName);
                cmd.Prepare();
                UserRole role = null; ;
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                    {
                        role = new UserRole()
                        {
                            Id = reader.GetString(0),
                            Name = reader.GetString(1)
                        };
                    }
                return role;
            }
            finally
            {
                connection.Close();
            }
            
        }

        public async Task<int> CreateUser(AppUser user)
        {
            string sql = "INSERT INTO identity_user " +
                "VALUES (@user_name, @normalized_user_name, @email, @normalized_email, @password_hash)";
            var cmd = new MySqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@name", user.UserName);
            cmd.Parameters.AddWithValue("@normalized_user_name", user.UserName.ToUpper());
            cmd.Parameters.AddWithValue("@email", user.Email);
            cmd.Parameters.AddWithValue("@normalized_email", user.Email.ToUpper());
            cmd.Parameters.AddWithValue("@password_hash", user.PasswordHash);
            cmd.Prepare();

            return await cmd.ExecuteNonQueryAsync();
        }

        public int CreateRole(UserRole userRole)
        {
            string sql = "INSERT INTO identity_role " +
                "VALUES (@name, @normalized_name)";
            var cmd = new MySqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@name", "someName");
            cmd.Parameters.AddWithValue("@normalized_name", "someName");
            cmd.Prepare();

            return cmd.ExecuteNonQuery();
        }

        public int AddRoleToUser(AppUser user, UserRole userRole)
        {
            string sql = "INSERT INTO identity_user_roles " +
                "VALUES (@user_id, @role_id)";
            var cmd = new MySqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@user_id", user.Id);
            cmd.Parameters.AddWithValue("@role_id", userRole.Id);
            cmd.Prepare();

            return cmd.ExecuteNonQuery();
        }
    }
}
