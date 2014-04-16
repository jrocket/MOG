using MoG.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.SqlClient;

namespace MoG.Domain.Repository
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(IdbContextProvider provider)
            : base(provider)
        {

        }

        public bool Create(UserProfileInfo usr)
        {


            dbContext.Users.Add(usr);
            int result = dbContext.SaveChanges();
            return (result > 0);
        }





        public IQueryable<UserProfileInfo> GetAll()
        {
            return dbContext.Users;
        }


        public UserProfileInfo GetByLogin(string login)
        {
            return dbContext.Users.Where(u => u.Login == login).FirstOrDefault();
        }


        public List<UserProfileInfo> GetCollabs(int projectId)
        {
            return dbContext.Database.SqlQuery<UserProfileInfo>(
                       @"SELECT DISTINCT UserProfileInfoes.DisplayName,
UserProfileInfoes.Id,
UserProfileInfoes.[Login], 
UserProfileInfoes.PictureUrl,
UserProfileInfoes.Email,
UserProfileInfoes.AppUserId,
UserProfileInfoes.CreatedOn
  FROM [Projects]
  JOIN ProjectFiles on ProjectFiles.ProjectId = Projects.Id
  JOIN UserProfileInfoes on UserProfileInfoes.Id = ProjectFiles.Creator_Id
WHERE Projects.Id = @projectId
", new SqlParameter("projectId", projectId)
                       ).ToList();
        }


        public int SaveChanges(UserProfileInfo user)
        {
            this.dbContext.Entry(user).State = EntityState.Modified;
            this.dbContext.SaveChanges();
            return user.Id;
        }


        public UserProfileInfo GetById(int id)
        {
            return dbContext.Users.Find(id);
        }


        public UserProfileInfo GetByAppUserId(string userId)
        {
            return this.dbContext.Users.Where(u => u.AppUserId == userId).FirstOrDefault();
        }


        public void CreateOrSave(UserProfileInfo infos)
        {
            var test = this.dbContext.Users
                .Where(u => u.AppUserId == infos.AppUserId)
                .FirstOrDefault();
            if (test == null)
            {
                this.Create(infos);
            }
            else
            {
                this.SaveChanges(infos);
            }
        }


        public IList<UserProfileInfo> GetFriends(int userId)
        {
            SqlParameter user = new SqlParameter("@userId", userId);


            return dbContext.Database.SqlQuery<UserProfileInfo>(
               @"
SELECT 
DISTINCT UserProfileInfoes.ID,
UserProfileInfoes.AppUserId,
UserProfileInfoes.CreatedOn,
UserProfileInfoes.DisplayName,
UserProfileInfoes.Email,
UserProfileInfoes.Login,
UserProfileInfoes.PictureUrl

  FROM [ProjectFiles] a
  JOIN [ProjectFiles] b on a.ProjectId = b.ProjectId
  join UserProfileInfoes on b.Creator_Id = UserProfileInfoes.Id
  where a.Creator_Id = @userId
  and UserProfileInfoes.Id != @userId
",
            user).ToList();
        }


        public IQueryable<UserProfileInfo> Search(string query)
        {
            query = query.ToLower();
            return this.dbContext.Users
                .Where(u => u.DisplayName.ToLower().Contains(query) || u.Login.ToLower().Contains(query))
                .OrderByDescending(u => u.DisplayName);
           
        }
    }

    public interface IUserRepository
    {
        bool Create(UserProfileInfo usr);


        UserProfileInfo GetById(int id);

        IQueryable<UserProfileInfo> GetAll();

        UserProfileInfo GetByLogin(string login);

        List<UserProfileInfo> GetCollabs(int projectId);

        int SaveChanges(UserProfileInfo user);

        UserProfileInfo GetByAppUserId(string userId);

        void CreateOrSave(UserProfileInfo infos);

        IList<UserProfileInfo> GetFriends(int p);

        IQueryable<UserProfileInfo> Search(string query);
    }
}