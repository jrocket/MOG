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

        public string  Title { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ReplyedOn { get; set; }


        public string Tag { get; set; }

        [NotMapped]
        public IEnumerable<int> DestinationIds { get; set; }

        public bool Deleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }


    public class MessageDestination
    {
        public int Id { get; set; }
        public int MessageId { get; set; }
        public int UserId { get; set; }
    }


}
