using MoG.Exceptions;
using MoG.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoG.Domain.Repository
{
    public class ProjectRepository : BaseRepository, IProjectRepository
    {


        public ProjectRepository(IdbContextProvider provider)
            : base(provider)
        {

        }

        public bool Create(Project p)
        {
            dbContext.Projects.Add(p);
            int result = dbContext.SaveChanges();
            return (result > 0);
        }




        public IQueryable<Project> GetByUserLogin(int page, int pagesize, string login, bool bExcludePrivate, bool bExcludeDelete = true)
        {
            IQueryable<Project> allProjects = getAll(bExcludePrivate, bExcludeDelete);
            allProjects = allProjects
               .Where(p => p.Creator.Login == login)
                .OrderByDescending(p => p.CreatedOn);
            if (page > 0)
            {
                allProjects = allProjects

                    .Skip((page - 1) * pagesize)
                    .Take(pagesize);
            }
            return allProjects;
        }

        public IQueryable<Project> GetByUserId(int userID)
        {
            IQueryable<Project> allProjects = getAll(true, true);
            allProjects = allProjects
               .Where(p => p.Creator.Id == userID)
                .OrderByDescending(p => p.CreatedOn);
          
            return allProjects;
        }


        public IQueryable<Project> GetNew(int page, int pagesize, bool bExcludePrivate, bool bExcludeDelete = true)
        {
            IQueryable<Project> allProjects = getAll(bExcludePrivate, bExcludeDelete);
            return allProjects
                .OrderByDescending(p => p.CreatedOn)
                .Skip((page - 1) * pagesize)
                .Take(pagesize);
        }


        public IQueryable<Project> GetPopular(int page, int pagesize, bool bExcludePrivate, bool bExcludeDelete = true)
        {
            IQueryable<Project> allProjects = getAll(bExcludePrivate, bExcludeDelete);

            int skip = (page - 1) * pagesize;
            if (pagesize > 0)
                return allProjects

                     .OrderByDescending(p => p.Likes)
                     .Skip(skip)
                     .Take(pagesize);
            else
                return allProjects

                     .OrderByDescending(p => p.Likes)
                       .Skip(skip);
        }


        public ICollection<Project> GetRandom(int pagesize, bool bExcludePrivate, bool bExcludeDelete = true)
        {


            if (pagesize < 0)
                throw new RepositoryException("GetRandom must specify a valid limit");
            IQueryable<Project> allProjects = getAll(bExcludePrivate, bExcludeDelete);

            List<Project> result = new List<Project>();
            var rndGen = new Random();
            int projectCount = allProjects.Count();

            if (pagesize >= projectCount)
            {//return all the projects
                return allProjects.ToArray();
            }

            for (int i = 0; i < pagesize && i < projectCount; i++)
            {

                int random = rndGen.Next(0, projectCount);
                Project p = allProjects
                    .OrderBy(itm => itm.Id)
                    .Skip(random).First();
                if (result.Where(itm => itm.Id == p.Id).Count<Project>() == 0)
                {
                    result.Add(p);
                }
                else
                {// project was allready in the result ... same player plays again..
                    i--;
                }

            }
            return result;
        }

        private IQueryable<Project> getAll(bool bExcludePrivate, bool bExcludeDelete)
        {
            IQueryable<Project> result = dbContext.Projects;
            if (bExcludePrivate)
            {
                result = result.Where(p => p.VisibilityType != Visibility.Private);
            }
            if (bExcludeDelete)
            {
                result = result.Where(p => p.Deleted == false);

            }
            return result;

        }


        public Project GetById(int id)
        {
            return dbContext.Projects.Find(id);
        }


        public Project SaveChanges(Project project)
        {
            dbContext.Entry(project).State = System.Data.Entity.EntityState.Modified;
            dbContext.SaveChanges();
            return project;
        }

        public IQueryable<Project> Search(string searchQuery, bool includePrivate, bool includeDeleted)
        {
            searchQuery = searchQuery.ToLower();
            var result = this.dbContext.Projects
                 .Where(f => f.Name.ToLower().Contains(searchQuery));
                 

            if (!includeDeleted)
            {
                result = result.Where(x => !x.Deleted);
            }

            if (!includePrivate)
            {
                result = result.Where(x => x.VisibilityType ==Visibility.Public );
            }
            result = result.OrderByDescending(p => p.CreatedOn);

            return result;
            ;
        }
      
    }

    public interface IProjectRepository
    {
        bool Create(Project p);
        IQueryable<Project> GetByUserLogin(int pageNumber, int pageSize, string login, bool bExcludePrivate, bool bExcludeDelete = true);

        IQueryable<Project> GetByUserId(int userID);
        IQueryable<Project> GetNew(int page, int pagesize, bool bExcludePrivate, bool bExcludeDelete = true);

        IQueryable<Project> GetPopular(int page, int pagesize, bool bExcludePrivate, bool bExcludeDelete = true);

        ICollection<Project> GetRandom(int limit, bool bExcludePrivate, bool bExcludeDelete = true);



        Project GetById(int id);

        Project SaveChanges(Project project);



        IQueryable<Project> Search(string searchQuery, bool includePrivate, bool includeDeleted);
    }
}