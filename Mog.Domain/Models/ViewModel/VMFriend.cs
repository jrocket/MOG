using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoG.Domain.Models
{
    public class VMFriend
    {
        public string DisplayName { get; set; }

        public string Login { get; set; }

        public int Id { get; set; }

        public string PictureUrl { get; set; }

        public int ProjectCount { get; set; }

        public int FileCount { get; set; }

        public string ProfileUrl { get; set; }

        public string JoinedOn { get; set; }

    }
}
