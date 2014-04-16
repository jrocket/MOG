using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoG.Domain.Models
{
    public class VMInvit
    {
        public int Id { get; set; }
        public string ProjectName { get; set; }

        public string ProjectUrl { get; set; }

        public string InviterName { get; set; }

        public string InviterUrl { get; set; }

        public String Date { get; set; }

        public string Status { get; set; }
        public InvitStatus InvitStatus { get; set; }

        public String Message { get; set; }

        public string InviteeName { get; set; }

        public string InviteeUrl { get; set; }
    }
}
