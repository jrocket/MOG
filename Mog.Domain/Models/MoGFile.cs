using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.Serialization.Json;


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

        public FileStatus FileStatus { get; set; }

        [NotMapped]
        public int FileStatusAsInt { get { return (int)FileStatus; } }

        public string Metadata { get; set; }

        public string MetadataType { get; set; }
       

        public string PublicUrl { get; set; }

        public string ThumbnailUrl { get; set; }

        public int ProjectId { get; set; }

        [ForeignKey("ProjectId")]
        public virtual Project Project { get; set; }

        public bool Deleted { get; set; }

        public int? DeletedById { get; set; }

        [ForeignKey("DeletedById")]
        public virtual UserProfile DeletedBy { get; set; }

        public DateTime? DeletedOn { get; set; }


        public void SetMetadata(Metadata data)
        {
            MemoryStream stream1 = new MemoryStream();
            DataContractJsonSerializer ser = new DataContractJsonSerializer(data.GetType());
            this.MetadataType = data.GetType().AssemblyQualifiedName;
            ser.WriteObject(stream1, data);


            stream1.Position = 0;
            StreamReader reader = new StreamReader(stream1);
            this.Metadata = reader.ReadToEnd();
        

        }

        public Metadata GetMetadata()
        {
            string assemblyQualifiedName = this.MetadataType;

            var futureType = Type.GetType(assemblyQualifiedName);

            var serializer = new DataContractJsonSerializer(futureType);

            MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(this.Metadata ?? ""));
            var result = (Metadata)serializer.ReadObject(stream);

         
            return result;
        }

    }
}
