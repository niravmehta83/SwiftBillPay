using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using BingHousingMVC.Filters;
using BingHousingMVC.Models;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using BingHousingMVC.Utility;
using System.Web.Routing;
using System.Text.RegularExpressions;
using BingHousingMVC.GlobalOperations;
using BingHousing_BO;
using BingHousingMVC_DAL;
using System.Web.Configuration;
using AutoMapper;

namespace BingHousingMVC.Controllers
{
    [Authorize]
    //[InitializeSimpleMembership]
    public class AccountController : Controller
    {
        public IMembershipService MembershipService { get; set; }
        private IBHDbase dbase = new BHDbase();

        protected override void Initialize(RequestContext requestContext)
        {
            if (MembershipService == null)
            { MembershipService = new AccountMembershipService(); }

            base.Initialize(requestContext);

        }
        //
        // GET: /Account/Login

        [AllowAnonymous]
        public PartialViewResult Captcha()
        {
            // Create a random code and store it in the Session object.
            this.Session["CaptchaImageText"] = GenerateRandomCode();
            // Create a CAPTCHA image using the text stored in the Session object.
            RandomImage ci = new RandomImage(this.Session
                ["CaptchaImageText"].ToString(), 300, 75);
            // Change the response headers to output a JPEG image.

            // Write the image to the file in JPEG format.

            string outputFileName = Server.MapPath("~/Images/Captcha/captcha.jpg");
            using (MemoryStream memory = new MemoryStream())
            {
                using (FileStream fs = new FileStream(outputFileName, FileMode.Create, FileAccess.ReadWrite))
                {
                    ci.BitImage.Save(memory, ImageFormat.Jpeg);
                    byte[] bytes = memory.ToArray();
                    fs.Write(bytes, 0, bytes.Length);
                }
            }


            // Dispose of the CAPTCHA image object.
            ci.Dispose();

            return PartialView();
        }
        //
        // GET: /Account/Login

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            WebSecurity.Logout();
            //Session.Clear();
            //Session.Abandon();
            string Username = User.Identity.Name;
            if (returnUrl != "/Account/LogOff")
            {
                ViewBag.ReturnUrl = returnUrl;
            }
            else
            {
                ViewBag.ReturnUrl = "/Home/Index";
            }
            ViewBag.Show = false;
            return View();
        }

        //
        // POST: /Account/Login

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {

            if (WebSecurity.IsConfirmed(model.UserName))
            {

                if (!dbase.IsUserLocked(model.UserName))
                {
                    if (WebSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe))
                    {

                        try
                        {
                           
                            if (model.Password2 == dbase.GetPassword2(model.UserName) && model.Captcha == this.Session["CaptchaImageText"].ToString())
                            {
                                const string _usersKey = "Users";

                                //Cache UsersList

                                //_Users = (List<SelectListItem>)System.Web.HttpContext.Current.Cache[_usersKey];
                                List<SelectListItem> _Users = dbase.GetallUserName(true).ToSelectListItem();
                                System.Web.HttpContext.Current.Cache.Add(_usersKey, _Users, null, DateTime.MaxValue, System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, null);


                                return RedirectToLocal(returnUrl);
                            }
                            else
                            {
                                if (this.Session["CaptchaImageText"] != null && model.Captcha == this.Session["CaptchaImageText"].ToString())
                                {
                                    ModelState.AddModelError("", "Invalid Credentials .");
                                }
                                else
                                {
                                    ModelState.AddModelError("", "Captch Text is wrong.");
                                }
                            }
                        }
                        catch (Exception ex)
                        {

                            throw ex;
                        }



                    }
                    WebSecurity.Logout();                   
                    ViewBag.Show = false;
                    // If we got this far, something failed, redisplay form
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                }

                else
                {
                    ViewBag.Show = false;
                    // If we got this far, something failed, redisplay form
                    ModelState.AddModelError("", "The user was Locked please Contact Admin to Unlock.");
                }
            }
            else
            {
                ViewBag.Show = true;
                // If we got this far, something failed, redisplay form
                ModelState.AddModelError("", "The user is not verified please Use Above link to send verification link again.");
            }
            ////LoginModel newmodel = new LoginModel();
            ////newmodel.UserName = model.UserName;
            ////newmodel.RememberMe = model.RememberMe;
            //ModelState.Clear();
            //model.Captcha = "";S
            return View(model);
        }

        //
        // POST: /Account/LogOff

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {

            WebSecurity.Logout();
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }



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
        public ActionResult Register(RegisterModel model)
        {
            //
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                try
                {
                    //WebSecurity.CreateUserAndAccount(model.UserName, model.Password);
                    //dbase.InsertPassword2(model);
                    //WebSecurity.Login(model.UserName, model.Password);
                    Session["RegisterModel"] = model;
                    return RedirectToAction("RegisterUserDetails");
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/RegisterUserDetails

        [AllowAnonymous]
        public ActionResult RegisterUserDetails()
        {

            return View();

        }

        //
        // POST: /Account/RegisterUserDetails

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult RegisterUserDetails(UserModel model)
        {
            

            if (ModelState.IsValid)
            {
                // Attempt to register the user
                try
                {
                    RegisterModel regmodel = (RegisterModel)Session["RegisterModel"];
                    string confirmationtoken = WebSecurity.CreateUserAndAccount(regmodel.UserName, regmodel.Password, null, true);
                    dbase.InsertPassword2(new RegisterData(regmodel.UserName, regmodel.Password, regmodel.Password2, regmodel.Email));
                    Roles.AddUserToRole(regmodel.UserName, "User");
                    model.UserId = dbase.GetUserId(regmodel.UserName);
                    dbase.InsertUserDetails(Mapper.Map<UserDetail>(model));

                    bool success = MembershipService.SendVerification(confirmationtoken, regmodel.UserName, SupportMailType.Verification);

                    if (success)
                    {
                        ViewBag.Message = "Your Account was Created Successfully. We sent an email verification. Please follow the link to verify your account.";
                        return View("Success");
                    }
                    else
                    {
                        return View();
                    }


                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", e.Message);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        [Authorize]
        public ActionResult EditUserDetails()
        {

            

            string name = System.Web.HttpContext.Current.Session["SelectedUserName"] as string ?? User.Identity.Name;

            UserModel model = Mapper.Map<UserModel>(dbase.GetUserDetails(name));

            return View(model);

        }

        //
        // POST: /Account/RegisterUserDetails

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult EditUserDetails(UserModel model)
        {
            

            if (ModelState.IsValid)
            {
                // Attempt to register the user
                try
                {

                    dbase.UpdateUserDetails(Mapper.Map<UserDetail>(model));

                    return RedirectToAction("Index", "Home");

                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", e.Message);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        //verify the confirmation token
        [AllowAnonymous]
        public ActionResult Verify(string ID)
        {
            if (WebSecurity.ConfirmAccount(ID))
            {
                ViewBag.Message = "Your Account verified successfully. please continue to login";
            }
            else
            {
                ViewBag.Message = "The confirmation Token was not valid ";
            }
            return View("Success");
        }

        //get send verification again
        [AllowAnonymous]
        public ActionResult SendBackVerificationMail()
        {
            return View();
        }


        //post send verification again
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult SendBackVerificationMail(LoginModel model)
        {
            


            if (WebSecurity.UserExists(model.UserName)) //checking whether user name is available on the table or not
            {


                if (!WebSecurity.IsConfirmed(model.UserName))
                {

                    bool sucess = MembershipService.SendVerification(dbase.GetConfirmationToken(model.UserName), model.UserName, SupportMailType.Verification);

                    if (sucess)
                    {

                        ViewBag.Message = "Check Your mail, we sent verification mail again, please folow the link to verify your account thanks";
                        return View("Success");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Sorry Try Again Later");

                        return View();
                    }
                }
                else
                {
                    WebSecurity.Logout();
                    Session.Clear();
                    Session.Abandon();
                    TempData["tempMessage"] = "You have already confirmed your email address... please log in.";

                    return RedirectToAction("Login", "Account", TempData["tempMessage"]);
                }
            }

            else
            {
                TempData["tempMessage"] = " You Are Not Yet Registered... please Register.";

                return RedirectToAction("Register", "account", TempData["tempMessage"]);
            }

        }

        //get send verification again
        [AllowAnonymous]
        public ActionResult ForgetPassword()
        {
            return View();
        }


        //post send verification again
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult ForgetPassword(LoginModel model)
        {

            if (model.UserName != null)
            {
                
                if (WebSecurity.UserExists(model.UserName)) //checking whether user name is available on the table or not
                {


                    if (!WebSecurity.IsConfirmed(model.UserName.Trim()))
                    {
                        string token =dbase.GetConfirmationToken(model.UserName);
                        bool sucess = token == "" ? false : MembershipService.SendVerification(token, model.UserName, SupportMailType.Verification);

                        if (sucess)
                        {
                            ModelState.AddModelError("", "You haven't confirm your email address. we have sent a mail to your mail address use the link to confirm your Email.");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Your Account Not verified.");
                        }
                        return View();

                    }
                    else
                    {

                        string[] roles = Roles.GetRolesForUser(model.UserName.Trim());

                        bool flag = roles.Length == 0 ? false : roles[0] == "Customer" ? true : false;

                        if (flag)
                        {
                            // user has no role
                            ModelState.AddModelError("", "Invalid UserName");

                            return View();
                        }
                        else
                        {
                            string confiramtiontoken = WebSecurity.GeneratePasswordResetToken(model.UserName, 1440);
                            bool sucess = MembershipService.SendVerification(confiramtiontoken, model.UserName, SupportMailType.ResetPassword);

                            if (sucess)
                            {

                                ViewBag.Message = "Check Your mail, we sent Password reset token to your mail which is registered with us, please folow the link to reset your password, and this is for your account security thanks";
                                return View("Success");
                            }
                            else
                            {
                                ModelState.AddModelError("", "Sorry Try Again Later");

                                return View();
                            }
                        }
                    }
                }

                else
                {
                    TempData["tempMessage"] = " You Are Not Yet Registered... please Register.";

                    return RedirectToAction("Register", "account", TempData["tempMessage"]);
                }
            }
            else
            {
                ModelState.AddModelError("", "Please Enter UserName");

                return View();
            }

        }



        [HttpGet]
        [AllowAnonymous]
        public ActionResult ChangePassword(string UserName)
        {            
            LocalPasswordModel chPass = new LocalPasswordModel();
            return View("ChangePassword", chPass);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult ChangePassword(LocalPasswordModel model)
        {

            //string confiramtiontoken = WebSecurity.GeneratePasswordResetToken(UserName, 1440);
            //bool sucess = MembershipService.SendVerification(confiramtiontoken, UserName, SupportMailType.ResetPassword);
            bool sucess = WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword1, model.NewPassword1);
            if (model.OldPassword2 != model.NewPassword2)
            {
                dbase.ChangePassword2(User.Identity.Name, model.NewPassword1, model.NewPassword2);
            }

            if (sucess)
            {
                WebSecurity.Logout();
                Session.Clear();
                Session.Abandon();
                //ViewBag.Message = "Check Your mail, we sent Password reset token to your mail which is registered with us, please folow the link to reset your password, and this is for your account security thanks";
                ViewBag.Message = "Your Password change has done Successfully, now continue login with User name and new passwords.";
                return View("Success");
            }
            else
            {
                ViewBag.Message = "sorry some error occured while processing your request please try again later";
                return View("Success");
            }

        }

        //post send reset passwordtoken again
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[AllowAnonymous]
        //public ActionResult ChangePassword(string UserName)
        //{

        //    string confiramtiontoken = WebSecurity.GeneratePasswordResetToken(UserName, 1440);
        //    bool sucess = MembershipService.SendVerification(confiramtiontoken, UserName, SupportMailType.ResetPassword);

        //    if (sucess)
        //    {
        //        WebSecurity.Logout();
        //        ViewBag.Message = "Check Your mail, we sent Password reset token to your mail which is registered with us, please folow the link to reset your password, and this is for your account security thanks";
        //        return View("Success");
        //    }
        //    else
        //    {
        //        ViewBag.Message = "sorry some error occured while processing your request please try again later";
        //        return View("Success");
        //    }

        //}


        //get send verification again
        [AllowAnonymous]
        public ActionResult ResetPassword(string ID)
        {
            string[] arry = new string[2];
            arry = ID.Split('@').ToArray();
            int userid = WebSecurity.GetUserIdFromPasswordResetToken(ID);
            ResetPasswordModel model = new ResetPasswordModel();
            model.ResetPasswordToken = arry[0];
            model.UserName = arry[1];
            return View(model);
        }


        //post send verification again
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult ResetPassword(ResetPasswordModel model)
        {
            if (WebSecurity.UserExists(model.UserName)) //checking whether user name is available on the table or not
            {
                
                try
                {
                    if (WebSecurity.ResetPassword(model.ResetPasswordToken, model.NewPassword1))
                    {
                        dbase.ChangePassword2(model.UserName, model.NewPassword1, model.NewPassword2);
                        ViewBag.Message = "Your Password Reset has done Successfully, now continue login with User name and new passwords.";
                        return View("Success");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            else
            {
                ModelState.AddModelError("", "User is Not yet Regitered");
            }

            return View();

        }

        //
        // POST: /Account/Disassociate

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Disassociate(string provider, string providerUserId)
        {
            string ownerAccount = OAuthWebSecurity.GetUserName(provider, providerUserId);
            ManageMessageId? message = null;

            // Only disassociate the account if the currently logged in user is the owner
            if (ownerAccount == User.Identity.Name)
            {
                // Use a transaction to prevent the user from deleting their last login credential
                using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
                    if (hasLocalAccount || OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name).Count > 1)
                    {
                        OAuthWebSecurity.DeleteAccount(provider, providerUserId);
                        scope.Complete();
                        message = ManageMessageId.RemoveLoginSuccess;
                    }
                }
            }

            return RedirectToAction("Manage", new { Message = message });
        }

        //
        // GET: /Account/Manage

        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : "";
            ViewBag.HasLocalPassword = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        //
        // POST: /Account/Manage

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(LocalPasswordModel model)
        {
            
            bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.HasLocalPassword = hasLocalAccount;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasLocalAccount)
            {
                if (ModelState.IsValid)
                {
                    // ChangePassword will throw an exception rather than return false in certain failure scenarios.
                    bool changePasswordSucceeded;
                    try
                    {
                        if (model.OldPassword2 == dbase.GetPassword2(User.Identity.Name))
                        {
                            changePasswordSucceeded = WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword1, model.NewPassword1);
                            dbase.ChangePassword2(User.Identity.Name, model.NewPassword1, model.NewPassword2);
                        }
                        else
                        {
                            changePasswordSucceeded = false;
                        }
                    }
                    catch (Exception)
                    {
                        changePasswordSucceeded = false;
                    }

                    if (changePasswordSucceeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                    }
                }
            }
            else
            {
                // User does not have a local password so remove any validation errors caused by a missing
                // OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        WebSecurity.CreateAccount(User.Identity.Name, model.NewPassword1);
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    catch (Exception e)
                    {
                        ModelState.AddModelError("", e);
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/ExternalLogin

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            return new ExternalLoginResult(provider, Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback

        [AllowAnonymous]
        public ActionResult ExternalLoginCallback(string returnUrl)
        {
            AuthenticationResult result = OAuthWebSecurity.VerifyAuthentication(Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
            if (!result.IsSuccessful)
            {
                return RedirectToAction("ExternalLoginFailure");
            }

            if (OAuthWebSecurity.Login(result.Provider, result.ProviderUserId, createPersistentCookie: false))
            {
                return RedirectToLocal(returnUrl);
            }

            if (User.Identity.IsAuthenticated)
            {
                // If the current user is logged in add the new account
                OAuthWebSecurity.CreateOrUpdateAccount(result.Provider, result.ProviderUserId, User.Identity.Name);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                // User is new, ask for their desired membership name
                string loginData = OAuthWebSecurity.SerializeProviderUserId(result.Provider, result.ProviderUserId);
                ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(result.Provider).DisplayName;
                ViewBag.ReturnUrl = returnUrl;
                return View("ExternalLoginConfirmation", new RegisterExternalLoginModel { UserName = result.UserName, ExternalLoginData = loginData });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLoginConfirmation(RegisterExternalLoginModel model, string returnUrl)
        {
            string provider = null;
            string providerUserId = null;

            if (User.Identity.IsAuthenticated || !OAuthWebSecurity.TryDeserializeProviderUserId(model.ExternalLoginData, out provider, out providerUserId))
            {
                return RedirectToAction("Manage");
            }

            if (ModelState.IsValid)
            {
                // Insert a new user into the database
                using (UsersContext db = new UsersContext())
                {
                    BingHousingMVC.Models.UserProfile user = db.UserProfiles.FirstOrDefault(u => u.UserName.ToLower() == model.UserName.ToLower());
                    // Check if user already exists
                    if (user == null)
                    {
                        // Insert name into the profile table
                        db.UserProfiles.Add(new BingHousingMVC.Models.UserProfile { UserName = model.UserName });
                        db.SaveChanges();

                        OAuthWebSecurity.CreateOrUpdateAccount(provider, providerUserId, model.UserName);
                        OAuthWebSecurity.Login(provider, providerUserId, createPersistentCookie: false);

                        return RedirectToLocal(returnUrl);
                    }
                    else
                    {
                        ModelState.AddModelError("UserName", "User name already exists. Please enter a different user name.");
                    }
                }
            }

            ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(provider).DisplayName;
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // GET: /Account/ExternalLoginFailure

        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [AllowAnonymous]
        [ChildActionOnly]
        public ActionResult ExternalLoginsList(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return PartialView("_ExternalLoginsListPartial", OAuthWebSecurity.RegisteredClientData);
        }

        [ChildActionOnly]
        public ActionResult RemoveExternalLogins()
        {
            ICollection<OAuthAccount> accounts = OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name);
            List<ExternalLogin> externalLogins = new List<ExternalLogin>();
            foreach (OAuthAccount account in accounts)
            {
                AuthenticationClientData clientData = OAuthWebSecurity.GetOAuthClientData(account.Provider);

                externalLogins.Add(new ExternalLogin
                {
                    Provider = account.Provider,
                    ProviderDisplayName = clientData.DisplayName,
                    ProviderUserId = account.ProviderUserId,
                });
            }

            ViewBag.ShowRemoveButton = externalLogins.Count > 1 || OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            return PartialView("_RemoveExternalLoginsPartial", externalLogins);
        }

        #region Helpers

        // Function to generate random string with Random class.
        private string GenerateRandomCode()
        {
            Random r = new Random();
            //string s = "";
            //for (int j = 0; j < 5; j++)
            //{
            //    int i = r.Next(3);
            //    int ch;
            //    switch (i)
            //    {
            //        case 1:
            //            ch = r.Next(0, 9);
            //            s = s + ch.ToString();
            //            break;
            //        case 2:
            //            ch = r.Next(65, 90);
            //            s = s + Convert.ToChar(ch).ToString();
            //            break;
            //        case 3:
            //            ch = r.Next(97, 122);
            //            s = s + Convert.ToChar(ch).ToString();
            //            break;
            //        default:
            //            ch = r.Next(97, 122);
            //            s = s + Convert.ToChar(ch).ToString();
            //            break;
            //    }
            //    r.NextDouble();
            //    r.Next(100, 1999);
            //}
            //return s;

            return r.Next(00000, 99999).ToString();
        }
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

        internal class ExternalLoginResult : ActionResult
        {
            public ExternalLoginResult(string provider, string returnUrl)
            {
                Provider = provider;
                ReturnUrl = returnUrl;
            }

            public string Provider { get; private set; }
            public string ReturnUrl { get; private set; }

            public override void ExecuteResult(ControllerContext context)
            {
                OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
            }
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
