using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoG.Domain.Models
{
    public class DownloadCartItem
    {
        public int Id { get; set; }

        public int? FileId { get; set; }
        [ForeignKey("FileId")]
        public virtual ProjectFile File { get; set; }

        public virtual UserProfileInfo User { get; set; }
    }

}
