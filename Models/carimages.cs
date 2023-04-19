using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Dalelk.Models
{
    public class carimages
    {
        public int id { get; set; }

        [Display(Name = "اسم الصوره ")]
        public string imagename { get; set; }


        [Display(Name = "تاريخ المنتج ")]
        public int driverid { get; set; }
        public virtual drivers driver { get; set; }


    }
}