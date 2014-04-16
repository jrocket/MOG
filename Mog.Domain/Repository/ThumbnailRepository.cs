using MoG.Exceptions;
using MoG.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoG.Domain.Repository
{
    public class ThumbnailRepository : BaseRepository, IThumbnailRepository
    {


        public ThumbnailRepository(IdbContextProvider provider)
            : base(provider)
        {
          
        }



        public Thumbnail GetById(int id)
        {
            return dbContext.Thumbnails.Find(id);
        }

        public bool Create(Thumbnail thumb)
        {
            dbContext.Thumbnails.Add(thumb);
            int result = dbContext.SaveChanges();

            return (result > 0);
        }

        public bool Delete(Thumbnail thumb)
        {

            dbContext.Thumbnails.Remove(thumb);
            int result = dbContext.SaveChanges();
            return (result > 0);
        }



        public int SaveChanges(Thumbnail data)
        {
            dbContext.Entry(data).State = System.Data.Entity.EntityState.Modified;
            return dbContext.SaveChanges();
        }
    }

    public interface IThumbnailRepository
    {
        bool Create(Thumbnail thumb);

        bool Delete(Thumbnail thumb);

        Thumbnail GetById(int id);



        int SaveChanges(Thumbnail thumb);
    }
}