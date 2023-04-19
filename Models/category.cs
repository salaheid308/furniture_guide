using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Dalelk.Models
{
    public class category
    {
        public int id { get; set; }
        [Required]
        [Display(Name = "اسم الصنف")]
        public string categoryname { get; set; }

        [Display(Name = "city ")]
        public int cityid { get; set; }
        public virtual city city { get; set; }

        

        public virtual ICollection<product> products { get; set; }
        public virtual ICollection<gallries> gallries { get; set; }
        public virtual ICollection<productcolor> productcolor { get; set; }
        public virtual ICollection<productcolor2> productcolor2 { get; set; }
        public virtual ICollection<productprice> productprice { get; set; }
        
        public virtual ICollection<productsize> productsize { get; set; }

    }
}