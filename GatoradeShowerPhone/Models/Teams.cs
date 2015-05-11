using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GatoradeShowerPhone.Models
{
    public class Teams
    {
        [JsonProperty("TeamID")]
        private int teamID { get; set; }
        [JsonProperty("TeamName")]
        private string teamName { get; set; }
        [JsonProperty("TeamCity")]
        private string teamCity { get; set; }
        
        public static List<Teams> TeamsList = new List<Teams>();

        public Teams(int id, string name, string city)
        {
            teamID = id;
            teamName = name;
            teamCity = city;
        }

        public override string ToString()
        {
            return "Team: " + teamID.ToString() + " " + teamName.ToString() + " City: " + teamCity;
        }

        public static int getIdFromTeamName(string name)
        {
            int id = -1;
            switch (name)
            {
                case "Arizona Cardinals":
                    id = 65;
                    break;
                case "Atlanta Falcons":
                    id = 66;
                    break;
                case "Baltimore Ravens":
                    id = 67;
                    break;
                case "Buffalo Bills":
                    id = 68;
                    break;
                case "Carolina Panthers":
                    id = 69;
                    break;
                case "Chicago Bears":
                    id = 70;
                    break;
                case "Cincinnati Bengals":
                    id = 71;
                    break;
                case "Cleveland Browns":
                    id = 72;
                    break;
                case "Dallas Cowboys":
                    id = 73;
                    break;
                case "Denver Broncos":
                    id = 74;
                    break;
                case "Detroit Lions":
                    id = 75;
                    break;
                case "Green Bay Packers":
                    id = 76;
                    break;
                case "Houston Texans":
                    id = 77;
                    break;
                case "Indianapolis Colts":
                    id = 78;
                    break;
                case "Jacksonville Jaguars":
                    id = 79;
                    break;
                case "Kansas City Chiefs":
                    id = 80;
                    break;
                case "Miami Dolphins":
                    id = 81;
                    break;
                case "Minnesotta Vikings":
                    id = 82;
                    break;
                case "New England Patriots":
                    id = 83;
                    break;
                case "New Orleans Saints":
                    id = 84;
                    break;
                case "New York Giants":
                    id = 85;
                    break;
                case "New York Jets":
                    id = 86;
                    break;
                case "Oakland Raiders":
                    id = 87;
                    break;
                case "Philadelphia Eagles":
                    id = 88;
                    break;
                case "Pittsburg Steelers":
                    id = 89;
                    break;
                case "San Diego Chargers":
                    id = 90;
                    break;
                case "San Francisco 49ers":
                    id = 91;
                    break;
                case "Seattle Seahawks (Go 'Hawks!)":
                    id = 92;
                    break;
                case "St. Louis Rams":
                    id = 93;
                    break;
                case "Tampa Bay Buccaneers":
                    id = 94;
                    break;
                case "Tennessee Titans":
                    id = 95;
                    break;
                case "Washington Redskins":
                    id = 96;
                    break;
                default:
                    id = 0;
                    break;
            }
            return id;
        }
    }
}