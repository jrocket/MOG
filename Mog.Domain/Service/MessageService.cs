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
        public IList<Models.Message> GetInbox(int userId)
        {
            var messages =  repositoryMessage.GetInbox(userId);

            return messages;
        }

        public IQueryable<Models.Message> GetSent(int userId)
        {
            return repositoryMessage.GetSent(userId);
        }

        public Message Send(Message newMessage)
        {
            bool result = true;
            newMessage.CreatedBy = serviceUser.GetCurrentUser();
            newMessage.CreatedOn = DateTime.Now;
            result &= repositoryMessage.Create(newMessage);

            result &= repositoryMessage.Send(newMessage);
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



        public IEnumerable<Message> GetFolder(int userId, string folderName)
        {
            IEnumerable<Message> result = null;
            switch (folderName.ToLower())
            {
                case "inbox" : 
                    result = GetInbox(userId);
                    break;
                case "outbox" :
                    result = GetSent(userId);
                    break;
            }
            return result;
        }


        public Message GetById(int id)
        {
            return repositoryMessage.GetById(id);
        }



        public Message Archive(int id, UserProfile currentUser,string folder)
        {
            var message = GetById(id);
            Message result = null;
            switch (folder.ToLower())
            {
                case "inbox":
                    result = repositoryMessage.ArchiveInbox(message,currentUser.Id);
                    break;
                case "outbox":
                    result = repositoryMessage.ArchiveSent(message);
                    break;

            }


            return result;
        }
    }

    public interface IMessageService
    {


        IList<Models.Message> GetInbox(int userId);

        IQueryable<Models.Message> GetSent(int userId);

        Message Send(Models.Message newMessage);

        IEnumerable<int> GetDestinationIds(string to);

        IEnumerable<Message> GetFolder(int userId, string folderName);

        Message GetById(int id);

        Message Archive(int id, UserProfile currentUser, string folder);

      
    }
}
