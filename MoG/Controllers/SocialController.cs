using MoG.Domain.Models;
using MoG.Domain.Service;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Caching;
using MoG.Code;

namespace MoG.Controllers
{
    [MogAuthAttribut]
    public class SocialController : MogController
    {
        private ISocialService serviceSocial = null;
        private IInvitService serviceInvit = null;
        private IProjectService serviceProject = null;
        private IActivityService serviceActivity = null;
        private IUserStatisticsService serviceStatistics = null;

        private ICacheService serviceCache = null;

        public SocialController(ISocialService socialService
            , IInvitService invitService
            , IProjectService projectService
            , IActivityService activityService
            , IUserStatisticsService statService
            , IUserService userService
             , ILogService logService
            ,ICacheService cacheService
            )
            : base(userService, logService)
        {
            this.serviceSocial = socialService;
            this.serviceInvit = invitService;
            this.serviceProject = projectService;
            this.serviceActivity = activityService;
            this.serviceActivity.ServiceProject = projectService;
            this.serviceStatistics = statService;
            this.serviceCache = cacheService;
            
        }

        // GET: /Social/
        public ActionResult Friends()
        {

            return View();
        }

        public ActionResult Profile(string id)
        {
            UserProfileInfo user = null;
            int userId = -1;
            if (int.TryParse(id, out userId))
            {
                user = this.serviceUser.GetById(userId);
            }
            else
            {
                user = this.serviceUser.GetByLogin(id);
            }
            VMProfile model = mapdata(user);
            return View(model);
        }

        private VMProfile mapdata(UserProfileInfo user)
        {//todo use automapper
            VMProfile model = null;
            if (user != null)
            {
                var stats = serviceStatistics.GetStatByUserId(user.Id);
                model = new VMProfile();

                model.Id = user.Id;
                model.Stats = stats;
                model.DisplayName = user.DisplayName;
                model.Email = user.Email;
                model.Login = user.Login;
                model.PictureUrl = user.PictureUrl;
                model.CreatedOn = user.CreatedOn;

            }


            return model;
        }

        public ActionResult GetProjects(string id = "")
        {
            List<Project> model = null;
            if (!String.IsNullOrEmpty(id))
            {
                model = this.serviceProject.GetByUserLogin(-1, -1, id, true).ToList();
            }

            return PartialView("_projectPartial", model);
        }
        public ActionResult GetVcard(int id = -1)
        {
            UserProfileInfo user = null;
            if (id > 0)
            {
                user = this.serviceUser.GetById(id);
            }
            VMProfile model = mapdata(user);
            return PartialView("_vcardPartial", model);
        }
        public ActionResult GetActivity(int id = -1)
        {
            List<Activity> data = null;
            if (id > 0)
            {
                data = this.serviceActivity.GetByUserId(id, 20);
            }
            //Todo : use automapper
            List<VMActivity> model = new Mapper().MapActivities(data);

            return PartialView("_activityPartial", model);
        }


        public ActionResult GetNotificationsPartial()
        {

            var model = serviceCache.Get("catalog.products", () => getNoticiationsFromCache());
            
            //return new JsonResult() { Data = model, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            return View("_GetNotificationsPartial",model);
        }

        private List<VMActivity> getNoticiationsFromCache()
        {
            List<Activity> data = null;

            if (CurrentUser != null)
            {
                data = this.serviceActivity.GetNotificationForUserId(CurrentUser.Id, 20);
            }
            //Todo : use automapper
            List<VMActivity> model = new Mapper().MapActivities(data);
            return model;
        }

      

        public JsonResult GetMyInvits()
        {

            var invits = this.serviceInvit.GetInvits(CurrentUser.Id);
            List<VMInvit> data = new List<VMInvit>();
            foreach (var invit in invits)
            {
                VMInvit model = new VMInvit();
                model.Id = invit.Id;
                model.InviterName = invit.CreatedBy.DisplayName;
                model.ProjectName = invit.Project.Name;
                model.Date = invit.CreatedOn.ToString("dd-MMM-yyyy");
                model.Status = invit.Status.ToString();
                model.ProjectUrl = Url.Action("Detail", "Project", new { id = invit.ProjectId });
                model.InviterUrl = Url.Action("Detail", "Profil", new { id = invit.CreatedById });
                model.InvitStatus = invit.Status;
                model.InviteeName = invit.User.DisplayName;
                model.InviteeUrl = Url.Action("Detail", "Profil", new { id = invit.UserId });
                model.Message = (invit.Message != null ? invit.Message.Replace("\n", "<br/>") : null);
                data.Add(model);
            }
            return new JsonResult() { Data = data, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetInvitsByProjectId(int id)
        {
            var invits = this.serviceInvit.GetInvitsByProjectId(id,CurrentUser.Id);
            List<VMInvit> data = new List<VMInvit>();
            foreach (var invit in invits)
            {
                VMInvit model = new VMInvit();
                model.Id = invit.Id;
                model.InviterName = invit.CreatedBy.DisplayName;
                model.ProjectName = invit.Project.Name;
                model.Date = invit.CreatedOn.ToString("dd-MMM-yyyy");
                model.Status = invit.Status.ToString();
                model.ProjectUrl = Url.Action("Detail", "Project", new { id = invit.ProjectId });
                model.InviterUrl = Url.Action("Detail", "Profil", new { id = invit.CreatedById });
                model.InvitStatus = invit.Status;
                model.InviteeName = invit.User.DisplayName;
                model.InviteeUrl = Url.Action("Detail", "Profil", new { id = invit.UserId });
                model.Message = (invit.Message != null ? invit.Message.Replace("\n", "<br/>") : null);
                data.Add(model);
            }
            return new JsonResult() { Data = data, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetMyFriends()
        {
            var friends = this.serviceSocial.GetFriends(CurrentUser);
            List<VMFriend> data = new List<VMFriend>();
            foreach (var friend in friends)
            {
                VMFriend model = new VMFriend();
                model.Id = friend.Id;
                model.DisplayName = friend.DisplayName;
                model.PictureUrl = friend.PictureUrl;
                model.ProfileUrl = Url.Action("Display", "Account", new { id = friend.Id });
                model.Login = friend.Login;
                data.Add(model);

            }
            return new JsonResult() { Data = data, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        public JsonResult InvitAccept(int id)
        {
            this.serviceInvit.Accept(id);
            return this.GetMyInvits();
        }
        public JsonResult InvitReject(int id)
        {
            this.serviceInvit.Reject(id);
            return this.GetMyInvits();
        }
        public JsonResult InvitDelete(int id)
        {
            this.serviceInvit.Delete(id);
            return this.GetMyInvits();
        }

        //
        // GET: /Social/
        public ActionResult Invits()
        {
            return View();
        }

        public ActionResult AddInvit(int id)
        {
            var user = this.serviceUser.GetById(id);
            VMAddInvit model = new VMAddInvit();
            model.DisplayName = user.DisplayName;
            model.UserId = user.Id;
            model.ThumbnailUrl = user.PictureUrl;
            model.Message = string.Format(
@"Hello {0},

I'm working on a project that could use your talents.  Would you consider contributing to my project?

Thanks,
{1}

",
                user.DisplayName,
                CurrentUser.DisplayName);

            //todo : find a better way
            JsonSerializer js = JsonSerializer.Create(new JsonSerializerSettings());
            var jw = new System.IO.StringWriter();
            js.Serialize(jw, model);
            model.JSON = jw.ToString();

            return View(model);
        }

        [HttpPost]
        public JsonResult AddInvit(VMAddInvit model)
        {

            int result = this.serviceInvit.Invit(model.ProjectId, model.UserId, model.Message, CurrentUser);

            return new JsonResult() { Data = result > 0 };
        }


    }
}