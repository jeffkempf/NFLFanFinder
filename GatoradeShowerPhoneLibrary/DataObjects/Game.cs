namespace GatoradeShowerPhoneLibrary.DataObjects
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Game
    {
        public int GameID { get; set; }

        [Required]
        [StringLength(100)]
        public string HomeTeam { get; set; }

        [Required]
        [StringLength(100)]
        public string AwayTeam { get; set; }

        public int StadiumID { get; set; }

        /*
        [Column(TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        */
        public TimeSpan GameTime { get; set; }
         

        public DateTime? GameDate { get; set; }
    }
}
