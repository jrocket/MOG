using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MoG.Controllers
{
    public class SocialController : MogController
    {
        //
        // GET: /Social/
        public ActionResult Friends()
        {
            return View();
        }
        //
        // GET: /Social/
        public ActionResult Invits()
        {
            return View();
        }
	}
}