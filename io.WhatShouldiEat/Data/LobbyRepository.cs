using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace io.WhatShouldiEat.Models
{

    //we could surround every db call with a try catch for better error handling
    public class LobbyRepository
    {
        private static SqlConnection SqlConnection { get; set; }
        private static string ConnectionString { get; set; }

        //this is called on start up
        public static void SetConnection(string connectionString)
        {
            SqlConnection = new SqlConnection();
            ConnectionString = connectionString;
        }

        public static Lobby GetLobbyById(string lobbyId)
        {
            Lobby lobbyObj = new Lobby();

            using (SqlConnection)
            {
                SqlConnection.ConnectionString = ConnectionString;
                SqlConnection.Open();

                var getLobbyComannd = $@"select * from Lobby where LobbyId = '{lobbyId}'";

                using(var command = new SqlCommand(getLobbyComannd, SqlConnection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lobbyObj.LobbyId = reader[0].ToString();
                            lobbyObj.OwnerId = reader[1].ToString();
                            lobbyObj.RestaurantListId = reader[2].ToString();
                            lobbyObj.EntryPinNumber = reader[3].ToString();
                            lobbyObj.CreatedOn = reader[4].ToString();                              
                        }
                    }
                }
            }
           return lobbyObj;
        }
        public static Lobby GetLobbyByIdAndPinNumber(string lobbyId, string pinNumber)
        {
            Lobby lobbyObj = new Lobby();

            using (SqlConnection)
            {
                SqlConnection.ConnectionString = ConnectionString;
                SqlConnection.Open();

                var getLobbyComannd = $@"select * from Lobby where LobbyId = '{lobbyId}' and EntryPinNumber = '{pinNumber}'";

                using (var command = new SqlCommand(getLobbyComannd, SqlConnection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lobbyObj.LobbyId = reader[0].ToString();
                            lobbyObj.OwnerId = reader[1].ToString();
                            lobbyObj.RestaurantListId = reader[2].ToString();
                            lobbyObj.EntryPinNumber = reader[3].ToString();
                            lobbyObj.CreatedOn = reader[4].ToString();
                        }
                    }
                }
            }
            return lobbyObj;
        }

        public static string AddLobbyMember(string userId, string lobbyId)
        {
            string lobbyMemberId = String.Empty;

            using (SqlConnection)
            {
                SqlConnection.ConnectionString = ConnectionString;
                SqlConnection.Open();

                var getLobbyMemberId = $@"insert into LobbyMembers 
                                          values(NewId(), '{userId}', '{lobbyId}')";

                using (var command = new SqlCommand(getLobbyMemberId, SqlConnection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            //do nothing
                        }
                    }
                }

                var getLobbyMemberIdCommand = $@"select * from LobbyMembers where LobbyId = '{lobbyId}' and UserId = '{userId}'";

                using (var command = new SqlCommand(getLobbyMemberIdCommand, SqlConnection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lobbyMemberId = reader[0].ToString();
                        }
                    }
                }

            }

            return lobbyMemberId;
        }

        public static List<Lobby> GetExistingPinNumbers()
        {
            List<Lobby> Lobbies = new List<Lobby>();

            using (SqlConnection)
            {
                SqlConnection.ConnectionString = ConnectionString;
                SqlConnection.Open();

                var CreateLobbyCommand = $@"select * from Lobby";

                using (var command = new SqlCommand(CreateLobbyCommand, SqlConnection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Lobbies.Add(new Lobby
                            {
                                LobbyId = reader[0].ToString(),
                                OwnerId = reader[1].ToString(),
                                RestaurantListId = reader[2].ToString(),
                                EntryPinNumber = reader[3].ToString(),
                                CreatedOn = reader[4].ToString()
                            });
                        }
                    }
                }
            }
            //return the entire object in case we would like to use any properties anywhere else in the program
            return Lobbies;
        }

        public static string CreateALobbyRecord(string userId, string restaurantListId, string pinNumber)
        {
            //initial implementation of a simple check for whether the query executed or not, CHANGE WHEN PROJECT BECOMES MORE THOROUGH 
            string LobbyId = String.Empty;

            using (SqlConnection)
            {
                SqlConnection.ConnectionString = ConnectionString;
                SqlConnection.Open();

                var CreateLobbyCommand = $@"Insert into Lobby(LobbyId, OwnerId, RestaurantListId, EntryPinNumber, CreatedOn)
                                        Values(NewId(), '{userId}', '{restaurantListId}', '{pinNumber}', GetDate())";

                using (var command = new SqlCommand(CreateLobbyCommand, SqlConnection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            //do nothing
                        }
                    }
                }

                var ReturnLobbyIdCommand = $@"select LobbyId from Lobby where EntryPinNumber = '{pinNumber}'";

                using (var command = new SqlCommand(ReturnLobbyIdCommand, SqlConnection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            LobbyId = reader[0].ToString();
                        }
                    }
                }
            }

            return LobbyId;

        }

        public static void AddUserTolobbyMembersTable(string LobbyId, string UserId)
        {
            using (SqlConnection)
            {
                SqlConnection.ConnectionString = ConnectionString;
                SqlConnection.Open();

                var SaveListCommand = $@"Insert into LobbyMembers(LobbyMemberId, UserId, LobbyId)
                                        Values(NEWID(), '{UserId}', '{LobbyId}')";

                using (var command = new SqlCommand(SaveListCommand, SqlConnection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                        }
                    }
                }
            }
        }

        public static List<User> GetAllLobbyMembersByLobbyId(string LobbyId)
        {
            List<User> LobbyMembers = new List<User>();

            using (SqlConnection)
            {
                SqlConnection.ConnectionString = ConnectionString;
                SqlConnection.Open();

                var LobbyMemberCommand = $@"select LobbyMemberId, UserId, LobbyId, UserName from LobbyMembers
                                        join AspNetUsers on AspNetUsers.Id = LobbyMembers.UserId
                                        where LobbyId = '{LobbyId}'";

                using (var command = new SqlCommand(LobbyMemberCommand, SqlConnection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            LobbyMembers.Add(new User
                            {
                                LobbyMemberId = reader[0].ToString(),
                                Id = Guid.Parse(reader[1].ToString()),
                                LobbyId = reader[2].ToString(),
                                Name = reader[3].ToString()
                            });
                        }
                    }
                }

            }

            return LobbyMembers;
        }


        //we will need to ensure the pinnumber is correct also
        public static void DeleteLobbyRecords(string LobbyId)
        {
            var errorString = String.Empty;

            using (SqlConnection)
            {
                SqlConnection.ConnectionString = ConnectionString;
                SqlConnection.Open();

                var DeleteLobbyMembersCommand = $@"delete from LobbyMembers where LobbyId = '{LobbyId}'";

                using (var command = new SqlCommand(DeleteLobbyMembersCommand, SqlConnection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            
                        }
                    }
                }

                var DeleteLobbyCommand = $@"delete from Lobby where LobbyId = '{LobbyId}'";

                using (var command = new SqlCommand(DeleteLobbyCommand, SqlConnection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                        }
                    }
                }

            }
        }

    }
}
