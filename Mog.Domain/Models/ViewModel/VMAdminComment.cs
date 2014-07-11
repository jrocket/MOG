using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoG.Domain.Models
{
   public class VMAdminComment
    {
       public string CreatedOn { get; set; }
       public string CreatedBy { get; set; }

       public string ModifiedOn { get; set; }
       public int Id { get; set; }

       public string TargetName { get; set; }
       public String Comment { get; set; }

       public String Url { get; set; }
    }
}
