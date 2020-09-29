using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace io.WhatShouldiEat.Models
{
    public class RestaurantListRepository
    {
        private static SqlConnection SqlConnection { get; set; }
        private static string ConnectionString { get; set; }

        //this is called on start up
        public static void SetConnection(string connectionString)
        {
            SqlConnection = new SqlConnection();
            ConnectionString = connectionString;
        }

        //in progress, we do not seem to need this function just yet
        public static RestaurantList GetRestaurantListByid(string restaurantId, string userId)
        {
            RestaurantList restaurantList = null;

            using (SqlConnection)
            {
                SqlConnection.ConnectionString = ConnectionString;
                SqlConnection.Open();

                var getRestuarantCommand = $@"select * from ";
            }
            return restaurantList;
        }

        public static string SaveRestaurantListToUser(User user, RestaurantList restaurantList)
        {
            //initial implementation of a simple check for whether the query executed or not, CHANGE WHEN PROJECT BECOMES MORE THOROUGH 
            var EverythingWentWell = false;

            var JsonRestaurantList = JsonConvert.SerializeObject(restaurantList.RestaurantListProp);

            using (SqlConnection)
            {
                SqlConnection.ConnectionString = ConnectionString;
                SqlConnection.Open();

                var SaveListCommand = $@"Insert into RestaurantList(RestuarantListId, Name, ListData, UserId)
                                      Values(NEWID(), '{restaurantList.Name}', '{JsonRestaurantList}', '{user.Id}')";

                using (var command = new SqlCommand(SaveListCommand, SqlConnection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            EverythingWentWell = true;
                        }
                    }
                }
            }

            return EverythingWentWell ? "Everyhting went well" : "We had some issues";
        }

        public static List<Restaurant> GetRestaurantsByUser(User user, string additionalFilter)
        {
            List<Restaurant> restaurantList = new List<Restaurant>();

            using (SqlConnection)
            {
                SqlConnection.ConnectionString = ConnectionString;
                SqlConnection.Open();

                var RestaurantListCommand = $@"select Name from RestaurantList where UserId = '{user.Id}'
                                            {additionalFilter}";

                using (var command = new SqlCommand(RestaurantListCommand, SqlConnection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            restaurantList.Add(new Restaurant
                            {
                                Name = reader[0].ToString()
                            });
                        }
                    }
                }
            }

            return restaurantList;
        }
    }
}
