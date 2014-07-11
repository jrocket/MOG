using MoG.Domain.Models;
using MoG.Domain.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MoG.Controllers
{
    [MogAuthAttribut]
    public class SearchController : FlabbitController
    {
        private IProjectService serviceProject = null;
        public SearchController(IUserService _userService,IProjectService projectService
             , ILogService logService
            )
            : base(_userService, logService)
        {

            this.serviceProject = projectService;
        }
        //
        // GET: /Search/
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult SearchPeople(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return null;
            }
            List<VMSearchResult> result = new List<VMSearchResult>();
            foreach (var model in this.serviceUser.Search(query, 1, 50))
            {//todo : use automapper
                VMSearchResult user = new VMSearchResult();
                user.name = model.DisplayName;
                if (model.PictureUrl.StartsWith("~"))
                {
                    user.pictureUrl = Url.Content(model.PictureUrl);
                }
                else
                {
                    user.pictureUrl = model.PictureUrl;
                }
                
                user.url = Url.Action("Profile", "Social", new { id = model.Id });
                result.Add(user);
            }
            return new JsonResult() { Data = result };

        }
        public JsonResult SearchProject(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return null;
            }
            List<VMSearchResult> result = new List<VMSearchResult>();
            foreach (var model in this.serviceProject.Search(query,1,50))
            {//todo : use automapper
                VMSearchResult project = new VMSearchResult();
                project.name = model.Name;
                if (model.ImageUrlThumb1.StartsWith("~"))
                {
                    project.pictureUrl = Url.Content(model.ImageUrlThumb1);
                }
                else
                {
                    project.pictureUrl = model.ImageUrlThumb1;
                }
              
                project.url = Url.Action("Detail","Project",new {id = model.Id});
                result.Add(project);
            }
            return new JsonResult() { Data = result };

        }

    }
}