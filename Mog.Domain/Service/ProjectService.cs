using MoG.Domain.Models;
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

        public ProjectService(IProjectRepository _repo
            , IActivityRepository _activityRepo
            , IFileRepository _fileRepo)
        {
            activytRepo = _activityRepo;
            projectRepo = _repo;
            fileRepo = _fileRepo;
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


        public List<MoGFile> GetProjectFile(int projectId)
        {
            return fileRepo.GetByProjectId(projectId).ToList();
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

        List<MoGFile> GetProjectFile(int p);
    }
}