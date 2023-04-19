using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Dalelk.Models
{
    public class Replay
    {
        public int id { get; set; }

        [Display(Name = "الرد ")]
        public string userreplay { get; set; }

        public string userid { get; set; }
        public virtual ApplicationUser user { get; set; }

        public int plogid { get; set; }

        public int commentid { get; set; }
        public virtual comments comment { get; set; }

        public DateTime datetime { get; set; }


    }
}