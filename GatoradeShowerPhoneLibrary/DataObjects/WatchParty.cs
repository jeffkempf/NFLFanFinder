namespace GatoradeShowerPhoneLibrary.DataObjects
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class WatchParty
    {
        [Key]
        public int PartyID { get; set; }

        public int MemberID { get; set; }

        [Required]
        [StringLength(50)]
        public string PartyStreet { get; set; }

        [Required]
        [StringLength(50)]
        public string PartyCity { get; set; }

        [Required]
        [StringLength(50)]
        public string PartyState { get; set; }

        public bool Private { get; set; }
    }
}
