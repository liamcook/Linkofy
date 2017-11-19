using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Linkofy.Models
{
    public class Status
    {
            public enum StatusC
        {
            NeedArticle, OrderedArticle, SentArticle
        }
        public int StatusID { get; set; }

        public int? IdentifierID { get; set; }
        public virtual Identifier Identifier { get; set; }

        public int? ClientID { get; set; }
        public virtual Client Client { get; set; }

        [Required]
        public StatusC? status { get; set; }

        [Required]
        public DateTime Last { get; set; }

        [Required]
        public int UserTableID { get; set; }
        public virtual UserTable UserTable { get; set; }


    }
}