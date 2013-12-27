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
        private IWaveformService serviceWaveform = null;
        private IFileService serviceMogFile = null;
        public string DefaultSavePath  {get;set;}

        public TempFileService(ITempFileRepository _repo
            , IUserRepository _userRepo
            , IWaveformService _waveformService
            , IFileService _fileService
            )
        {
            fileRepo = _repo;
        
            userRepo = _userRepo;
            serviceWaveform = _waveformService;
            this.serviceMogFile = _fileService;
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

        public IList<TempUploadedFile> GetByProjectId(int id, UserProfile user)
        {
            return this.fileRepo.GetByProjectId(id,user.Id).ToList();
        }



        public bool Cancel(int projectId, UserProfile User)
        {
            bool result = true;
            var files = GetByProjectId(projectId, User);
            foreach (var file in files)
            {
                result &= this.DeleteById(file.Id);
            }
            return result;
        }



        public bool Process(TempUploadedFile file)
        { 
            // Generate thumbnail
            //TODO : rework, too many hardcoded things
            serviceWaveform.Initialize(file.Path);
            var thumbnail = serviceWaveform.GetWaveform();
            Metadata metadata = serviceWaveform.Metadata;
            string filename = file.Name + ".png";
            if (String.IsNullOrEmpty(this.DefaultSavePath))
            {
                throw new ArgumentException("DefaultSavePath is not initialized");
            }
            string path = System.IO.Path.Combine(this.DefaultSavePath, filename);
            thumbnail.Save( path);
            file.ThumbnailPath = path;
            file.ThumbnailUrl = "~/Data/" + filename;
 
            // create file
            MoGFile projectFile = new MoGFile()
            {
                Description = file.Description,
                FileStatus = FileStatus.Draft,
                Name = file.Name,
                ProjectId = file.ProjectId,
                Tags = file.Tags,
                ThumbnailUrl = file.ThumbnailUrl
            };
            projectFile.SetMetadata(metadata);
            this.serviceMogFile.Create(projectFile, file.Creator);

            // delete temp
            this.DeleteById(file.Id);

            return true;
        }


        public TempUploadedFile  Update(TempUploadedFile modelFile)
        {
            TempUploadedFile data = GetById(modelFile.Id);
            if (data!=null)
            {
                data.Description = modelFile.Description;
                data.Name = modelFile.Name;
                data.Tags = modelFile.Tags;
                this.fileRepo.SaveChanges(data);
            }
            return data;
        }


        
    }
   
    public interface ITempFileService
    {
     
        int Create(TempUploadedFile file, UserProfile userProfile);


        bool DeleteById(int id);

        IList<TempUploadedFile> GetByProjectId(int id, UserProfile user);
        TempUploadedFile GetById(int id);



        bool Cancel(int projectId, UserProfile CurrentUser);

        bool Process(TempUploadedFile file);


        TempUploadedFile Update(TempUploadedFile modelFile);

         string DefaultSavePath { get; set; }

    }
}