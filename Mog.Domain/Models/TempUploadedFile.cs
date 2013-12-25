using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoG.Domain.Models
{
    public class TempUploadedFile
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public int Size { get; set; }
        public string Url { get; set; }

        public string Path { get; set; }
        public string ThumbnailUrl { get; set; }

        public string ThumbnailPath { get; set; }

        public virtual UserProfile Creator { get; set; }

        public int ProjectId { get; set; }

        public string Description { get; set; }

        public string Tags { get; set; }


        [NotMapped]
        public byte[] Data { get; set; }





    }
}
