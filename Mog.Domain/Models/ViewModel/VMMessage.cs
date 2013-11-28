using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoG.Domain.Models
{
    public class VMMessage
    {
        public int Id { get; set; }
        public string Sender { get; set; }

        public string To { get; set; }
        public string Body { get; set; }

        public string SentOn { get; set; }

        public string Title { get; set; }
    }
}
