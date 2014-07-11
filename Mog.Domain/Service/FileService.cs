using MoG.Domain.Models;
using MoG.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoG.Domain.Service
{
    public class FileService : IFileService
    {
        public IFileRepository repoFile = null;
        public ICommentRepository repoComment = null;
        public IActivityService serviceActivity = null;
        public ISkydriveService serviceSkydrive = null;
        public IDropBoxService serviceDropBox = null;

        public FileService(IFileRepository _fileRepo
            , ICommentRepository _commentRepo
            , IActivityService _activityServ
            , ISkydriveService _skydriveService
            , IDropBoxService _dropboxService)
        {
            this.repoFile = _fileRepo;
            this.repoComment = _commentRepo;
            this.serviceActivity = _activityServ;
            this.serviceSkydrive = _skydriveService;
            this.serviceDropBox = _dropboxService;
        }
        public List<ProjectFile> GetProjectFile(int projectId)
        {
            return repoFile.GetByProjectId(projectId).ToList();
        }

        private ProjectFile RefreshIfNeeded(ProjectFile file)
        {
            if (file.StorageCredential != null)
            {
                string refreshedUrl = String.Empty;
                if (!this.CheckIfFileExists(file.PublicUrl))
                {
                    try
                    {
                        switch (file.StorageCredential.CloudService)
                        {
                            case CloudStorageServices.Dropbox:

                                refreshedUrl = serviceDropBox.RefreshFile(file);


                                //todo : check later...
                                break;
                            case CloudStorageServices.GoogleDrive:
                                //todo : check later...
                                break;
                            case CloudStorageServices.Skydrive:
                                //file maybe deleted, or public link has expired
                                file.PublicUrl = serviceSkydrive.RefreshFile(file);
                                //TODO : save the file.

                                break;

                        }
                        if (!String.IsNullOrEmpty(refreshedUrl))
                        {// we need to update the record in the DB
                            file.PublicUrl = refreshedUrl;
                            this.repoFile.Save(file);
                        }
                    }
                    catch (Exception exc)
                    {//toDO : file does not exists anymore
                        // mark it for deletion

                    }


                }

            }
            return file;
        }

        public ProjectFile GetById(int id)
        {
            ProjectFile file = repoFile.GetById(id);
            if (file == null)
                return null;
            
           

            return RefreshIfNeeded(file);
        }



        public List<Comment> GetFileComments(int fileId)
        {
            var result =  repoComment.GetByFileId(fileId).ToList();
            //foreach (var comment in result)
            //{
            //    comment.Body = comment.Body.Replace("\n", "<br>");

            //}
            return result;
        }


        public int Create(ProjectFile file, UserProfileInfo userProfile)
        {
            file.CreatedOn = DateTime.Now;
            file.ModifiedOn = DateTime.Now;
            file.Creator = userProfile;
            file.Likes = 0;

            file.DownloadCount = 0;
            file.FileStatus = FileStatus.Draft;
            //file.FileType = FileType.Unknown;

            if (this.repoFile.Create(file))
            {
                file = this.repoFile.GetById(file.Id);
                serviceActivity.LogFileCreation(file);

                return file.Id;
            }
            return -1;
        }

        public int Create(TempUploadedFile modelFile, UserProfileInfo CurrentUser)
        {
            ProjectFile f = new ProjectFile();
            f.AuthCredentialId = modelFile.AuthCredentialId;
            f.Description = modelFile.Description;
            f.DisplayName = modelFile.Name;
            f.InternalName = modelFile.Name;
            f.ProjectId = modelFile.ProjectId;
            f.Tags = modelFile.Tags;
            f.ThumbnailId = null;
            f.TempFileId = modelFile.Id;
            f.PublicUrl = "~/Content/Data/tempfile_sound.mp3";
            return Create(f, CurrentUser);
        }





        public bool Accept(int fileId)
        {
            return this.repoFile.SetStatus(fileId, FileStatus.Accepted);

        }


        public bool Reject(int fileId)
        {
            return this.repoFile.SetStatus(fileId, FileStatus.Rejected);
        }


        public bool Delete(int fileId, UserProfileInfo userProfile)
        {
            bool result = true;
            var file = this.repoFile.GetById(fileId);
            if (file != null)
            {
                file.Deleted = true;
                file.DeletedOn = DateTime.Now;
                file.DeletedBy = userProfile;
                result = (this.repoFile.Save(file) > 0);
            }
            else
            {
                result = false;
            }
            return result;
        }


        public int SaveChanges(ProjectFile file)
        {
            return this.repoFile.Save(file);
        }



        public IQueryable<ProjectFile> GetAll(bool bExcludeDeleted, bool bExcludePrivate)
        {
            return this.repoFile.GetAll(bExcludeDeleted, bExcludePrivate);
        }





        public ProjectFile GetByTempFileId(int tempFileId)
        {
            return this.repoFile.GetByTempFileId(tempFileId);
        }

        public bool CheckIfFileExists(string  url)
        {
            // using MyClient from linked post
            using (var client = new MoG.Domain.Common.SimpleWebClient())
            {
                client.HeadOnly = true;
                // fine, no content downloaded
                try
                {
                    string s1 = client.DownloadString(url);
                }
                catch (Exception)
                { // throws 404
                    return false;
                }
            }
            return true;
        }

        public bool CheckIfFileExists(int fileId)
        {

            if (fileId <= 0)
            {
                return false;
            }
            var file = this.repoFile.GetById(fileId);
            if (file == null)
            {
                return false;
            }


            return CheckIfFileExists(file.PublicUrl);
        }

        private bool incrementPlayCount(ProjectFile file)
        {
            file.PlayCount++;
            this.repoFile.Save(file);
            return true;
        }

        public bool IncrementPlayCount(int fileId)
        {
            ProjectFile file = this.repoFile.GetById(fileId);
            return this.incrementPlayCount(file);
        }

        public bool IncrementPlayCount(string url)
        {
            ProjectFile file = this.repoFile.GetByUrl(url);
            return this.incrementPlayCount(file);
        }


        public bool IncrementDownloadCount(int fileId)
        {
            ProjectFile file = this.repoFile.GetById(fileId);
            file.DownloadCount++;
            this.repoFile.Save(file);
            return true;
        }


        public List<ProjectFile> GetByIds(IEnumerable<int?> fileIds)
        {
            return this.repoFile.GetAll(false,false).Where(f => fileIds.Contains(f.Id)).ToList();
        }


       
    }

    public interface IFileService
    {
        List<ProjectFile> GetProjectFile(int projectId);


        ProjectFile GetById(int id);

        List<Comment> GetFileComments(int fileId);

        int Create(ProjectFile file, UserProfileInfo userProfile);

        int Create(TempUploadedFile modelFile, UserProfileInfo CurrentUser);


        bool Accept(int fileId);

        bool Reject(int fileId);

        bool Delete(int fileId, UserProfileInfo userProfile);

        int SaveChanges(ProjectFile file);


        IQueryable<ProjectFile> GetAll(bool bExcludeDeleted, bool bExcludePrivate);



        ProjectFile GetByTempFileId(int p);


        Boolean CheckIfFileExists(int fileId);

        Boolean CheckIfFileExists(string fileUrl);

        bool IncrementPlayCount(string url);

        bool IncrementPlayCount(int fileId);

        bool IncrementDownloadCount(int fileId);

        List<ProjectFile> GetByIds(IEnumerable<int?> fileIds);

     
    }
}
