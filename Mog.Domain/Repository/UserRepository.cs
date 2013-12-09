using MoG.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace MoG.Domain.Repository
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(IdbContextProvider provider)
            : base(provider)
        {

        }

        public bool Create(UserProfile usr)
        {
            dbContext.Users.Add(usr);
            int result = dbContext.SaveChanges();
            return (result > 0);
        }


        public UserProfile GetById(int id)
        {
            return dbContext.Users.Find(id);
        }


        public IQueryable<UserProfile> GetAll()
        {
            return dbContext.Users;
        }


        public UserProfile GetByLogin(string login)
        {
            return dbContext.Users.Where(u => u.Login == login).FirstOrDefault();
        }


        public List<UserProfile> GetCollabs(int projectId)
        {
            return dbContext.Database.SqlQuery<UserProfile>(
                       @"SELECT DISTINCT UserProfiles.DisplayName, UserProfiles.Id, UserProfiles.[Login]
  FROM [Projects]
  JOIN MoGFiles on MoGFiles.ProjectId = Projects.Id
  JOIN UserProfiles on UserProfiles.Id = MoGFiles.Creator_Id").ToList();
        }
    }

    public interface IUserRepository
    {
        bool Create(UserProfile usr);


        UserProfile GetById(int p);

        IQueryable<UserProfile> GetAll();

        UserProfile GetByLogin(string login);

        List<UserProfile> GetCollabs(int projectId);
    }
}