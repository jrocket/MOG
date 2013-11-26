using MoG.Domain.Models;
using MoG.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoG.Domain.Service
{
    public class UserService : IUserService
    {
        IUserRepository repo = null;

        public UserService(IUserRepository _repo)
        {
            this.repo = _repo;
        }

        public UserProfile GetCurrentUser()
        {
            UserProfile result = repo.GetById(1);
            if (result == null)
            {
                return new UserProfile() { Id = 1, Login = "jrocket", DisplayName = "Johnny Rocket" };


            }
            return result;
        }


        public IQueryable<UserProfile> GetAll()
        {
            return repo.GetAll();
        }
    }


    public interface IUserService
    {
        UserProfile GetCurrentUser();


        IQueryable<UserProfile> GetAll();
    }
}