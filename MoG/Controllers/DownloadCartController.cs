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
    public class DownloadCartController : FlabbitController
    {
        private IDownloadCartService serviceDownloadCart = null;
        public DownloadCartController(
             IUserService userService,
            IDownloadCartService downloadCartService,
            ILogService logService
           )
            : base(userService,logService)
        {
            serviceDownloadCart = downloadCartService;

        }

        // GET: /DownloadCart/
        public ActionResult Index()
        {

            var downloadcarts = serviceDownloadCart.GetByUserId(CurrentUser.Id);
            List<VMDownloadCartItem> model = new List<VMDownloadCartItem>();
            foreach (var item in downloadcarts)
            {
                model.Add(new VMDownloadCartItem()
                {
                    FileName = item.File.DisplayName,
                    Id = item.Id,
                    ProjectName = item.File.Project.Name,
                    Url = item.File.PublicUrl
                });

            }
            return View(model);
        }





        [HttpPost]
        public JsonResult Create(int fileId)
        {
            int id = this.serviceDownloadCart.AddToCart(fileId, CurrentUser);
            JsonResult json = new JsonResult();
            json.Data = new { result = id > 0 };
            return json;

        }



        // POST: /DownloadCart/Delete/5
        [HttpPost]
        public JsonResult Delete(int id)
        {
            bool serviceResult = this.serviceDownloadCart.Delete(id, CurrentUser);
            JsonResult json = new JsonResult();
            json.Data = new { result = serviceResult };
            return json;
        }


        [HttpPost]
        public JsonResult ClearCart()
        {
            bool serviceResult = this.serviceDownloadCart.ClearCart(CurrentUser.Id);
            JsonResult json = new JsonResult();
            json.Data = new { result = serviceResult };
            return json;
        }

    }
}
