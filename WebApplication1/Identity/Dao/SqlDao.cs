using Microsoft.AspNetCore.Identity;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
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

        public AppUser getUserByUserName(string userName)
        {
            string sql = "select* from identity_user where user_name=@username";
            try
            {
                connection.Open();
                var cmd = new MySqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@username", userName);
                cmd.Prepare();
                AppUser user = null; ;
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                    {
                        user = new AppUser()
                        {
                            Id = reader.GetString("id"),
                            UserName = reader.GetString("user_name"),
                            Email = reader.GetString("email"),
                            PasswordHash = reader.GetString("password_hash")

                        };
                        return user;
                    }
                return null;
            }
            finally
            {
                connection.Close();
            }
        }

        public async Task<AppUser> getUserById(string userId)
        {
            string sql = "select* from identity_user where id=@userid";
            try
            {
                await connection.OpenAsync();
                var cmd = new MySqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@userid", int.Parse(userId));
                cmd.Prepare();
                AppUser user = null; ;
                using (var reader = await cmd.ExecuteReaderAsync())
                    while (reader.Read())
                    {
                        user = new AppUser()
                        {
                            Id = reader.GetString("id"),
                            UserName = reader.GetString("user_name"),
                            Email = reader.GetString("email"),
                            PasswordHash = reader.GetString("password_hash")
                        };
                        return user;
                    }
                return null;
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task<IList<IdentityRole>> getRolesForUser(int userId)
        {
            string sql = "SELECT r.id, r.name FROM identity_role r JOIN identity_user_roles ur ON(r.id = ur.role_id AND ur.user_id = @userId)";
            try
            {
                await connection.OpenAsync();
                var cmd = new MySqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@userId", userId);
                cmd.Prepare();
                IList<IdentityRole> userRoles = new List<IdentityRole>();
                using (var reader = await cmd.ExecuteReaderAsync())
                    while (reader.Read())
                    {
                        IdentityRole role = new IdentityRole()
                        {
                            Id = reader.GetString(0),
                            Name = reader.GetString(1)
                        };
                        userRoles.Add(role);
                    }
                return userRoles;   
            }
            finally
            {
                await connection.CloseAsync();
            }
        }


        public async Task<IdentityRole> getRole(string roleName)
        {
            string sql = "select * from identity_role where name = @name";
            try
            {
                await connection.OpenAsync();
                var cmd = new MySqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@name", roleName);
                cmd.Prepare();
                IdentityRole role = null; ;
                using (var reader = await cmd.ExecuteReaderAsync())
                    while (reader.Read())
                    {
                        role = new IdentityRole()
                        {
                            Id = reader.GetString(0),
                            Name = reader.GetString(1),
                            NormalizedName = reader.GetString(1).ToUpper()
                        };
                    }
                return role;
            }
            finally
            {
                await connection.CloseAsync();
            }
            
        }

        public async Task<int> CreateUser(AppUser user)
        {
            string sql = "INSERT INTO identity_user (user_name, normalized_user_name, email, normalized_email, password_hash) " +
                "VALUES (@user_name, @normalized_user_name, @email, @normalized_email, @password_hash)";
            connection.Open();
            try
            {
                var cmd = new MySqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@user_name", user.UserName);
                cmd.Parameters.AddWithValue("@normalized_user_name", user.NormalizedUserName);
                cmd.Parameters.AddWithValue("@email", user.Email);
                cmd.Parameters.AddWithValue("@normalized_email", user.NormalizedEmail);
                cmd.Parameters.AddWithValue("@password_hash", user.PasswordHash);
                cmd.Prepare();

                return await cmd.ExecuteNonQueryAsync();
            }
            finally
            {
                connection.Close();
            }
        }

        public int CreateRole(IdentityRole userRole)
        {
            string sql = "INSERT INTO identity_role " +
                "VALUES (@name, @normalized_name)";
            var cmd = new MySqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@name", "someName");
            cmd.Parameters.AddWithValue("@normalized_name", "someName");
            cmd.Prepare();

            return cmd.ExecuteNonQuery();
        }

        public int AddRoleToUser(AppUser user, IdentityRole userRole)
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
