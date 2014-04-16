using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoG.Domain.Models
{
    public class RegistrationCode
    {
        public int Id { get; set; }

        public string Code { get; set; }

        [ForeignKey("UserId")]
        public virtual UserProfileInfo User { get; set; }

        public int? UserId { get; set; }

        public DateTime? RegistratedOn { get; set; }
    }
}
