using MoG.Domain.Models;
using MoG.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoG.Domain.Service
{
    public class DownloadCartService : IDownloadCartService
    {

        private IDownloadCartRepository repoDownloadCart;
        private IFileService serviceFile;

        public DownloadCartService(IDownloadCartRepository downloadRepository, IFileService fileService)
        {
            this.repoDownloadCart = downloadRepository;
            this.serviceFile = fileService;
        }



        public IList<DownloadCartItem> GetByUserId(int userId)
        {
            return this.repoDownloadCart.GetByUserId(userId).ToList();
        }



        public int AddToCart(int fileId, UserProfileInfo user)
        {
            if (fileId <= 0)
                return -1;
            if (user == null || user.Id < 0)
            {
                return -1;
            }
            var preExisitingItem = this.repoDownloadCart.GetByUserId(user.Id).Where(d => d.FileId == fileId).FirstOrDefault();
            if (preExisitingItem != null)
                return preExisitingItem.Id;
            this.serviceFile.IncrementDownloadCount(fileId);
            return this.repoDownloadCart.AddToCart(fileId, user);


        }


        public bool Delete(int id, UserProfileInfo user)
        {
            if (id <= 0)
                return false;
            DownloadCartItem item = this.repoDownloadCart.GetById(id);
            if (item.User.Id != user.Id)
            {
                return false;
            }
            return this.repoDownloadCart.Delete(item);
        }


        public bool ClearCart(int userId)
        {
            if (userId <= 0)
                return false;
            return this.repoDownloadCart.ClearCart(userId);
        }
    }
    public interface IDownloadCartService
    {
        IList<DownloadCartItem> GetByUserId(int userId);


        int AddToCart(int fileId, UserProfileInfo user);

        bool Delete(int id, UserProfileInfo user);

        bool ClearCart(int userId);
    }
}
