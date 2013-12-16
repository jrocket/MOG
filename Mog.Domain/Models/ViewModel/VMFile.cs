using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoG.Domain.Models
{
    public class VMFile
    {
        public MoGFile File {get;set;}

        public Project Project { get; set; }

    }
}
