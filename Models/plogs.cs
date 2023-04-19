using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Dalelk.Models
{
    public class plogs
    {
        public int id { get; set; }

        [Display(Name = "عنوان المنشور ")]
        public string title { get; set; }

        [Display(Name = "محتوي المشروع ")]
        public string content { get; set; }


        public DateTime datetime { get; set; }

        [Display(Name = "صوره المنشور ")]
        public string image { get; set; }

        [Display(Name = "الناشر")]
        public string userid { get; set; }
        public virtual ApplicationUser user { get; set; }

        public virtual ICollection<comments> comments { get; set; }

    }
}