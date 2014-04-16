using MoG.Domain.Models;
using MoG.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoG.Domain.Repository
{
    public class AuthCredentialRepository : BaseRepository, IAuthCredentialRepository
    {

        public AuthCredentialRepository(IdbContextProvider provider)
            : base(provider)
        {

        }


     

        public bool Create(AuthCredential credential)
        {
            dbContext.AuthCredentials.Add(credential);
            int result = dbContext.SaveChanges();
            return (result > 0);
        }

        public IQueryable<AuthCredential> GetByUserId(int id)
        {
            return dbContext.AuthCredentials.Where(a => a.UserId == id);
        }


        public AuthCredential GetById(int Id)
        {
            return dbContext.AuthCredentials.Find(Id);
        }


        public void SaveChanges(AuthCredential partialCredential)
        {
            this.dbContext.Entry(partialCredential).State = System.Data.Entity.EntityState.Modified;
            this.dbContext.SaveChanges();
        }


        public bool CancelCredential(int id)
        {
            var credentialToUpdate = this.GetById(id);
           if (credentialToUpdate == null)
           {
               return false;
           }
           credentialToUpdate.Status = CredentialStatus.Canceled;
           this.SaveChanges(credentialToUpdate);
           return true;
        }
    }

    public interface IAuthCredentialRepository
    {

        bool Create(AuthCredential credential);

        IQueryable<AuthCredential> GetByUserId(int id);

        AuthCredential GetById(int Id);

        void SaveChanges(AuthCredential partialCredential);

        bool CancelCredential(int id);
    }
}
