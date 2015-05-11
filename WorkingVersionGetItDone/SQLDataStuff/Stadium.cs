namespace WorkingVersionGetItDone.SQLDataStuff
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Stadiums")]
    public partial class Stadium
    {
        public int StadiumID { get; set; }

        [Required]
        [StringLength(50)]
        public string StadiumName { get; set; }

        [Required]
        [StringLength(50)]
        public string StadiumAddress { get; set; }

        [Required]
        [StringLength(50)]
        public string StadiumCity { get; set; }

        [Required]
        [StringLength(50)]
        public string StadiumState { get; set; }

        public int StadiumZip { get; set; }
    }
}
