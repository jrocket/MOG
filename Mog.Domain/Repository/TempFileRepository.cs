using MoG.Exceptions;
using MoG.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoG.Domain.Repository
{
    public class TempFileRepository : BaseRepository, ITempFileRepository
    {


        public TempFileRepository(IdbContextProvider provider)
            : base(provider)
        {
          
        }



        public TempUploadedFile GetById(int id)
        {
            return dbContext.TempUploadedFiles.Find(id);
        }

        public bool Create(TempUploadedFile file)
        {

            string path = System.IO.Path.GetTempPath();
            string filename = Guid.NewGuid().ToString();
            file.Path = System.IO.Path.Combine(path, filename);
            System.IO.File.WriteAllBytes(file.Path, file.Data);
            dbContext.TempUploadedFiles.Add(file);
            int result = dbContext.SaveChanges();

            return (result > 0);
        }

        public bool Delete(TempUploadedFile file)
        {
            System.IO.File.Delete(file.Path);
            dbContext.TempUploadedFiles.Remove(file);
            int result = dbContext.SaveChanges();
            return (result > 0);
        }


        public IQueryable<TempUploadedFile> GetByProjectId(int id, int userId)
        {
            return dbContext.TempUploadedFiles.Where(file => file.ProjectId == id && file.Creator.Id == userId);
        }


        public int SaveChanges(TempUploadedFile data)
        {
            dbContext.Entry(data).State = System.Data.Entity.EntityState.Modified;
            return dbContext.SaveChanges();
        }
    }

    public interface ITempFileRepository
    {
        bool Create(TempUploadedFile file);

        bool Delete(TempUploadedFile file);

        TempUploadedFile GetById(int id);

        IQueryable<TempUploadedFile> GetByProjectId(int id, int userID);

        int SaveChanges(TempUploadedFile data);
    }
}