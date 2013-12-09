using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoG.Domain.Models
{
    public class Message
    {
        public int Id { get; set; }
        public virtual UserProfile CreatedBy { get; set; }

     

        public string Body { get; set; }

        public string Title { get; set; }

        public DateTime CreatedOn { get; set; }


        public string Tag { get; set; }

        public string SentTo { get; set; }
    }

    public  class MessageBox
    {
        public int Id { get; set; }
        public int MessageId { get; set; }

        [ForeignKey("MessageId")]
        public virtual Message Message { get; set; }
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual UserProfile User { get; set; }

        public DateTime? ReplyedOn { get; set; }


        public bool Archived { get; set; }
        public bool Deleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public string From { get; set; }

        public string To { get; set; }

        public BoxType BoxType { get; set; }
    }

    public enum BoxType
    {
        Inbox = 0,
        Outbox = 1
    }

    //public class Inbox : MessageBox
    //{

    //}

    //public class Outbox : MessageBox
    //{

    //}



}
