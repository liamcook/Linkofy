using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Linkofy.Models
{
    public class UserTable
    {
        public int ID { get; set; }
        public string userIdentity { get; set; }

        public virtual ICollection<Identifier> Identifiers { get; set; }
        public virtual ICollection<Client> Clients { get; set; }
        public virtual ICollection<Template> Templates { get; set; }
        public virtual ICollection<Link> Links { get; set; }

        public virtual ApplicationUser Users { get; set; }

        public string ApplicationUserId { get; set; }

    }
}