//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WorkingVersionGetItDone.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class WatchParty
    {
        public int PartyID { get; set; }
        public int MemberID { get; set; }
        public string PartyStreet { get; set; }
        public string PartyCity { get; set; }
        public string PartyState { get; set; }
        public bool Private { get; set; }
    }
}
