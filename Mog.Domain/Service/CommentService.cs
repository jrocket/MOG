using MoG.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoG.Domain.Service
{
    public class CommentService : ICommentService
    {
        private ICommentRepository repoComment = null;
        public CommentService(ICommentRepository repo)
        {
            repoComment = repo;

        }

        public Models.Comment Create(Models.Comment newComment)
        {//todo : add the activity
            newComment.CreatedOn = DateTime.Now;
            newComment.ModifiedOn = DateTime.Now;
            repoComment.Create(newComment);
            return newComment;
        }
    }

    public interface ICommentService
    {

        Models.Comment Create(Models.Comment newComment);
    }
}
