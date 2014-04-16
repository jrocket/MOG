using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoG.Domain.Models.ViewModel
{
    public class VMSelect2
    {
        public bool more { get; set; }

        public List<VMSelect2item> results { get; set; }
    }

    public class VMSelect2item
    {

        public int id { get; set; }
        public string text { get; set; }
    }
}
