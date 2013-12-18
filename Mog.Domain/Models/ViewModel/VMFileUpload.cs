using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoG.Domain.Models
{

    public class UploadedFile
    {
        public string name { get; set; }
        public int size { get; set; }
        public string url { get; set; }
        public string thumbnailUrl { get; set; }
        public string deleteUrl { get; set; }
        public string deleteType { get; set; }
        public string error { get; set; }

    }

    public class VMFileUpload
    {
        public List<UploadedFile> files { get; set; }
    }
}
