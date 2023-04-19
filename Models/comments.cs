using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Dalelk.Models
{
    public class comments
    {
        public int id { get; set; }

        [Display(Name = "التعليق ")]
        public string usercomment { get; set; }

        public string userid { get; set; }
        public virtual ApplicationUser user { get; set; }

        
        public int plogid { get; set; }
        public virtual plogs plog { get; set; }

        public DateTime datetime { get; set; }

        public virtual ICollection<Replay> replaies { get; set; }

    }
}