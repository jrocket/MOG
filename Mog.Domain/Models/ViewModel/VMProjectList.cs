using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoG.Domain.Models.ViewModel
{
    public class VMProjectList
    {
      

        public string  ProjectUrl { get; set; }

        public string  ImageUrl { get; set; }

        public string Name { get; set; }

        public string OwnerName { get; set; }

        public int Id { get; set; }

        public int Likes { get; set; }

        public string Description { get; set; }

        public bool IsPrivate { get; set; }
      
      
    }
}