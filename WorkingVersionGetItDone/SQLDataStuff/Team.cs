namespace WorkingVersionGetItDone.SQLDataStuff
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Team
    {
        public int TeamID { get; set; }

        [Required]
        [StringLength(50)]
        public string TeamName { get; set; }

        [Required]
        [StringLength(50)]
        public string TeamCity { get; set; }
    }
}
