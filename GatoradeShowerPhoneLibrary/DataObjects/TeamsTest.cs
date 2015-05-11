namespace GatoradeShowerPhoneLibrary.DataObjects
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TeamsTest")]
    public partial class TeamsTest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int teamID { get; set; }

        [Required]
        [StringLength(50)]
        public string teamName { get; set; }
    }
}
