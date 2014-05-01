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
   
    public class VMActivity
    {
        [Key] 
        public int Id { get; set; }
        public DateTime When { get; set; }
        public virtual UserProfileInfo Who { get; set; }

       // public string What { get; set; }

        public string Description { get; set; }

        public string Url { get; set; }

        public ActivityType Type { get; set; }
    }
}
