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


        public IOrderedQueryable<MessageBox> GetBox(int userId,BoxType typeOfBox)
        {

            return dbContext.MessageBoxes
                .Where(box => box.UserId == userId)
                .Where(box => !box.Archived )
                .Where(box => !box.Deleted)
                .Where(box => box.BoxType == typeOfBox)
                .OrderByDescending(box => box.Message.CreatedOn);
        }


        public IOrderedQueryable<MessageBox> GetArchived(int userId)
        {
            return dbContext.MessageBoxes
                .Where(box => box.UserId == userId)
                .Where(box => box.Archived)
                .Where(box => !box.Deleted)
                .OrderByDescending(box => box.Message.CreatedOn);
        }

        public bool Create(Message newMessage)
        {
            dbContext.Messages.Add(newMessage);
            int result = dbContext.SaveChanges();
            return (result > 0);
        }


        //public IOrderedQueryable<Outbox> GetOutbox(int userId)
        //{
        //    return dbContext.Outbox
        //        .Where(m => !m.Deleted)
        //        .Where(m => m.UserId == userId)
        //        .Where(m => !m.Archived)
        //        .OrderByDescending(m => m.Message.CreatedOn);
        //}


        public bool Send(Message newMessage, IEnumerable<UserProfile> destinations)
        {
            bool result = true;
            string Tos = destinations.Select(u => u.DisplayName).Aggregate((current, next) => current + ", " + next);
            foreach (var sendTo in destinations)
            {
                MessageBox received = new MessageBox() 
                { 
                    MessageId = newMessage.Id, 
                    UserId = sendTo.Id,
                    BoxType = BoxType.Inbox,
                    From = newMessage.CreatedBy.DisplayName,
                    To = Tos
                };
                dbContext.MessageBoxes.Add(received);

            }
            MessageBox sent = new MessageBox() 
            { 
                MessageId = newMessage.Id, 
                UserId = newMessage.CreatedBy.Id,
                BoxType= BoxType.Outbox,
                From = newMessage.CreatedBy.DisplayName,
                    To = Tos
            };
            dbContext.MessageBoxes.Add(sent);

            dbContext.SaveChanges();
            return result;
        }


        public Message GetById(int id)
        {
            return this.dbContext.Messages.Find(id);
        }
        public  Message Archive(Message message, int userId, BoxType typeOfBox)
        {
           
            var md = dbContext.MessageBoxes.Where(x => x.MessageId == message.Id && x.UserId == userId && x.BoxType == typeOfBox).FirstOrDefault();

            if (md != null)
            {
                md.Archived = true;
            }
            dbContext.SaveChanges();
            
            return md.Message;
        }
    

       



    }

    public interface IMessageRepository
    {

        IOrderedQueryable<MessageBox> GetBox(int userId,BoxType typeOfBox);

        IOrderedQueryable<MessageBox> GetArchived(int userId);
     

        bool Create(Models.Message newMessage);

       
        bool Send(Message newMessage, IEnumerable<UserProfile> destinations);

        Message GetById(int id);



        Message Archive(Message message, int userId, BoxType typeOfBox);

        //Message ArchiveInbox(Message message, int p);



       
    }
}
