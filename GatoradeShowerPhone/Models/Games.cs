using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
//using System.Web;

namespace GatoradeShowerPhone.Models
{
    public class Games
    {
        [JsonProperty("GameID")]
        public int gameID { get; private set; }
        [JsonProperty("HomeTeam")]
        private string homeTeam { get; set; } //I still think this should be home team's teamID, not string of team's name
        [JsonProperty("AwayTeam")]
        private string awayTeam { get; set; } //teamid?
        [JsonProperty("StadiumID")]
        private int stadiumID { get; set; }
        [JsonProperty("GameTime")]
        private DateTime gameTime { get; set; } 
        [JsonProperty("GameDate")]
        private DateTime gameDate { get; set; }

        public static List<Games> GamesList = new List<Games>();

        public Games(string home, string away, int stad, DateTime time, DateTime date)
        {
            homeTeam = home;
            awayTeam = away;
            stadiumID = stad;
            gameTime = time;
            gameDate = date;
        }

        public override string ToString()
        {
            return awayTeam + homeTeam + ":\nGame Time: " + gameTime;
        }

        public static Games returnGameById(int id)
        {
            foreach (Games game in GamesList)
            {
                if (game.gameID == id)
                {
                    return game;
                }
            }
            return null;
        }
    }
}