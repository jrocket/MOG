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

       


        public Message Send(Message newMessage, UserProfileInfo from, IEnumerable<int> destinationIds, int? replyTo = null)
        {
            bool result = true;
            IEnumerable<UserProfileInfo> destinations = serviceUser.GetByIds(destinationIds);

            newMessage.CreatedBy = from;
            newMessage.CreatedOn = DateTime.Now;
            newMessage.SentTo = destinations.Select(u => u.DisplayName).Aggregate((current, next) => current + ", " + next);


            result &= repositoryMessage.Create(newMessage);

            result &= repositoryMessage.Send(newMessage, destinations,replyTo);
            return newMessage;
        }



        public IEnumerable<int> GetDestinationIds(string to)
        {
            List<int> result = new List<int>();
            string[] destinationsLogins = to.Split(';');
            foreach (var login in destinationsLogins)
            {
                UserProfileInfo user = this.serviceUser.GetByLogin(login);
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



        public Message Archive(int id, UserProfileInfo currentUser)
        {
            //var message = GetById(id);
            Message result = null;

            result = repositoryMessage.Archive(id, currentUser.Id);
              


            return result;
        }





        public VMMessage GetByBoxId(int boxId)
        {
            var boxMessage = repositoryMessage.GetByBoxId(boxId);
            VMMessage result = new VMMessage(boxMessage,true);
            return result;
        }
    }

    public interface IMessageService
    {


        //IList<Outbox> GetInbox(int userId);

        //IList<Outbox> GetOutbox(int userId);


        Message Send(Message message,UserProfileInfo from, IEnumerable<int> destinationIds, int?  replyTo);

        IEnumerable<int> GetDestinationIds(string to);

        IEnumerable<VMMessage> GetFolder(int userId, string folderName);

        Message GetById(int id);

        Message Archive(int id, UserProfileInfo currentUser);





        VMMessage GetByBoxId(int boxId);
    }
}
