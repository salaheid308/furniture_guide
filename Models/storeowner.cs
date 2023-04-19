using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace Dalelk.Models
{
    public class storeowner
    {

        
        public int id { get; set; }

        [Required(ErrorMessage = "يرجي ادخال اسم")]
        [Display(Name = "الاسم")]

        [System.Web.Mvc.Remote("doesUserNameExist", "Account", HttpMethod = "POST", ErrorMessage = "اختر اسم مستخدم او ايميل اخر ")]
       
        //[StringLength(30, MinimumLength = 3, ErrorMessage = "يرجي ادخال اسم بحد ادني 3 حروف")]
        public string name { get; set; }

        [Required(ErrorMessage = "يرجي ادخال رقم الموبايل")]
        [Display(Name = "رقم الموبايل")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression("^01[0-2,5][0-9]{8}$", ErrorMessage = "ادخل رقم هاتف صالح")]
        [System.Web.Mvc.Remote("doesUserphoneExist", "Account", HttpMethod = "POST", ErrorMessage = "رقم الهاتف هذا موجود  بالفعل ")]

        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "يرجي ادخال كلمة المرور")]
        [DataType(DataType.Password)]
        [Display(Name = "كلمه السر")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "تأكيد كلمه السر")]
        [Compare("Password", ErrorMessage = "كلمة السر غير متطابقة")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "city ")]
        public int cityid { get; set; }
        public virtual city city { get; set; }

        [Display(Name = "صاحب المعرض")]
        public string userid { get; set; }
        public virtual ApplicationUser user { get; set; }

        [Required(ErrorMessage = "يرجي ادخال العنوان بالتفصيل")]
        [Display(Name = "العنوان بالتفصيل")]
        public string adress { get; set; }
        [Required(ErrorMessage = "يرجي ادخال  اسم المعرض")]
        [Display(Name = "اسم المعرض")]
        public string galleryname { get; set; }

        [Required(ErrorMessage = "يرجي ادخال تيليفون المعرض")]
        [Display(Name = "تيليفون المعرض")]
        public string galleryphone { get; set; }

        [Required]
        [Display(Name = "نوع منتجات المعرض")]
        public int categoryid { get; set; }
        public virtual category category { get; set; }

    }
}