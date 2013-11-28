﻿using MoG.Domain.Models;
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
            return View("Messages");

        }

        [HttpPost]
        public JsonResult Send(string to, string body, string title)
        {
            var currentUser = serviceUser.GetCurrentUser();
            IEnumerable<int> destinationIds = this.serviceMessage.GetDestinationIds(to);
            Message message = new Message() { Body = body, Title = title, DestinationIds = destinationIds };
            message = this.serviceMessage.Send(message);
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
            var currentUser = serviceUser.GetCurrentUser();
            var folder = serviceMessage.GetFolder(currentUser.Id,folderName);

            //todo  : use automapper
            List<VMMessage> vm = new List<VMMessage>();
            foreach (var message in folder)
            {
                vm.Add(new VMMessage()
                {
                    Body = message.Body,
                    Sender = message.CreatedBy.DisplayName,
                    To = currentUser.DisplayName,
                    SentOn = message.CreatedOn.ToString("dd-MMM-yyyy hh:mm"),
                    Title = message.Title,
                    Id = message.Id
                });

            }

            var result = new JsonResult() { Data = vm, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            return result;
        }

        [HttpPost]
        public JsonResult Archive(int id, string folder)
        {

            var currentUser = serviceUser.GetCurrentUser();

            Message message = serviceMessage.Archive(id, currentUser,folder);
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