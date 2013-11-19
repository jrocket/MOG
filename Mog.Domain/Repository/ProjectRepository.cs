using MoG.Exceptions;
using MoG.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoG.Domain.Repository
{
    public class ProjectRepository : IProjectRepository, IDisposable
    {
        private MogDbContext dbContext = null;
        IdbContextProvider contextProvider = null;


        public ProjectRepository(IdbContextProvider provider)
        {
            contextProvider = provider;
            dbContext = contextProvider.GetCurrent();
        }

        public bool Create(Project p)
        {
            dbContext.Projects.Add(p);
            int result = dbContext.SaveChanges();
            return (result > 0);
        }



        public void Dispose()
        {
            dbContext.Dispose();
        }


        public IQueryable<Project> GetByUserLogin(string login)
        {
            return dbContext.Projects.Where(p => p.Creator.Login == login);
        }


        public IQueryable<Project> GetNew(int limit)
        {

            if (limit > 0)
                return dbContext.Projects.OrderByDescending(p => p.CreatedOn).Take(limit);
            else
                return dbContext.Projects.OrderByDescending(p => p.CreatedOn);
        }


        public IQueryable<Project> GetPopular(int limit)
        {
            if (limit > 0)
                return dbContext.Projects.OrderByDescending(p => p.Likes).Take(limit);
            else
                return dbContext.Projects.OrderByDescending(p => p.Likes);
        }


        public ICollection<Project> GetRandom(int limit)
        {
            if (limit < 0)
                throw new RepositoryException("GetRandom must specify a valid limit");
            List<Project> result = new List<Project>();
            var rndGen = new Random();
            int projectCount = dbContext.Projects.Count();

            if (limit >=projectCount)
            {//return all the projects
                return dbContext.Projects.ToArray();
            }

            for (int i=0;i<limit && i<projectCount;i++)
            {

                int random = rndGen.Next(0, projectCount);
                Project p = dbContext
                    .Projects
                    .OrderBy(itm => itm.Id)
                    .Skip(random).First();
                if (result.Where (itm=>itm.Id == p.Id).Count<Project>() ==0)
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





        public Project GetById(int id)
        {
            return dbContext.Projects.Find(id);
        }
    }

    public interface IProjectRepository
    {
        bool Create(Project p);
        IQueryable<Project> GetByUserLogin(string login);

        IQueryable<Project> GetNew(int limit);

        IQueryable<Project> GetPopular(int limit);

        ICollection<Project> GetRandom(int limit);



        Project GetById(int id);
    }
}