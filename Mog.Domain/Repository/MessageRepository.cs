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
WHERE messageDestinations.UserId = @UserId
";
            return dbContext.Messages.SqlQuery(sql,
                new SqlParameter("@UserId",userId)
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
                .Where(m => m.CreatedBy.Id == userId);
        }


        public bool Send(Models.Message newMessage)
        {
            bool result = true;
            foreach (int destinationId in newMessage.DestinationIds)
            {
                MessageDestination md = new MessageDestination() { MessageId = newMessage.Id, UserId = destinationId };
                md = dbContext.MessagesDestinations.Add(md);
            }
            return result;
        }
    }

    public interface IMessageRepository
    {

        IList<Models.Message> GetInbox(int userId);

        IQueryable<Models.Message> GetSent(int userId);

        bool Create(Models.Message newMessage);

        bool Send(Models.Message newMessage);
    }
}
