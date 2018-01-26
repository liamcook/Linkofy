using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Linkofy.Models
{
    public class Outreach
    {
        public int ID { get; set; }

        public string name { get; set; }

        [Required]
        public string domain { get; set; }
        public string email { get; set; }

        public int UserTableID { get; set; }
        public virtual UserTable UserTable { get; set; }
    }
}