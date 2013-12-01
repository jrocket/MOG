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
      
        private IMessageService serviceMessage;

        public MessageController(IMessageService _messageService, IUserService userService)
            : base(userService)
        {
          
            serviceMessage = _messageService;

        }
        public ActionResult Index()
        {
            return View("Messages");

        }

        [HttpPost]
        public JsonResult Send(string to, string body, string title)
        {
          
            IEnumerable<int> destinationIds = this.serviceMessage.GetDestinationIds(to);
            Message message = new Message() { Body = body, Title = title };
            message = this.serviceMessage.Send(message,destinationIds);
            //todo  : use automapper
            VMMessage vm = new VMMessage()
            {
                Body = message.Body,
                Sender = message.CreatedBy.DisplayName,
                SentOn = message.CreatedOn.ToString("dd-MMM-yyyy hh:mm"),
                Title = message.Title,
                Id = message.Id
            };
            var result = new JsonResult() { Data = vm, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            return result;
        }
        public JsonResult GetFolder(string folderName)
        {
         
            var folder = serviceMessage.GetFolder(CurrentUser.Id,folderName);

         

            var result = new JsonResult() { Data = folder, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            return result;
        }

        [HttpPost]
        public JsonResult Archive(int id, string folder)
        {

          

            Message message = serviceMessage.Archive(id, CurrentUser,folder);
            var data = new VMMessage()
                {
                    Body = message.Body,
                    Sender = message.CreatedBy.DisplayName,
                    SentOn = message.CreatedOn.ToString("dd-MMM-yyyy hh:mm"),
                    Title = message.Title,
                    Id = message.Id
                };
            var result = new JsonResult() { Data = data };
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