using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MoG.Domain.Models
{
    public class Project
    {

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
         [DisplayName("Creation Date")]
        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }
        public string ImageUrl { get; set; }
        public string ImageUrlThumb1 { get; set; }
        public string ImageUrlThumb2  { get; set; }

        public string Name { get; set; }

         [DisplayName("Description")]
        public string Description { get; set; }

        public int Likes { get; set; }

        [DisplayName("Tags")]
        public String Tags { get; set; }

        [DisplayName("Visiblity")]
        public Visibility VisibilityType { get; set; }

         [DisplayName("Licence")]
        public Licence LicenceType { get; set; }

        public virtual UserProfile Creator { get; set; }
    }
}