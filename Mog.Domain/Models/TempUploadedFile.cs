using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoG.Domain.Models
{
    public enum ProcessStatus
    {
        ProcessingNotStarted,
        ProcessInProgress,
        Failed,
        Completed,
        UploadInProgress
    }
    public class TempUploadedFile
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public int Size { get; set; }
        public string Url { get; set; }

        public string Path { get; set; }
       

        public virtual UserProfileInfo Creator { get; set; }

        public int ProjectId { get; set; }

        public string Description { get; set; }

        public string Tags { get; set; }

        public ProcessStatus Status { get; set; }

        public int FailedCount { get; set; }


        [NotMapped]
        public byte[] Data { get; set; }


        public int AuthCredentialId { get; set; }

        [ForeignKey("AuthCredentialId")]
        public virtual AuthCredential StorageCredential { get; set; }
    }
}
