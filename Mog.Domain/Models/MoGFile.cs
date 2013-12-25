using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace MoG.Domain.Models
{
    public class MoGFile
    {
        public int Id { get; set; }
        [DisplayName("Creation Date")]
        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public string Name { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        public int Likes { get; set; }

        [DisplayName("Tags")]
        public String Tags { get; set; }

        public virtual UserProfile Creator { get; set; }

        public int PlayCount { get; set; }

        public int DownloadCount { get; set; }

        public FileStatus FileStatus{ get;set;}

        public int FileStatusAsInt { get { return (int)FileStatus; } }


        //public FileType FileType { get; set; }


        public int ProjectId { get; set; }

        [ForeignKey("ProjectId")]
        public virtual Project Project { get; set; }

        public bool Deleted { get; set; }

        public int? DeletedById { get; set; }

        [ForeignKey("DeletedById")]
        public virtual UserProfile DeletedBy { get; set; }

        public DateTime? DeletedOn { get; set; }

        public string ThumbnailUrl { get; set; }
    }
}
