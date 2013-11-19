using MoG;
using MoG.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoG.Domain.Models
{
   
    public class Activity
    {
        public DateTime When { get; set; }
        public virtual UserProfile Who { get; set; }

        public string What { get; set; }

        public int Id { get; set; }

        public ActivityType Type { get; set; }

        public int ProjectId { get; set; }

    }
}
