using MoG.Domain.Models;
using MoG.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoG.Domain.Service
{
    public class UserStatisticsService : IUserStatisticsService
    {
        private IFileRepository repoFile = null;
        private IProjectRepository repoProject = null;
        private ICommentRepository repoComment = null;
        private IInvitRepository repoInvit = null;

        public UserStatisticsService(IFileRepository serviceFile
                                      , IProjectRepository serviceProject
                                      , ICommentRepository serviceComment
                                      , IInvitRepository serviceInvit)
        {
            this.repoFile = serviceFile;
            this.repoProject = serviceProject;
            this.repoComment = serviceComment;
            this.repoInvit = serviceInvit;
        }




        public UserStatistics GetStatByUserId(int userID)
        {
            UserStatistics stats = new UserStatistics();
            stats.CountProject = this.repoProject.GetByUserId(userID).Count();
            stats.CountComment = this.repoComment.GetByUserId(userID).Count();
            stats.CountFile = this.repoFile.GetByCreatorId(userID).Count() ;
            stats.CountInvit = this.repoInvit.Get(userID, InvitStatus.Accepted | InvitStatus.Pending | InvitStatus.Rejected).Count();


            stats.RatioAcceptedTracks = this.repoFile.GetRatioAcceptedTracks(userID);
            stats.RatioCollaboration = 0;
            stats.RatioFinishedProject = 0;
            stats.RatioTrackAcceptance = this.repoFile.GetRatioTrackAcceptance(userID);

            return stats;
        }
    }
    public interface IUserStatisticsService
    {
        UserStatistics GetStatByUserId(int userID);

    }
}
