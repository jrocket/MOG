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
    }
    public interface IFileRepository
    {
        IQueryable<MoGFile> GetByProjectId(int projectId);


        IQueryable<MoGFile> GetByCreatorId(int creatorId);

    }
}
