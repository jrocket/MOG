using MoG.Domain.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MoG.Controllers
{
    [MogAuthAttribut]
    public class LikeController : FlabbitController
    {
        public ILikeService serviceLike { get; set; }

        public LikeController(ILikeService likeService,
            IUserService userService
             , ILogService logService
            )
            : base(userService, logService)
        {
            this.serviceLike = likeService;

        }

        public JsonResult ILike(int id)
        {
            int liked = -1;
            JsonResult result = new JsonResult();
            bool bFlag = this.serviceLike.LikeIt(id, CurrentUser.Id);
            if (bFlag)
            {
                liked = this.serviceLike.GetLikeCount(id);
            }
            result.Data = new { result = bFlag , likedCount = liked};
            
            return result;

        }

    }
}