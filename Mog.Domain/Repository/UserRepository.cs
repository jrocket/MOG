using MoG.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoG.Domain.Repository
{
    public class UserRepository : IUserRepository, IDisposable
    {
        private MogDbContext dbContext = new MogDbContext();

        public bool Create(UserProfile usr)
        {
            dbContext.Users.Add(usr);
            int result = dbContext.SaveChanges();
            return (result > 0);
        }

        public void Dispose()
        {
            dbContext.Dispose();
        }
    }

    public interface IUserRepository
    {
        bool Create(UserProfile usr);

    }
}