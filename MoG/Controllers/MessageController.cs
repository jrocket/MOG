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
            message = this.serviceMessage.Send(message, destinationIds, replyTo);
            //todo  : use automapper
            VMMessage vm = new VMMessage(message);
           

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
            string[] result = { "jrocket", "mvegas" };
            return new JsonResult() { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

      
    }
}