using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
//using System.Web;

namespace GatoradeShowerPhone.Models
{
    public class Events
    {
        [JsonProperty("EventID")]
        private int eventID { get; set; }
        [JsonProperty("EventType")]
        private string eventType { get; set; }
        [JsonProperty("EventLocationID")]
        private int eventLocationID { get; set; }

        public Events()
        {
            //empty constructor
        }
    }
}