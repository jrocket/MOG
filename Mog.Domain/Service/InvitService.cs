using MoG.Domain.Models;
using MoG.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoG.Domain.Service
{
    public class InvitService : IInvitService
    {
        private IInvitRepository repoInvit = null;

        public InvitService(IInvitRepository repo)
        {
            this.repoInvit = repo;
        }


        public int Invit(int projectId, int userId, string message, UserProfileInfo currentUser)
        {
           
            Invit test = this.repoInvit.Get(projectId, userId);
            if (test != null)
            {
                test.Message = message;
                test.ModifiedOn = DateTime.Now;
                test.Status = InvitStatus.Pending;
                this.repoInvit.SaveChanges(test);
                return test.Id;
            }
               
            Invit Invit = new Invit() { ProjectId = projectId
                , UserId = userId
                , CreatedBy = currentUser
                ,Status = InvitStatus.Pending
                ,Message = message
            };
            int createdId = this.repoInvit.Create(Invit);

          
            return createdId;
        }

        public IList<Invit> GetInvits(int userId)
        {
            return this.repoInvit.Get(userId, InvitStatus.Accepted | InvitStatus.Pending | InvitStatus.Rejected).ToList();
        }

        private bool SaveChanges(Invit invit)
        {
            invit.ModifiedOn = DateTime.Now;
            return this.repoInvit.SaveChanges(invit);
        }
        public Invit GetById(int id)
        {
            return this.repoInvit.GetById(id);
        }

        public Invit Accept(int id)
        {
            var invit = this.GetById(id);
            invit.Status = InvitStatus.Accepted;
            SaveChanges(invit);
            return invit;
        }

        public Invit Reject(int id)
        {
            var invit = this.GetById(id);
            invit.Status = InvitStatus.Rejected;
            SaveChanges(invit);
            return invit;
        }



        public bool Delete(int id)
        {
            var invit = this.GetById(id);
            if (invit!=null)
            {
                invit.DeletedOn = DateTime.Now;
                invit.Deleted = true;
            }
            return this.repoInvit.SaveChanges(invit);
        }


        public bool IsInvited(Project project, UserProfileInfo user)
        {
            return this.repoInvit.IsInvited(project.Id, user.Id);
        }
    }

    public interface IInvitService
    {

        int Invit(int projectId, int userId,string message, UserProfileInfo currentUser);
        IList<Invit> GetInvits(int userId);
        Invit Accept(int id);
        Invit Reject(int id);
        Invit GetById(int id);


        bool Delete(int id);

        bool IsInvited(Project project, UserProfileInfo user);
    }
}
