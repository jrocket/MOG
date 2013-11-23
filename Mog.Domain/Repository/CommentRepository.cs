using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoG.Domain.Repository
{
    public class CommentRepository : BaseRepository, ICommentRepository
    {
      

        public CommentRepository(IdbContextProvider provider) : base(provider)
        {
           
        }


        public IQueryable<Models.Comment> GetByFileId(int fileId)
        {
            return dbContext.Comments.Where(p => p.FileId == fileId);
        }


        public bool Create(Models.Comment comment)
        {
            dbContext.Comments.Add(comment);
            int result = dbContext.SaveChanges();
            return (result > 0);
        }
    }

    public interface ICommentRepository
    {

        IQueryable<Models.Comment> GetByFileId(int fileId);

        bool Create(Models.Comment newComment);
    }
}
