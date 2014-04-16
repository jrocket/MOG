using MoG.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoG.Domain.Repository
{
    public class FileRepository : IFileRepository
    {
        private MogDbContext dbContext = null;
        IdbContextProvider contextProvider = null;

        public FileRepository(IdbContextProvider provider)
        { 
            contextProvider = provider;
            dbContext = provider.GetCurrent();
        
        }

        public IQueryable<ProjectFile> GetByProjectId(int projectId)
        {
            return dbContext.Files.Where(p => p.ProjectId == projectId);
        }


        public IQueryable<ProjectFile> GetByCreatorId(int creatorId)
        {
            return dbContext.Files.Where(p => p.Creator.Id == creatorId);
        }


        public ProjectFile GetById(int id)
        {
            return dbContext.Files.Find(id);
        }


        public bool Create(ProjectFile file)
        {
            dbContext.Files.Add(file);
            int result = dbContext.SaveChanges();
            return (result > 0);
        }



        public bool SetStatus(int fileId, FileStatus fileStatus)
        {
            var file = dbContext.Files.Find(fileId);
            file.FileStatus = fileStatus;
            int result = dbContext.SaveChanges();
            return (result > 0);
        }


        public int Save(ProjectFile file)
        {
            if (file.Id >0)
            {//todo : check if file is attached to dbContext
                return dbContext.SaveChanges();

            }
            else
            {
                throw new Exception("use Create() instead!");
            }
           
        }


        public IQueryable<ProjectFile> GetAll(bool bExcludeDeleted, bool bExcludePrivate)
        {
            IQueryable<ProjectFile> result = this.dbContext.Files;
            if (bExcludeDeleted)
            {
                result = result.Where(f => f.Deleted == false);
            }
            if (bExcludePrivate)
            {
                result = result.Where(f => f.Project.VisibilityType != Visibility.Private);
            }
            return result;
        }


        public ProjectFile GetByTempFileId(int tempFileId)
        {
            return this.dbContext.Files
                .Where(t => t.TempFileId == tempFileId)
                .FirstOrDefault();
        }


        public ProjectFile GetByUrl(string url)
        {
            return this.dbContext.Files
                .Where(f => f.PublicUrl == url)
                .FirstOrDefault();
        }


        public int GetRatioAcceptedTracks(int userID)
        {
            var files = this.dbContext.Files
                .Where(f => f.Creator.Id == userID && f.Project.Creator.Id!= userID)
                .Select(f => new { status = f.FileStatus }).ToList();
            int acceptedCount = 0;
            int totalCount = 0;
            foreach (var filestatus in files)
            {
                totalCount++;
                if (filestatus.status == FileStatus.Accepted)
                {
                    acceptedCount++;
                }
            }
            if (totalCount > 0)
            {
                return acceptedCount * 100 / totalCount;
            }
            return 0;
        }


        public int GetRatioTrackAcceptance(int userID)
        {
            var files = this.dbContext.Files
                 .Where(f => f.Project.Creator.Id == userID)
                 .Select(f => new { status = f.FileStatus }).ToList();
            int acceptedCount = 0;
            int totalCount = 0;
            foreach (var filestatus in files)
            {
                totalCount++;
                if (filestatus.status == FileStatus.Accepted)
                {
                    acceptedCount++;
                }
            }
            if (totalCount > 0)
            {
                return acceptedCount * 100 / totalCount;
            }
            return 0;
        }


       
    }
    public interface IFileRepository
    {
        IQueryable<ProjectFile> GetByProjectId(int projectId);


        IQueryable<ProjectFile> GetByCreatorId(int creatorId);


        ProjectFile GetById(int id);

        bool Create(ProjectFile file);

        bool SetStatus(int fileId,FileStatus fileStatus);

        int Save(ProjectFile file);

        IQueryable<ProjectFile> GetAll(bool bExcludeDeleted, bool bExcludePrivate);

        ProjectFile GetByTempFileId(int tempFileId);

        ProjectFile GetByUrl(string url);

        int GetRatioAcceptedTracks(int userID);

        int GetRatioTrackAcceptance(int userID);

    }
}
