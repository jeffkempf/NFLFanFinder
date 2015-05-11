namespace GSPortableLibrary.DataObjects
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;


    public partial class MemberGroup
    {
        [Key]
        public int MemGrpID { get; set; }

        [Required]
        public int GroupID { get; set; }

        [Required]
        public int MemberID { get; set; }

        [Required]
        public int GameID { get; set; }

    }
}