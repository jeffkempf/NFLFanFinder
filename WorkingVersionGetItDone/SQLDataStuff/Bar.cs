namespace WorkingVersionGetItDone.SQLDataStuff
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Bar
    {
        public int BarID { get; set; }

        [Required]
        [StringLength(255)]
        public string BarName { get; set; }

        [Required]
        [StringLength(255)]
        public string BarStreet { get; set; }

        [Required]
        [StringLength(50)]
        public string BarCity { get; set; }

        [Required]
        [StringLength(50)]
        public string BarState { get; set; }

        [Required]
        [StringLength(50)]
        public string BarPhone { get; set; }

        [Required]
        [StringLength(50)]
        public string BarZip { get; set; }
    }
}
