namespace GatoradeShowerLibrary.DataObjects
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;

    public partial class Group
    {
        public int GroupID { get; set; }

        [Required]
        [StringLength(50)]
        public string GroupName { get; set; }

        [Required]
        public int GroupOwnerID { get; set; }

        public DateTime? CreationDate { get; set; }

        [Required]
        public int GameID { get; set; }

        [Required]
        [StringLength(50)]
        public string EventType { get; set; }

        [Required]
        [StringLength(100)]
        public string EventLocationName { get; set; }

        [Required]
        public bool Confirmed { get; set; }
    }
}
