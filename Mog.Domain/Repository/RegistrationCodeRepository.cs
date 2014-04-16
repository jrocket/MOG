using MoG.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoG.Domain.Repository
{
    public class RegistrationCodeRepository : BaseRepository, IRegistrationCodeRepository
    {

        public RegistrationCodeRepository(IdbContextProvider provider)
            : base(provider)
        {

        }



        public bool SaveChanges(RegistrationCode code)
        {
            this.dbContext.Entry(code).State = System.Data.Entity.EntityState.Modified;
            int result = this.dbContext.SaveChanges();
            return result > 0;
        }

        public RegistrationCode GetByCode(string code)
        {
            return this.dbContext.RegistrationCodes
                .Where(r => r.Code == code).FirstOrDefault();
        }

       
    }

    public interface IRegistrationCodeRepository
    {

        RegistrationCode GetByCode(string code);

        bool SaveChanges(RegistrationCode reg);
    }
}
