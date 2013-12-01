using MoG.Domain.Service;
using MoG.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace MoG.Controllers
{
    public class ProfileController : MogController
    {
        public ProfileController(IUserService userService)
            : base(userService)
        { }
        //
        // GET: /Profile/
        public ActionResult Index()
        {
            return View();
        }

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
            return RedirectToAction("Index");
        }

        public ActionResult Storage()
        {
            return View();
        }
        public ActionResult Avatar()
        {
            return View();
        }
        public ActionResult Alert()
        {
            return View();
        }

        public ActionResult Dashboard()
        {
            return View();
        }


    }
}