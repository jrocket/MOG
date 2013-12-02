using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoG.Domain.Models
{
    public class VMMessage
    {


        public VMMessage()
        { }

        public VMMessage(Message message)
            : this(message, false)
        {

            if (message.Body.Length > 10)
            {
                this.Body = message.Body.Substring(0, 7) + "...";
                this.IsAbstract = true;
            }
            else
            {
                this.Body = message.Body.Substring(0, 7);
                this.IsAbstract = true;
            }

        }

        public VMMessage(Message message, bool keepBody)
        {
            Sender = message.CreatedBy.DisplayName;
            SentOn = message.CreatedOn.ToString("dd-MMM-yyyy hh:mm");
            Title = message.Title;
            Id = message.Id;
            this.Body = message.Body;
            this.IsAbstract = false;
        }

        public VMMessage(MessageBox boxMessage)
            : this(boxMessage.Message)
        {

            Sender = boxMessage.From;

            To = boxMessage.To;

        }

        public int Id { get; set; }
        public string Sender { get; set; }

        public string To { get; set; }
        public string Body { get; set; }

        public string SentOn { get; set; }

        public string Title { get; set; }

        public bool IsAbstract { get; set; }
    }
}
