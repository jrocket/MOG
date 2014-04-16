using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoG.Domain.Models
{
    public class Log
    {
        public int Id { get; set; }
        public DateTime? Date { get; set; }

        public String Location { get; set; }

        public String Message { get; set; }

        public String Stacktrace { get; set; }

        public EnumSeverity Severity { get; set; }
    }

    public enum EnumSeverity
    {
        Verbose,
        Message,
        Error
    }
}
