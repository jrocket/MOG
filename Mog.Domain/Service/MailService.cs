using MoG.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace MoG.Domain.Service
{
    public class MailService : IMailService
    {
        private IParameterService serviceParams = null;

        public MailService(IParameterService param)
        {
            this.serviceParams = param;

        }
        public bool SendMail(string to, string subject, string body)
        {
            bool result = false;
            string from = "jrocket.666@gmail.com"; //Replace this with your own correct Gmail Address


            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
            mail.To.Add(to);
            mail.From = new MailAddress(from, "One Ghost", System.Text.Encoding.UTF8);
            mail.Subject = subject;
            mail.SubjectEncoding = System.Text.Encoding.UTF8;
            mail.Body = body;
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.High;

            SmtpClient client = new SmtpClient();
            //Add the Creddentials- use your own email id and password

            client.Credentials = new System.Net.NetworkCredential(from, "password");

            client.Port = 587; // Gmail works on this port
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true; //Gmail works on Server Secured Layer
            try
            {
                client.Send(mail);
                result = true;
            }
            catch (Exception ex)
            {

            } // end try 
            return result;
        }


        public bool SendAdminMail(string title, string body)
        {
            Parameter to = this.serviceParams.GetByKey(MogConstants.PARAM_ADMINMAIL);
            return SendMail(to.Value, title, body);
        }
    }
    public interface IMailService
    {
        bool SendMail(string to, string subject, string body);

        bool SendAdminMail(string subject, string body);
    }
}
