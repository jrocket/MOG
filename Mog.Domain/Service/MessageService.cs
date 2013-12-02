using MoG.Domain.Models;
using MoG.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;

namespace MoG.Domain.Service
{
    public class MessageService : IMessageService
    {
        private IUserService serviceUser = null;
        private IMessageRepository repositoryMessage = null;

        public MessageService(IUserService _userService, IMessageRepository _messageRepo)
        {
            serviceUser = _userService;
            repositoryMessage = _messageRepo;
            System.Threading.Thread.Sleep(1000);
        }
        public List<MessageBox> GetBox(int userId, BoxType typeofBox)
        {
            var messages = repositoryMessage.GetBox(userId, typeofBox);

            return messages.ToList<MessageBox>();
        }

        private List<MessageBox> GetBox(int userId, bool archived)
        {
            var messages = repositoryMessage.GetArchived(userId);

            return messages.ToList<MessageBox>();
        }

        //public List<Inbox> GetInbox(int userId)
        //{
        //    var messages = repositoryMessage.GetInbox(userId);

        //    return messages.ToList<Inbox>();
        //}

        //public List<Outbox> GetOutbox(int userId)
        //{
        //    var messages = repositoryMessage.GetOutbox(userId);
        //    return messages.ToList<Outbox>();
        //}

        public Message Send(Message newMessage, IEnumerable<int> destinationIds)
        {
            bool result = true;
            newMessage.CreatedBy = serviceUser.GetCurrentUser();
            newMessage.CreatedOn = DateTime.Now;
            result &= repositoryMessage.Create(newMessage);

            IEnumerable<UserProfile> destinations = serviceUser.GetByIds(destinationIds);


            result &= repositoryMessage.Send(newMessage, destinations);
            return newMessage;
        }



        public IEnumerable<int> GetDestinationIds(string to)
        {
            List<int> result = new List<int>();
            string[] destinationsLogins = to.Split(';');
            foreach (var login in destinationsLogins)
            {
                UserProfile user = this.serviceUser.GetByLogin(login);
                if (user != null)
                    result.Add(user.Id);
            }
            return result;

        }



        public IEnumerable<VMMessage> GetFolder(int userId, string folderName)
        {
            List<VMMessage> result = new List<VMMessage>();
            List<MessageBox> messages = null;
            switch (folderName.ToLower())
            {
                case "inbox":
                    messages = GetBox(userId,BoxType.Inbox).ToList();
                    break;
                case "outbox":
                    messages = GetBox(userId, BoxType.Outbox).ToList();
                    break;
                case "archive":
                    messages = GetBox(userId,true).ToList();
                    break;
            }

            foreach (MessageBox boxMessage in messages)
            {
                VMMessage msg = new VMMessage(boxMessage);
               
                result.Add(msg);
            }

            return result;
        }




        public Message GetById(int id)
        {
            return repositoryMessage.GetById(id);
        }



        public Message Archive(int id, UserProfile currentUser, string folder)
        {
            var message = GetById(id);
            Message result = null;
            switch (folder.ToLower())
            {
                case "inbox":
                    result = repositoryMessage.Archive(message, currentUser.Id, BoxType.Inbox);
                    break;
                case "outbox":
                    result = repositoryMessage.Archive(message, currentUser.Id, BoxType.Outbox);
                    break;

            }


            return result;
        }
    }

    public interface IMessageService
    {


        //IList<Outbox> GetInbox(int userId);

        //IList<Outbox> GetOutbox(int userId);

        Message Send(Models.Message newMessage, IEnumerable<int> destinationIds);

        IEnumerable<int> GetDestinationIds(string to);

        IEnumerable<VMMessage> GetFolder(int userId, string folderName);

        Message GetById(int id);

        Message Archive(int id, UserProfile currentUser, string folder);


    }
}
