using MoG.Domain.Models;
using MoG.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MoG
{
    public class MyDataContextDbInitializer : DropCreateDatabaseIfModelChanges<MogDbContext>
    {
        protected override void Seed(MogDbContext context)
        {
            context.Users.Add(new UserProfile() { DisplayName = "Johnny ROCKET", Login = "jrocket" });
            context.Users.Add(new UserProfile() { DisplayName = "Mike VEGAS", Login = "mvegas" });
            context.SaveChanges();
            UserProfile jrocket = context.Users.Where(p => p.Login == "jrocket").First();
            UserProfile mvegas = context.Users.Where(p => p.Login == "mvegas").First();


            DateTime initialDate = DateTime.Now;
            for (int i = 0; i < 20; i++)
            {
                var project = context.Projects.Add(new Project()
                {
                    Creator = jrocket,
                    Name = "Project " + i,
                    ImageUrl = "http://placehold.it/700x400",
                    ImageUrlThumb1 = "http://placehold.it/350x200",
                    Description = "lorem ipsum",
                    Likes = 42 + i,
                    CreatedOn = initialDate.AddDays(-i),
                    ModifiedOn = DateTime.Now,
                    Tags = "pop ,rock ,electro, plouf"

                });
                context.SaveChanges();
                addActivityToProjec(project, jrocket, context);

                addFilesToProject(project, jrocket, context);

             
                
            }


            addMessages(jrocket, mvegas, context);


        }

        private void addMessages(UserProfile jrocket, UserProfile mvegas, MogDbContext context)
        {
            Message message = new Message();
            message.CreatedBy = jrocket;
            message.Body = "Lorem Ipsmus du message";
            message.CreatedOn = DateTime.Now;
            //message.DestinationIds = new List<int> { jrocket.Id, mvegas.Id };
            message.Title = "Title Lorem";

            message = context.Messages.Add(message);
            context.SaveChanges();

            MessageDestination md = new MessageDestination() { MessageId = message.Id, UserId = jrocket.Id };
            context.MessagesDestinations.Add(md);
            context.SaveChanges();
            md = new MessageDestination() { MessageId = message.Id, UserId = mvegas.Id };
            context.MessagesDestinations.Add(md);
            context.SaveChanges();
        }

        private void addActivityToProjec(Project project, UserProfile jrocket, MogDbContext context)
        {
            for (int j = 0; j < 20; j++)
            {
                Activity activity = new Activity()
                {
                    Type = ActivityType.File,
                    What = "lorem ipsum",
                    When = DateTime.Now,
                    Who = jrocket,
                    ProjectId = project.Id
                };
                context.Activities.Add(activity);
            }
        }

        private void addFilesToProject(Project project, UserProfile jrocket, MogDbContext context)
        {
            List<FileType> filetypes = new List<FileType>() { FileType.Bass, FileType.Drums, FileType.Guitar, FileType.Idea, FileType.Mixdown };
            List<FileStatus> filestatus = new List<FileStatus>() { FileStatus.Accepted, FileStatus.Draft, FileStatus.Rejected, FileStatus.Submitted };

            for (int j = 0; j < 20; j++)
            {
                MoGFile file = new MoGFile()
                {
                   CreatedOn = DateTime.Now,
                   Creator = jrocket,
                   Description = "Lorem Ipsum",
                   DownloadCount = 42,
                   FileStatus = filestatus[j % 4],
                   FileType = filetypes[j%5],
                   Likes = 12,
                   ModifiedOn = DateTime.Now,
                   Name = "Track name",
                   PlayCount = 32,
                   ProjectId = project.Id

                };
                file = context.Files.Add(file);
                addCommentToFile(file,jrocket,context);
            }
        }

        private void addCommentToFile(MoGFile file, UserProfile jrocket, MogDbContext context)
        {
            for (int i=0;i<1;i++)
            {
                Comment comment = new Comment()
                {
                    Body = "Lorem Ipsums.... ",
                    CreatedOn = DateTime.Now,
                    Creator = jrocket,
                    FileId = file.Id
                };
                context.Comments.Add(comment);

            }
        }
    }
}