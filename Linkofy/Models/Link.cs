using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Linkofy.Models
{
    public class Link
    {
        public int LinkID { get; set; }
       

        [Required]
        [Display(Name = "Page Containing Link")]
        public string Obdomain { get; set; }

        
        public int? ClientID { get; set; }
        public virtual Client Client { get; set; }

        [Required]
        [Display(Name = "Outbound Link")]
        public string Obpage { get; set; }

        [Required]
        [Display(Name = "Anchor Text")]
        public string Anchor { get; set; }

        [Required]
        [Display(Name = "Date Built")]
        public DateTime BuildDate { get; set; }

        public int? IdentifierID { get; set; }
        public virtual Identifier Identifier { get; set; }

        public int UserTableID { get; set; }
        public virtual UserTable UserTable { get; set; }

    }
}
