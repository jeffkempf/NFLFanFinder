using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
//using System.Web;

namespace GatoradeShowerPhone.Models
{
    public class Stadiums
    {
        [JsonProperty("StadiumID")]
        private int stadiumID { get; set; }
        [JsonProperty("StadiumName")]
        private string stadiumName { get; set; }
        [JsonProperty("StadiumAddress")]
        private string stadiumAddress { get; set; }
        [JsonProperty("StadiumCity")]
        private string stadiumCity { get; set; }
        [JsonProperty("StadiumState")]
        private string stadiumState { get; set; }
        [JsonProperty("StadiumZip")]
        private string stadiumZip { get; set; }
        [JsonProperty("HomeTeamID")]
        private int teamID { get; set; }

        public Stadiums(string name, string address, string city, string state, string zip, int team)
        {
            stadiumName = name;
            stadiumAddress = address;
            stadiumCity = city;
            stadiumState = state;
            stadiumZip = zip;
            teamID = team;
        }

        public override string ToString()
        {
            return stadiumName + " " + teamID.ToString();
        }
    }
}