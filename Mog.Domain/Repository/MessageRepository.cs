using MoG.Domain.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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


     

        public bool Send(Message newMessage, IEnumerable<UserProfile> destinations, int? replyTo)
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

            if (replyTo.HasValue)
            {// reply to a message, set the repliedOn
                var replyedMsg = this.GetBox(newMessage.CreatedBy.Id,BoxType.Inbox).Where(msg => msg.MessageId == replyTo.Value).FirstOrDefault();
                if (replyedMsg != null)
                {
                    replyedMsg.ReplyedOn = DateTime.Now;
                    dbContext.Entry(replyedMsg).State = EntityState.Modified;
                }
            }



            dbContext.SaveChanges();
            return result;
        }


        public Message GetById(int id)
        {
            return this.dbContext.Messages.Find(id);
        }
        public  Message Archive(int  messageBoxId, int userId)
        {//ToDo : Message -> archived By 
            //Todo : Message -> deleted By

            var md = dbContext.MessageBoxes.Where(x => x.Id == messageBoxId ).FirstOrDefault();

           


            if (md != null)
            {
                if (!md.Archived)
                {
                    md.Archived = true;
                }    
                else
                {
                    md.Deleted = true;
                }

            }
            dbContext.SaveChanges();
            
            return md.Message;
        }








        public MessageBox GetByBoxId(int boxId)
        {
            return this.dbContext.MessageBoxes.Find(boxId);
        }
    }

    public interface IMessageRepository
    {

        IOrderedQueryable<MessageBox> GetBox(int userId,BoxType typeOfBox);

        IOrderedQueryable<MessageBox> GetArchived(int userId);
     

        bool Create(Models.Message newMessage);


        bool Send(Message newMessage, IEnumerable<UserProfile> destinations, int? replyTo);

        Message GetById(int id);



        Message Archive(int  messageBoxId, int userId);

        //Message ArchiveInbox(Message message, int p);





        MessageBox GetByBoxId(int boxId);
    }
}
