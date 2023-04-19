using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Dalelk.Models
{
    public class order
    {
        public int id { get; set; }

        public string userid { get; set; }
        public virtual ApplicationUser user { get; set; }

        public int productid { get; set; }
        public virtual product product { get; set; }

        [DataType(DataType.Currency)]
        public int total { get; set; }

        [Required]
        public DateTime datetime { get; set; }
        [Required]
        public string First_Name { get; set; }
        [Required]
        public string last_Name { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string City { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression("^01[0-5][0-9]{8}$", ErrorMessage = "Not a valid phone number")]
        public string Phone1 { get; set; }
        
        public string Phone2 { get; set; }
        
        public string Order_notes { get; set; }

        public string orderstatus { get; set; }

    }
}