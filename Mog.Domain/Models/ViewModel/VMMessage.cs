using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoG.Domain.Models
{
    public class VMMessage
    {
        public static int MESSAGE_MAX_SIZE = 60;

        public VMMessage()
        { }

        public VMMessage(Message message)
            : this(message, false)
        {
            if (!String.IsNullOrEmpty(message.Body))
            {
                int displayedSize = MESSAGE_MAX_SIZE;
                int indexOfFirstCarriageReturn = message.Body.IndexOf('\n');
                if (indexOfFirstCarriageReturn>0)
                {
                    displayedSize = indexOfFirstCarriageReturn;
                }
                bool isLongMessage = message.Body.Length > Math.Min(displayedSize,MESSAGE_MAX_SIZE);
             
                if (isLongMessage)
                {
                    this.Body = message.Body.Substring(0, displayedSize) + "...";
                    this.IsAbstract = true;
                }
                else
                {
                    this.Body = message.Body;
                    this.IsAbstract = false;
                }
            }



        }

        public VMMessage(Message message, bool keepBody)
        {
            Sender = message.CreatedBy.DisplayName;
            SentOn = message.CreatedOn.ToString("dd-MMM-yyyy hh:mm");
            Title = message.Title;
            Id = message.Id;
            To = message.SentTo;
            this.Body = message.Body;
            this.IsAbstract = false;
            ReplyToLogin = message.CreatedBy.Login;


        }

        public VMMessage(MessageBox boxMessage )
            : this(boxMessage.Message)
        {

            this.Sender = boxMessage.From;

            this.To = boxMessage.To;
            this.BoxId = boxMessage.Id;

            if (boxMessage.ReplyedOn.HasValue)
            {
                this.ReplyedOn = boxMessage.ReplyedOn.Value.ToString("dd-MMM-yyyy hh:mm");
            }

        }
        public VMMessage(MessageBox boxMessage, bool keepbody)
            : this(boxMessage.Message,keepbody)
        {

            this.Sender = boxMessage.From;

            this.To = boxMessage.To;
            this.BoxId = boxMessage.Id;

            if (boxMessage.ReplyedOn.HasValue)
            {
                this.ReplyedOn = boxMessage.ReplyedOn.Value.ToString("dd-MMM-yyyy hh:mm");
            }

        }




        public int BoxId { get; set; }
        public int Id { get; set; }
        public string Sender { get; set; }

        public string To { get; set; }

        public string Body;
        public string BodyHtml
        {
            get
            {
                return Body.Replace("\n", "<br />");

            }
            set
            {
                this.Body = value; ;
            }
        }

        public string SentOn { get; set; }

        public string ReplyedOn { get; set; }
        public string Title { get; set; }

        public bool IsAbstract { get; set; }

        public string ReplyToLogin { get; set; }

    }
}
