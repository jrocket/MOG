using MoG.Domain.Models;
using MoG.Domain.Models.ViewModel;
using MoG.Domain.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace MoG.Domain.Service
{
    public class TempFileService : ITempFileService
    {
        private const string _container = "temporaryfiles";
        private ITempFileRepository fileRepo = null;
        private IUserRepository userRepo = null;
        private IWaveformService serviceWaveform = null;
        private IFileService serviceMogFile = null;
        private IDropBoxService serviceDropbox = null;
        private ISkydriveService serviceSkydrive = null;
        private IThumbnailRepository repositoryThumbnail = null;
        private ILogService serviceLog = null;
        private ILocalStorageService serviceLocalStorage = null;
        //  public string DefaultSavePath  {get;set;}

        public TempFileService(ITempFileRepository _repo
            , IUserRepository _userRepo
            , IWaveformService _waveformService
            , IFileService _fileService
            , IDropBoxService _dropboxService
            , ISkydriveService _skydriveService
            , IThumbnailRepository _thumbnailRepository
            , ILogService _logService
            , ILocalStorageService _azureService
            )
        {
            fileRepo = _repo;

            userRepo = _userRepo;
            serviceWaveform = _waveformService;
            this.serviceMogFile = _fileService;
            this.serviceDropbox = _dropboxService;
            this.serviceSkydrive = _skydriveService;
            this.repositoryThumbnail = _thumbnailRepository;
            this.serviceLog = _logService;
            this.serviceLocalStorage = _azureService;
        }





        public TempUploadedFile GetById(int id)
        {
            return this.fileRepo.GetById(id);
        }


        /// <summary>
        /// store the byte[] locally and create a record in the tempfile table
        /// </summary>
        /// <param name="file"></param>
        /// <param name="userProfile"></param>
        /// <returns></returns>
        public int Create(TempUploadedFile file, UserProfileInfo userProfile)
        {
            file.Creator = userProfile;
            file.Status = Models.ProcessStatus.UploadInProgress;
            file.FailedCount = 0;
            try
            {
                file.Path = Guid.NewGuid().ToString() + file.Name;
                this.serviceLocalStorage.UploadFile(file.Data, file.Path, _container);
            }
            catch (Exception exc)
            {
                this.serviceLog.LogError("TempFileService::Create", exc);
                return -1;
            }
            if (this.fileRepo.Create(file))
            {
                return file.Id;
            }
            return -1;
        }

        public bool DeleteById(int id)
        {
            var fileToDelete = this.GetById(id);
            if (fileToDelete != null)
            {
                return this.fileRepo.Delete(fileToDelete);
            }
            return false;

        }

        public IList<TempUploadedFile> GetByProjectId(int id, UserProfileInfo user)
        {
            return this.fileRepo.GetByProjectId(id, user.Id).ToList();
        }



        public bool Cancel(int projectId, UserProfileInfo User)
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
            Thumbnail thumb = null;
            // Generate thumbnail

            thumb = ProcessThumbnail(file);


            try
            {
                ProjectFile projectFile = ProcessProjectFile(file, thumb);
            }
            catch (Exception exc)
            {
                this.serviceLog.LogError("TempFileService::Process-projectFile", exc);

            }




            // delete temp
            this.DeleteById(file.Id);

            return true;
        }


        private Thumbnail ProcessThumbnail(TempUploadedFile file)
        {
            Thumbnail thumb = null;
            try
            {
                Stream ms = new MemoryStream();

                string thumbPublicUrl = string.Empty;
                if (this.serviceLocalStorage.DownloadFile(ref ms, file.Path, _container))
                {
                    var picture = serviceWaveform.GetWaveform(file.Path, ms);
                    byte[] pictureasByteArray = BitmapHelper.ImageToByte2(picture);

                    thumbPublicUrl = this.serviceLocalStorage.UploadFile(pictureasByteArray, file.Path, "projects");
                }

                ms.Close();

                string internalName = GenerateThumbnailName(file);
                string path = GenerateThumbnailPath(file);


                thumb = new Thumbnail()
                {
                    AuthCredentialId = file.AuthCredentialId,
                    DisplayName = file.Name,
                    InternalName = internalName,
                    Path = path,
                    PublicUrl = thumbPublicUrl//dropboxsavedFile.PublicUrl
                };
            }
            catch (Exception exc)
            {
                this.serviceLog.LogError("TempFileService::Process-Thumbnail", exc);
                thumb = new Thumbnail()
                {
                    PublicUrl = "/content/images/thumbnail_error.png"
                };
            }

            this.repositoryThumbnail.Create(thumb);

            return thumb;
        }


        private ProjectFile ProcessProjectFile(TempUploadedFile file, Thumbnail thumb)
        {
            if (file == null)
            {
                this.serviceLog.LogError("TempFileService::ProcessProjectFile", "WTF : file is null!",null);
                return null;
            }
            Stream ms = new MemoryStream();
            Metadata metadata = null;
            if (this.serviceLocalStorage.DownloadFile(ref ms, file.Path, _container))
            {
                metadata = serviceWaveform.GetMetadata(file.Path, ms);

            }
            string internalName = this.GenerateProjectFileInternalName(file);
            string path = this.GenerateProjectFilePath(file);
            MogFile uploadedFile = null;
            if (file.StorageCredential == null)
            {
                this.serviceLog.LogError("TempFileService::ProcessProjectFile", "WTF : StorageCredential is null!", null);
                return null;
            }
            switch (file.StorageCredential.CloudService)
            {
                case CloudStorageServices.Dropbox:
                     uploadedFile = this.serviceDropbox.UploadFile((MemoryStream)ms, internalName, file.StorageCredential, path);
                    if (uploadedFile == null)
                    {
                        this.serviceLog.LogError("TempFileService::ProcessProjectFile", "WTF : dropbox uploadedFile is null!", null);
                        return null;
                    }
                    uploadedFile.PublicUrl = this.serviceDropbox.GetMedialUrl(uploadedFile.Path, file.StorageCredential);
                    break;
                case CloudStorageServices.Skydrive  :
                    uploadedFile = this.serviceSkydrive.UploadFile((MemoryStream)ms, internalName, file.StorageCredential, path);
                    if (uploadedFile == null)
                    {
                        this.serviceLog.LogError("TempFileService::ProcessProjectFile", "WTF : Skydrive uploadedFile is null!", null);
                        return null;
                    }
                    uploadedFile.PublicUrl = this.serviceSkydrive.GetMedialUrl(uploadedFile.Path, file.StorageCredential);
                    break;
            }
           

            ProjectFile projectFile = this.serviceMogFile.GetByTempFileId(file.Id);
           
            if (projectFile == null)
            {
                this.serviceLog.LogError("TempFileService::ProcessProjectFile", "WTF : projectFile is null!", null);
                return null;
            }

            if (thumb != null)
            {
                projectFile.ThumbnailId = thumb.Id;
            }

            projectFile.Path = uploadedFile.Path;
            projectFile.PublicUrl = uploadedFile.PublicUrl;
            projectFile.TempFileId = null;

            projectFile.SetMetadata(metadata);
            this.serviceMogFile.SaveChanges(projectFile);



            return projectFile;
        }

        private string GenerateThumbnailName(TempUploadedFile file)
        {
            string encodedFilename = new HtmlString(file.Name).ToString();
            return string.Format("{0}_{1}.png", encodedFilename, DateTime.Now.Ticks);
        }

        private string GenerateThumbnailPath(TempUploadedFile file)
        {

            // return String.Format("/Projects/{0}/Thumbnails/", file.ProjectId);
            return String.Format("{0}/Files/", file.ProjectId);
        }

        private string GenerateProjectFileInternalName(TempUploadedFile file)
        {
            string filenameWithoutExtension = file.Name;// System.IO.Path.GetFileNameWithoutExtension(file.Path);
            string extension = System.IO.Path.GetExtension(file.Path);
            string encodedFilename = new HtmlString(filenameWithoutExtension).ToString();
            return string.Format("{0}_{1}{2}", encodedFilename, DateTime.Now.Ticks, extension);
        }

        private string GenerateProjectFilePath(TempUploadedFile file)
        {
            return String.Format("/Projects/{0}/Files/", file.ProjectId);
        }


        public TempUploadedFile Update(TempUploadedFile modelFile)
        {
            TempUploadedFile data = GetById(modelFile.Id);
            if (data != null)
            {
                data.Description = modelFile.Description;
                data.Name = modelFile.Name;
                data.Tags = modelFile.Tags;
                data.Status = modelFile.Status;
                this.fileRepo.SaveChanges(data);
            }
            return data;
        }





        public TempUploadedFile GetNextInQueue()
        {
            var result = this.fileRepo.GetNextInQueue();
            if (result != null)
            {
                result.Status = Models.ProcessStatus.ProcessInProgress;
                this.fileRepo.SaveChanges(result);
            }
            return result;
        }


        public void MoveToErrorQueue(TempUploadedFile nextFile)
        {
            nextFile.Status = Models.ProcessStatus.Failed;
            nextFile.FailedCount++;
            this.fileRepo.SaveChanges(nextFile);
        }


        public int GetQueueLength()
        {
            return this.fileRepo.GetQueueLength();
        }
    }

    public interface ITempFileService
    {

        int Create(TempUploadedFile file, UserProfileInfo userProfile);


        bool DeleteById(int id);

        IList<TempUploadedFile> GetByProjectId(int id, UserProfileInfo user);
        TempUploadedFile GetById(int id);



        bool Cancel(int projectId, UserProfileInfo CurrentUser);

        bool Process(TempUploadedFile file);


        TempUploadedFile Update(TempUploadedFile modelFile);

        //  string DefaultSavePath { get; set; }


        TempUploadedFile GetNextInQueue();

        void MoveToErrorQueue(TempUploadedFile nextFile);

        int GetQueueLength();
    }
}