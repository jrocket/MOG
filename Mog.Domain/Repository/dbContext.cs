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
        public DbSet<UserProfileInfo> Users { get; set; }

        public DbSet<AuthCredential> AuthCredentials { get; set; }

        public DbSet<Activity> Activities { get; set; }

        public DbSet<ProjectFile> Files { get; set; }

        public DbSet<TempUploadedFile> TempUploadedFiles { get; set; }

        public DbSet<Thumbnail> Thumbnails { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Message> Messages { get; set; }
       

        public DbSet<MessageBox> MessageBoxes { get; set; }

        public DbSet<DownloadCartItem> DownloadCarts { get; set; }

        public DbSet<Like> Likes { get; set; }

        public DbSet<Log> Logs { get; set; }

        public DbSet<Invit> Invits { get; set; }
        public DbSet<Follow> Follows { get; set; }
        public DbSet<FollowUser> FollowUsers { get; set; }

        public DbSet<Note> Notes { get; set; }

        public DbSet<RegistrationCode> RegistrationCodes { get; set; }

        public DbSet<InviteMe> InviteMes { get; set; }

        public DbSet<Parameter> Parameters { get; set; }

        public DbSet<Notification> Notifications { get; set; }

    
    }
}