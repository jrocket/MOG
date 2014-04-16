using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoG.Domain.Models
{
    public class Comment
    {
        public int Id { get; set; }

        public string Body { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public virtual UserProfileInfo Creator { get; set; }

        public int? ProjectId { get; set; }


        public int? FileId { get; set; }

        [ForeignKey("FileId")]
        public virtual ProjectFile File { get; set; }

        public string CreatorName
        {
            get
            {

                return (Creator!=null ? Creator.DisplayName : String.Empty);
            }
        }

        public string CreatedOnAsString
        {
            get
            {
                return CreatedOn.ToString("MM/dd/yyyy hh:mm:ss");
            }
        }

    }
}
