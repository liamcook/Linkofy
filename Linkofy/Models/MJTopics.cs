using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Linkofy.Models
{
    public class MJTopics
    {

        public int ID { get; set; }

        [Display(Name = "Topic")]
        public string topicalTF { get; set; }

        public virtual ICollection<Identifier> Identifiers { get; set; }

        public virtual ICollection<Client> Clients { get; set; }
    }
}