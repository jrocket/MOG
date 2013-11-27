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
        private IMessageRepository repositoryMessage= null;

        public MessageService(IUserService _userService, IMessageRepository _messageRepo)
        {
            serviceUser = _userService;
            repositoryMessage = _messageRepo;
        }
        public IList<Models.Message> GetInbox(int userId)
        {
            return repositoryMessage.GetInbox(userId);
        }

        public IQueryable<Models.Message> GetSent(int userId)
        {
            return repositoryMessage.GetSent(userId);
        }

        public bool Send(Models.Message newMessage)
        {
            bool result = true;
            newMessage.CreatedBy = serviceUser.GetCurrentUser();
            newMessage.CreatedOn = DateTime.Now;
            result &= repositoryMessage.Create(newMessage);

            result &= repositoryMessage.Send(newMessage);
            return result;
        }



        public IEnumerable<int> GetDestinationIds(string to)
        {
            List<int> result = new List<int>();
            string[] destinationsLogins = to.Split(';');
            foreach(var login in destinationsLogins)
            {
                UserProfile user = this.serviceUser.GetByLogin(login);
                if (user != null)
                    result.Add(user.Id);
            }
            return result;

        }
    }

    public interface IMessageService
    {


        IList<Models.Message> GetInbox(int userId);

        IQueryable<Models.Message> GetSent(int userId);

        bool Send(Models.Message newMessage);

        IEnumerable<int> GetDestinationIds(string to);
    }
}
