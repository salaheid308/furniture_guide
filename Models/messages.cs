using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dalelk.Models
{
    public class messages
    {
        public int id { get; set; }

        public string userid { get; set; }
        public virtual ApplicationUser user { get; set; }
        

        public int count { get; set; }

        public string sendermessage { get; set; }

        public DateTime datetim { get; set; }

        public string usersendid { get; set; }


        public string username { get; set; }
    }
}