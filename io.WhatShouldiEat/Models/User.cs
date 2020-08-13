using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace io.WhatShouldiEat.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string LobbyId { get; set; }
        public string LobbyMemberId { get; set; }
        public enum Role
        {
            BaseUser, 
            Admin
        }

        public User.Role GetRole()
        {
            User.Role role = User.Role.BaseUser;

            switch (UserRepository.GetUserRole(Id.ToString()))
            {
                case "Base": role = User.Role.BaseUser; break;
                case "Admin": role = User.Role.Admin; break;
            }

            return role;
        }

        public static bool CheckIfUserExists(string userId)
        {            
            return UserRepository.GetUserById(userId) == null ? false : true;
        }

        //we will need a method to return the role of a user

        //we will need a method to authenticate user

        //we will need a method to possibly update the display name
    }
}
