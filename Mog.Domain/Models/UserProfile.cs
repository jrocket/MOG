using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoG.Domain.Models
{
    public class UserProfile
    {
        public int Id { get; set; }

        public string DisplayName { get; set; }

        public string Login { get; set; }


        public override string ToString()
        {
            return DisplayName;
        }
    }
}