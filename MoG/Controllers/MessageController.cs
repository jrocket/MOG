using MoG.Domain.Models;
using MoG.Domain.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MoG.Controllers
{
    public class MessageController : MogController
    {
        private IUserService serviceUser;
        private IMessageService serviceMessage;

        public MessageController(IUserService _userService, IMessageService _messageService)
        {
            serviceUser = _userService;
            serviceMessage = _messageService;

        }
        public ActionResult Index()
        {
            return View("Messages") ;

        }


        public JsonResult GetInbox()
        {
            var currentUser = serviceUser.GetCurrentUser();
            var inbox = serviceMessage.GetInbox(currentUser.Id);
            //todo  : use automapper
            List<VMMessage> vm = new List<VMMessage>();
            foreach (var message in inbox)
            {
                vm.Add(new VMMessage()
                {
                    Body = message.Body,
                    Sender = message.CreatedBy.DisplayName,
                    SentOn = message.CreatedOn.ToString("dd-MMM-yyyy hh:mm"),
                    Title = message.Title
                });

            }

            var result = new JsonResult() { Data = vm, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            return result;

        }

        //
        // GET: /Message/
        public ActionResult Received()
        {
            return View();
        }

        public ActionResult Sent()
        {
            return View();

        }

        public ActionResult Chat()
        {
            return View();
        }

        public ActionResult Mention()
        {
            return View();
        }

        public ActionResult Search()
        {
            return View();
        }
	}
}