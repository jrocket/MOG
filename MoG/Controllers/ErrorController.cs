using MoG.Domain.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MoG.Controllers
{
   
    public class ErrorController : MogController
    {
        public ErrorController (IUserService userService)
            : base(userService)
        { }

        public ViewResult Index()
        {
            DisplayErrorMessage(TempData[MogConstants.TEMPDATA_ERRORMESSAGE] as string);
            return View("Error");
        }
        public ViewResult NotFound()
        {
            Response.StatusCode = 404;  //you may want to set this to 200
            return View("NotFound");
        }
    }
}