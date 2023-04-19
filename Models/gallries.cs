using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Dalelk.Models
{
    public class gallries
    {
        public int id { get; set; }

        [Display(Name = "اسم المعرض")]
        public string gallryname { get; set; }

        [Display(Name = "  اسم المعرض ")]
        public string realgallryname { get; set; }

        [Display(Name = "العنوان بالتفصيل")]
        public string adress { get; set; }

        [Display(Name = "تيليفون المعرض")]
        public string galleryphone { get; set; }

        [Display(Name = "فيس بوك ")]
        [Url(ErrorMessage = "ادخل لينك صحيح")]
        public string faceurl { get; set; }

        [Display(Name = "تويتر ")]
        [Url(ErrorMessage = "ادخل لينك صحيح")]
        public string tuiterurl { get; set; }

        [Display(Name = " يوتيوب")]
        [Url(ErrorMessage = "ادخل لينك صحيح")]
        public string youtupurl { get; set; }

        [Display(Name = "صاحب المعرض")]
        public string userid { get; set; }
        public virtual ApplicationUser user { get; set; }

        [Display(Name = "صنف  المنتج ")]
        public int categoryid { get; set; }
        public virtual category category { get; set; }

        [Display(Name = "city ")]
        public int cityid { get; set; }
        public virtual city city { get; set; }


        public virtual ICollection<product> products { get; set; }
        public virtual ICollection<productcolor> productcolor { get; set; }
        public virtual ICollection<productcolor2> productcolor2 { get; set; }
        public virtual ICollection<productprice> productprice { get; set; }
       
        public virtual ICollection<productsize> productsize { get; set; }
    }
}