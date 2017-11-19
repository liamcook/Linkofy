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

        [Url]
        [Display(Name = "Website")]
        public int webPage{ get; set; }

        [EmailAddress]
        [Display(Name = "Contact Email")]
        public int contactEmail { get; set; }

        [Display(Name = "Contact Name")]
        public int contactName { get; set; }

        [Display(Name = "Monthly Links")]
        public int monthlyQuota { get; set; }
 
        public int? MJTopicsID { get; set; }
        public virtual MJTopics MJTopics { get; set; }

        public int UserTableID { get; set; }
        public virtual UserTable UserTable { get; set; }

        public virtual ICollection<Link> Links { get; set; }
    }
}