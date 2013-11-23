using MoG.Domain.Models;
using MoG.Domain.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MoG.Controllers
{
    public class CommentController : Controller
    {

        private IUserService serviceUser = null;
        private ICommentService serviceComment = null;
        
        public CommentController(IUserService _userService, ICommentService _commentService)
        {
            serviceComment = _commentService;
            serviceUser = _userService;
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
            newComment.Creator = serviceUser.GetCurrentUser();
            newComment.CreatedOn = DateTime.Now;
            newComment.FileId = fileId;
            newComment = serviceComment.Create(newComment);

            var result = new JsonResult() { Data = newComment, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            return result;

        }
	}
}