using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace MoG.Domain.Models
{
    public class VMFile
    {
        public int Id { get; set; }

        public string DisplayName { get; set; }

        public string InternalName { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }


        public string Description { get; set; }

        public int Likes { get; set; }

        public String Tags { get; set; }

        public virtual UserProfileInfo Creator { get; set; }

        public int PlayCount { get; set; }

        public int DownloadCount { get; set; }

        public FileStatus FileStatus { get; set; }

        public int FileStatusAsInt { get { return (int)FileStatus; } }

        public int ProjectId { get; set; }

        public virtual Project Project { get; set; }

        public bool Deleted { get; set; }

        public int? DeletedById { get; set; }

        public virtual UserProfileInfo DeletedBy { get; set; }

        public DateTime? DeletedOn { get; set; }


        public string PublicUrl { get; set; }

        public string ThumbnailUrl { get; set; }

        public string Metadata { get; set; }

        public string MetadataType { get; set; }

      
        public bool isPendingProcessing { get; set; }
        public bool Promoted
        {
            get
            {
                return this.Project != null && this.Project.PromotedId != null && this.Project.PromotedId.Value == this.Id;
            }
        }
        public Metadata GetMetadata()
        {
            if (String.IsNullOrEmpty(this.Metadata))
            {
                return new Metadata();
            }
            string assemblyQualifiedName = this.MetadataType;

            var futureType = Type.GetType(assemblyQualifiedName);

            var serializer = new DataContractJsonSerializer(futureType);

            MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(this.Metadata ?? ""));
            var result = (Metadata)serializer.ReadObject(stream);


            return result;
        }

    }
}
