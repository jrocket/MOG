using DropNet;
using DropNet.Models;
using MoG.Domain.Models;
using MoG.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoG.Domain.Service
{
    public class DropBoxService : IDropBoxService
    {
        IAuthCredentialRepository repoAuthCredential = null;

        public DropBoxService(IAuthCredentialRepository _repoAuthCredential)
        {
            this.repoAuthCredential = _repoAuthCredential;
        }

        public string AskForRegistrationUrl(UserProfileInfo user, string redirectUrl, out int tempCredentialId)
        {

            var _client = new DropNetClient(MogConstants.DROPBOX_KEY, MogConstants.DROPBOX_SECRET);
            UserLogin login = _client.GetToken();
            // UserLogin login = _client.GetAccessToken();
            var url = _client.BuildAuthorizeUrl(login, redirectUrl);
            var query = repoAuthCredential.GetByUserId(user.Id);
            List<AuthCredential> existingCredentials = null;
            if (query != null)
            {//TODO : gerer le cas des accounts multiples
                existingCredentials = query.Where(a => a.CloudService == CloudStorageServices.Dropbox).ToList();
                foreach (var credential in existingCredentials)
                {
                    repoAuthCredential.Delete(credential);
                }
            }

            AuthCredential newCredential = new AuthCredential();
            newCredential.Token = login.Token;
            newCredential.Secret = login.Secret;
            newCredential.UserId = user.Id;
            newCredential.CloudService = CloudStorageServices.Dropbox;
            this.repoAuthCredential.Create(newCredential);
            tempCredentialId = newCredential.Id;

            return url;
        }


        public void RegisterAccount(int tempCredentialId)
        {
            AuthCredential partialCredential = this.repoAuthCredential.GetById(tempCredentialId);

            try
            {
                UserLogin lg = new UserLogin { Token = partialCredential.Token, Secret = partialCredential.Secret };
                DropNetClient client = new DropNetClient(MogConstants.DROPBOX_KEY, MogConstants.DROPBOX_SECRET);
                client.UserLogin = lg;
                UserLogin accessToken = client.GetAccessToken();

                partialCredential.Token = accessToken.Token;
                partialCredential.Secret = accessToken.Secret;
                partialCredential.Status = CredentialStatus.Approved;
                this.repoAuthCredential.SaveChanges(partialCredential);
            }
            catch (DropNet.Exceptions.DropboxException exc)
            {//
                this.repoAuthCredential.Delete(partialCredential);
                throw new Exception("failed to register accoutn", exc);
            }
           
        }



        public MogFile UploadFile(string filename, int credentialId, string remotePath = null)
        {

            var credential = this.repoAuthCredential.GetById(credentialId);


            return UploadFile(filename, credential, remotePath);

        }

        public MogFile UploadFile(string filename, AuthCredential credential, string remotePath = null)
        {
            //upload file to dropbox
            //if everything is OK, return the file
            byte[] data = System.IO.File.ReadAllBytes(filename);
            return UploadFile(data, filename, credential, remotePath);

        }

        public MogFile UploadFile(byte[] data, string filename, AuthCredential credential, string remotePath = null)
        {
            MetaData meta = this.Upload(data, filename, credential, remotePath);
            if (!String.IsNullOrEmpty(meta.Name))
            {//ok upload went fine

                ProjectFile file = new ProjectFile();
                file.InternalName = meta.Name;
                file.DisplayName = meta.Name;
                file.CreatedOn = DateTime.Now;
                file.Path = meta.Path;

                return file;
            }
            return null;
        }

        public MogFile UploadFile(System.IO.MemoryStream ms, string internalName, AuthCredential authCredential, string path)
        {
            byte[] buffer = ms.ToArray();
            return UploadFile(buffer, internalName, authCredential, path);


        }
        private MetaData Upload(byte[] data, string filename, AuthCredential credential, string remotePath = null)
        {
            UserLogin User = new UserLogin() { Token = credential.Token, Secret = credential.Secret };

            string file = System.IO.Path.GetFileName(filename);


            var _client = new DropNetClient(MogConstants.DROPBOX_KEY, MogConstants.DROPBOX_SECRET);
            _client.UserLogin = User;
            _client.UseSandbox = true;
            if (String.IsNullOrEmpty(remotePath))
            {
                remotePath = "/";
            }
            var uploaded = _client.UploadFile(remotePath, file, data); //FileInfo

            return uploaded;
        }

        public String GetMedialUrl(string path, AuthCredential credential)
        {
            if (String.IsNullOrEmpty(path))
            {
                return null;
            }
            var _client = new DropNetClient(MogConstants.DROPBOX_KEY, MogConstants.DROPBOX_SECRET);
            UserLogin User = new UserLogin() { Token = credential.Token, Secret = credential.Secret };
            _client.UserLogin = User;
            _client.UseSandbox = true;
            ShareResponse response = _client.GetMedia(path);
            return (response != null ? response.Url : null);
        }

        internal void UpdateStats(int Id)
        {
            //AuthCredentialService service = new AuthCredentialService(null, new AuthCredentialRepository());
            //AuthCredentials c = service.GetById(Id);

            //UserLogin lg = new UserLogin { Token = c.Token, Secret = c.Secret };
            //DropNetClient client = new DropNetClient(AppConstants.DROPBOX_KEY, AppConstants.DROPBOX_SECRET);
            //client.UserLogin = lg;
            //AccountInfo info = client.AccountInfo();

            //c.Login = info.display_name;
            //service.Update(c);

        }






        public String RefreshFile(ProjectFile file)
        {
            if (file == null)
            {
                return null;
            }
            string refreshedUrl = GetMedialUrl(file.Path, file.StorageCredential);
            return refreshedUrl;

        }
    }

    public interface IDropBoxService
    {
        string AskForRegistrationUrl(UserProfileInfo user, string redirectUrl, out int tempCredentialId);

        void RegisterAccount(int tempCredentialId);
        MogFile UploadFile(string filename, int credentialId, string remotePath = null);

        MogFile UploadFile(string filename, AuthCredential credential, string remotePath = null);

        MogFile UploadFile(byte[] data, string filename, AuthCredential credential, string remotePath = null);

        MogFile UploadFile(System.IO.MemoryStream ms, string internalName, AuthCredential authCredential, string path);

        String GetMedialUrl(string path, AuthCredential credential);




        String RefreshFile(ProjectFile file);
    }
}
