using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoG.Domain.Models
{
    public class VMProject
    {
        public Project Project { get; set; }

        public String PromotedUrl
        {
            get
            {
                if (this.Project!=null && this.Project.PromotedId.HasValue)
                {
                    var promotedFile = this.Project.Files.FirstOrDefault(f => f.Id == this.Project.PromotedId.Value);
                    if (promotedFile != null)
                        return promotedFile.PublicUrl;

                }
                return null;
            }
        }
        public bool HasEdit { get; set; }

        public bool IsFollowed { get; set; }
    }
}
