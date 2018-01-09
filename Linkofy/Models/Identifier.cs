using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Linkofy.Models
{
    public class Identifier
    {
    public enum Ltype
        {
            GuestPost, ExistingLink
        }

        public int ID { get; set; }

        [Required]
        [Display(Name = "Domain")]
        public string domain { get; set; }

        [Required]
        [Display(Name = "Contact Email")]
        [EmailAddress]
        public string contact { get; set; }

        [Display(Name = "Contact Name")]
        public string contactname { get; set; }

        [Required]
        [Display(Name = "Price")]
        public int price { get; set; }

        [Display(Name = "Type of Link")]
        public Ltype? type { get; set; }

        [Display(Name = "TF")]
        public int TrustFlow { get; set; }

        [Display(Name = "CF")]
        public int CitationFlow { get; set; }

        [Display(Name = "RIPs")]
        public int RI { get; set; }

        public int? MJTopicsID { get; set; }
        public virtual MJTopics MJTopics { get; set; }

        public virtual UserTable UserTable { get; set; }
        public int UserTableID { get; set; }

        public virtual ICollection<Link> Links { get; set; }
    }
}