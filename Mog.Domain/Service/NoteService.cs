using MoG.Domain.Models;
using MoG.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoG.Domain.Service
{
    public class NoteService : INoteService
    {
        private INoteRepository repo = null;

        public NoteService(INoteRepository repo)
        {
            this.repo = repo;
        }


        public int Note(int projectId, int userId,string message)
        {
           

            Note note = new Note()
            {
                ProjectId = projectId,
                Message = message,
                CreatedById = userId
            };
            int createdId = this.repo.Create(note);

          
            return createdId;
        }



        private bool SaveChanges(Note note)
        {
            return this.repo.SaveChanges(note);
        }
      

        public bool Delete(int id)
        {
            Note note = this.GetById(id);

            return this.repo.Delete(note); ;
        }



        public IList<Note> GetNotesByUser(int userId)
        {
            return this.repo.GetByUser(userId).ToList();
        }

        public IList<Note> GetNotesByProject(int projectId)
        {
            return this.repo.GetByProject(projectId)
                .OrderByDescending(n => n.CreatedOn)
                .ToList();
        }

        public Note GetById(int id)
        {
            return this.repo.GetById(id);
          
        }



        public Note Get(int projectId, int userId)
        {
            return this.repo.Get(projectId, userId);
        }
    }

    public interface INoteService
    {

        int Note(int projectId, int userId,string message);
        IList<Note> GetNotesByUser(int userId);

        Note Get(int projectId, int userId);
        IList<Note> GetNotesByProject(int projectId);
        Note GetById(int id);


        bool Delete(int id);
     
    }
}
