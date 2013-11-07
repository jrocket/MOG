using MogMockup.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MogMockup.Controllers
{
    public class ProjectController : Controller
    {
        //
        // GET: /Project/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult New()
        {
            var projects = getDummyProjects();

            ViewBag.Title = "Nouveaux projets";

            return View("ProjectList", projects);
        }

        public ActionResult My()
        {

            var projects = getDummyProjects();

            ViewBag.Title = "Mes Projets";

            return View("ProjectList",projects);
        }

     

        public ActionResult Detail(int id = 1)
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();

        }

        public ActionResult Activity(int id = 1)
        {
            return View();
        }

        public ActionResult Tracks(int id = 1)
        {
            return View();
        }

        public ActionResult Administration(int id = 1)
        {
            return View();
        }

        public ActionResult Popular()
        {
            var projects = getDummyProjects();

            ViewBag.Title = "Projets Populaires";

            return View("ProjectList", projects);
        }
        public ActionResult Random()
        {
            var projects = getDummyProjects();

            ViewBag.Title = "Au hasard";

            return View("ProjectList", projects);
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

        private List<Project> getDummyProjects()
        {
            var result = new List<Project>();
            for (int i = 0; i < 20; i++)
            {
                result.Add(new Project()
                {
                    Id = i,
                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam viverra euismod odio, gravida pellentesque urna varius vitae.",
                    ImageUrl = "http://placehold.it/700x400",
                    Name = "project name " + i
                });
            }
            return result;

        }

    }
}