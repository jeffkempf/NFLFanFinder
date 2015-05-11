using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
//using System.Web;

namespace GatoradeShowerPhone.Models
{
    public class Members
    {
        //[JsonProperty("MemberID")]
        //public int memberID { get; private set; }

        [JsonProperty("UserName")]
        //public string userName { get; private set; } //note: I don't think we should track first and last names
        private string userName { get; set; }

        [JsonProperty("Password")]
        //public string password { get; private set; } //will probably need to implement validation in setter so won't be able to auto-implement
        private string password { get; set; }

        [JsonProperty("City")]
        private string city { get; set; }

        [JsonProperty("State")]
        private string state { get; set; }

        [JsonProperty("FavTeamID")]
        private int favTeamID { get; set; }

        public static List<Members> MembersList = new List<Members>();
        public Dictionary<string, int> memberTeams = new Dictionary<string, int>();

        public Dictionary<string, int> getMemberTeams()
        {
            return memberTeams;
        }

        public Members(string user, string pass, string memCity, string memState, int team)
        {
            userName = user;
            password = pass;
            city = memCity;
            state = memState;
            favTeamID = team;
        }

        public override string ToString()
        {
            return userName;
        }

        public bool compareUserAndPassword(string currUser, string currPass)
        {
            //username should be case-insensitive since phone capitalizes first letter
            if (currUser.Equals(userName, StringComparison.OrdinalIgnoreCase) && currPass.Equals(password))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //ensures username is unique when creating new member
        public bool usernameIsUnique(string user)
        {
            foreach (var member in MembersList) //this is not getting populated.  Causing unique validation problem
            {
                if (member.userName.Equals(user, StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
            }
            return true;
        }

        //ensures that all fields on signup sheet filled out
        public bool completelyFilled()
        {
            if (userName.Length > 0 && password.Length > 0 && city.Length > 0 && state.Length > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void trackTeams(List<string> teams)
        {
            foreach (string team in teams)
            {
                if (!memberTeams.ContainsKey(team))
                {
                    memberTeams.Add(team, 1);
                }
                else
                {
                    memberTeams[team] = memberTeams[team] + 1;
                }
            }
        }

        public void removeAts(List<string> teamList)
        {
            List<string> cleanList = new List<string>(); ;
            foreach (string team in teamList)
            {
                if (team.Contains("@"))
                {
                    string temp = team.Substring(1);
                    cleanList.Add(temp);
                }
                else
                {
                    cleanList.Add(team);
                }
            }
            trackTeams(cleanList);
        }
    }
}