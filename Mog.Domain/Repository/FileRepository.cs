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

        public IQueryable<MoGFile> GetByProjectId(int projectId)
        {
            return dbContext.Files.Where(p => p.ProjectId == projectId);
        }


        public IQueryable<MoGFile> GetByCreatorId(int creatorId)
        {
            return dbContext.Files.Where(p => p.Creator.Id == creatorId);
        }


        public MoGFile GetById(int id)
        {
            return dbContext.Files.Find(id);
        }


        public bool Create(MoGFile file)
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


        public int Save(MoGFile file)
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
    }
    public interface IFileRepository
    {
        IQueryable<MoGFile> GetByProjectId(int projectId);


        IQueryable<MoGFile> GetByCreatorId(int creatorId);


        MoGFile GetById(int id);

        bool Create(MoGFile file);

        bool SetStatus(int fileId,FileStatus fileStatus);

        int Save(MoGFile file);
    }
}
