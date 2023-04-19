using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Dalelk.Models
{
    public class productimages
    {
        public int id { get; set; }

        [Display(Name = "اسم الصوره ")]
        public string imagename { get; set; }

        [Required]
        [Display(Name = "تاريخ رفع الصوره ")]
        public DateTime datetime { get; set; }

        [Display(Name = "تاريخ المنتج ")]
        public int productid { get; set; }


        public virtual product product { get; set; }

       

    }
}