using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MoG.Domain.Models
{
    public class UserProfileInfo
    {
        [Key]
        public int Id { get; set; }

        public string DisplayName { get; set; }

        public string Login { get; set; }

        public string Email { get; set; }

        public string AppUserId { get; set; }
     
        public override string ToString()
        {
            return DisplayName;
        }

        public string PictureUrl { get; set; }


        public DateTime CreatedOn { get; set; }
    }
}