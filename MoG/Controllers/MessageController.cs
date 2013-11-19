using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MoG.Controllers
{
    public class MessageController : MogController
    {

        public ActionResult Index()
        {
            return RedirectToAction("Received");

        }
        //
        // GET: /Message/
        public ActionResult Received()
        {
            return View();
        }

        public ActionResult Sent()
        {
            return View();

        }

        public ActionResult Chat()
        {
            return View();
        }

        public ActionResult Mention()
        {
            return View();
        }

        public ActionResult Search()
        {
            return View();
        }
	}
}