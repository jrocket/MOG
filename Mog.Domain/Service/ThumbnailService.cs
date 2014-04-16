using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoG.Domain.Service
{
    public class ThumbnailService : IThumbnailService
    {
        private ILocalStorageService serviceLocalStorage = null;
        private IProjectService serviceProject = null;
        private IUserService serviceUser = null;

        public ThumbnailService(ILocalStorageService azureService
            , IProjectService projectService
            , IUserService userService)
        {
            this.serviceLocalStorage = azureService;
            this.serviceProject = projectService;
            this.serviceUser = userService;
        }

        public string StoreTemporaryProjectArtwork(int projectId, Stream data)
        {
            string  result = string.Empty;
            var project = serviceProject.GetById(projectId);
            int[] widths = { 700, 350 };
            int[] heights = { 400, 200 };
            if (project != null)
            {
                for (int i=0; i<widths.Length ; i++)
                {
                    data.Position = 0;
                    string filenameandpath = getTemporaryArtworkFullFileName(projectId, widths[i], heights[i]);
                    using (Stream resized = BitmapHelper.ResizeImage(data, widths[i], heights[i]))
                    {
                        string thumbnailPath = this.serviceLocalStorage.UploadFile(resized, filenameandpath, "projects");
                        result = thumbnailPath;
                    }
                }
            }
            return result;
        }

        public async Task<bool> PromoteTemporaryProjectArtwork(int projectId)
        {
            bool result = false;
            var project = serviceProject.GetById(projectId);
            int[] widths = { 700, 350 };
            int[] heights = { 400, 200 };
            if (project != null)
            {
                for (int i = 0; i < widths.Length; i++)
                {
                    string oldname = getTemporaryArtworkFullFileName(projectId, widths[i], heights[i]);
                    string newname = getPromotedArtworkFullFileName(projectId, widths[i], heights[i]);
                    if (i ==0)
                    {
                        project.ImageUrl = await this.serviceLocalStorage.RenameFile(oldname, newname, "projects");
                    }
                    else
                    {
                        project.ImageUrlThumb1 = await this.serviceLocalStorage.RenameFile(oldname, newname, "projects");
                    }
                   

                }
                serviceProject.SaveChanges(project);
                result = true;
            }
            return result;

        }

        private string getTemporaryArtworkFullFileName(int projectId,int width, int height)
        {
            return String.Format("{0}/thumbnails/thumbnail_{1}x{2}_t.png", projectId,width,height);
        }
        private string getPromotedArtworkFullFileName(int projectId, int width, int height)
        {
            return String.Format("{0}/thumbnails/thumbnail_{1}x{2}.png", projectId, width, height);
        }

        private string getTemporaryAvatarFullFileName(int userId, int width, int height)
        {
            return String.Format("{0}/avatars/thumbnail_{1}x{2}_t.png", userId, width, height);
        }

        private string getPromotedAvatarFullFileName(int userId, int width, int height)
        {
            return String.Format("{0}/avatars/thumbnail_{1}x{2}.png", userId, width, height);
        }


        public string StoreTemporaryAvatar(int userId, Stream stream)
        {
            string result = string.Empty;
            var user = serviceUser.GetById(userId);
            int[] widths = { 200 };
            int[] heights = { 200};
            if (user != null)
            {
                for (int i = 0; i < widths.Length; i++)
                {
                    stream.Position = 0;
                    string filenameandpath = getTemporaryAvatarFullFileName(userId, widths[i], heights[i]);
                    using (Stream resized = BitmapHelper.ResizeImage(stream, widths[i], heights[i]))
                    {
                        string thumbnailPath = this.serviceLocalStorage.UploadFile(resized, filenameandpath, "users");
                        result = thumbnailPath;
                    }
                }
            }
            return result;
        }


        public async  Task<bool> PromoteTemporaryAvatar(int userId)
        {
            bool result = false;
            var user = serviceUser.GetById(userId);
            int[] widths = { 200 };
            int[] heights = {200};
            if (user != null)
            {
                for (int i = 0; i < widths.Length; i++)
                {
                    string oldname = getTemporaryAvatarFullFileName(userId, widths[i], heights[i]);
                    string newname = getPromotedAvatarFullFileName(userId, widths[i], heights[i]);
                    if (i ==0)
                    {
                        user.PictureUrl = await this.serviceLocalStorage.RenameFile(oldname, newname, "users");
                    }
                }
                serviceUser.SaveChanges(user);
                result = true;
            }
            return result;
        }
    }

    public interface IThumbnailService
    {
        string StoreTemporaryProjectArtwork(int projectId, Stream data);

         Task<bool> PromoteTemporaryProjectArtwork(int projectId);


         string StoreTemporaryAvatar(int userId, Stream stream);

         Task<bool> PromoteTemporaryAvatar(int userId);
    }
}
