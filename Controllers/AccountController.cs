using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Dalelk.Models;
using System.IO;
using static Dalelk.Controllers.productsController;
using System.Collections.Generic;
using System.Web.Security;
using System.Net.Mail;
//using System.Net;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Data.Entity;

namespace Dalelk.Controllers
{

    public class AccountController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: trues
            var user = db.Users.FirstOrDefault(o => o.PhoneNumber == model.phonenumber);
            var result = await SignInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.name, PhoneNumber = model.PhoneNumber, usertype = "buy" };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");
                    await UserManager.AddToRoleAsync(user.Id, "buy");
                    buyer nbuyer = new buyer();
                    nbuyer.name = model.name;
                    nbuyer.password = model.Password;
                    nbuyer.phonenumber = model.PhoneNumber;
                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult Registerasowner(int? id)
        {
            var findcity = db.cities.Find(id);
            if (findcity != null && id != null)
            {
                ViewBag.categoryid = db.categories.Where(d => d.cityid == id).ToList();
                ViewBag.cityid = id;
                return View();
            }
            return RedirectToAction("index", "Home");
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Registerasowner(storeowner model, int? ctyid, int? catid)
        {

            if (ModelState.IsValid && ctyid != null && catid != null)
            {
                var con = db.Users.Where(a => a.PhoneNumber == model.PhoneNumber).Any();
                if (con == false)
                {
                    var user = new ApplicationUser { UserName = model.name, PhoneNumber = model.PhoneNumber, usertype = "sales" };


                    var result = await UserManager.CreateAsync(user, model.Password);

                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                        // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                        // Send an email with this link
                        // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                        // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                        // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");
                        await UserManager.AddToRoleAsync(user.Id, "sales");

                    }
                    else
                    {
                        AddErrors(result);
                        ViewBag.categoryid = db.categories.Where(d => d.cityid == ctyid).ToList();

                        ViewBag.cityid = ctyid;
                        return View(model);
                    }
                    model.userid = user.Id;
                    model.cityid = Convert.ToInt32(ctyid);
                    model.categoryid = Convert.ToInt32(catid);

                    db.storeowners.Add(model);
                    db.SaveChanges();

                    gallries newgallry = new gallries();
                    newgallry.adress = model.adress;
                    newgallry.galleryphone = model.galleryphone;
                    string gname = "معرض" + db.Users.Count();
                    newgallry.gallryname = gname;
                    newgallry.cityid = Convert.ToInt32(ctyid);
                    newgallry.categoryid = Convert.ToInt32(catid);
                    newgallry.realgallryname = model.galleryname;
                    newgallry.userid = user.Id;
                    db.gallries.Add(newgallry);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.categoryid = db.categories.Where(d => d.cityid == ctyid).ToList();

                    ViewBag.cityid = ctyid;
                    ViewBag.eror = "دخل رقم جديد دا موجود قبل كدا";
                    return View(model);
                }
            }

            ViewBag.categoryid = db.categories.Where(d => d.cityid == ctyid).ToList();

            ViewBag.cityid = ctyid;
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public JsonResult CheckUsername(string userdata)
        {
            System.Threading.Thread.Sleep(200);
            var result = db.Users.Where(o => o.UserName == userdata).Any();
            if (result)
            {
                return Json(1);
            }
            else
            {
                return Json(0);
            }
        }
        [HttpPost]
        public JsonResult doesUserphoneExist(string PhoneNumber)
        {

            var user = db.Users.Where(o => o.PhoneNumber == PhoneNumber).FirstOrDefault();

            return Json(user == null);
        }

        public JsonResult doesUserNameExist(string name)
        {
            //string loginurl = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, "Identity/Account/Login");
            if (UserNameExists(name))
            {
                

                while (UserNameExists(name))
                {
                    Random random = new Random();
                    int randomNumber = random.Next(1, 100);
                    name = name + randomNumber.ToString();
                }

                return Json($"هذا الاسم موجود مسبقا يمكنك استخدام  avalable: <a onClick='insertText(this)' href='javascript: void(0)'>{name}</a>");

                
            }
            
            
                return Json(true);
            
            //var user = db.Users.Where(o => o.UserName == name).SingleOrDefault();

            //return Json(user == null);
        }
        public bool UserNameExists(string userName)
        {
            var user = db.Users.Where(o => o.UserName == userName).FirstOrDefault();
            return user != null;
        }
            [HttpPost]
        public JsonResult doesUserNameExistforforget(string Email)
        {

            var user = db.Users.Where(o => o.UserName == Email).SingleOrDefault();

            return Json(user != null);
        }
        public JsonResult doesphoneExistforforget(string phonenumber)
        {

            var user = db.Users.Where(o => o.PhoneNumber == phonenumber).SingleOrDefault();

            return Json(user != null);
        }

        [AllowAnonymous]
        public ActionResult Registerasdriver()
        {

            //ViewBag.cityid = new SelectList(db.cities, "id", "cityname");
            ViewBag.cityid = db.cities.ToList();
            return View();
        }

        //
        // POST: /Account/Registe
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Registerasdriver(drivers model, HttpPostedFileBase upload, imgfiless obgct, string usertype, string Segmentation, int citytype)
        {
            var con = db.Users.Where(a => a.PhoneNumber == model.PhoneNumber).Any();
            model.cityid = citytype;
            if (ModelState.IsValid && con == false)
            {


                var user = new ApplicationUser { UserName = model.name, PhoneNumber = model.PhoneNumber, usertype = "delivery" };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");
                    await UserManager.AddToRoleAsync(user.Id, "delivery");


                    var name = Guid.NewGuid() + Path.GetExtension(upload.FileName);
                    string bath = Path.Combine(Server.MapPath("~/driverimgs"), name);
                    upload.SaveAs(bath);

                    model.drivertype = usertype;
                    model.cartype = Segmentation;
                    model.userid = user.Id;
                    model.userimg = name;
                    db.drivers.Add(model);
                    db.SaveChanges();

                    carimages carimgs = new carimages();
                    foreach (var file in obgct.files)
                    {
                        if (file != null && file.ContentLength > 0)
                        {
                            string name1 = Guid.NewGuid() + Path.GetExtension(file.FileName);
                            file.SaveAs(Path.Combine(Server.MapPath("~/carimages"), name1));
                            carimgs.imagename = name1;
                            carimgs.driverid = model.id;
                            db.carimages.Add(carimgs);
                            db.SaveChanges();

                        }

                    }

                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
                ViewBag.cityid = db.cities.ToList();
                return View(model);

            }

            if (con != false)
            {
                ViewBag.eror = "دخل رقم جديد دا موجود قبل كدا";
                if (usertype == "مندوب توصيل")
                {
                    ViewBag.driverpepo = "مندوب توصيل";
                }
                if (usertype == "مندوب شحن")
                {
                    ViewBag.driverpepo = "مندوب شحن";
                }
            }
            if (usertype == "مندوب توصيل")
            {
                ViewBag.driverpepo = "مندوب توصيل";
            }
            if (usertype == "مندوب شحن")
            {
                ViewBag.driverpepo = "مندوب شحن";
            }

            ViewBag.cityid = db.cities.ToList();
            return View(model);
        }

        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        [AllowAnonymous]
        public ActionResult ForgotPasswordbyphone()
        {
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPasswordbyphone(ForgotPasswordViewModel_phone model)
        {

            if (ModelState.IsValid)
            {
                var userph = db.Users.FirstOrDefault(o => o.PhoneNumber == model.phonenumber);
                var user = await UserManager.FindByNameAsync(userph.UserName);
                if (user == null)
                {
                    return HttpNotFound();
                }

                Random generator = new Random();
                int r = generator.Next(1000, 10000);
                var nverfycod = new vefycods();
                nverfycod.phonenumber = model.phonenumber;
                nverfycod.code = r;
                db.vefycods.Add(nverfycod);
                db.SaveChanges();


                return View();
            }

            // If we got this far, something failed, redisplay formD:\مشاريع\Dalelk\Dalelk\Views\Account\ForgotPassword.cshtml
            return View(model);
        }
        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {

            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null )
                {
                    return HttpNotFound();
                }
                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);

                var email = new MailMessage();
                var loginfo = new NetworkCredential("salaheid308@gmail.com", "01095591324#*#");
                email.From = new MailAddress("salaheid308@gmail.com");
                email.To.Add(new MailAddress(model.Email));
                email.Subject = "reset passowrd";
                email.IsBodyHtml = true;
                string body = "برجاء اعادة تعيين كلمة المرور الخاصه بك من خلال الضغط علي هذا الرابط <a href=\"" + callbackUrl + "\">هنا</a>";
                email.Body = body;

                var smtpc = new SmtpClient("smtp.gmail.com", 587);
                smtpc.EnableSsl = true;
                smtpc.Credentials = loginfo;
                smtpc.Send(email);
                return View("ForgotPasswordConfirmation");

            }

            // If we got this far, something failed, redisplay formD:\مشاريع\Dalelk\Dalelk\Views\Account\ForgotPassword.cshtml
            return View(model);
        }
     
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return HttpNotFound();
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                if (user.usertype == "sales")
                {
                    var fondstor = db.storeowners.FirstOrDefault(r => r.userid == user.Id);
                    fondstor.Password = model.Password;
                    db.Entry(fondstor).State = EntityState.Modified;
                    db.SaveChanges();
                }
                if (user.usertype == "buy")
                {
                    var fondsbuer = db.buyers.FirstOrDefault(r => r.userid == user.Id);
                    fondsbuer.password = model.Password;
                    db.Entry(fondsbuer).State = EntityState.Modified;
                    db.SaveChanges();
                }
                if (user.usertype == "delivery")
                {
                    var fondsdriver = db.drivers.FirstOrDefault(r => r.userid == user.Id);
                    fondsdriver.Password = model.Password;
                    db.Entry(fondsdriver).State = EntityState.Modified;
                    db.SaveChanges();
                }

                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }

    internal class UsersEntities
    {
    }
}