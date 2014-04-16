using MoG.Domain.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MoG.Controllers
{
    [MogAuthAttribut]
    public class FollowController : MogController
    {
        private IFollowService serviceFollow = null;

        public FollowController(IFollowService followService, IUserService userService
            , ILogService logService
            )
            : base(userService, logService)
        {
            this.serviceFollow = followService;
        }
        
        //
        // GET: /Follow/
        public JsonResult GetFollowers(int id)
        {
            JsonResult result = new JsonResult();
            result.Data = this.serviceFollow.GetFollowsByProject(id);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        public JsonResult GetFollowed(int id = -1)
        {
            JsonResult result = new JsonResult();
            if (id < 0)
            {
                id = CurrentUser.Id;
            }
            var followedItems= this.serviceFollow.GetFollowsByUser(id);
            foreach (var followedItem in followedItems)
            {
                followedItem.Project.ImageUrlThumb1 = Url.Content(followedItem.Project.ImageUrlThumb1);
            }
            result.Data = followedItems;
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        public JsonResult Follow (int id)
        {
            JsonResult result = new JsonResult();
            result.Data = this.serviceFollow.Follow(id,CurrentUser.Id);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        public JsonResult UnFollow(int id)
        {
            JsonResult result = new JsonResult();
            var follow = this.serviceFollow.Get(id, CurrentUser.Id);
            if (follow!=null)
            {
                result.Data = this.serviceFollow.Delete(follow.Id);
            }
            else
            {
                result.Data = false;
            }
            
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }
	}
}