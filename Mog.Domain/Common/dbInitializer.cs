using MoG.Domain.Models;
using MoG.Domain.Repository;
using System;
using System.Collections.Generic;
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
            context.Users.Add(new UserProfile() { DisplayName = "Johnny ROCKET", Login = "jrocket" });
            context.Users.Add(new UserProfile() { DisplayName = "Mike VEGAS", Login = "mvegas" });
            context.SaveChanges();
            //UserProfile jrocket = context.Users.Where(p => p.Login == "jrocket").First();
            //UserProfile mvegas = context.Users.Where(p => p.Login == "mvegas").First();
            List<UserProfile> profiles = context.Users.ToList();

            DateTime initialDate = DateTime.Now;
            Random rand = new Random((int)DateTime.Now.Ticks);
            for (int i = 0; i < 20; i++)
            {
                var project = context.Projects.Add(new Project()
                {
                    Creator = profiles[rand.Next(2)],
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
               

                addFilesToProject(project, profiles, context);

                addActivityToProjec(project, profiles, context);
                
            }


            addMessages(profiles, context);


        }

        private void addMessages(List<UserProfile> profiles, MogDbContext context)
        {
            Message message = new Message();
            message.CreatedBy = profiles[0];
            message.Body = "Lorem Ipsmus du message";
            message.CreatedOn = DateTime.Now;
          
            message.Title = "Title Lorem";

            
            

            message = context.Messages.Add(message);
            context.SaveChanges();

            MessageBox md = new MessageBox() { MessageId = message.Id, UserId =  profiles[1].Id, From = "JRocket" , To = "MVegas", BoxType = BoxType.Inbox};
            context.MessageBoxes.Add(md);
            MessageBox md2 = new MessageBox() { MessageId = message.Id, UserId = profiles[0].Id, From = "JRocket", To = "MVegas", BoxType = BoxType.Outbox };
            context.MessageBoxes.Add(md);
            
            
          
            context.SaveChanges();
        }

        private void addActivityToProjec(Project project, List<UserProfile> profiles, MogDbContext context)
        {
            Random rand = new Random((int)DateTime.Now.Ticks);
            for (int j = 0; j < 20; j++)
            {
                Activity activity = new Activity()
                {
                    Type = (ActivityType)rand.Next(33),
                    When = DateTime.Now.AddDays(rand.Next(100)-50),
                    Who = profiles[rand.Next(2)],
                    ProjectId = project.Id,
                    FileId = 1
                };
                context.Activities.Add(activity);
            }
            context.SaveChanges();
        }

        private void addFilesToProject(Project project, List<UserProfile> profiles, MogDbContext context)
        {
            List<FileType> filetypes = new List<FileType>() { FileType.Bass, FileType.Drums, FileType.Guitar, FileType.Idea, FileType.Mixdown };
            List<FileStatus> filestatus = new List<FileStatus>() { FileStatus.Accepted, FileStatus.Draft, FileStatus.Rejected, FileStatus.Submitted };
            Random rand = new Random((int)DateTime.Now.Ticks);
            for (int j = 0; j < 5; j++)
            {
                MoGFile file = new MoGFile()
                {
                   CreatedOn = DateTime.Now,
                   Creator = profiles[rand.Next(2)],
                   Description = "Lorem Ipsum",
                   DownloadCount = 42,
                   FileStatus = filestatus[j % 4],
                   //FileType = filetypes[j%5],
                   Tags = filetypes[j%5].ToString(),
                   Likes = 12,
                   ModifiedOn = DateTime.Now,
                   Name = "Track name",
                   PlayCount = 32,
                   ProjectId = project.Id

                };
                file = context.Files.Add(file);
                addCommentToFile(file,profiles,context);
            }
            context.SaveChanges();
        }

        private void addCommentToFile(MoGFile file, List<UserProfile> profiles, MogDbContext context)
        {
            Random rand = new Random((int)DateTime.Now.Ticks);
            for (int i=0;i<1;i++)
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