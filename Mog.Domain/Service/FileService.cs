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
        public IActivityService servActivity = null;

        public FileService(IFileRepository _fileRepo
            , ICommentRepository _commentRepo
            , IActivityService _activityServ)
        {
            this.repoFile = _fileRepo;
            this.repoComment = _commentRepo;
            this.servActivity = _activityServ;
        }
        public List<MoGFile> GetProjectFile(int projectId)
        {
            return repoFile.GetByProjectId(projectId).ToList();
        }

        public MoGFile GetById(int id)
        {
            return repoFile.GetById(id);
        }



        public IQueryable<Comment> GetFileComments(int fileId)
        {
            return repoComment.GetByFileId(fileId);
        }


        public int  Create(MoGFile file, UserProfile userProfile)
        {
            file.CreatedOn = DateTime.Now;
            file.ModifiedOn = DateTime.Now;
            file.Creator = userProfile;
            file.Likes = 0;

            file.DownloadCount = 0;
            file.FileStatus = FileStatus.Draft;
            file.FileType = FileType.Unknown;

            if (this.repoFile.Create(file))
            {
                servActivity.LogFileCreation(file);

                return file.Id;
            }
            return -1;
        }






        public bool Accept(int fileId)
        {
            return this.repoFile.SetStatus(fileId,FileStatus.Accepted);
          
        }


        public bool Reject(int fileId)
        {
            return this.repoFile.SetStatus(fileId,FileStatus.Rejected);
        }


        public bool Delete(int fileId, UserProfile userProfile)
        {
            bool result = true;
            var file = this.repoFile.GetById(fileId);
            if (file!=null)
            {
                file.Deleted = true;
                file.DeletedOn = DateTime.Now;
                file.DeletedBy = userProfile;
                result = (this.repoFile.Save(file) >0);
            }
            else
            {
                result = false;
            }
            return result;
        }
    }

    public interface IFileService
    {
        List<MoGFile> GetProjectFile(int projectId);


        MoGFile GetById(int id);

        IQueryable<Comment> GetFileComments(int fileId);

        int Create(MoGFile file, UserProfile userProfile);



        bool Accept(int fileId);

        bool Reject(int fileId);

        bool Delete(int fileId, UserProfile userProfile);
    }
}
