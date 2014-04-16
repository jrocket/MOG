using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoG.Domain.Models
{
    public enum CredentialStatus
    {
        Pending,
        Approved,
        NotRegistered,
        Canceled
    }
    [Table("AuthCrendentials")]
    public class AuthCredential
    {

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Login { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        public virtual UserProfileInfo User { get; set; }
        public string Key { get; set; }
        public string Secret { get; set; }
        public string Token { get; set; }
        public string Refresh { get; set; }
        public string Authentication { get; set; }
        public CloudStorageServices CloudService { get; set; }
        public byte[] Data { get; set; }

        public CredentialStatus Status { get; set; }





       
    }

}
