namespace WorkingVersionGetItDone.SQLDataStuff
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Member
    {
        public int MemberID { get; set; }

        //[Required]
        //[StringLength(255)]
        //public string FirstName { get; set; }

        ////[Required]
        //[StringLength(255)]
        //public string LastName { get; set; }

        [Required]
        [StringLength(50)]
        public string UserName { get; set; }

        [Required]
        [StringLength(32)]
        public string Password { get; set; }

        //[Required]
        //[StringLength(50)]
        //public string Email { get; set; }

        [Required]
        [StringLength(50)]
        public string City { get; set; }

        [Required]
        [StringLength(30)]
        public string State { get; set; }

        //[Required]
        //[StringLength(255)]
        //public string Zip { get; set; }

        ////[Required]
        //[StringLength(255)]
        //public string Phone { get; set; }

        [Required]
        public int FavTeamID { get; set; }
    }
}
