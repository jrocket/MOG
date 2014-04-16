using MoG.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace MoG.Domain.Repository
{
    public class InviteMeRepository : BaseRepository, IInviteMeRepository
    {

        public InviteMeRepository(IdbContextProvider provider)
            : base(provider)
        {

        }





        public int Create(InviteMe item)
        {
            item.CreatedOn = DateTime.Now;
            this.dbContext.InviteMes.Add(item);
            this.dbContext.SaveChanges();
            return item.Id;
        }




        public bool SaveChanges(InviteMe item)
        {
            this.dbContext.Entry(item).State = System.Data.Entity.EntityState.Modified;
            int result = this.dbContext.SaveChanges();
            return result > 0;
        }



        public InviteMe GetById(int id)
        {
            return this.dbContext.InviteMes.Find(id);
        }

        public InviteMe GetByEmail(string email)
        {
            return this.dbContext.InviteMes.Where(i => i.Email == email).FirstOrDefault();
        }

        public bool Delete(InviteMe item)
        {

            if (item != null)
            {
                this.dbContext.InviteMes.Remove(item);
                this.dbContext.SaveChanges();
                return true;
            }
            return false;
        }
    }

    public interface IInviteMeRepository
    {


        InviteMe GetById(int id);

       int Create(InviteMe item);


       bool SaveChanges(InviteMe item);

       InviteMe GetByEmail(string email);

       bool Delete(InviteMe item);
    }
}
