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
    }

    public interface ICommentRepository
    {

        IQueryable<Models.Comment> GetByFileId(int fileId);
    }
}
