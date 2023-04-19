using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dalelk.Models
{
    public class productrate
    {
        public int id { get; set; }

        
        public int rate { get; set; }

        public string userid { get; set; }
        public virtual ApplicationUser user { get; set; }

        public int productid { get; set; }
        public virtual product product { get; set; }
    }
}