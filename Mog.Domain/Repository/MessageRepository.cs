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


        public IList<Message> GetInbox(int userId)
        {
            string sql = @"
SELECT * FROM messages 
LEFT JOIN  MessageDestinations on MessageDestinations.MessageId = messages.id
WHERE messageDestinations.UserId = @UserId ORDER BY CreatedOn DESC
";
            return dbContext.Messages.SqlQuery(sql,
                new SqlParameter("@UserId", userId)
                ).ToList<Message>();
        }


        public bool Create(Message newMessage)
        {
            dbContext.Messages.Add(newMessage);
            int result = dbContext.SaveChanges();
            return (result > 0);
        }


        public IQueryable<Message> GetSent(int userId)
        {
            return dbContext.Messages
                .Where(m => !m.Deleted)
                .Where(m => m.CreatedBy.Id == userId)
                .Where(m => !m.Archived )
                .OrderByDescending(m => m.CreatedOn);
        }


        public bool Send(Models.Message newMessage)
        {
            bool result = true;
            foreach (int destinationId in newMessage.DestinationIds)
            {
                MessageDestination md = new MessageDestination() { MessageId = newMessage.Id, UserId = destinationId };
                md = dbContext.MessagesDestinations.Add(md);

            }
            dbContext.SaveChanges();
            return result;
        }


        public Message GetById(int id)
        {
            return this.dbContext.Messages.Find(id);
        }


       




        public Message ArchiveInbox(Message message, int userId)
        {
           
            var md = dbContext.MessagesDestinations.Where(x => x.MessageId == message.Id && x.UserId == userId).FirstOrDefault();
            if (md != null)
            {
                dbContext.MessagesDestinations.Remove(md);
            }

            dbContext.SaveChanges();

            return message;
        }


        public Message ArchiveSent(Message message)
        {
            message.Archived = true;
            dbContext.SaveChanges();
            return message;
        }


    }

    public interface IMessageRepository
    {

        IList<Models.Message> GetInbox(int userId);

        IQueryable<Models.Message> GetSent(int userId);

        bool Create(Models.Message newMessage);

        bool Send(Models.Message newMessage);

        Message GetById(int id);



        Message ArchiveSent(Message message);

        Message ArchiveInbox(Message message, int p);
    }
}
