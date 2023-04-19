using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Dalelk.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]

        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required(ErrorMessage = "يرجي ادخال رقم الموبايل")]
        [Display(Name = "رقم الموبايل")]
        [RegularExpression("^01[0-2,5][0-9]{8}$", ErrorMessage = "ادخل رقم هاتف صالح")]
        public string phonenumber { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "كلمه السر")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
    
    public class RegisterViewModel
    {

        [Required(ErrorMessage = "يرجي ادخال اسم المستخدم")]
        [Display(Name = "الاسم")]

        [System.Web.Mvc.Remote("doesUserNameExist", "Account", HttpMethod = "POST")]

        public string name { get; set; }

        [Required(ErrorMessage = "يرجي ادخال رقم الموبايل")]
        [Display(Name = "رقم الموبايل")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression("^01[0-2,5][0-9]{8}$", ErrorMessage = "ادخل رقم هاتف صالح")]
        [System.Web.Mvc.Remote("doesUserphoneExist", "Account", HttpMethod = "POST", ErrorMessage = "رقم الهاتف هذا موجود  بالفعل ")]

        public string PhoneNumber { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "كلمه السر")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "تأكيد كلمه السر")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    

    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "يرجي ادخال بريدك الالكتروني ")]
        [EmailAddress(ErrorMessage = "يرجي ادخال بريد الكتروني صالح")]
        [Display(Name = "بريدك الالكتروني")]

        [System.Web.Mvc.Remote("doesUserNameExistforforget", "Account", HttpMethod = "POST", ErrorMessage = "هذا الايميل غير موجود")]

        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "كلمة المرور")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "تأكيد كلمة المرور")]
        [Compare("Password", ErrorMessage = "كلمة المرور غير متطابقة")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "يرجي ادخال بريدك الالكتروني ")]
        [EmailAddress(ErrorMessage = "يرجي ادخال بريد الكتروني صالح")]
        [Display(Name = "Email")]

        [System.Web.Mvc.Remote("doesUserNameExistforforget", "Account", HttpMethod = "POST", ErrorMessage = "هذا الايميل غير موجود")]

        public string Email { get; set; }
    }
    public class ForgotPasswordViewModel_phone
    {
        [Required(ErrorMessage = "يرجي ادخال رقم هاتفك ")]
        [Display(Name = "رقم الموبايل")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression("^01[0-2,5][0-9]{8}$", ErrorMessage = "ادخل رقم هاتف صالح")]
        [System.Web.Mvc.Remote("doesphoneExistforforget", "Account", HttpMethod = "POST", ErrorMessage = "رقم الهاتف غير موجود")]

        public string phonenumber { get; set; }
    }
    
}
