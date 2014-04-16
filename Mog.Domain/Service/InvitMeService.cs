using MoG.Domain.Models;
using MoG.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoG.Domain.Service
{
    public class InviteMeService : IInviteMeService
    {
        private IInviteMeRepository repo = null;
     
        private IMailService serviceMail = null;

        public InviteMeService(IInviteMeRepository repo, IMailService mail)
        {
            this.repo = repo;
          
            this.serviceMail = mail;
        }


        public int InviteMe(string email, string ip)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
               
            }
            catch
            {
                return -1;
            }

            InviteMe test = this.repo.GetByEmail(email);
            if (test != null)
            {
                return test.Id;
            }

            InviteMe item = new InviteMe()
            {
                Email = email,
                Processed = false,
                Source = ip,
                CreatedOn = DateTime.Now
            };
            int createdId = this.repo.Create(item);
            this.serviceMail.SendAdminMail("New subscription : ", "whooot someone drop his/her email : " + email);


            return createdId;
        }



        private bool SaveChanges(InviteMe item)
        {
            return this.repo.SaveChanges(item);
        }


        public bool Delete(int id)
        {
            InviteMe item = this.GetById(id);

            return this.repo.Delete(item); ;
        }



        public InviteMe GetById(int id)
        {
            return this.repo.GetById(id);

        }


    }

    public interface IInviteMeService
    {

        int InviteMe(string email, string ip);
        InviteMe GetById(int id);


        bool Delete(int id);

    }
}
