using MoG.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoG.Domain.Repository
{
    public class InvitRepository : BaseRepository, IInvitRepository
    {

        public InvitRepository(IdbContextProvider provider)
            : base(provider)
        {

        }




    

        public IQueryable<Invit> Get(int userId, InvitStatus filterBy)
        {
            return this.dbContext.Invits
                .Where( i => !i.Deleted)
               .Where(i => i.UserId == userId);
              
        }

        public int Create(Invit invit)
        {
            invit.CreatedOn = DateTime.Now;
            this.dbContext.Invits.Add(invit);
            this.dbContext.SaveChanges();
            return invit.Id;
        }


        public Invit Get(int projectId, int userId)
        {
            return this.dbContext.Invits
                .Where(i => !i.Deleted)
                .Where(i => i.UserId == userId && i.ProjectId == projectId).FirstOrDefault();
        }


        public bool SaveChanges(Invit invit)
        {
            this.dbContext.Entry(invit).State = System.Data.Entity.EntityState.Modified;
            int result = this.dbContext.SaveChanges();
            return result >0;
        }

        public Invit GetById(int id)
        {
            return this.dbContext.Invits.Find(id);
        }


        public bool IsInvited(int projectId, int userId)
        {
            return this.dbContext.Invits.
                Where(i => i.ProjectId == projectId && i.UserId == userId && i.Deleted == false)
                .Count()>0;
        }
    }

    public interface IInvitRepository
    {

        IQueryable<Invit> Get(int userId,InvitStatus filterBy);

        int Create(Invit invit);





        Invit Get(int projectId, int userId);

        bool SaveChanges(Invit invit);

        Invit GetById(int id);

        bool IsInvited(int projectId, int userId);
    }
}
