using MoG.Domain.Models;
using MoG.Domain.Models.ViewModel;
using MoG.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoG.Domain.Service
{
    public class ProjectService : IProjectService
    {
        private IProjectRepository projectRepo = null;
        private IActivityRepository activytRepo = null;
        private IFileRepository fileRepo = null;
        private IUserRepository userRepo = null;

        public ProjectService(IProjectRepository _repo
            , IActivityRepository _activityRepo
            , IFileRepository _fileRepo
            , IUserRepository _userRepo
            )
        {
            activytRepo = _activityRepo;
            projectRepo = _repo;
            fileRepo = _fileRepo;
            userRepo = _userRepo;
        }



        public IQueryable<Project> GetByUserLogin(string login)
        {
            return projectRepo.GetByUserLogin(login);
        }


        public IQueryable<Project> GetNew(int limit)
        {
           return projectRepo.GetNew( limit);
        }


        public IQueryable<Project> GetPopular(int limit)
        {
            return projectRepo.GetPopular(limit);
        }


        public ICollection<Project> GetRandom(int limit)
        {
            return projectRepo.GetRandom(limit);
        }


        public int  Create(Project project, UserProfile userProfile)
        {
            project.CreatedOn = DateTime.Now;
            project.ModifiedOn = DateTime.Now;
            project.Creator = userProfile;
            project.Likes = 0;

            project.ImageUrl = "http://placehold.it/700x400";
            project.ImageUrlThumb1 = "http://placehold.it/350x200";

            if ( projectRepo.Create(project))
            {
                return project.Id;
            }
            return -1;
           
        }


        public Project GetById(int id)
        {
            return projectRepo.GetById(id);
        }


        public List<Activity> GetProjectActivity(int projectId)
        {
            //var project = GetById(projectId);
            //VMProjectActivity result = new VMProjectActivity();
            //result.Project = project;

            var result = activytRepo.GetByProjectId(projectId).ToList();
          
            return result;
        }


       


        public ICollection<string> GetFileStatuses(Project project)
        {
           if (project!=null && project.Files!=null && project.Files.Count>0)
           {
               return project.Files.Select(file => file.FileStatus.ToString()).Distinct().ToList();
           }
           return null;
        }

        public ICollection<string> GetFileAuthors(Project project)
        {
            if (project != null && project.Files != null && project.Files.Count > 0)
            {
                return project.Files.Select(file => file.Creator.DisplayName).Distinct().ToList();
            }
            return null;
        }

        public ICollection<string> GetFileTypes(Project project)
        {
            if (project != null && project.Files != null && project.Files.Count > 0)
            {
                return project.Files.Select(file => file.FileType.ToString()).Distinct().ToList();
            }
            return null;
        }


        public IList<MoGFile> GetFilteredFiles(Project project, string filterByAuthor, string filterByStatus, string filterByType)
        {
            var files = project.Files;
            IEnumerable<MoGFile> result = files.Where(f => true);
            if (!String.IsNullOrEmpty(filterByAuthor))
            {
                result = result.Where(f => f.Creator.DisplayName == filterByAuthor);
            }
            if (!String.IsNullOrEmpty(filterByStatus))
            {
                FileStatus testStatus = (FileStatus)Enum.Parse(typeof(FileStatus), filterByStatus);
                result = result.Where(f => f.FileStatus == testStatus);
            }
            if (!String.IsNullOrEmpty(filterByType))
            {
                FileType testType = (FileType)Enum.Parse(typeof(FileType), filterByType);
                result = result.Where(f => f.FileType == testType);
            }

            return result.ToList();
        }


        public VMCollabs GetCollabs(int projectId)
        {
            List<UserProfile> users = userRepo.GetCollabs(projectId);
            VMCollabs result = new VMCollabs();
            result.Collabs = users;
            return result;
        }
    }

    public interface IProjectService
    {
        IQueryable<Project> GetByUserLogin(string login);

        IQueryable<Project> GetNew(int limit);

        IQueryable<Project> GetPopular(int limit);

        ICollection<Project> GetRandom(int limit);


        int Create(Project project, UserProfile userProfile);

        Project GetById(int id);

        List<Activity> GetProjectActivity(int projectId);

        
        ICollection<string> GetFileStatuses(Project project);

        ICollection<string> GetFileAuthors(Project project);

        ICollection<string> GetFileTypes(Project project);

        IList<MoGFile> GetFilteredFiles(Project projet, string filterByAuthor, string filterByStatus, string filterByType);

        VMCollabs GetCollabs(int projectId);
    }
}