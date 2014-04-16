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
    public class CommentController : MogController
    {

       
        private ICommentService serviceComment = null;

        public CommentController(IUserService _userService, ICommentService _commentService
             , ILogService logService
            )
            : base(_userService, logService)
        {
            serviceComment = _commentService;
          
        }
        
        //
        // GET: /Comment/
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Create(int fileId, string body)
        {
            Comment newComment = new Comment();
            newComment.Body = body;
            newComment.Creator = CurrentUser;
            newComment.CreatedOn = DateTime.Now;
            newComment.FileId = fileId;
            newComment = serviceComment.Create(newComment);

            var result = new JsonResult() { Data = newComment, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            return result;

        }
        public JsonResult GetComments(int id)
        {
            List<Comment> comments = serviceComment.GetByProjectId(id);
            //TODO : use automapper
            var data = comments.Select(x => new VMAdminComment()
            {
                Comment = x.Body,
                CreatedBy = x.Creator.DisplayName,
                CreatedOn = x.CreatedOn.ToString(),
                TargetName = x.File.DisplayName,
                Id = x.Id,
                Url = Url.Action("Display", "File", new { id = x.FileId })
            }

            );
            JsonResult result = new JsonResult() { Data = data, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            return result;
        }

         [HttpPost]
        public JsonResult Delete(int id)
        {
            //TODO : implement security
            bool bflag = this.serviceComment.Delete(id);
            JsonResult result = new JsonResult() { Data = bflag };
            return result;
        }

	}
}