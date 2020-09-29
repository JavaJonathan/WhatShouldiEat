using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace io.WhatShouldiEat.Models
{
    public class UserRepository
    {
        private static SqlConnection SqlConnection { get; set; }
        private static string ConnectionString { get; set; }

        //this is called on start up
        public static void SetConnection(string connectionString)
        {
            SqlConnection = new SqlConnection();
            ConnectionString = connectionString;
        }

        public static User GetUserById(string Id)
        {
            User user = null;

            using (SqlConnection)
            {
                SqlConnection.ConnectionString = ConnectionString;
                SqlConnection.Open();

                var getUserCommand = $@"select * from AspNetUsers where Id = '{Id}'";

                using (var command = new SqlCommand(getUserCommand, SqlConnection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (!Convert.IsDBNull(0))
                            {
                                user = new User
                                {
                                    Id = Guid.Parse(reader[0].ToString()),
                                    Name = reader[1].ToString()
                                };
                            }
                        }
                    }
                }
            }
            return user;
        }

        public static string GetUserRole(string UserId)
        {
            var UserRole = string.Empty;

            using (SqlConnection)
            {
                SqlConnection.ConnectionString = ConnectionString;
                SqlConnection.Open();

                var RestaurantListCommand = $@"select Name from AspNetRoles where UserId = '{UserId}'";

                using (var command = new SqlCommand(RestaurantListCommand, SqlConnection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            UserRole = reader[0].ToString();
                        }
                    }
                }
            }

            return UserRole;

        }
    }
}
