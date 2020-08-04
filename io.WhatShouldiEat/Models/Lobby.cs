using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace io.WhatShouldiEat.Models
{
    public class Lobby
    {
        public string LobbyId { get; set; }
        public string OwnerId { get; set; }
        public string RestaurantListId { get; set; }
        public string EntryPinNumber { get; set; }
        public string CreatedOn { get; set; }

        //we will first need to create the lobby
        //this method will need to return a pin number so the user can give to their friends
        public static string Create(string userId, string restaurantList)
        {
            //we will need error handling to make sure restaurant list id exists, we are validating the userId in the user class
            //we will need to create a list to add all friends in the lobby into

            //then we will need insert a value into the Lobby Table with a User Id as the foreign key fro the column value Lobby owner
            //in the Lobby Table we can also save the Pin Number to check against when a user would like to join a lobby

            //there will also need to be a food list chose for the lobby to eventually vote on
            List<Lobby> Lobbies = Repository.GetExistingPinNumbers();

            string pinNumber;

            while (true)
            {
                pinNumber = String.Empty;
                Random randomNumber = new Random();
                for (var i = 0; i < 4; i++)
                {
                    pinNumber += randomNumber.Next(0,9);
                }

                var checkList = Lobbies.FirstOrDefault(Lobby => Lobby.EntryPinNumber.Equals(pinNumber));

                if(checkList == null) break;
            }
            
            var LobbyId = Repository.CreateALobbyRecord(userId, restaurantList, pinNumber);

            if (LobbyId != String.Empty) { return LobbyId; }

            //we wil return null here to know something went wrong in the process
            return null;
        }

        //we will also need a method to allow joining a party
        public static string Join(string pinNumber, string lobbyId, string userId)
        {
            Lobby lobbyToJoin = Repository.GetExistingPinNumbers().FirstOrDefault(lobby => lobby.EntryPinNumber.Equals(pinNumber));

            if(lobbyToJoin == null) { return "Invalid Creds"; }
            //we will still throw an error saying incorrect credentials if the lobby id doesnt match even if the pin number is correct
            if (lobbyToJoin.LobbyId != lobbyId) { return "Invalid Creds"; }

            //if the lobby the user is trying  to join exists, we will need to insert a value into the LobbyMembers Table with the user id and lobby id as the foreign key 
            //it will be used as a mapping table
            return Repository.AddLobbyMember(lobbyId, userId);
        }

        //we will also need a method to return everyone in a specific lobby
        public static List<User> GetMemberList(Guid LobbyId)
        {
            return Repository.GetAllLobbyMembersByLobbyId(LobbyId);
        }

        //we will need a method to close the lobby, aka remove the record from the lobby table and all memebers from lobby member table
        public string Close(string LobbyId)
        {
            Repository.DeleteLobbyRecords(LobbyId);

            return "Lobby has been deleted";
        }
    }
}
