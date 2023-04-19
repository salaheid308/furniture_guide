using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Dalelk.Models
{
    public class city
    {
        public int id { get; set; }
        [Required]
        [Display(Name = "اسم المدينه")]
        public string cityname { get; set; }

        [Display(Name = "صوره المدينه ")]
        public string cityimg { get; set; }

        public virtual ICollection<gallries> gallries { get; set; }

        public virtual ICollection<category> categories { get; set; }

       


    }
}