using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using io.WhatShouldiEat.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Google;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace io.WhatShouldiEat.Controllers
{
    /*
         * Lobby Controller will need the following api endpoints:
         * Create Lobby
         * Add User to Lobby
         * Remove User from Lobby
         * Get All Lobby Members
         * End Lobby
    */

    [Route("api/Lobby/")]
    public class LobbyController : Controller
    {
        [HttpPost("Create")]        
        public string CreateLobby([FromHeader] string userInfo)
        {
            /* 
              Sample Json Payload 
              { "userId": "hi", "secretKey": "hi", "resturantListId": "hi" }
              secretKey is a method to limit requests from inorganic sources, tries its best to ensure it is coming from our system
              we might hold off on the secret key implementation for later
            */

            var usersInfo = JObject.Parse(userInfo);
            var userId = usersInfo["userId"].ToString();
            var restaurntListId = usersInfo["resturantListId"].ToString();
            var secretKey = usersInfo["secretKey"].ToString();

            //Guard Clauses
            if (userId == null || secretKey == null || restaurntListId == null) return "{ \"Error\": \"Required paramater null\" }";
            //checks if the users exists
            if (!Models.User.CheckIfUserExists(usersInfo["userId"].ToString())) { return "{ \"Error\": \"User does not exist\" }"; }

            return Lobby.Create(userId, restaurntListId);
        }

        [HttpPost("Join")]
        public string JoinLobby([FromHeader] string lobbyId, [FromHeader] string userInfo, [FromHeader] string pinNumber)
        {
            //we will need to ensure at least a guid is coming in to prevent anything malicious
            try { Guid.Parse(lobbyId); } catch (Exception) { return "Invalid Lobby ID"; }

            var usersInfo = JObject.Parse(userInfo);
            var userId = usersInfo["userId"].ToString();
            var restaurntListId = usersInfo["resturantListId"].ToString();
            var secretKey = usersInfo["secretKey"].ToString();

            //Guard Clauses
            if (userId == null || secretKey == null || restaurntListId == null) return "{ \"Error\": \"Required paramater null\" }";

            //we will need a function to validate the id's as they come in
            return Lobby.Join(pinNumber, lobbyId, userId);
        }

        [HttpPost("Close")]
        public string CloseLobby([FromHeader] string lobbyId, [FromHeader] string pinNumber, [FromHeader] string secretKey)
        {
            return Lobby.Close(lobbyId, pinNumber);
        }

        [HttpGet("GetMembers")]
        public string GetAllLobbyMembers([FromHeader] string lobbyId, [FromHeader] string pinNumber)
        {
            return JsonConvert.SerializeObject(Lobby.GetMemberList(lobbyId, pinNumber));
        }

        [HttpGet("hi")]
        public string TestGet()
        {
            return "hi";
        }

        //// GET api/<controller>/5
        //[HttpGet]
        //[AllowAnonymous]
        //public string ExternalLogin(string returnUrl)
        //{
        //    return "hi"; 
        //}

        //// POST api/<controller>
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/<controller>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/<controller>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
