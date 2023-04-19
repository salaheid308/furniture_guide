using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Dalelk.Models
{
    public class product
    {
        public int id { get; set; }

        [Display(Name = "اسم المنتج")]
        public string productname { get; set; }

        [Display(Name = "وصف المنتج ")]
        public string productdescrption { get; set; }

        [Display(Name = "صوره كامله للمنتج ")]
        public string productimg { get; set; }

        [Display(Name = "سعر المنتج ")]
        [DataType(DataType.Currency)]
        public int productprice { get; set; }

        [Display(Name = "اخفاء سعر المنتج  ")]
        public bool hidepric { get; set; }

        [Display(Name = "لون الطلاء ")]
        public string productcolor { get; set; }

        [Display(Name = " لون القماش اذا وجد")]
        public string productcolor2 { get; set; }

        [Display(Name = "حجم المنتج ")]
        public string productsize { get; set; }
        
        [Required]
        public DateTime datetime { get; set; }

        [Display(Name = "صنف  المنتج ")]
        public int categoryid { get; set; }
        public virtual category category { get; set; }
        
        [Display(Name = "معرض المنتج ")]
        public int gallryid { get; set; }
        public virtual gallries gallry { get; set; }

        public virtual ICollection<productimages> images { get; set; }

        public virtual ICollection<productrate> productrates { get; set; }

        public int colorid { get; set; }
        public virtual productcolor color { get; set; }

        public int color2id { get; set; }
        public virtual productcolor2 color2 { get; set; }

        public int priceid { get; set; }
        public virtual productprice price { get; set; }

       

        public int sizeid { get; set; }
        public virtual productsize size { get; set; }
    }
}