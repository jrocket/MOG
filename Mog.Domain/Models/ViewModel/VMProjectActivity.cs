using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoG.Domain.Models
{
    public class VMProjectActivity
    {
        public Project Project { get; set; }

        public ICollection<Activity> Activities{ get; set; }
    }

    
}