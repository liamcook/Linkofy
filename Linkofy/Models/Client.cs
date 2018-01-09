using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Linkofy.Models
{
    public class Client
    {
        public int ID { get; set; }

        [Required]
        [Display(Name = "Client")]
        public string clientN { get; set; }

        [Display(Name = "Website")]
        public string homePage{ get; set; }

        [EmailAddress]
        [Display(Name = "Contact Email")]
        public string clientEmail { get; set; }

        [Display(Name = "Contact Name")]
        public string contName { get; set; }

        [Display(Name = "Monthly")]
        public int monthlyQuota { get; set; }

        [Display(Name = "TF")]
        public int TrustFlow { get; set; }

        [Display(Name = "CF")]
        public int CitationFlow { get; set; }

        [Display(Name = "RIPs")]
        public int RI { get; set; }

        public int? MJTopicsID { get; set; }
        public virtual MJTopics MJTopics { get; set; }

        public int UserTableID { get; set; }
        public virtual UserTable UserTable { get; set; }

        public virtual ICollection<Link> Links { get; set; }
        public virtual ICollection<Status> Statuss { get; set; }
    }
}