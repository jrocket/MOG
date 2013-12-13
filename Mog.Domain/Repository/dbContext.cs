using MoG.Domain.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
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
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

        }


        public DbSet<Project> Projects { get; set; }
        public DbSet<UserProfile> Users { get; set; }

        public DbSet<Activity> Activities { get; set; }

        public DbSet<MoGFile> Files { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Message> Messages { get; set; }
        //public DbSet<Inbox> Inbox { get; set; }
        //public DbSet<Outbox> Outbox { get; set; }
        public DbSet<MessageBox> MessageBoxes { get; set; }
    }
}