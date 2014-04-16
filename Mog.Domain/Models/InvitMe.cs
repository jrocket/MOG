using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoG.Domain.Models
{
    public class InviteMe
    {
        public int Id { get; set; }
        public string Email { get; set; }

        public bool Processed { get; set; }

        public DateTime CreatedOn { get; set; }

        public String Source { get; set; }

    }
}
