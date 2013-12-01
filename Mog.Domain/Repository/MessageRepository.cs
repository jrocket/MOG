using MoG.Domain.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoG.Domain.Repository
{
    public class MessageRepository : BaseRepository, IMessageRepository
    {


        public MessageRepository(IdbContextProvider provider)
            : base(provider)
        {

        }


        public IOrderedQueryable<Inbox> GetInbox(int userId)
        {
           
            return dbContext.Inbox
                .Where(box => box.UserId == userId)
                .Where(box => !box.Archived )
                .Where(box => !box.Deleted)
                .OrderByDescending(box => box.Message.CreatedOn);
        }


        public bool Create(Message newMessage)
        {
            dbContext.Messages.Add(newMessage);
            int result = dbContext.SaveChanges();
            return (result > 0);
        }


        public IOrderedQueryable<Outbox> GetOutbox(int userId)
        {
            return dbContext.Outbox
                .Where(m => !m.Deleted)
                .Where(m => m.UserId == userId)
                .Where(m => !m.Archived)
                .OrderByDescending(m => m.Message.CreatedOn);
        }


        public bool Send(Message newMessage, IEnumerable<UserProfile> destinations)
        {
            bool result = true;
            string Tos = destinations.Select(u => u.DisplayName).Aggregate((current, next) => current + ", " + next);
            foreach (var sendTo in destinations)
            {
                Inbox received = new Inbox() 
                { 
                    MessageId = newMessage.Id, 
                    UserId = sendTo.Id,
                    From = newMessage.CreatedBy.DisplayName,
                    To = Tos
                };
                dbContext.Inbox.Add(received);

            }
            Outbox sent = new Outbox() 
            { 
                MessageId = newMessage.Id, 
                UserId = newMessage.CreatedBy.Id,
                From = newMessage.CreatedBy.DisplayName,
                    To = Tos
            };
            dbContext.Outbox.Add(sent);

            dbContext.SaveChanges();
            return result;
        }


        public Message GetById(int id)
        {
            return this.dbContext.Messages.Find(id);
        }
        private bool archiveMessage(MessageBox messageInBox)
        {
            bool result = false;
            if (messageInBox != null)
            {
                messageInBox.Archived = true;
            }
            dbContext.SaveChanges();
            result = true;
            return result;
        }

        public Message ArchiveInbox(Message message, int userId)
        {

            var md = dbContext.Inbox.Where(x => x.MessageId == message.Id && x.UserId == userId).FirstOrDefault();

            bool flag = archiveMessage(md);

            return message;
        }


        public Message ArchiveOutBox(Message message, int userId)
        {
            var md = dbContext.Outbox.Where(x => x.MessageId == message.Id && x.UserId == userId).FirstOrDefault();
            bool flag = archiveMessage(md);
            return message;
        }




    }

    public interface IMessageRepository
    {

        IOrderedQueryable<Inbox> GetInbox(int userId);

        IOrderedQueryable<Outbox> GetOutbox(int userId);

        bool Create(Models.Message newMessage);

       
        bool Send(Message newMessage, IEnumerable<UserProfile> destinations);

        Message GetById(int id);



        Message ArchiveOutBox(Message message, int userId);

        Message ArchiveInbox(Message message, int p);

      
    }
}
