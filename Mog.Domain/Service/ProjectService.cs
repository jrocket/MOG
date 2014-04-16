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
        private IActivityService serviceActivity = null;
        private IFileRepository fileRepo = null;
        private IUserRepository userRepo = null;

        public ProjectService(IProjectRepository _repo
            , IActivityService _activityService
            , IFileRepository _fileRepo
            , IUserRepository _userRepo
            )
        {
            serviceActivity = _activityService;
            projectRepo = _repo;
            fileRepo = _fileRepo;
            userRepo = _userRepo;
        }



        public IQueryable<Project> GetByUserLogin(int pageNumber, int pageSize, string login, bool bExcludePrivate, bool bExcludeDelete = true)
        {
            return projectRepo.GetByUserLogin(pageNumber,pageSize,login,bExcludePrivate,bExcludeDelete);
        }


        public IQueryable<Project> GetNew(int page, int pagesize, bool bExcludePrivate, bool bExcludeDelete = true)
        {
            return projectRepo.GetNew( page,  pagesize,bExcludePrivate,bExcludeDelete);
        }


        public IQueryable<Project> GetPopular(int page, int pagesize, bool bExcludePrivate, bool bExcludeDelete = true)
        {
            return projectRepo.GetPopular(page, pagesize,bExcludePrivate,bExcludeDelete);
        }


        public ICollection<Project> GetRandom(int limit, bool bExcludePrivate, bool bExcludeDelete = true)
        {
            return projectRepo.GetRandom(limit,bExcludePrivate,bExcludeDelete);
        }


        public int Create(Project project, UserProfileInfo userProfile)
        {
            project.CreatedOn = DateTime.Now;
            project.ModifiedOn = DateTime.Now;
            project.Creator = userProfile;
            project.Likes = 0;

            project.ImageUrl = "~/Content/Images/nothingyetbw.png";//http://placehold.it/700*400
            project.ImageUrlThumb1 = "~/Content/Images/nothingyetbw.png";// "http://placehold.it/350x200";

            if (projectRepo.Create(project))
            {
                serviceActivity.LogProjectCreation(project);

                return project.Id;
            }
            return -1;

        }


        public Project GetById(int id)
        {
            return projectRepo.GetById(id);
        }





        public ICollection<string> GetFileStatuses(Project project)
        {
            if (project != null && project.Files != null && project.Files.Count > 0)
            {
                return project.Files.Select(file => file.FileStatus.ToString()).Distinct().ToList();
            }
            return new List<String>();
        }

        public ICollection<string> GetFileAuthors(Project project)
        {
            if (project != null && project.Files != null && project.Files.Count > 0)
            {
                return project.Files.Select(file => file.Creator.DisplayName).Distinct().ToList();
            }
            return new List<String>();
        }

        public ICollection<string> GetFileTags(Project project)
        {
            List<string> result = new List<string>();
            if (project != null && project.Files != null && project.Files.Count > 0)
            {

                var filetags = project.Files
                    .Where(f => f.Deleted == false)
                    .Select(file => (file.Tags != null ? file.Tags.Split(',') : null));
                if (filetags != null)
                {
                    foreach (var filetag in filetags)
                    {
                        if (filetag != null)
                        {
                            foreach (string tag in filetag)
                            {
                                result.Add(tag);
                            }

                        }
                    }

                }
                return result.Distinct().ToList();
            }
            return new List<String>();
        }


        public IList<ProjectFile> GetFilteredFiles(Project project, string filterByAuthor, string filterByStatus, string filterByTag)
        {
            var files = project.Files;
            IEnumerable<ProjectFile> result = files.Where(f => f.Deleted == false);
            if (!String.IsNullOrEmpty(filterByAuthor))
            {
                result = result.Where(f => f.Creator.DisplayName == filterByAuthor);
            }
            if (!String.IsNullOrEmpty(filterByStatus))
            {
                FileStatus testStatus = (FileStatus)Enum.Parse(typeof(FileStatus), filterByStatus);
                result = result.Where(f => f.FileStatus == testStatus);
            }
            if (!String.IsNullOrEmpty(filterByTag))
            {
                //FileType testType = (FileType)Enum.Parse(typeof(FileType), filterByType);
                result = result.Where(f => f.Tags.Contains(filterByTag));
            }

            return result.ToList();
        }


        public VMCollabs GetCollabs(int projectId)
        {
            List<UserProfileInfo> users = userRepo.GetCollabs(projectId);
            VMCollabs result = new VMCollabs();
            result.Collabs = users;
            return result;
        }


        public IList<Activity> GetProjectActivity(int projectId)
        {
            return this.serviceActivity.GetByProjectId(projectId);
        }


        public Project SaveChanges(Project project)
        {
            project.ModifiedOn = DateTime.Now;
            return projectRepo.SaveChanges(project);
        }


        public bool Delete(int id, UserProfileInfo deletor)
        {
            bool result = false;
            Project p = GetById(id);
            if (p != null)
            {
                p.DeletedOn = DateTime.Now;
                p.DeletedBy = deletor;
                p.Deleted = true;
                this.projectRepo.SaveChanges(p);
                result = true;
            }
            return result;
        }



        void IProjectService.IncreaseLike(int projectId)
        {
            Project p = GetById(projectId);
            p.Likes++;
            this.projectRepo.SaveChanges(p);
        }

        public bool ResetLikeCount(int projectId)
        {
            Project p = GetById(projectId);
            p.Likes = 0;
            this.projectRepo.SaveChanges(p);
            return true;
        }


        public bool IsOwner(Project project, UserProfileInfo user)
        {
            return project.Creator.Login == user.Login;
        }


        public bool Promote(int fileId)
        {
            var file = this.fileRepo.GetById(fileId);
            var project = file.Project;
            project.PromotedId = fileId;
            this.SaveChanges(project);
            return true;
        }


        public List<Project> Search(string searchQuery, int page, int pageSize)
        {
            IQueryable<Project> result = this.projectRepo.Search(searchQuery, false,false);
               int skip = (page - 1) * pageSize;
               return result
                   .Skip(skip)
                   .Take(pageSize)
                   .ToList();
        }
    }

    public interface IProjectService
    {
        IQueryable<Project> GetByUserLogin(int PageNumber, int pageSize, string login, bool bExcludePrivate, bool bExcludeDelete = true);

        IQueryable<Project> GetNew(int page, int pagesize, bool bExcludePrivate, bool bExcludeDelete = true);

        IQueryable<Project> GetPopular(int page, int pagesize, bool bExcludePrivate, bool bExcludeDelete = true);

        ICollection<Project> GetRandom(int limit, bool bExcludePrivate, bool bExcludeDelete = true);


        int Create(Project project, UserProfileInfo userProfile);

        Project GetById(int id);

        IList<Activity> GetProjectActivity(int projectId);


        ICollection<string> GetFileStatuses(Project project);

        ICollection<string> GetFileAuthors(Project project);

        ICollection<string> GetFileTags(Project project);

        IList<ProjectFile> GetFilteredFiles(Project projet, string filterByAuthor, string filterByStatus, string filterByTag);

        VMCollabs GetCollabs(int projectId);

        Project SaveChanges(Project project);

        bool Delete(int id, UserProfileInfo deletor);

        void IncreaseLike(int projectId);

        bool ResetLikeCount(int projectId);

        bool IsOwner(Project project, UserProfileInfo user);


        bool Promote(int fileId);

        List<Project> Search(string searchQuery, int page, int pageSize);
    }
}