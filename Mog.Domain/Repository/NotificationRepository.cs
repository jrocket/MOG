using MoG.Domain.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoG.Domain.Repository
{
    public class NotificationRepository: BaseRepository, INotificationRepository
    {

        public NotificationRepository(IdbContextProvider provider)
            : base(provider)
        {

        }


        public IQueryable<Notification> GetUnRead(int userId)
        {
            return this.dbContext.Notifications
             .Where(n => n.UserId == userId && n.IsRead == false);
        }

        public int Create(List<Notification> notifications)
        {
            int result = 0;
            this.dbContext.Configuration.AutoDetectChangesEnabled = false;
            foreach (var notif in notifications)
            {
                this.Create(notif);
                result++;
            }
            return result;
           
        }

        public int Create(Notification notification)
        {
            notification.CreatedOn = DateTime.Now;
            notification.IsRead = false;
            this.dbContext.Notifications.Add(notification);
            this.dbContext.SaveChanges();
            return notification.Id;
        }

        public bool SaveChanges(Notification notification)
        {
            this.dbContext.Entry(notification).State = System.Data.Entity.EntityState.Modified;
            int result = this.dbContext.SaveChanges();
            return result > 0;
        }

        public bool Delete(Notification notification)
        {
            if (notification != null)
            {
                this.dbContext.Notifications.Remove(notification);
                this.dbContext.SaveChanges();
                return true;
            }
            return false;
        }

        public bool MarkAsRead(int userId, DateTime before)
        {
            SqlParameter user = new SqlParameter("@userId", userId);
            SqlParameter date = new SqlParameter("@before", before);

            dbContext.Database.ExecuteSqlCommand(
              @"UPDATE Notifications SET IsRead = 1 WHERE UserId = @userId and IsRead = 0 and CreatedOn < @before"
              , user, date);
            return true;
        }


        public int GetCount()
        {
            return this.dbContext.Notifications.Count();
        }


        public IQueryable<Notification> GetByUserId(int userId)
        {
            return this.dbContext.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedOn);
        }


        public IQueryable<Notification> GetUnSent()
        {
            return this.dbContext.Notifications
                .Where(n => n.IsSent == false);
        }


        public void SetSent(IEnumerable<int> ids)
        {
            
            int count = 0;
            List<SqlParameter> sqlparams = new  List<SqlParameter>();
            foreach (int id in ids)
            {

                sqlparams.Add(new SqlParameter("@param" + count, id));
                count++;
            }
            string paramNames = string.Join(", ", Enumerable.Range(0, count).Select(e => "@param" + e));

            if (count>0)
            {
                dbContext.Database.ExecuteSqlCommand(
                  @"UPDATE Notifications SET IsSent = 1 WHERE Id IN (" + paramNames + ")"
                  , sqlparams.ToArray());
            }
        }
    }
    public interface INotificationRepository
    {
        IQueryable<Notification> GetUnRead(int userId);

        int Create(Notification notification);

        bool SaveChanges(Notification notification);

        bool Delete(Notification note);

        bool MarkAsRead(int userId, DateTime before);

        int GetCount();

        int Create(List<Notification> notifications);

        IQueryable<Notification> GetByUserId(int userId);

        IQueryable<Notification> GetUnSent();

        void SetSent(IEnumerable<int> enumerable);
    }
}
