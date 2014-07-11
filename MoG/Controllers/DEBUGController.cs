using MoG.Domain.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MoG.Controllers
{
    public class DEBUGController : FlabbitController
    {
        public DEBUGController(IUserService userService
           , ILogService logService
            )
            : base(userService, logService)
        {

        }
        //
        // GET: /DEBUG/
        public ActionResult Index()
        {
            Session["MagicKey"] = 1;
            return View();
        }
        public JsonResult SwitchUser()
        {
            int? currentUserId = (int?)Session["CURRENTUSER"];
            if (currentUserId.HasValue)
            {
                if (currentUserId.Value == 1)
                    Session["CURRENTUSER"] = 2;
                else
                    Session["CURRENTUSER"] = 1;
            }
            else
            {
                Session["CURRENTUSER"] = 1;
            }
            var result = new JsonResult();
            result.Data = "OK";
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;

        }
	}
}