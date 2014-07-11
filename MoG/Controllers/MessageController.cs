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
    public class MessageController : FlabbitController
    {
      
        private IMessageService serviceMessage;
        private ISocialService serviceSocial;

        public MessageController(IMessageService _messageService,
            IUserService userService,
            ISocialService socialService
             , ILogService logService
            )
            : base(userService, logService)
        {
          
            serviceMessage = _messageService;
            this.serviceSocial = socialService;
        }
        public ActionResult Index()
        {
            return View();

        }

        public JsonResult Detail(int id)
        {
            VMMessage msg = this.serviceMessage.GetByBoxId(id);
           //VMMessage vm = new VMMessage(msg, true);
            var result = new JsonResult() { Data = msg, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
           return result;
        }

        [HttpPost]
        public JsonResult Send(string to, string body, string title, int?  replyTo)
        {
          
            IEnumerable<int> destinationIds = this.serviceMessage.GetDestinationIds(to);
            Message message = new Message() { Body = body, Title = title };
            message = this.serviceMessage.Send(message,CurrentUser, destinationIds, replyTo);
            //todo  : use automapper
            VMMessage vm = new VMMessage(message);
           

            var result = new JsonResult() { Data = vm, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            return result;
        }
        public JsonResult GetFolder(string folderName)
        {

            var folder = serviceMessage.GetFolder(CurrentUser.Id, folderName);

         

            var result = new JsonResult() { Data = folder, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            return result;
        }

        [HttpPost]
        public JsonResult Archive(int id)
        {

          

            Message message = serviceMessage.Archive(id, CurrentUser);
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

        public JsonResult GetFriends(int id =-1)
        {
            var friends = this.serviceSocial.GetFriends(CurrentUser);
            string[] result = friends.Select(f => f.Login).ToArray();
            //string[] result = { "jrocket", "mvegas" };
            return new JsonResult() { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

      
    }
}