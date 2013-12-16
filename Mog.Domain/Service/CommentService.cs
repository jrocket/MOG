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
        private IActivityService servActivity = null;
        public CommentService(ICommentRepository repo, IActivityService activityService)
        {
            repoComment = repo;
            this.servActivity = activityService;

        }

        public Models.Comment Create(Models.Comment newComment)
        {
            newComment.CreatedOn = DateTime.Now;
            newComment.ModifiedOn = DateTime.Now;
            if (repoComment.Create(newComment))
            {
                this.servActivity.LogCommentCreation(newComment);
            }

            return newComment;
        }
    }

    public interface ICommentService
    {

        Models.Comment Create(Models.Comment newComment);
    }
}
