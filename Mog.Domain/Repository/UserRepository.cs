using MoG.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
    }

    public interface IUserRepository
    {
        bool Create(UserProfile usr);


        UserProfile GetById(int p);

        IQueryable<UserProfile> GetAll();

        UserProfile GetByLogin(string login);
    }
}