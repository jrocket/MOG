using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoG.Domain.Models
{
    public class Comment
    {
        public int Id { get; set; }

        public string Body { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public virtual UserProfile Creator { get; set; }

        public int? ProjectId { get; set; }

        public int? FileId { get; set; }

    }
}
