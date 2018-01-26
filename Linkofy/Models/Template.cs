using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Linkofy.Models
{
    public class Template
    {
        public enum defaul
        {
             GuestPost, ExistingLink
        }
        public int ID { get; set; }

        [Required(ErrorMessage = "Name is Required")]
        public string templateName { get; set; }

        [Required(ErrorMessage = "Subject is Required")]
        public string subject { get; set; }

        [Required(ErrorMessage = "Body is Required")]
        public string Body { get; set; }

        public defaul? Default { get; set; }
    }
}