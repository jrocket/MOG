using MoG;
using MoG.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoG.Domain.Models
{
   
    public class Activity
    {
        [Key] 
        public int Id { get; set; }
        public DateTime When { get; set; }
        public virtual UserProfile Who { get; set; }

       // public string What { get; set; }

        public int? ProjectId { get; set; }

        [ForeignKey("ProjectId")]
        public virtual Project Project { get; set; }

        public int? FileId { get; set; }

        [ForeignKey("FileId")]
        public virtual MoGFile File { get; set; }

        public ActivityType Type { get; set; }

    }
}
