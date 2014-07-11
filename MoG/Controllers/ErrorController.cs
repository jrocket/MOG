using MoG.Domain.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MoG.Controllers
{

    public partial class ErrorController : FlabbitController
    {
         public ErrorController (IUserService userService
             , ILogService logService
            )
            : base(userService, logService)
        { }
        //
        // GET: /Error/Internal

        public virtual ActionResult Internal()
        {
            return View();
        }

        public ViewResult NotFound()
        {
            Response.StatusCode = 404;  //you may want to set this to 200
            return View("NotFound");
        }

        public ViewResult ComingSoon()
        {
            return View();
        }

         public ViewResult Index()
        {
           
            return View();
        }
         public ViewResult Error()
         {
             DisplayErrorMessage(TempData[MogConstants.TEMPDATA_ERRORMESSAGE] as string);
             return View();
         }

    }

    //public class ErrorController : MogController
    //{
    //    public ErrorController (IUserService userService
    //         , ILogService logService
    //        )
    //        : base(userService, logService)
    //    { }

    //    public ViewResult Index()
    //    {
    //        DisplayErrorMessage(TempData[MogConstants.TEMPDATA_ERRORMESSAGE] as string);
    //        return View();
    //    }
    //    public ViewResult NotFound()
    //    {
    //        Response.StatusCode = 404;  //you may want to set this to 200
    //        return View("NotFound");
    //    }

    //    public ViewResult ComingSoon()
    //    {
    //        return View();
    //    }



      



    //}
}