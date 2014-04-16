using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoG.Domain.Models
{
    public class VMProfile
    {
        public VMProfile()
        {
            this.Stats = new UserStatistics();
        }
        public int Id { get; set; }
        public string DisplayName { get; set; }

        public string Login { get; set; }

        public string Email { get; set; }

   
        public string PictureUrl { get; set; }

        public DateTime CreatedOn { get; set; }

        public UserStatistics Stats { get; set; }
    }
}
