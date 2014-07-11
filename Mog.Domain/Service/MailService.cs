using MoG.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System;
using SendGrid;


namespace MoG.Domain.Service
{
    public class MailService : IMailService
    {
        private IParameterService serviceParams = null;
        private ILogService serviceLog = null;

        public MailService(IParameterService param, ILogService log)
        {
            this.serviceParams = param;
            this.serviceLog = log;

        }
        public bool SendMail(string to, string subject, string body)
        {
            bool result = false;



            // Create the email object first, then add the properties.
            var myMessage = new SendGridMessage();

            // Add the message properties.
            myMessage.From = new MailAddress("flabbit@flabbit.com");

            // Add multiple addresses to the To field.
            List<String> recipients = new List<String>
{
    to
};

            myMessage.AddTo(recipients);

            myMessage.Subject = subject;

            //Add the HTML and Text bodies
            myMessage.Html = body;


            // Create network credentials to access your SendGrid account.
            var username = "azure_79d207b495627abeda221d74446ca235@azure.com";
            var pswd = "EMf0tV9PVhAl4a7";

            var credentials = new NetworkCredential(username, pswd);

            // Create an Web transport for sending email.
            var transportWeb = new Web(credentials);

       

            //System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
            //mail.To.Add(to);
          
            //mail.Subject = subject;
            //mail.SubjectEncoding = System.Text.Encoding.UTF8;
            //mail.Body = body;
            //mail.BodyEncoding = System.Text.Encoding.UTF8;
            //mail.IsBodyHtml = true;
            //mail.Priority = MailPriority.High;

            //SmtpClient client = new SmtpClient();
            //client.EnableSsl = true; //Gmail works on Server Secured Layer
            try
            {
                // Send the email.
                transportWeb.Deliver(myMessage);
                result = true;
            }
            catch (Exception ex)
            {
                this.serviceLog.LogError("MailService::SendMail", ex);
                throw new Exception("something went wrong while sending email ", ex);
            } // end try 
            return result;
        }


        public bool SendAdminMail(string title, string body)
        {
            Parameter to = this.serviceParams.GetByKey(MogConstants.PARAM_ADMINMAIL);
            return SendMail(to.Value, title, body);
        }


        public bool SendMail(MailMessage message)
        {
            return this.SendMail(message.To.ToString(), message.Subject, message.Body);
        }
    }
    public interface IMailService
    {
        bool SendMail(string to, string subject, string body);

        bool SendAdminMail(string subject, string body);

        bool SendMail(MailMessage message);
    }
}
