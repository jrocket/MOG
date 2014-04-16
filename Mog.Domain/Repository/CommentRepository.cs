using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace MoG.Domain.Repository
{
    public class CommentRepository : BaseRepository, ICommentRepository
    {
      

        public CommentRepository(IdbContextProvider provider) : base(provider)
        {
           
        }


        public IQueryable<Models.Comment> GetByFileId(int fileId)
        {
            this.dbContext.Configuration.ProxyCreationEnabled = false;
            return dbContext.Comments
                .Include(c => c.Creator)
                .Where(p => p.FileId == fileId);
        }


        public bool Create(Models.Comment comment)
        {
            dbContext.Comments.Add(comment);
            int result = dbContext.SaveChanges();
            return (result > 0);
        }


        public IQueryable<Models.Comment> GetByProjectId(int id)
        {
            this.dbContext.Configuration.ProxyCreationEnabled = false;

           return  dbContext.Comments
               .Include(c => c.File)
               .Include(c => c.Creator)
                .Where(c => c.ProjectId == id || (c.File != null && c.File.ProjectId == id));
        }


        public IQueryable<Models.Comment> GetByUserId(int userID)
        {
            this.dbContext.Configuration.ProxyCreationEnabled = false;

            return dbContext.Comments
                .Include(c => c.Creator)
                 .Where(c => c.Creator.Id == userID);
        }


        public bool DeleteById(int id)
        {
            var comment = dbContext.Comments.Find(id);
            if (comment == null)
            {
                return false;

            }
            dbContext.Comments.Remove(comment);
            int result = dbContext.SaveChanges();
            return (result > 0);
        }
    }

    public interface ICommentRepository
    {

        IQueryable<Models.Comment> GetByFileId(int fileId);

        bool Create(Models.Comment newComment);

        IQueryable<Models.Comment> GetByProjectId(int id);

        IQueryable<Models.Comment> GetByUserId(int userID);

        bool DeleteById(int id);
    }
}
