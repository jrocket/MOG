using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MogMockup.Models.ViewModel
{
    public class Project
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}