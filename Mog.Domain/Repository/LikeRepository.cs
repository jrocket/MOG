using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoG.Domain.Repository
{
    public class LikeRepository : BaseRepository, ILikeRepository
    {

        public LikeRepository(IdbContextProvider provider)
            : base(provider)
        {

        }



        public Models.Like Get(int projectId, int userId)
        {
            return this.dbContext.Likes
                .Where(l => l.ProjectId == projectId && l.UserId == userId)
                .FirstOrDefault();
        }

        public int Create(Models.Like like)
        {
            this.dbContext.Likes.Add(like);
            this.dbContext.SaveChanges();
            return like.Id;
        }

        public int GetLikeCount(int projectId)
        {
            return this.dbContext.Likes
                .Where(l => l.ProjectId == projectId)
                .Count();
        }

        public bool ResetLikeCount(int projectId)
        {
           
            throw new NotImplementedException();
        }
    }

    public interface ILikeRepository
    {

        Models.Like Get(int projectId, int userId);

        int Create(Models.Like like);

        int GetLikeCount(int projectId);

        bool ResetLikeCount(int projectId);
    }
}
