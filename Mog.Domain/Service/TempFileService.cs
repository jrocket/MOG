using MoG.Domain.Models;
using MoG.Domain.Models.ViewModel;
using MoG.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoG.Domain.Service
{
    public class TempFileService : ITempFileService
    {
        private ITempFileRepository fileRepo = null;
        private IUserRepository userRepo = null;

        public TempFileService(ITempFileRepository _repo
            , IUserRepository _userRepo
            )
        {
            fileRepo = _repo;
        
            userRepo = _userRepo;
        }





        public TempUploadedFile GetById(int id)
        {
            return this.fileRepo.GetById(id);
        }

  
        public int Create(TempUploadedFile file, UserProfile userProfile)
        {
            file.Creator = userProfile;
            if (this.fileRepo.Create(file))
            {
                return file.Id;
            }
            return -1;
        }

        public bool DeleteById(int id)
        {
            var fileToDelete = this.GetById(id);
            if (fileToDelete!=null)
            {
                return this.fileRepo.Delete(fileToDelete);
            }
            return false;
          
        }

        public IList<TempUploadedFile> GetByProjectId(int id)
        {
            return this.fileRepo.GetByProjectId(id).ToList();
        }

    }

    public interface ITempFileService
    {
     
        int Create(TempUploadedFile file, UserProfile userProfile);


        bool DeleteById(int id);

        IList<TempUploadedFile> GetByProjectId(int id);
        TempUploadedFile GetById(int id);

            
    }
}