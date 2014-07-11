using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoG.Domain.Models
{
    public class VMComment
    {
        public int Id { get; set; }

        public string Body { get; set; }

        public DateTime CreatedOn { get; set; }

        public string DeleteUrl { get; set; }

        public virtual UserProfileInfo Creator { get; set; }
        public string CreatorName {get;set;}
      

        public string CreatedOnAsString         {
            get
            {
                return CreatedOn.ToString("MM/dd/yyyy hh:mm:ss");
            }
        }

        public string ModifiedOnAsString
        {
            get
            {
                if (CreatedOn == ModifiedOn || ModifiedOn == null)
                    return String.Empty;
                return ModifiedOn.Value.ToString("MM/dd/yyyy hh:mm:ss");
            }
        }

        public DateTime? ModifiedOn { get; set; }
    }
}
