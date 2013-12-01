using MoG.Domain.Models;
using MoG.Domain.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MoG.Controllers
{
    public class CommentController : MogController
    {

       
        private ICommentService serviceComment = null;

        public CommentController(IUserService _userService, ICommentService _commentService)
            : base(_userService)
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
	}
}