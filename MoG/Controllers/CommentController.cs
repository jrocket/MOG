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
    public class CommentController : FlabbitController
    {

         private ISecurityService serviceSecurity = null;
        private ICommentService serviceComment = null;

        public CommentController(IUserService _userService, ICommentService _commentService
            ,ISecurityService _securityService
             , ILogService logService
            )
            : base(_userService, logService)
        {
            serviceComment = _commentService;
            serviceSecurity = _securityService;
          
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

            var model = new VMComment()
            {
                Body = newComment.Body,
                CreatedOn = newComment.CreatedOn,
                Creator = newComment.Creator,
                CreatorName = newComment.CreatorName,
                DeleteUrl = Url.Action("Delete",new {id = newComment.Id}),
                Id = newComment.Id
            };



            var result = new JsonResult() { Data = model, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            return result;

        }
        public JsonResult GetComments(int id)
        {
            List<Comment> comments = serviceComment.GetByProjectId(id);
            //TODO : use automapper
            var data = comments.Select(x => new VMAdminComment()
            {
                Comment = x.Body.Replace("\n","<br />"),
                CreatedBy = x.Creator.DisplayName,
                CreatedOn = x.CreatedOn.ToString(),
                TargetName = x.File.DisplayName,
                Id = x.Id,
                Url = Url.Action("Display", "File", new { id = x.FileId }),
                ModifiedOn = (x.ModifiedOn !=  x.CreatedOn ? x.ModifiedOn.ToString() : "")
            }

            );
            JsonResult result = new JsonResult() { Data = data, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            return result;
        }

         [HttpPost]
        public JsonResult Delete(int id)
        {
           
            bool bflag = this.serviceComment.Delete(id,CurrentUser);
            JsonResult result = new JsonResult() { Data = bflag };
            return result;
        }

         [HttpPost]
         public JsonResult Edit(VMComment model)
         {
             JsonResult result = new JsonResult();
             if (model==null)
             {
                 result.Data = new {message = "No data?"};
                 return result;
             }

             Comment comment = this.serviceComment.GetById(model.Id);
             if (!this.serviceSecurity.HasRight(SecureActivity.CommentEdit, CurrentUser, comment))
             {
                 result.Data = new { message = "No, I don't want to :)" };
                 return result;
             }
             comment.Body = model.Body;
             bool bflag = this.serviceComment.Edit(comment);
             result.Data = new { data = bflag };
             return result;
         }



	}
}