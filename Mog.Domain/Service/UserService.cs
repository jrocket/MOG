using MoG.Domain.Models;
using MoG.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;

namespace MoG.Domain.Service
{
    public class UserService : IUserService
    {
        IUserRepository repoUser = null;
        IAuthCredentialRepository repoAuthCredential = null;
        IRegistrationCodeService serviceRegistrationCode = null;
        public UserManager<ApplicationUser> UserManager { get; set; }

        public System.Security.Principal.IIdentity Identity { get; set; }

        public UserService(IUserRepository _repo,
            IRegistrationCodeService registrationService,
            IAuthCredentialRepository _authCredentialRepo)
        {
            this.repoUser = _repo;
            this.repoAuthCredential = _authCredentialRepo;
            this.serviceRegistrationCode = registrationService;


        }

        public UserProfileInfo GetCurrentUser()
        {
            if (Identity == null)
            {
                throw new ArgumentNullException("Please initialize the 'Identity' property first! I need to user Identity.GetUserId() :) ");
            }
            UserProfileInfo result = null;
            string userId = Identity.GetUserId();


            result = repoUser.GetByAppUserId(userId);


            return result;
        }


        public IQueryable<UserProfileInfo> GetAll()
        {
            return repoUser.GetAll();
        }


        public UserProfileInfo GetByLogin(string login)
        {
            return repoUser.GetByLogin(login);
        }



        public IEnumerable<UserProfileInfo> GetByIds(IEnumerable<int> destinationIds)
        {
            return repoUser.GetAll().Where(u => destinationIds.Contains(u.Id));
        }

        public UserStorageVM GetUserStorages(UserProfileInfo userProfile)
        {
            UserStorageVM model = new UserStorageVM();
            List<AuthCredential> credentials = this.repoAuthCredential
                .GetByUserId(userProfile.Id)
                .Where(c => c.Status != CredentialStatus.Canceled)
                .ToList();
            var values = from MoG.Domain.Models.CloudStorageServices e in Enum.GetValues(typeof(MoG.Domain.Models.CloudStorageServices))
                         select new { Id = e, Name = e.ToString() };
            foreach (var cloudStorage in Enum.GetValues(typeof(MoG.Domain.Models.CloudStorageServices)))
            {
                CloudStorageServices currentCloudStorage = (CloudStorageServices)cloudStorage;
                var userCredentials = credentials.Where(c => c.CloudService == currentCloudStorage).ToList();
                if (userCredentials.Count > 0)
                {
                    model.CloudStorages.AddRange(userCredentials);
                }
                else
                {
                    model.CloudStorages.Add(new AuthCredential()
                    {
                        CloudService = currentCloudStorage,
                        Status = CredentialStatus.NotRegistered
                    });
                }

            }


            return model;
        }


        public bool CancelRegistration(int id)
        {
            return this.repoAuthCredential.CancelCredential(id);
        }




        public int SaveChanges(UserProfileInfo user)
        {
            return this.repoUser.SaveChanges(user);
        }





        public async Task<IdentityResult> CreateAsync(ApplicationUser user,
            string password,
            string registrationCode,
            string email,
            UserManager<ApplicationUser> UserManager)
        {


            var result = await UserManager.CreateAsync(user, password);

            if (result != null && result.Errors == null || result.Errors.Count() < 1)
            {// user creation OK



                UserProfileInfo infos = new UserProfileInfo()
                {
                    DisplayName = user.UserName,
                    Login = user.UserName,
                    AppUserId = user.Id,
                    Email = email,
                    PictureUrl = "/Content/Images/NoAvatar.png",
                    CreatedOn = DateTime.Now
                };
                this.repoUser.CreateOrSave(infos);
                this.serviceRegistrationCode.Register(infos, registrationCode);
            }
            return result;
        }






        public UserProfileInfo GetById(int userId)
        {
            return this.repoUser.GetById(userId);
        }


        public List<UserProfileInfo> Search(string query, int page, int pageSize)
        {
            IQueryable<UserProfileInfo> result = this.repoUser.Search(query);
            int skip = (page - 1) * pageSize;
            return result
                .Skip(skip)
                .Take(pageSize)
                .ToList();
        }
    }


    public interface IUserService
    {
        System.Security.Principal.IIdentity Identity { get; set; }

        UserProfileInfo GetCurrentUser();


        IQueryable<UserProfileInfo> GetAll();

        UserProfileInfo GetByLogin(string login);

        IEnumerable<UserProfileInfo> GetByIds(IEnumerable<int> destinationIds);

        UserStorageVM GetUserStorages(UserProfileInfo userProfile);

        bool CancelRegistration(int id);


        int SaveChanges(UserProfileInfo user);




        Task<IdentityResult> CreateAsync(ApplicationUser user, string password, string registrationcode, string email, UserManager<ApplicationUser> UserManager);

        UserProfileInfo GetById(int userId);

        UserManager<ApplicationUser> UserManager { get; set; }

        List<UserProfileInfo> Search(string query, int page, int pageSize);
    }
}