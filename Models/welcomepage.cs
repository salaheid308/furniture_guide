using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Dalelk.Models
{
    public class welcomepage
    {
        public int id { get; set; }

        [Display(Name = "العنوان")]
        public string title { get; set; }

        [Display(Name = "صوره الحلفيه ")]
        public string backgroundtimg { get; set; }

        [Display(Name = "تفاصيل العنوان")]
        public string details { get; set; }

       
    }
}