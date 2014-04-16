using MoG.Domain.Models;
using MoG.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoG.Domain.Service
{
    public class RegistrationCodeService : IRegistrationCodeService
    {
        private IRegistrationCodeRepository repoRegCode = null;


        public RegistrationCodeService(IRegistrationCodeRepository repo)
        {
            this.repoRegCode = repo;
          
        }



        public RegistrationCode GetByCode(string code)
        {
            code = code.ToLower();
            code = code.Trim();
            return this.repoRegCode.GetByCode(code);
        }



        public bool Register(UserProfileInfo user,string code)
        {

            RegistrationCode reg = this.GetByCode(code);
            if (reg.User != null)
            {
                return false;
            }
            reg.RegistratedOn = DateTime.Now;
            reg.User = user;
            this.repoRegCode.SaveChanges(reg);
            return true;
        }


    }

    public interface IRegistrationCodeService
    {
        RegistrationCode GetByCode(string code);
        bool Register(UserProfileInfo user, string code);
    }
}
