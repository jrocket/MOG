using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using MoG.Domain.Models;
using MoG.Domain.Service;
using MoG.Helpers;
using MoG.Domain.Skydrive;

namespace MoG.Controllers
{
    [Authorize]
    public class AccountController : FlabbitController
    {
        IDropBoxService serviceDropbox = null;
        ISkydriveService serviceSkydrive = null;
        IRegistrationCodeService serviceRegistrationCode = null;
        ISecurityService serviceSecurity = null;
        IActivityService serviceActivity = null;
        public UserManager<ApplicationUser> UserManager { get; private set; }


        public AccountController(IUserService userService,
            IDropBoxService dropboxService,
            ISkydriveService skydriveService,
            IRegistrationCodeService registrationService
            , ILogService logService
            , ISecurityService securityService
            , IActivityService activityService

            )
            : this(userService
            , dropboxService
            , skydriveService
            , registrationService
            , new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()))
            , logService
            , securityService
            , activityService
            )
        {

        }

        public AccountController(IUserService userService
            , IDropBoxService dropboxService
            , ISkydriveService skydriveService
            , IRegistrationCodeService registrationService
            , UserManager<ApplicationUser> userManager
            , ILogService logService
            , ISecurityService securityService
            , IActivityService activityService
            )
            : base(userService, logService)
        {
            this.serviceDropbox = dropboxService;
            this.serviceSkydrive = skydriveService;
            UserManager = userManager;
            userService.UserManager = userManager;
            this.serviceRegistrationCode = registrationService;
            this.serviceSecurity = securityService;
            this.serviceActivity = activityService;
        }

        #region Profile Management
        ////
        //// GET: /Account/
        //public ActionResult Index(ManageMessageId? message)
        //{
        //    ViewBag.StatusMessage =
        //       message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
        //       : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
        //       : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
        //       : message == ManageMessageId.Error ? "An error has occurred."
        //       : "";
        //    ViewBag.HasLocalPassword = HasPassword();
        //    ViewBag.ReturnUrl = Url.Action("Manage");
        //    return View(CurrentUser);
        //}

        public ActionResult SetLanguage(string culture)
        {
            // Validate input
            culture = CultureHelper.GetImplementedCulture(culture);
            // Save culture in a cookie
            HttpCookie cookie = Request.Cookies["_culture"];
            if (cookie != null)
                cookie.Value = culture;   // update cookie value
            else
            {
                cookie = new HttpCookie("_culture");
                cookie.Value = culture;
                cookie.Expires = DateTime.Now.AddYears(1);
            }
            Response.Cookies.Add(cookie);
            return RedirectToAction("Manage");
        }

        [HttpPost]
        public JsonResult UpdateProfile(int id, string displayName, string email)
        {
            if (id != this.CurrentUser.Id)
            {
                return JsonHelper.ResultError("... no, ... no, ... no!", null);
            }
            this.CurrentUser.DisplayName = displayName;
            this.CurrentUser.Email = email;
            this.serviceUser.SaveChanges(this.CurrentUser);

            return JsonHelper.ResultOk(true);
        }

        public ActionResult SetNofications(string frequency)
        {
            NotificationFrequency freq = NotificationFrequency.Never;
            if (Enum.TryParse(frequency,out freq))
            {
                this.CurrentUser.NotificationFrequency = freq;
                this.serviceUser.SaveChanges(this.CurrentUser);
            
            }
            return RedirectToAction("Manage");
        }

        public ActionResult DashBoard(int id = -1)
        {
            if (id == -1)
            {
                id = CurrentUser.Id;
            }
            if (!this.serviceSecurity.HasRight(SecureActivity.ViewUserDashboard, CurrentUser, id))
            {
                return this.RedirectToErrorPage(Resources.Resource.COMMON_PermissionDenied);
            }
            return View();
        }

        public JsonResult GetNotifications()
        {
            int id = CurrentUser.Id;

            JsonResult result = new JsonResult();
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            var data = this.serviceActivity.GetNotificationByUserId(id);
            //todo use automapper
            result.Data = data.Select(n => new VMNotification()
                { 
                    url = n.Url,
                    message = n.Message,
                    pictureUrl = n.PictureUrl,
                    isRead = (n.IsRead ? "true" : "false"),
                    when = n.CreatedOn
                }
                );
            this.serviceActivity.MarkNotificationsAsRead(id);
            return result;
        }

        public ActionResult Followed()
        {
            return View();
        }


        public ActionResult Storage()
        {
            UserStorageVM model = this.serviceUser.GetUserStorages(this.CurrentUser);
            return View(model);
        }

        public ActionResult RegisterStorage(CloudStorageServices service)
        {
            string redirectUrl = "";
            string targetUrl = "";
            switch (service)
            {
                case CloudStorageServices.Dropbox:
                    int credentialId = -1;
                    targetUrl = this.Url.Action("RegisterDropboxStorage", "Account", null, this.Request.Url.Scheme);
                    redirectUrl = this.serviceDropbox.AskForRegistrationUrl(this.CurrentUser, targetUrl, out credentialId);
                    TempData["tempCredentialId"] = credentialId;
                    break;
                case CloudStorageServices.GoogleDrive:
                    return this.RedirectToComingSoon();
                    break;
                case CloudStorageServices.Skydrive:
                    return this.RedirectToComingSoon();
                    targetUrl = this.Url.Action("RegisterSkydriveStorage", "Account", null, this.Request.Url.Scheme);
                    redirectUrl = this.serviceSkydrive.AskForRegistrationUrl(this.CurrentUser, targetUrl, out credentialId);
                    break;

            }
            return Redirect(redirectUrl);
        }

        public ActionResult RegisterSkydriveStorage(string oauth_token)
        {
            string redirectUrl = this.Url.Action("RegisterSkydriveStorage", "Account", null, this.Request.Url.Scheme);
            if (!string.IsNullOrEmpty(Request.QueryString[OAuthConstants.AccessToken]))
            {
                // There is a token available already. It should be the token flow. Ignore it.
                return RedirectToAction("Storage");
            }
            string verifier = Request.QueryString[OAuthConstants.Code];
            OAuthToken token;
            OAuthError error;
            if (!string.IsNullOrEmpty(verifier))
            {
                serviceSkydrive.RequestAccessTokenByVerifier(verifier, redirectUrl, out token, out error);
                serviceSkydrive.RegisterAccount(token, CurrentUser);
                return RedirectToAction("Storage");
            }



            string errorCode = Request.QueryString[OAuthConstants.Error];
            string errorDesc = Request.QueryString[OAuthConstants.ErrorDescription];

            if (!string.IsNullOrEmpty(errorCode))
            {
                //todo : do something
            }
            return RedirectToAction("Storage");
        }

        public ActionResult RegisterDropboxStorage(string oauth_token)
        {
            String dropBoxToken = Request["oauth_token"];
            int tempCredentialId = (int)TempData["tempCredentialId"];
            if (String.IsNullOrEmpty(dropBoxToken))
            {
                RedirectToErrorPage("Dropbox authorization failed... your local account is not linked to dropbox");

            }
            try
            {
                this.serviceDropbox.RegisterAccount(tempCredentialId);

            }
            catch (Exception)
            {
                RedirectToErrorPage(Resources.Resource.STORAGE_RegisterDropboxFailed);
            }

            return RedirectToAction("Storage");
        }

        [HttpPost]
        public JsonResult CancelRegistration(int id)
        {
            this.serviceUser.CancelRegistration(id);
            var result = new
            {
                result = true,
                redirectUrl = Url.Action("Storage")
            };

            JsonResult json = new JsonResult() { Data = result };
            return json;
        }

        public ActionResult Avatar(int id = -1)
        {
            UserProfileInfo model = null;
            if (id > 0)
            {
                model = this.serviceUser.GetById(id);
            }
            else
            {
                model = CurrentUser;
            }
            return View(model);
        }

        public ActionResult Alert()
        {
            return View();
        }



        #endregion

        #region Login / register
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
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindAsync(model.UserName, model.Password);
                if (user != null)
                {
                    await SignInAsync(user, model.RememberMe);
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
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
                var user = new ApplicationUser() { UserName = model.UserName };
                var registrationCode = this.serviceRegistrationCode.GetByCode(model.RegistrationCode);
                if (registrationCode == null || registrationCode.UserId != null)
                {
                    return this.RedirectToErrorPage("wrong registration code....");
                }
                //var result = await UserManager.CreateAsync(user, model.Password);
                var result = await this.serviceUser.CreateAsync(user,
                    model.Password,
                    model.RegistrationCode,
                    model.Email,
                    UserManager);

                //var result = await  UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {

                    await SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    AddErrors(result);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/Disassociate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Disassociate(string loginProvider, string providerKey)
        {
            ManageMessageId? message = null;
            IdentityResult result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
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
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            ViewBag.HasLocalPassword = HasPassword();
            ViewBag.ReturnUrl = Url.Action("Manage");
            ManageUserViewModel model = new ManageUserViewModel();
            model.User = CurrentUser;
            return View(model);
        }

        //
        // POST: /Account/Manage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Manage(ManageUserViewModel model)
        {
            bool hasPassword = HasPassword();
            ViewBag.HasLocalPassword = hasPassword;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasPassword)
            {
                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }
            else
            {
                // User does not have a password so remove any validation errors caused by a missing OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            model.User = CurrentUser;
            return View(model);
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
            var user = await UserManager.FindAsync(loginInfo.Login);
            if (user != null)
            {
                await SignInAsync(user, isPersistent: false);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                // If the user does not have an account, then prompt the user to create an account
                ViewBag.ReturnUrl = returnUrl;
                ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { UserName = loginInfo.DefaultUserName });
            }
        }

        //
        // POST: /Account/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new ChallengeResult(provider, Url.Action("LinkLoginCallback", "Account"), User.Identity.GetUserId());
        }

        //
        // GET: /Account/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            if (result.Succeeded)
            {
                return RedirectToAction("Manage");
            }
            return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
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
                return RedirectToAction("Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser() { UserName = model.UserName };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInAsync(user, isPersistent: false);
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
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult RemoveAccountList()
        {
            var linkedAccounts = UserManager.GetLogins(User.Identity.GetUserId());
            ViewBag.ShowRemoveButton = HasPassword() || linkedAccounts.Count > 1;
            return (ActionResult)PartialView("_RemoveAccountPartial", linkedAccounts);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && UserManager != null)
            {
                UserManager.Dispose();
                UserManager = null;
            }
            base.Dispose(disposing);
        }
        #endregion

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

        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);

            var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            Error
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

        private class ChallengeResult : HttpUnauthorizedResult
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
                var properties = new AuthenticationProperties() { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}