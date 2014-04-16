using MoG.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoG.Domain.Repository
{
    public class DownloadCartRepository : BaseRepository, IDownloadCartRepository
    {

        public DownloadCartRepository(IdbContextProvider provider)
            : base(provider)
        {

        }



        public IQueryable<DownloadCartItem> GetByUserId(int userId)
        {
            return this.dbContext.DownloadCarts.Where(d => d.User.Id == userId);
        }


        public int AddToCart(int fileId, UserProfileInfo user)
        {
            DownloadCartItem item = new DownloadCartItem();
            item.FileId = fileId;
            item.User = user;
            item = this.dbContext.DownloadCarts.Add(item);
            this.dbContext.SaveChanges();
            return item.Id;
        }


        public bool Delete(DownloadCartItem item)
        {

                this.dbContext.DownloadCarts.Remove(item);
                this.dbContext.SaveChanges();
                return true;
           
        }

        public bool ClearCart(int userId)
        {

            var result = this.dbContext.DownloadCarts.Where(d => d.User.Id == userId);
            //check if there is a record with this id

            if (result.Count() > 0)
            {

                foreach (DownloadCartItem item in result)
                {
                    this.dbContext.DownloadCarts.Remove(item);
                    this.dbContext.SaveChanges();
                } 
                return true;
            }
            return false;
        }



        public DownloadCartItem GetById(int id)
        {
            return this.dbContext.DownloadCarts.Where(d => d.Id == id).FirstOrDefault();
        }
    }

    public interface IDownloadCartRepository
    {


        IQueryable<DownloadCartItem> GetByUserId(int userId);

        int AddToCart(int fileId, UserProfileInfo user);

        bool Delete(DownloadCartItem item);

        bool ClearCart(int userId);

        DownloadCartItem GetById(int id);
    }
}