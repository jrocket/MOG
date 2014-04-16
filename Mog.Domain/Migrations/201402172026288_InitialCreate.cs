namespace MoG.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Activities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        When = c.DateTime(nullable: false),
                        ProjectId = c.Int(),
                        FileId = c.Int(),
                        Type = c.Int(nullable: false),
                        CommentId = c.Int(),
                        Who_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Comments", t => t.CommentId)
                .ForeignKey("dbo.ProjectFiles", t => t.FileId)
                .ForeignKey("dbo.Projects", t => t.ProjectId)
                .ForeignKey("dbo.UserProfileInfoes", t => t.Who_Id)
                .Index(t => t.CommentId)
                .Index(t => t.FileId)
                .Index(t => t.ProjectId)
                .Index(t => t.Who_Id);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Body = c.String(),
                        CreatedOn = c.DateTime(nullable: false),
                        ModifiedOn = c.DateTime(),
                        ProjectId = c.Int(),
                        FileId = c.Int(),
                        Creator_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserProfileInfoes", t => t.Creator_Id)
                .ForeignKey("dbo.ProjectFiles", t => t.FileId)
                .Index(t => t.Creator_Id)
                .Index(t => t.FileId);
            
            CreateTable(
                "dbo.UserProfileInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DisplayName = c.String(),
                        Login = c.String(),
                        Email = c.String(),
                        AppUserId = c.String(),
                        PictureUrl = c.String(),
                        CreatedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProjectFiles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreatedOn = c.DateTime(nullable: false),
                        ModifiedOn = c.DateTime(),
                        Description = c.String(),
                        Likes = c.Int(nullable: false),
                        Tags = c.String(),
                        PlayCount = c.Int(nullable: false),
                        DownloadCount = c.Int(nullable: false),
                        FileStatus = c.Int(nullable: false),
                        ThumbnailId = c.Int(),
                        ProjectId = c.Int(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                        DeletedById = c.Int(),
                        DeletedOn = c.DateTime(),
                        Metadata = c.String(),
                        MetadataType = c.String(),
                        DisplayName = c.String(),
                        InternalName = c.String(),
                        AuthCredentialId = c.Int(),
                        Path = c.String(),
                        PublicUrl = c.String(),
                        TempFileId = c.Int(),
                        Creator_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserProfileInfoes", t => t.Creator_Id)
                .ForeignKey("dbo.UserProfileInfoes", t => t.DeletedById)
                .ForeignKey("dbo.Projects", t => t.ProjectId)
                .ForeignKey("dbo.AuthCrendentials", t => t.AuthCredentialId)
                .ForeignKey("dbo.Thumbnails", t => t.ThumbnailId)
                .Index(t => t.Creator_Id)
                .Index(t => t.DeletedById)
                .Index(t => t.ProjectId)
                .Index(t => t.AuthCredentialId)
                .Index(t => t.ThumbnailId);
            
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreatedOn = c.DateTime(nullable: false),
                        ModifiedOn = c.DateTime(),
                        ImageUrl = c.String(),
                        ImageUrlThumb1 = c.String(),
                        ImageUrlThumb2 = c.String(),
                        Name = c.String(),
                        Description = c.String(),
                        Likes = c.Int(nullable: false),
                        Tags = c.String(),
                        VisibilityType = c.Int(nullable: false),
                        LicenceType = c.Int(nullable: false),
                        DeletedOn = c.DateTime(),
                        Deleted = c.Boolean(nullable: false),
                        PromotedId = c.Int(),
                        Creator_Id = c.Int(),
                        DeletedBy_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserProfileInfoes", t => t.Creator_Id)
                .ForeignKey("dbo.UserProfileInfoes", t => t.DeletedBy_Id)
                .Index(t => t.Creator_Id)
                .Index(t => t.DeletedBy_Id);
            
            CreateTable(
                "dbo.AuthCrendentials",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Login = c.String(),
                        UserId = c.Int(nullable: false),
                        Key = c.String(),
                        Secret = c.String(),
                        Token = c.String(),
                        Refresh = c.String(),
                        Authentication = c.String(),
                        CloudService = c.Int(nullable: false),
                        Data = c.Binary(),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserProfileInfoes", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Thumbnails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Metadata = c.String(),
                        MetadataType = c.String(),
                        DisplayName = c.String(),
                        InternalName = c.String(),
                        AuthCredentialId = c.Int(),
                        Path = c.String(),
                        PublicUrl = c.String(),
                        TempFileId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AuthCrendentials", t => t.AuthCredentialId)
                .Index(t => t.AuthCredentialId);
            
            CreateTable(
                "dbo.DownloadCartItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FileId = c.Int(),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ProjectFiles", t => t.FileId)
                .ForeignKey("dbo.UserProfileInfoes", t => t.User_Id)
                .Index(t => t.FileId)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Follows",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FollowerId = c.Int(nullable: false),
                        ProjectId = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserProfileInfoes", t => t.FollowerId)
                .ForeignKey("dbo.Projects", t => t.ProjectId)
                .Index(t => t.FollowerId)
                .Index(t => t.ProjectId);
            
            CreateTable(
                "dbo.InviteMes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(),
                        Processed = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        Source = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Invits",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Message = c.String(),
                        CreatedById = c.Int(nullable: false),
                        ProjectId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        ModifiedOn = c.DateTime(),
                        DeletedOn = c.DateTime(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserProfileInfoes", t => t.CreatedById)
                .ForeignKey("dbo.Projects", t => t.ProjectId)
                .ForeignKey("dbo.UserProfileInfoes", t => t.UserId)
                .Index(t => t.CreatedById)
                .Index(t => t.ProjectId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Likes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProjectId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Logs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(),
                        Location = c.String(),
                        Message = c.String(),
                        Stacktrace = c.String(),
                        Severity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MessageBoxes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MessageId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        ReplyedOn = c.DateTime(),
                        Archived = c.Boolean(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                        DeletedOn = c.DateTime(),
                        From = c.String(),
                        To = c.String(),
                        BoxType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Messages", t => t.MessageId)
                .ForeignKey("dbo.UserProfileInfoes", t => t.UserId)
                .Index(t => t.MessageId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Body = c.String(),
                        Title = c.String(),
                        CreatedOn = c.DateTime(nullable: false),
                        Tag = c.String(),
                        SentTo = c.String(),
                        CreatedBy_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserProfileInfoes", t => t.CreatedBy_Id)
                .Index(t => t.CreatedBy_Id);
            
            CreateTable(
                "dbo.Notes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Message = c.String(),
                        CreatedById = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        ProjectId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserProfileInfoes", t => t.CreatedById)
                .ForeignKey("dbo.Projects", t => t.ProjectId)
                .Index(t => t.CreatedById)
                .Index(t => t.ProjectId);
            
            CreateTable(
                "dbo.Parameters",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Key = c.String(),
                        Value = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RegistrationCodes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        UserId = c.Int(),
                        RegistratedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserProfileInfoes", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.TempUploadedFiles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Size = c.Int(nullable: false),
                        Url = c.String(),
                        Path = c.String(),
                        ProjectId = c.Int(nullable: false),
                        Description = c.String(),
                        Tags = c.String(),
                        Status = c.Int(nullable: false),
                        FailedCount = c.Int(nullable: false),
                        AuthCredentialId = c.Int(nullable: false),
                        Creator_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserProfileInfoes", t => t.Creator_Id)
                .ForeignKey("dbo.AuthCrendentials", t => t.AuthCredentialId)
                .Index(t => t.Creator_Id)
                .Index(t => t.AuthCredentialId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TempUploadedFiles", "AuthCredentialId", "dbo.AuthCrendentials");
            DropForeignKey("dbo.TempUploadedFiles", "Creator_Id", "dbo.UserProfileInfoes");
            DropForeignKey("dbo.RegistrationCodes", "UserId", "dbo.UserProfileInfoes");
            DropForeignKey("dbo.Notes", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.Notes", "CreatedById", "dbo.UserProfileInfoes");
            DropForeignKey("dbo.MessageBoxes", "UserId", "dbo.UserProfileInfoes");
            DropForeignKey("dbo.MessageBoxes", "MessageId", "dbo.Messages");
            DropForeignKey("dbo.Messages", "CreatedBy_Id", "dbo.UserProfileInfoes");
            DropForeignKey("dbo.Invits", "UserId", "dbo.UserProfileInfoes");
            DropForeignKey("dbo.Invits", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.Invits", "CreatedById", "dbo.UserProfileInfoes");
            DropForeignKey("dbo.Follows", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.Follows", "FollowerId", "dbo.UserProfileInfoes");
            DropForeignKey("dbo.DownloadCartItems", "User_Id", "dbo.UserProfileInfoes");
            DropForeignKey("dbo.DownloadCartItems", "FileId", "dbo.ProjectFiles");
            DropForeignKey("dbo.Activities", "Who_Id", "dbo.UserProfileInfoes");
            DropForeignKey("dbo.Activities", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.Activities", "FileId", "dbo.ProjectFiles");
            DropForeignKey("dbo.Activities", "CommentId", "dbo.Comments");
            DropForeignKey("dbo.Comments", "FileId", "dbo.ProjectFiles");
            DropForeignKey("dbo.ProjectFiles", "ThumbnailId", "dbo.Thumbnails");
            DropForeignKey("dbo.Thumbnails", "AuthCredentialId", "dbo.AuthCrendentials");
            DropForeignKey("dbo.ProjectFiles", "AuthCredentialId", "dbo.AuthCrendentials");
            DropForeignKey("dbo.AuthCrendentials", "UserId", "dbo.UserProfileInfoes");
            DropForeignKey("dbo.ProjectFiles", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.Projects", "DeletedBy_Id", "dbo.UserProfileInfoes");
            DropForeignKey("dbo.Projects", "Creator_Id", "dbo.UserProfileInfoes");
            DropForeignKey("dbo.ProjectFiles", "DeletedById", "dbo.UserProfileInfoes");
            DropForeignKey("dbo.ProjectFiles", "Creator_Id", "dbo.UserProfileInfoes");
            DropForeignKey("dbo.Comments", "Creator_Id", "dbo.UserProfileInfoes");
            DropIndex("dbo.TempUploadedFiles", new[] { "AuthCredentialId" });
            DropIndex("dbo.TempUploadedFiles", new[] { "Creator_Id" });
            DropIndex("dbo.RegistrationCodes", new[] { "UserId" });
            DropIndex("dbo.Notes", new[] { "ProjectId" });
            DropIndex("dbo.Notes", new[] { "CreatedById" });
            DropIndex("dbo.MessageBoxes", new[] { "UserId" });
            DropIndex("dbo.MessageBoxes", new[] { "MessageId" });
            DropIndex("dbo.Messages", new[] { "CreatedBy_Id" });
            DropIndex("dbo.Invits", new[] { "UserId" });
            DropIndex("dbo.Invits", new[] { "ProjectId" });
            DropIndex("dbo.Invits", new[] { "CreatedById" });
            DropIndex("dbo.Follows", new[] { "ProjectId" });
            DropIndex("dbo.Follows", new[] { "FollowerId" });
            DropIndex("dbo.DownloadCartItems", new[] { "User_Id" });
            DropIndex("dbo.DownloadCartItems", new[] { "FileId" });
            DropIndex("dbo.Activities", new[] { "Who_Id" });
            DropIndex("dbo.Activities", new[] { "ProjectId" });
            DropIndex("dbo.Activities", new[] { "FileId" });
            DropIndex("dbo.Activities", new[] { "CommentId" });
            DropIndex("dbo.Comments", new[] { "FileId" });
            DropIndex("dbo.ProjectFiles", new[] { "ThumbnailId" });
            DropIndex("dbo.Thumbnails", new[] { "AuthCredentialId" });
            DropIndex("dbo.ProjectFiles", new[] { "AuthCredentialId" });
            DropIndex("dbo.AuthCrendentials", new[] { "UserId" });
            DropIndex("dbo.ProjectFiles", new[] { "ProjectId" });
            DropIndex("dbo.Projects", new[] { "DeletedBy_Id" });
            DropIndex("dbo.Projects", new[] { "Creator_Id" });
            DropIndex("dbo.ProjectFiles", new[] { "DeletedById" });
            DropIndex("dbo.ProjectFiles", new[] { "Creator_Id" });
            DropIndex("dbo.Comments", new[] { "Creator_Id" });
            DropTable("dbo.TempUploadedFiles");
            DropTable("dbo.RegistrationCodes");
            DropTable("dbo.Parameters");
            DropTable("dbo.Notes");
            DropTable("dbo.Messages");
            DropTable("dbo.MessageBoxes");
            DropTable("dbo.Logs");
            DropTable("dbo.Likes");
            DropTable("dbo.Invits");
            DropTable("dbo.InviteMes");
            DropTable("dbo.Follows");
            DropTable("dbo.DownloadCartItems");
            DropTable("dbo.Thumbnails");
            DropTable("dbo.AuthCrendentials");
            DropTable("dbo.Projects");
            DropTable("dbo.ProjectFiles");
            DropTable("dbo.UserProfileInfoes");
            DropTable("dbo.Comments");
            DropTable("dbo.Activities");
        }
    }
}
