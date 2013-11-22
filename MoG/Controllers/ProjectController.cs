﻿using MoG.Domain.Models;
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
        private IUserService userService = null;
        public ProjectController(IProjectService project, IUserService user)
        {

            serviceProject = project;
            userService = user;
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
            var user = userService.GetCurrentUser();
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
                int newId = serviceProject.Create(project, userService.GetCurrentUser());


                return RedirectToAction("Detail", new { id = newId });
            }

            return View(project);
        }


        public ActionResult Activity(int id = 1)
        {
            VMProjectActivity model = new VMProjectActivity();
            if (id > 0)
            {
                model.Project = serviceProject.GetById(id);
                model.Activities = serviceProject.GetProjectActivity(id);
            }


            return View(model);
        }

        public ActionResult Files(int id = 1, string filterByType = "", string filterByStatus = "",string  filterByAuthor= "")
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
            model.Types = serviceProject.GetFileTypes(project);
            model.filterByAuthor = filterByAuthor;
            model.filterByStatus = filterByStatus;
            model.filterByType = filterByType;

            return View(model);
        }

        public ActionResult Administration(int id = 1)
        {
            var project = serviceProject.GetById(id);
            return View(project);
        }



        public ActionResult AdminSettings(int id)
        {

            return PartialView("_AdminSettings");
        }
        public ActionResult AdminCollabs(int id)
        {

            return PartialView("_AdminCollabs");
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

        public ActionResult AdminTags(int id)
        {

            return PartialView("_AdminTags");
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