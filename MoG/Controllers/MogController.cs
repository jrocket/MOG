﻿using MoG.Domain.Models;
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
    public class FlabbitController : Controller
    {
        //private List<String> errorMessage;
        protected IUserService serviceUser;
        private UserProfileInfo _currentUser;
        protected ILogService serviceLog; 

        public UserProfileInfo CurrentUser
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


        public FlabbitController(IUserService _userService, ILogService _logService)
        {
            serviceUser = _userService;
            this.serviceLog = _logService;
            //this.errorMessage = new List<string>();
        }


        protected void DisplayErrorMessage(string message)
        {
            ViewBag.ErrorMessage = message;
        }

        protected ActionResult RedirectToErrorPage(string message)
        {
            TempData[MogConstants.TEMPDATA_ERRORMESSAGE] = message;
            return RedirectToAction("error", "error");

        }

     

        protected ActionResult RedirectToComingSoon()
        {
            return RedirectToAction("ComingSoon", "error");
        }

        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {

            //DEBUGREDIRECT();

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

            // User management
            if (User != null)
            {
                this.serviceUser.Identity = User.Identity;
            }


            return base.BeginExecuteCore(callback, state);
        }

        //protected override void EndExecuteCore(IAsyncResult asyncResult)
        //{
        //    ViewBag.ErrorMessages= errorMessage;
        //    ViewBag.ErrorMessages = "toto mange des pommes";
        //    base.EndExecuteCore(asyncResult);
        //}
        //private void DEBUGREDIRECT()
        //{
        //    if (!User.Identity.IsAuthenticated && !this.Request.Url.AbsolutePath.Contains("/account"))
        //    //if (Session["MagicKey"]==null && !Request.Url.AbsolutePath.Contains("debug"))
        //    {
        //        HttpContext.Response.Redirect(Url.Action("login","account"));
        //    }
            
        //}

        //public void AddErrorMessage(string message)
        //{
        //    errorMessage.Add(message);
        //}
    }
}
