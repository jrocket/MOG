using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoG.Domain.Models
{
    public class VMAddInvit
    {
        public string DisplayName { get; set; }

        public int UserId { get; set; }

        public string ThumbnailUrl { get; set; }

        public string  Message { get; set; }

        public int ProjectId { get; set; }

        public string ProjectName { get; set; }

        public string JSON { get; set; }
       
    }
}
