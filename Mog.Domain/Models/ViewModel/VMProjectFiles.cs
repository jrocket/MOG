using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoG.Domain.Models
{
    public class VMProjectFiles
    {
        public Project Project {get ; set;}
      
        public ICollection<String> Statuses { get; set; }

        public ICollection<String> Types { get; set; }

        public ICollection<String> Authors { get; set; }

        public IList<MoGFile> FilteredFiles { get; set; }

        public string filterByStatus { get; set; }
        public string filterByType { get; set; }
        public string filterByAuthor { get; set; }
    }
}
