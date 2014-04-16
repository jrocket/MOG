using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace MoG.Domain.Models
{
   
    public class Note
    {
        public int Id { get; set; }

        public string Title { get; set; }
        public string Message { get; set; }
        public int CreatedById { get; set; }
        [ForeignKey("CreatedById")]
        public virtual UserProfileInfo CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public int ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        public virtual Project Project { get; set; }

    
    }
}
