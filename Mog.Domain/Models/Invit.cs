using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoG.Domain.Models
{
    [Flags]
    public enum InvitStatus
    {
        Pending = 1,
        Accepted = 2,
        Rejected = 4
    }
    public class Invit
    {
        public int Id { get; set; }

        public String Message { get; set; }
        public int CreatedById { get; set; }
        [ForeignKey("CreatedById")]
        public virtual UserProfileInfo CreatedBy { get; set; }

        public int ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        public virtual Project Project { get; set; }

        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual UserProfileInfo User { get; set; }

        public InvitStatus Status { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public DateTime? DeletedOn { get; set; }

        public bool Deleted { get; set; }
    }
}
