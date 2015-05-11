using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
//using System.Web;

namespace GatoradeShowerPhone.Models
{
    public class WatchParties
    {
        [JsonProperty("PartyID")]
        public int partyID { get; set; }
        [JsonProperty("MemberID")]
        private int memberID { get; set; }
        [JsonProperty("PartyStreet")]
        private string partyStreet { get; set; }
        [JsonProperty("PartyCity")]
        private string partyCity { get; set; }
        [JsonProperty("PartyState")]
        private string partyState { get; set; }
        [JsonProperty("Privacy")]
        private bool privacy { get; set; }//I think making this a boolean will make coding more intuitive

        public static List<WatchParties> PartiesList = new List<WatchParties>();

        public WatchParties(int member, string street, string city, string state, bool priv)
        {
            memberID = member;
            partyStreet = street;
            partyCity = city;
            partyState = state;
            privacy = priv;
        }
        public override string ToString()
        {
            string temp = null;
            if (privacy == true)
            {
                temp = "Private event";
            }
            return "Party: " +  partyStreet + ",\n " + partyCity + 
                " " + temp;
        }


    }
}