using MoG.Domain.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MoG.Controllers
{
    public class FileController : MogController
    {
        private IFileService serviceFile = null;
        public FileController(IFileService _fileService, IUserService userService)
            : base(userService)
        {
            serviceFile = _fileService;
        }
        public ActionResult Detail(int id=-1)
        {
            var file = serviceFile.GetById(id);
            ViewBag.Comments = serviceFile.GetFileComments(id);
            return View(file);
        }

        public JsonResult GetComments(int id=1)
        {
            var comments = serviceFile.GetFileComments(id);
            var result = new JsonResult() { Data = comments, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            return result;

        }
	}
}