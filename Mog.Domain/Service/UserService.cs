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
            int userId = 1;
            if (HttpContext.Current != null && HttpContext.Current.Session != null)
            {
                if (HttpContext.Current.Session["CURRENTUSER"] != null)
                {
                    userId = (int)HttpContext.Current.Session["CURRENTUSER"];
                }

            }
            UserProfile result = repo.GetById(userId);
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


        public UserProfile GetByLogin(string login)
        {
            return repo.GetByLogin(login);
        }


        public IEnumerable<UserProfile> GetByIds(IEnumerable<int> destinationIds)
        {
            return repo.GetAll().Where(u => destinationIds.Contains(u.Id));
        }
    }


    public interface IUserService
    {
        UserProfile GetCurrentUser();


        IQueryable<UserProfile> GetAll();

        UserProfile GetByLogin(string login);

        IEnumerable<UserProfile> GetByIds(IEnumerable<int> destinationIds);
    }
}