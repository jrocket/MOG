using MoG.Domain.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MoG.Domain.Repository
{
    public class MogDbContext :   DbContext
    {

        public MogDbContext()
            : base("DefaultConnection")
        {
        }


        public DbSet<Project> Projects { get; set; }
        public DbSet<UserProfile> Users { get; set; }

        public DbSet<Activity> Activities { get; set; }

        public DbSet<MoGFile> Files { get; set; }
    }
}