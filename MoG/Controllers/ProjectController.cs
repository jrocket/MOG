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
    public class ProjectController : MogController
    {
        private IProjectService serviceProject = null;

        public ProjectController(IProjectService project, IUserService userService)
            : base(userService)
        {

            serviceProject = project;

        }
        //
        // GET: /Project/
        public ActionResult Index()
        {
            return View();
        }

        #region Project Lists
        public ActionResult New()
        {
            var projects = serviceProject.GetNew(10);
            VMProjectList vmodel = new VMProjectList();
            vmodel.Projects = projects;

            ViewBag.Title = "Nouveaux projets";

            return View("ProjectList", vmodel);
        }

        public ActionResult My()
        {
            var user = CurrentUser;
            var projects = this.serviceProject.GetByUserLogin(user.Login);
            //  var viewModel = MogAutomapper.Map(projects);
            VMProjectList vm = new VMProjectList();
            vm.Projects = projects;
            ViewBag.Title = "Mes Projets";

            return View("ProjectList", vm);
        }

        public ActionResult Popular()
        {
            var projects = serviceProject.GetPopular(10);
            VMProjectList vmodel = new VMProjectList();
            vmodel.Projects = projects;

            ViewBag.Title = "Populaires";

            return View("ProjectList", vmodel);
        }
        public ActionResult Random()
        {
            var projects = serviceProject.GetRandom(10);
            VMProjectList vmodel = new VMProjectList();
            vmodel.Projects = projects;

            ViewBag.Title = "Au hasard";

            return View("ProjectList", vmodel);
        }

        #endregion

        public ActionResult Detail(int id = -1)
        {
            Project project = serviceProject.GetById(id);


            if (id < 0)
            {
                // return RedirectToErrorPage("project not found");
            }


            return View(project);
        }

        public ActionResult Edit(int id = -1)
        {
            Project project = serviceProject.GetById(id);


            if (id < 0)
            {
                // return RedirectToErrorPage("project not found");
            }


            return PartialView(project);
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


        public ActionResult Activity(int id = 1)
        {
            //VMProjectActivity model = new VMProjectActivity();
            //if (id > 0)
            //{
            //    model.Project = serviceProject.GetById(id);
            //    model.Activities = serviceProject.GetProjectActivity(id);
            //}
            Project model = this.serviceProject.GetById(id);

            return View(model);
        }


        public JsonResult GetActivities(int id)
        {
            Project project = serviceProject.GetById(id);
            IList<Activity> activities = serviceProject.GetProjectActivity(id);
            //todo : use automapper
            Root data = new Root();
            data.timeline = new Timeline();
            data.timeline.headline = String.Format("Here is what happened in {0}", project.Name);
            data.timeline.type = "default";
            data.timeline.text = "<p>Intro body text goes here, some HTML is ok</p>";
            data.timeline.asset = new Asset()
            {
                credit = "Credit Name Goes Here",
                caption = "Caption text goes here"
            };
            data.timeline.date = new List<Date>();
            foreach (var activity in activities)
            {
                Date theEvent = new Date()
               {
                   StartDate = activity.When,
                   EndDate = activity.When,
                   headline = activity.Who.DisplayName

               };

                if ((activity.Type & ActivityType.Project) == ActivityType.Project)
                {
                    theEvent.tag = "Project";
                    theEvent.text = "Something happened on the project <br> this is cool!";
                }
                else if ((activity.Type & ActivityType.File) == ActivityType.File)
                {
                    theEvent.tag = "File";
                    theEvent.text = "Someone did something on a file";
                }


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
            var project = serviceProject.GetById(id);
            model.FilteredFiles = serviceProject.GetFilteredFiles(project, filterByAuthor, filterByStatus, filterByType);
            model.Project = project;
            model.Statuses = serviceProject.GetFileStatuses(project);
            model.Authors = serviceProject.GetFileAuthors(project);
            model.Types = serviceProject.GetFileTags(project);
            model.filterByAuthor = filterByAuthor;
            model.filterByStatus = filterByStatus;
            model.filterByType = filterByType;

            return View(model);
        }

        public ActionResult Administration(int id = 1)
        {
            var project = new Project() { Id = id };
            return View(project);
        }

        public JsonResult JSON(int id)
        {
            var project = serviceProject.GetById(id);
            var projectVM = new Project() { Id = project.Id, Description = project.Description, Name = project.Name };
            return new JsonResult() { Data = projectVM, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public JsonResult SaveProjectSettings(Project model)
        {
            var project = serviceProject.GetById(model.Id);
            project.Name = model.Name;
            project.Description = model.Description;
            Project result = serviceProject.SaveChanges(project);
            return new JsonResult() { Data = true };
        }

        public ActionResult AdminSettings(int id)
        {
            var project = serviceProject.GetById(id);
            return PartialView("_AdminSettings",project);
        }
        public ActionResult AdminCollabs(int id)
        {
            VMCollabs collabs = serviceProject.GetCollabs(id);
            return PartialView("_AdminCollabs", collabs);
        }




        public ActionResult AdminTracking(int id)
        {

            return PartialView("_AdminTracking");
        }

        public ActionResult AdminReports(int id)
        {

            return PartialView("_AdminReports");
        }

        public ActionResult AdminComments(int id)
        {

            return PartialView("_AdminComments");
        }



        public ActionResult AdminNotes(int id)
        {

            return PartialView("_AdminNotes");
        }

        public ActionResult AdminArtwork(int id)
        {

            return PartialView("_AdminArtwork");
        }

        public ActionResult AdminInvits(int id)
        {

            return PartialView("_AdminInvits");
        }

        private VMProjectList getDummyProjects()
        {
            var result = new VMProjectList();
            List<Project> projects = new List<Project>();
            for (int i = 0; i < 20; i++)
            {
                projects.Add(new Project()
                {
                    Id = i,
                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam viverra euismod odio, gravida pellentesque urna varius vitae.",
                    ImageUrl = "http://placehold.it/700x400",
                    Name = "project name " + i
                });
            }
            result.Projects = projects;
            return result;

        }

    }
}