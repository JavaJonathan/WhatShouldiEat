using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace io.WhatShouldiEat.Models
{
    public class RestaurantList
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        //holds the list of restaurants
        public List<Restaurant> RestaurantListProp { get; set; }

        //we will need to take in a list from the Client Side, save it into the database then join it with the Lobby

        public static string ValidateRestuarantList(User user, RestaurantList restaurants)
        {
            //we will need to take in the JSON format of the list to pass to the save method to save in the db in JSON format

            foreach (var restaurant in restaurants.RestaurantListProp)
            {
                if (restaurant.Name == null || restaurant.Name.Equals(String.Empty)) return "List Not Saved, Null Name Value.";
            }

            Repository.SaveRestaurantListToUser(user, restaurants);

            return "List Saved Successfully";
        }
        //the restaurant list will need a name and list, we can add more properties at a later time

        // we can house the method to retrieve a users list from the databse in this class
        public static List<Restaurant> GetRestaurantListsByUser(User user, string additionalFilter)
        {
            //the additional filter string will be used to append onto the end of the sql query for any additional filtering, making the method more flexible
            return Repository.GetRestaurantsByUser(user, additionalFilter);
        }

        //method to give user pre-determined restaurant lists
    }
}
