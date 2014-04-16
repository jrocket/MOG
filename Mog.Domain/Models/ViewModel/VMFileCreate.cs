using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoG.Domain.Models
{
    public class VMFileCreate
    {
        public ProjectFile File {get;set;}

        public Project Project { get; set; }

    }
}
