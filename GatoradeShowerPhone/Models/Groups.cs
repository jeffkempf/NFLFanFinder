using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
//using System.Web;

namespace GatoradeShowerPhone.Models
{
    class Groups
    {
        //any entities that user creates can't have an ID in client model if want auto increment to work correctly
        //[JsonProperty("GroupID")]
        //public int groupID { get; private set; }

        [JsonProperty("GroupName")]
        public string groupName { get; set; }

        [JsonProperty("GroupOwnerID")]
        private int groupOwnerID { get; set; }

        [JsonProperty("CreationDate")]
        private DateTime creationDate { get; set; } //only non-required col in table. Changing to 
        //[JsonProperty("CreationDate")]
        //private string creationDate { get; set; }

        [JsonProperty("GameID")]
        public int gameID { get; private set; }//will need to sync this to gameID in Games.cs

        [JsonProperty("EventType")]
        private string eventType { get; set; } //same with this, and many more

        [JsonProperty("EventLocationName")]
        private string eventLocationName { get; set; }

        [JsonProperty("Confirmed")]
        private bool confirmed { get; set; }//true means linked to public event. false means private party

        public static List<Groups> GroupsList = new List<Groups>();

        public Groups(string name, int owner, DateTime date, int game, string eType, string loc, bool priv)
        {
           //groupID = id;
           groupName = name;
           groupOwnerID = owner;
           creationDate = date;
           gameID = game;
           eventType = eType;
           eventLocationName = loc;
           confirmed = priv;
        }

        public override string ToString()
        {
            return groupName;
        }

        public string getGroupDetails()
        {
            string gameName = "unknown game";
            Games tempGame = Games.returnGameById(gameID);
            if(tempGame != null) {
                gameName = tempGame.ToString();
            }
            return "Group: " + groupName + "\nEvent type: " + eventType + "\nGame: " + gameName + "\nEvent location: " + 
                eventLocationName + "\nCreated on: " + creationDate + "\n\n\nClick View Members above to see \nmembers and to join/leave this group";
        }

        public static Groups getGroupByName(string name)
        {
            foreach (var group in GroupsList)
            {
                if (group.groupName.Equals(name))
                {
                    return group;
                }
            }
            return null;
        }

        //checks that all fields in UI are filled out
        public bool completelyFilled()
        {
            //if (groupName.Length > 0 && eventType.Length > 0 && eventLocationName.Length > 0)
            if(groupName != null && eventType != null && eventLocationName != null && gameID != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //checks that name is unique
        public bool hasUniqueName()
        {
            foreach (var group in GroupsList)
            {
                if (group.groupName.Equals(groupName))
                {
                    return false;
                }
            }
            return true;
        }
    }
}