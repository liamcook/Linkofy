using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Linkofy.Models
{
    public class EmailAccount
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Sender Name is Required")]
        [Display(Name = "Sender Name")]
        public string senderName { get; set; }

        [Required,EmailAddress (ErrorMessage = "Email Address is Required")]
        [Display(Name = "Email Address")]
        public string emailAddress { get; set; }

        [Required(ErrorMessage = "Password is Required")]
        [Display(Name = "Password")]
        public string password { get; set; }

        public virtual UserTable UserTable { get; set; }
        public int UserTableID { get; set; }
    }
}