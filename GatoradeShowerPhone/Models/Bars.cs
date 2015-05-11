using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GatoradeShowerPhone.Models
{
    class Bars 
    {
        [JsonProperty("BarID")]
        public int barID { get; set; } //changing all props to public while testing POST
        [JsonProperty("BarName")]
        public string barName { get; set; }
        [JsonProperty("BarStreet")]
        public string barStreet { get; set; }
        [JsonProperty("BarCity")]
        public string barCity { get; set; }
        [JsonProperty("BarState")]
        public string barState { get; set; }
        [JsonProperty("BarPhone")]
        public string barPhone { get; set; }
        [JsonProperty("BarZip")]
        public string barZip { get; set; }

        public Bars(string name, string street, string city, string state, string phone, string zip)
        {
            barName = name;
            barStreet = street;
            barCity = city;
            barState = state;
            barPhone = phone;
            barZip = zip; //every value is required for bars entity 
        }

        public static List<Bars> barsList = new List<Bars>();

        public override string ToString()
        {
            return barName.ToString() + "\nPhone: " + barPhone;
        }
    }

}
