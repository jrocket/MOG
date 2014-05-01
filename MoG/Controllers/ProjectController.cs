using MoG.Code;
using MoG.Domain.Models;
using MoG.Domain.Models.TimeLine;
using MoG.Domain.Models.ViewModel;
using MoG.Domain.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MoG.Controllers
{
    [MogAuthAttribut]
    public class ProjectController : MogController
    {
        private IProjectService serviceProject = null;
        private ISecurityService serviceSecurity = null;
        private IFollowService serviceFollow = null;

        public ProjectController(IProjectService project
            , ISecurityService security
            , IFollowService follow
            , IUserService userService
            , ILogService logService
            )
            : base(userService, logService)
        {
            this.serviceSecurity = security;
            this.serviceProject = project;
            this.serviceFollow = follow;

        }
        //
        // GET: /Project/
        public ActionResult Index()
        {
            return View();
        }

        #region Project Lists
        public List<VMProjectList> TodoUseAutomapper(List<Project> model)
        {
            int maxDescriptionLength = 25;
            List<VMProjectList> viewmodel = new List<VMProjectList>();
            foreach (var project in model)
            {
                VMProjectList item = new VMProjectList();
                string projectDescription = project.Description;
                if (!String.IsNullOrEmpty(projectDescription))
                {
                    if (projectDescription.Length > maxDescriptionLength)
                    {
                        projectDescription = projectDescription.Substring(0, maxDescriptionLength - 3) + "...";
                    }


                }
                else
                {
                    projectDescription = "...";
                }
                item.Description = projectDescription;
                item.Id = project.Id;
                item.ImageUrl = Url.Content(project.ImageUrl);
                item.Likes = project.Likes;
                item.Name = project.Name;
                item.IsPrivate = project.VisibilityType == Visibility.Private;
                item.ProjectUrl = Url.Action("Detail", new { id = project.Id });
                item.OwnerName = project.Creator.DisplayName;
                viewmodel.Add(item);
            }
            return viewmodel;
        }

        public JsonResult GetNew(int PageNumber, int PageSize)
        {
            var projects = serviceProject.GetNew(PageNumber, PageSize, true, true);
            return new JsonResult() { Data = TodoUseAutomapper(projects.ToList()) };
        }


        public ActionResult New()
        {
            ViewBag.Title = Resources.Resource.COMMON_NewProjects;
            ViewBag.GetDataUrl = Url.Action("GetNew");

            return View("ProjectList");
        }

        public JsonResult GetMy(int PageNumber, int PageSize)
        {
            var projects = this.serviceProject.GetByUserLogin(PageNumber, PageSize, CurrentUser.Login, false, true);
            return new JsonResult() { Data = TodoUseAutomapper(projects.ToList()) };

        }

        public JsonResult GetMyProjectNames(string filter = null)
        {
            IQueryable<Project> data = this.serviceProject.GetByUserLogin(-1, -1, CurrentUser.Login, false, true);
            var model = data.Select(p => new VMSelect2item() { id = p.Id, text = p.Name });


            return new JsonResult() { Data = model, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        public ActionResult My()
        {

            ViewBag.Title = Resources.Resource.MAINMENU_MyProjects;
            ViewBag.GetDataUrl = Url.Action("GetMy");

            return View("ProjectList");

        }

        public JsonResult GetPopular(int PageNumber, int PageSize)
        {
            var projects = serviceProject.GetPopular(PageNumber, PageSize, true, true);
            return new JsonResult() { Data = TodoUseAutomapper(projects.ToList()) };
        }


        public ActionResult Popular()
        {

            ViewBag.Title = Resources.Resource.PROJECT_Popular;
            ViewBag.GetDataUrl = Url.Action("GetPopular");

            return View("ProjectList");
        }
        public JsonResult GetRandom(int PageNumber, int PageSize)
        {
            var projects = serviceProject.GetRandom(10, true, true);
            return new JsonResult() { Data = TodoUseAutomapper(projects.ToList()) };
        }

        public ActionResult Random()
        {
            ViewBag.Title = Resources.Resource.PROJECT_Random;
            ViewBag.GetDataUrl = Url.Action("GetRandom");
            return View("ProjectList");
        }

        #endregion

        public ActionResult Detail(int id = -1)
        {
            if (id < 0)
            {
                return RedirectToErrorPage("project not found");
            }
            VMProject model = this.getById(id);
            if (!this.serviceSecurity.HasRight(SecureActivity.ProjectView, CurrentUser, model.Project))
            {
                return this.RedirectToErrorPage(Resources.Resource.COMMON_PermissionDenied);
            }
            return View(model);
        }

        public ActionResult Edit(int id = -1)
        {
            if (id < 0)
            {
                return RedirectToErrorPage("project not found");
            }
            Project model = this.serviceProject.GetById(id);
            if (!this.serviceSecurity.HasRight(SecureActivity.ProjectEdit, CurrentUser, model))
            {
                return this.RedirectToErrorPage(Resources.Resource.COMMON_PermissionDenied);
            }


            return PartialView(model);
        }



        public ActionResult Create()
        {
            return View();

        }

        // POST: /DUMMY/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,VisibilityType,LicenceType,Tags")] Project project)
        {
            if (ModelState.IsValid)
            {
                int newId = serviceProject.Create(project, CurrentUser);


                return RedirectToAction("Detail", new { id = newId });
            }

            return View(project);
        }

        public ActionResult Delete(int id = -1)
        {
            if (id < 0)
                return RedirectToErrorPage("...?....");
            var model = this.getById(id);
            if (!this.serviceSecurity.HasRight(SecureActivity.ProjectDelete, CurrentUser, model.Project))
            {
                return this.RedirectToErrorPage(Resources.Resource.COMMON_PermissionDenied);
            }

            return View(model);
        }

        public ActionResult DeleteConfirmed()
        {
            return View();
        }

        [HttpPost]
        public JsonResult DeleteConfirmed(int id = -1)
        {
            JsonResult json = new JsonResult();

            if (id < 0)
            {
                json.Data = new { Result = false };
            }
            var project = this.serviceProject.GetById(id);
            if (!this.serviceSecurity.HasRight(SecureActivity.ProjectDelete, CurrentUser, project))
            {
                return JsonHelper.ResultError(null, null, Resources.Resource.COMMON_PermissionDenied);
            }

            bool result = this.serviceProject.Delete(id, CurrentUser);
            string url = Url.Action("DeleteConfirmed");


            json.Data = new { Url = url, Result = result };

            return json;

        }

        public ActionResult Activity(int id = 1)
        {
            VMProject model = this.getById(id);
            if (!this.serviceSecurity.HasRight(SecureActivity.ProjectView, CurrentUser, model.Project))
            {
                return this.RedirectToErrorPage(Resources.Resource.COMMON_PermissionDenied);
            }
            return View(model);
        }

        private VMProject getById(int id)
        {
            VMProject model = new VMProject();
            model.Project = this.serviceProject.GetById(id);
            model.HasEdit = this.serviceSecurity.HasRight(SecureActivity.ProjectEdit, CurrentUser, model.Project);
            model.IsFollowed = this.serviceFollow.IsFollowed(model.Project, CurrentUser);
            return model;
        }


        public JsonResult GetActivities(int id)
        {
            Project project = serviceProject.GetById(id);
            if (!this.serviceSecurity.HasRight(SecureActivity.ProjectView, CurrentUser, project))
            {
                return JsonHelper.ResultError(null, null, Resources.Resource.COMMON_PermissionDenied, JsonRequestBehavior.AllowGet);
            }


            IList<Activity> activities = serviceProject.GetProjectActivity(id);
            //todo : use automapper
            Root data = new Root();
            data.timeline = new Timeline();
            data.timeline.headline = String.Format("Here is what happened in {0}", project.Name);
            data.timeline.type = "default";
            data.timeline.text = "<p>Click on the left or right arrow to see the next event</p>";
            data.timeline.asset = new Asset()
            {
                credit = "Credit Name Goes Here",
                caption = "Caption text goes here"
            };
            data.timeline.date = new List<Date>();

            List<VMActivity> model = new Mapper().MapActivities(activities);

            foreach (var activity in model)
            {
                Date theEvent = new Date()
               {
                   StartDate = activity.When,
                   EndDate = activity.When,
                   headline = activity.Who.DisplayName,
                   tag = activity.Type.ToString(),
                   text = String.Format("<a href='{0}'>{1}</a>",
                   activity.Url,
                   activity.Description)
               };



                data.timeline.date.Add(theEvent);
            }

            return new JsonResult() { Data = data, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }



        public ActionResult Files(int id = 1, string filterByType = "", string filterByStatus = "", string filterByAuthor = "")
        {
            if (id < 0)
            {
                this.DisplayErrorMessage("Project not found...");
                return View();

            }
            VMProjectFiles model = new VMProjectFiles();



            model.Project = this.getById(id);
            var project = model.Project.Project;
            if (!this.serviceSecurity.HasRight(SecureActivity.ProjectView, CurrentUser, project))
            {
                return this.RedirectToErrorPage(Resources.Resource.COMMON_PermissionDenied);
            }

            model.FilteredFiles = serviceProject.GetFilteredFiles(project, filterByAuthor, filterByStatus, filterByType);
            model.Statuses = serviceProject.GetFileStatuses(project);
            model.Authors = serviceProject.GetFileAuthors(project);
            model.Types = serviceProject.GetFileTags(project);
            model.filterByAuthor = filterByAuthor;
            model.filterByStatus = filterByStatus;
            model.filterByType = filterByType;
            return View(model);
        }


        [HttpPost]
        public JsonResult Promote(int fileId)
        {
            JsonResult result = new JsonResult();
            if (this.serviceProject.Promote(fileId))
            {
                result.Data = true;
            }
            else
            {
                result.Data = false;
            }
            return result;
        }


        public JsonResult JSON(int id)
        {
            var project = serviceProject.GetById(id);

            if (!this.serviceSecurity.HasRight(SecureActivity.ProjectView, CurrentUser, project))
            {
                return JsonHelper.ResultError(null, null, Resources.Resource.COMMON_PermissionDenied, JsonRequestBehavior.AllowGet);
            }


            var projectVM = new Project()
            {
                Id = project.Id,
                Description = project.Description,
                Name = project.Name,
                VisibilityType = project.VisibilityType,
                LicenceType = project.LicenceType
            };
            return new JsonResult() { Data = projectVM, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }




        #region Administration

        public ActionResult Administration(int id = 1)
        {
            var model = this.getById(id);
            if (!this.serviceSecurity.HasRight(SecureActivity.ProjectEdit, CurrentUser, model.Project))
            {
                return this.RedirectToErrorPage(Resources.Resource.COMMON_PermissionDenied);
            }

            return View(model);
        }


        public ActionResult AdminSettings(int id)
        {
            var project = this.serviceProject.GetById(id);
            if (!this.serviceSecurity.HasRight(SecureActivity.ProjectEdit, CurrentUser, project))
            {
                return this.RedirectToErrorPage(Resources.Resource.COMMON_PermissionDenied);
            }
            return PartialView("_AdminSettings", project);
        }
        public ActionResult AdminCollabs(int id)
        {
            Project p = this.serviceProject.GetById(id);
            if (!this.serviceSecurity.HasRight(SecureActivity.ProjectEdit, CurrentUser, p))
            {
                return this.RedirectToErrorPage(Resources.Resource.COMMON_PermissionDenied);
            }
            return PartialView("_AdminCollabs", id);
        }

        public JsonResult GetCollabs(int id)
        {
            VMCollabs collabs = serviceProject.GetCollabs(id);
            JsonResult result = new JsonResult() { Data = collabs, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            return result;
        }


        public ActionResult AdminFollow(int id)
        {
            Project p = this.serviceProject.GetById(id);

            if (!this.serviceSecurity.HasRight(SecureActivity.ProjectEdit, CurrentUser, p))
            {
                return this.RedirectToErrorPage(Resources.Resource.COMMON_PermissionDenied);
            }

            return PartialView("_AdminFollow", id);
        }

        public ActionResult AdminReports(int id)
        {
            Project p = this.serviceProject.GetById(id);

            if (!this.serviceSecurity.HasRight(SecureActivity.ProjectEdit, CurrentUser, p))
            {
                return this.RedirectToErrorPage(Resources.Resource.COMMON_PermissionDenied);
            }

            return PartialView("_AdminReports");
        }

        public ActionResult AdminComments(int id)
        {
            Project p = this.serviceProject.GetById(id);

            if (!this.serviceSecurity.HasRight(SecureActivity.ProjectEdit, CurrentUser, p))
            {
                return this.RedirectToErrorPage(Resources.Resource.COMMON_PermissionDenied);
            }

            return PartialView("_AdminComments",id);
        }



        public ActionResult AdminNotes(int id)
        {
            Project p = this.serviceProject.GetById(id);

            if (!this.serviceSecurity.HasRight(SecureActivity.ProjectEdit, CurrentUser, p))
            {
                return this.RedirectToErrorPage(Resources.Resource.COMMON_PermissionDenied);
            }

            return PartialView("_AdminNotes",id);
        }


        public ActionResult AdminArtwork(int id)
        {
            var project = serviceProject.GetById(id);

            if (!this.serviceSecurity.HasRight(SecureActivity.ProjectEdit, CurrentUser, project))
            {
                return this.RedirectToErrorPage(Resources.Resource.COMMON_PermissionDenied);
            }
            return PartialView("_AdminArtwork", project);
        }

        public ActionResult AdminInvits(int id)
        {
            Project p = this.serviceProject.GetById(id);

            if (!this.serviceSecurity.HasRight(SecureActivity.ProjectEdit, CurrentUser, p))
            {
                return this.RedirectToErrorPage(Resources.Resource.COMMON_PermissionDenied);
            }

            return PartialView("_AdminInvits", id);
        }

        [HttpPost]
        public JsonResult SaveProjectSettings(Project model)
        {
            var project = serviceProject.GetById(model.Id);

            if (!this.serviceSecurity.HasRight(SecureActivity.ProjectEdit, CurrentUser, project))
            {
                return new JsonResult() { Data = false };
            }
            project.Name = model.Name;
            project.Description = model.Description;
            project.VisibilityType = model.VisibilityType;
            project.LicenceType = model.LicenceType;
            Project result = serviceProject.SaveChanges(project);
            return new JsonResult() { Data = true };
        }
        #endregion Administration

    }
}