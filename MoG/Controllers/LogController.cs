using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MoG.Domain.Models;
using MoG.Domain.Repository;
using MoG.Domain.Service;

namespace MoG.Controllers
{
    [MogAuthAttribut]
    public class LogController : MogController
    {
       
        public LogController(IUserService userService
           , ILogService logService
            )
            : base(userService, logService)
        {

          

        }

        // GET: /Log/
        public ActionResult Index(int startIndex = 0, int count = 10)
        {
            var model = this.serviceLog.Get(startIndex, count);
            return View(model);
        }

        // GET: /Log/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Log log = this.serviceLog.GetById(id.Value);
            if (log == null)
            {
                return HttpNotFound();
            }
            return View(log);
        }

   
        // GET: /Log/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Log log = this.serviceLog.GetById(id.Value);
            if (log == null)
            {
                return HttpNotFound();
            }
            return View(log);
        }

        // POST: /Log/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Log log = this.serviceLog.GetById(id);
            this.serviceLog.Delete(log);
          
            return RedirectToAction("Index");
        }

      
    }
}
