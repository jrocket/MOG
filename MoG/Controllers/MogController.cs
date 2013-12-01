using MoG.Domain.Models;
using MoG.Domain.Service;
using MoG.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace MoG.Controllers
{
    public class MogController : Controller
    {
        private IUserService serviceUser;
        private UserProfile _currentUser;

        public UserProfile CurrentUser
        {
            get
            {
                if (_currentUser == null)
                {
                    _currentUser = serviceUser.GetCurrentUser();
                }
                return _currentUser;
            }

        }


        public MogController(IUserService _userService)
        {
            serviceUser = _userService;
            ViewBag.CurrentUserDisplayName = CurrentUser.DisplayName; //TODO : remove this line once the identity management is done!
        }


        protected void DisplayErrorMessage(string message)
        {
            ViewBag.ErrorMessage = message;
        }

        protected ActionResult RedirectToErrorPage(string message)
        {
            TempData[MogConstants.TEMPDATA_ERRORMESSAGE] = message;
            return RedirectToAction("index", "error");

        }

        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            string cultureName = null;

            // Attempt to read the culture cookie from Request
            HttpCookie cultureCookie = Request.Cookies["_culture"];
            if (cultureCookie != null)
                cultureName = cultureCookie.Value;
            else
                cultureName = Request.UserLanguages != null && Request.UserLanguages.Length > 0 ?
                        Request.UserLanguages[0] :  // obtain it from HTTP header AcceptLanguages
                        null;
            // Validate culture name
            cultureName = CultureHelper.GetImplementedCulture(cultureName); // This is safe

            // Modify current thread's cultures            
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(cultureName);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            return base.BeginExecuteCore(callback, state);
        }


    }
}
