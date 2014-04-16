using MoG.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace MoG.Domain.Repository
{
    public class NoteRepository : BaseRepository, INoteRepository
    {

        public NoteRepository(IdbContextProvider provider)
            : base(provider)
        {

        }






        public IQueryable<Follow> Get(int userId)
        {
            return this.dbContext.Follows
               .Where(i => i.FollowerId == userId);

        }

        public int Create(Note note)
        {
            note.CreatedOn = DateTime.Now;
            this.dbContext.Notes.Add(note);
            this.dbContext.SaveChanges();
            return note.Id;
        }




        public bool SaveChanges(Note note)
        {
            this.dbContext.Entry(note).State = System.Data.Entity.EntityState.Modified;
            int result = this.dbContext.SaveChanges();
            return result > 0;
        }




        IQueryable<Note> INoteRepository.GetByUser(int userId)
        {
            this.dbContext.Configuration.ProxyCreationEnabled = false;

            return this.dbContext.Notes
                .Include(f => f.Project)
             .Where(i => i.CreatedById == userId);
        }


        public IQueryable<Note> GetByProject(int ProjectId)
        {
            this.dbContext.Configuration.ProxyCreationEnabled = false;
            return this.dbContext.Notes
                .Include(f => f.Project)
              .Where(i => i.ProjectId == ProjectId);
        }

        public Note GetById(int id)
        {
            return this.dbContext.Notes.Find(id);
        }

     



        public bool Delete(Note note)
        {

            if (note != null)
            {
                this.dbContext.Notes.Remove(note);
                this.dbContext.SaveChanges();
                return true;
            }
            return false;
        }





        public Note Get(int projectId, int userId)
        {
            this.dbContext.Configuration.ProxyCreationEnabled = false;
            return this.dbContext.Notes
                .Include(f => f.Project)
              .Where(i => i.ProjectId == projectId && i.CreatedById == userId).FirstOrDefault();
        }
    }

    public interface INoteRepository
    {

        IQueryable<Note> GetByUser(int userId);


        IQueryable<Note> GetByProject(int ProjectId);


        Note GetById(int id);

        int Create(Note note);


        bool SaveChanges(Note note);


      

        bool Delete(Note note);

        Note Get(int projectId, int userId);
    }
}
