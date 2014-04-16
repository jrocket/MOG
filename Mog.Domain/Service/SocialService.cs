using MoG.Domain.Models;
using MoG.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoG.Domain.Service
{
    public class SocialService : ISocialService
    {
        private IFileService serviceFile = null;
        private IProjectService serviceProject = null;
        private IUserRepository repoUser = null;

        public SocialService(IFileService _fileService,IProjectService _projectService, IUserRepository _userRepo )
        {
            this.serviceFile = _fileService;
            this.serviceProject = _projectService;
            this.repoUser = _userRepo;

        }

        public IList<UserProfileInfo> GetFriends(UserProfileInfo user)
        {
            return this.repoUser.GetFriends(user.Id);
            
        }
    }
    public interface ISocialService
    {
         IList<UserProfileInfo> GetFriends(UserProfileInfo user);

    }
}
