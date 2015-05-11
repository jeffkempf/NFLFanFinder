using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GatoradeShowerPhone.Models
{
    class MemberGroups
    {
        [JsonProperty("MemGrpID")]
        public int memGroupID { get; private set; }

        [JsonProperty("GroupID")]
        public int groupID { get; private set;}

        [JsonProperty("MemberID")]
        public int memberID {get; private set;}

        [JsonProperty("GameID")]
        private int gameID {get; set;}

        public static List<MemberGroups> MemberGroupList = new List<MemberGroups>();
 
        public MemberGroups(int grpID, int mID, int gID)
        {
           // memGroupID = mGrpID;
            groupID = grpID;
            memberID = mID;
            gameID = gID; 
        }

        public override string ToString()
        {
            String temp = null; 
            foreach (var members in MemberGroupList)
            {
                temp = temp + members + "\n"; 
            }
            return temp;
        }

        public bool notInGroup()
        {
            foreach (MemberGroups mg in MemberGroupList)
            {
                if (mg.groupID == this.groupID && mg.memberID == this.memberID)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
