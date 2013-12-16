using MoG.Domain.Models;
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
        private IProjectService serviceProject = null;
        private IFileService serviceFile = null;
        public FileController(IFileService _fileService
            , IUserService userService
            , IProjectService projectService)
            : base(userService)
        {
            serviceFile = _fileService;
            serviceProject = projectService;
        }
        public ActionResult Detail(int id = -1)
        {
            var file = serviceFile.GetById(id);
            ViewBag.Comments = serviceFile.GetFileComments(id);
            return View(file);
        }

        public JsonResult GetComments(int id = 1)
        {
            var comments = serviceFile.GetFileComments(id);
            var result = new JsonResult() { Data = comments, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            return result;
        }

        public ActionResult Create(int id)
        {
            var project = serviceProject.GetById(id);
            VMFile model = new VMFile() { Project = project };
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(VMFile data, HttpPostedFileBase file)
        {
            var project = serviceProject.GetById(data.Project.Id);
            data.File.Project = project;
            int fileId = this.serviceFile.Create(data.File, this.CurrentUser);
            if (fileId > 0)
            {
                return RedirectToAction("Detail", new { id = fileId });
            }
            return View(data);

        }

        [HttpPost]
        public JsonResult Accept(int fileId)
        {
            JsonResult result = new JsonResult();
            if (this.serviceFile.Accept(fileId))
            {
                result.Data = true;
            }
            else
            {
                result.Data = false;
            }
            return result;
        }

        [HttpPost]
        public JsonResult Reject(int fileId)
        {
            JsonResult result = new JsonResult();
            if (this.serviceFile.Reject(fileId))
            {
                result.Data = true;
            }
            else
            {
                result.Data = false;
            }
            return result;
        }

        [HttpPost]
        public JsonResult Delete(int fileId)
        {
            JsonResult result = new JsonResult();
            var file = this.serviceFile.GetById(fileId);
            if (file != null)
            {
                int projectId = file.ProjectId;
             
                if (this.serviceFile.Delete(fileId, this.CurrentUser))
                {
                    result.Data = new
                    {
                        Flag = true,
                        Url = Url.Action("Files", "Project",
                            new
                            {
                                id = projectId
                            })
                    };

                }
                else
                {
                    result.Data = false;
                }


            }
            else
            {
                throw new Exception("NOT THE RIGHT FILE");
            }
            return result;

        }
    }
}