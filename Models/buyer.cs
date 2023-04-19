using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dalelk.Models
{
    public class buyer
    {
        public int id { get; set; }
        public string name { get; set; }
        public string password { get; set; }
        public string phonenumber { get; set; }
        public string userid { get; set; }
        public virtual ApplicationUser user { get; set; }
    }
}