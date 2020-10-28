using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;
using VM.Core.Entities;
using VM.Core.Interfaces;

namespace VM.Data.Repository
{
    public class CosmosVmUserRepository : IVmUserRepository
    {
        private readonly MySqlConnection connection;
        private readonly ILogger<CosmosVmUserRepository> logger;

        private readonly IList<string> availableRoles = new List<String>() { "VM_CREW", "VM_ADMIN", "CUSTOMER" };

        public CosmosVmUserRepository(MySqlConnection con, ILogger<CosmosVmUserRepository> log)
        {
            connection = con;
            logger = log;
        }

        public VmUserRole GetRole(string roleName)
        {
            if (availableRoles.Contains(roleName))
            {
                return new VmUserRole()
                {
                    Name = roleName
                };
            }
            else
            {
                logger.LogWarning("Got unknown role: " + roleName);
                return null;
            }
        }

        public VmUser GetUserByUserName(string userName)
        {
            string sql = "select* from identity_user where user_name=@username";
            try
            {
                connection.Open();
                var cmd = new MySqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@username", userName);
                cmd.Prepare();
                VmUser user = null; ;
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                    {
                        user = new VmUser()
                        {
                            //   Id = reader.GetString("id"),
                            UserName = reader.GetString("user_name"),
                            Email = reader.GetString("email"),
                            PasswordHash = reader.GetString("password_hash")

                        };
                        return user;
                    }
                return null;
            }
            catch (Exception e)
            {
                logger.LogError("Failed to get user by username: " + userName, e);
                return null;
            }
            finally
            {
                connection.Close();
            }
        }

        public VmUser CreateUser(VmUser user)
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

                cmd.ExecuteNonQuery();
                return user;
            }
            catch (Exception e)
            {
                logger.LogError("Failed to create user: " + user, e);
                return null;
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
