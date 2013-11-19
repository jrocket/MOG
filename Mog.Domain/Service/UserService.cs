using MoG.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoG.Domain.Service
{
    public class UserService : IUserService
    {

        public UserProfile GetCurrentUser()
        {
            UserProfile u = new UserProfile() { Id = 1, Login = "jrocket", DisplayName = "Johnny Rocket" };
            return u;
        }
    }


    public interface IUserService
    {
        UserProfile GetCurrentUser();

    }
}