using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dalelk.Models
{
    public class notification
    {
        public int id { get; set; }

        public string userid { get; set; }
        public virtual ApplicationUser user { get; set; }


        public int plogid { get; set; }

        public int count { get; set; }

        public string notify { get; set; }

        public string username { get; set; }


    }
}