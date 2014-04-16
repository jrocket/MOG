using MoG.Domain.Models;
using MoG.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace MoG
{
    public class MyDataContextDbInitializer : DropCreateDatabaseIfModelChanges<MogDbContext>
    {

        public MyDataContextDbInitializer()
        {

        }


        protected override void Seed(MogDbContext context)
        {
            addParameters(context);
            addRegistrationCode(context);
          

            //context.Users.Add(new UserProfileInfo() { DisplayName = "Johnny ROCKET", 
            //    Login = "jrocket", 
            //    PictureUrl="/Content/Images/NoAvatar.png",
            //    CreatedOn = DateTime.Now
            //});
            //context.Users.Add(new UserProfileInfo() { DisplayName = "Mike VEGAS",
            //    Login = "mvegas",
            //    PictureUrl = "/Content/Images/NoAvatar.png",
            //    CreatedOn = DateTime.Now
            //});
            //context.SaveChanges();
            ////UserProfileInfo jrocket = context.Users.Where(p => p.Login == "jrocket").First();
            ////UserProfileInfo mvegas = context.Users.Where(p => p.Login == "mvegas").First();
            //List<UserProfileInfo> profiles = context.Users.ToList();

            //addDebugCredentials(context);
            //addThumbnail(context);

            //DateTime initialDate = DateTime.Now;
            //Random rand = new Random((int)DateTime.Now.Ticks);
            //for (int i = 0; i < 11; i++)
            //{
            //    var project = context.Projects.Add(new Project()
            //    {
            //        Creator = profiles[rand.Next(2)],
            //        Name = "Project " + i,
            //        ImageUrl = "~/Content/images/nothingyetbw.png",//http://placehold.it/700x400",
            //        ImageUrlThumb1 = "~/Content/images/nothingyetbw.png",//"http://placehold.it/350x200",
            //        Description = "lorem ipsum",
            //        Likes = 0,
            //        CreatedOn = initialDate.AddDays(-i),
            //        ModifiedOn = DateTime.Now,
            //        Tags = "pop ,rock ,electro, plouf",
            //        LicenceType = Licence.CCBYND,
            //        VisibilityType = Visibility.Public,
                    



            //    });
            //    context.SaveChanges();


            //    addFilesToProject(project, profiles, context);

            //    addActivityToProjec(project, profiles, context);

            //}


            //addMessages(profiles, context);


            //addInvits(context);
         
        }

        private void addParameters(MogDbContext context)
        {
            Parameter p = new Parameter() { Key = MogConstants.PARAM_ADMINMAIL, Value = "jrocket.666@gmail.com" };
            context.Parameters.Add(p);
            context.SaveChanges();
        }

        private void addRegistrationCode(MogDbContext context)
        {
            for (int i=0;i<10;i++)
            {
                RegistrationCode code = new RegistrationCode();
                code.Code =  string.Format("{0}{0}{0}{0}",i );
                context.RegistrationCodes.Add(code);
            }
            context.SaveChanges();
        }

        private void addInvits(MogDbContext context)
        {
            List<Invit> data = new List<Invit>();
            data.Add(new Invit()
            {
                CreatedById = 1,
                CreatedOn = DateTime.Now,
                ProjectId = 1,
                Status = InvitStatus.Accepted,
                UserId = 1,
                Deleted = false
            });
            data.Add(new Invit()
            {
                CreatedById = 2,
                CreatedOn = DateTime.Now,
                ProjectId = 1,
                Status = InvitStatus.Accepted,
                UserId = 1,
                Deleted = false
            });
            data.Add(new Invit()
            {
                CreatedById = 1,
                CreatedOn = DateTime.Now,
                ProjectId = 1,
                Status = InvitStatus.Pending,
                UserId = 1,
                Deleted = false
            });
            data.Add(new Invit()
            {
                CreatedById = 1,
                CreatedOn = DateTime.Now,
                ProjectId = 1,
                Status = InvitStatus.Rejected,
                UserId = 1,
                Deleted = false
            });

            data.Add(new Invit()
            {
                CreatedById = 1,
                CreatedOn = DateTime.Now,
                ProjectId = 1,
                Status = InvitStatus.Rejected,
                UserId = 1,
                Deleted = true,
                DeletedOn = DateTime.Now
            });
           
            foreach (Invit i in data)
                context.Invits.Add(i);
            context.SaveChanges();
        }

        private void addThumbnail(MogDbContext context)
        {
            Thumbnail thumb = new Thumbnail()
            {
                AuthCredentialId = 1,
                DisplayName = "thumb display name",
                InternalName = "thumb internal name",
                Path = "/mabenz.png",
                PublicUrl = "https://dl.dropboxusercontent.com/1/view/ig95bbcf6g28ekn/Applications/MyOwnGarage/mabenz.png"

            };
            context.Thumbnails.Add(thumb);
            context.SaveChanges();
        }

        private void addDebugCredentials(MogDbContext context)
        {
            string secret = ConfigurationManager.AppSettings["SAMPLE_SECRET"];
            string token = ConfigurationManager.AppSettings["SAMPLE_TOKEN"];
            AuthCredential credential = new AuthCredential()
            {
                Secret = secret,
                Token = token,
                UserId = 1,
                CloudService = CloudStorageServices.Dropbox,
                Status = CredentialStatus.Approved
            };
            context.AuthCredentials.Add(credential);
            context.SaveChanges();

        }

        private void addMessages(List<UserProfileInfo> profiles, MogDbContext context)
        {
            Message message = new Message();
            message.CreatedBy = profiles[0];
            message.Body = "Lorem Ipsmus du message";
            message.CreatedOn = DateTime.Now;

            message.Title = "Title Lorem";




            message = context.Messages.Add(message);
            context.SaveChanges();

            MessageBox md = new MessageBox() { MessageId = message.Id, UserId = profiles[1].Id, From = "JRocket", To = "MVegas", BoxType = BoxType.Inbox };
            context.MessageBoxes.Add(md);
            MessageBox md2 = new MessageBox() { MessageId = message.Id, UserId = profiles[0].Id, From = "JRocket", To = "MVegas", BoxType = BoxType.Outbox };
            context.MessageBoxes.Add(md);



            context.SaveChanges();
        }

        private void addActivityToProjec(Project project, List<UserProfileInfo> profiles, MogDbContext context)
        {
            Random rand = new Random((int)DateTime.Now.Ticks);
            for (int j = 0; j < 5; j++)
            {
                Activity activity = new Activity()
                {
                    Type = (ActivityType)rand.Next(33),
                    When = DateTime.Now.AddDays(rand.Next(100) - 50),
                    Who = profiles[rand.Next(2)],
                    ProjectId = project.Id,
                    FileId = 1
                };
                context.Activities.Add(activity);
            }
            context.SaveChanges();
        }

        private void addFilesToProject(Project project, List<UserProfileInfo> profiles, MogDbContext context)
        {
           
            List<FileType> filetypes = new List<FileType>() { FileType.Bass, FileType.Drums, FileType.Guitar, FileType.Idea, FileType.Mixdown };
            List<FileStatus> filestatus = new List<FileStatus>() { FileStatus.Accepted, FileStatus.Draft, FileStatus.Rejected, FileStatus.Submitted };
            Random rand = new Random((int)DateTime.Now.Ticks);
            Mp3Metadata metadata = new Mp3Metadata() { Duration = "4:07" };

            for (int j = 0; j < 5; j++)
            {
                ProjectFile file = new ProjectFile()
                {
                    CreatedOn = DateTime.Now,
                    Creator = profiles[rand.Next(2)],
                    Description = "Lorem Ipsum",
                    DownloadCount = 42,
                    FileStatus = filestatus[j % 4], 
                    Tags = filetypes[j % 5].ToString(),
                    Likes = 12,
                    ModifiedOn = DateTime.Now,
                    DisplayName = "Track name",
                    InternalName = "TrackInternalName",
                    PlayCount = 32,
                    ProjectId = project.Id,
                    ThumbnailId = 1,
                    Path = "/mabenz.mp3",
                    AuthCredentialId = 1,
                    PublicUrl = "https://dl.dropboxusercontent.com/1/view/4cbnwwcomsg1xl3/Applications/MyOwnGarage/mabenz.mp3"

                };
                file.SetMetadata(metadata);
                file = context.Files.Add(file);
                addCommentToFile(file, profiles, context);
                context.SaveChanges();
            }
        }

        private void addCommentToFile(ProjectFile file, List<UserProfileInfo> profiles, MogDbContext context)
        {
            Random rand = new Random((int)DateTime.Now.Ticks);
            for (int i = 0; i < 1; i++)
            {
                Comment comment = new Comment()
                {
                    Body = "Lorem Ipsums.... ",
                    CreatedOn = DateTime.Now,
                    Creator = profiles[rand.Next(2)],
                    FileId = file.Id
                };
                context.Comments.Add(comment);

            }
        }
    }
}