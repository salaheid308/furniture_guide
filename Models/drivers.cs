using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Dalelk.Models
{
    public class drivers
    {
        public int id { get; set; }

        [Required]
        [Display(Name = "الاسم")]
        public string name { get; set; }

        [Required]
        [Display(Name = "الاسم بالكامل")]
        public string username { get; set; }

        [Required(ErrorMessage = "يرجي ادخال رقم الموبايل")]
        [Display(Name = "رقم الموبايل")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression("^01[0-5][0-9]{8}$", ErrorMessage = "Not a valid phone number")]
        public string PhoneNumber { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "كلمه السر")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "تأكيد كلمه السر")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        
        public string userimg { get; set; }

        
        public string carname { get; set; }

        
        public string cartype { get; set; }

        
        public string drivertype { get; set; }

        [Required]
        [Display(Name = "city ")]
        public int cityid { get; set; }
        public virtual city city { get; set; }

        public string userid { get; set; }
        public virtual ApplicationUser user { get; set; }

        public virtual ICollection<carimages> carimages { get; set; }

        public virtual ICollection<driverprice> driverprice { get; set; }

    }
}