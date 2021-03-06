namespace WorkingVersionGetItDone.SQLDataStuff
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Event
    {
        public int EventID { get; set; }

        [Required]
        [StringLength(100)]
        public string EventType { get; set; }

        public int EventLocationID { get; set; }
    }
}
