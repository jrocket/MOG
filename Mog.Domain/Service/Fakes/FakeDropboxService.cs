using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoG.Domain.Service
{
    public class FakeDropboxService : IDropBoxService
    {
        public string AskForRegistrationUrl(Models.UserProfileInfo user, string redirectUrl, out int tempCredentialId)
        {
            tempCredentialId = -1;
            return "http://www.RegisterSomewher.com";
        }

        public void RegisterAccount(int tempCredentialId)
        {
            return;
        }

        public Models.MogFile UploadFile(string filename, int credentialId, string remotePath = null)
        {
            Models.MogFile file = new Models.MogFile();
            file.AuthCredentialId = -1;
            file.DisplayName = "fakely uploaded";
            file.Id = 42;
            file.InternalName = "fakely uploaded";
            file.Path = "/fake";
            file.PublicUrl = "http://www.google.com";
            file.TempFileId = 1;
            return file;
        }

        public Models.MogFile UploadFile(string filename, Models.AuthCredential credential, string remotePath = null)
        {
            return this.UploadFile(filename, credential.Id, remotePath);
        }

        public Models.MogFile UploadFile(byte[] data, string filename, Models.AuthCredential credential, string remotePath = null)
        {
            return this.UploadFile(filename, credential.Id, remotePath);
        }

        public Models.MogFile UploadFile(System.IO.MemoryStream ms, string internalName, Models.AuthCredential authCredential, string path)
        {
            return this.UploadFile(internalName, authCredential.Id, path);
        }

        public string GetMedialUrl(string path, Models.AuthCredential credential)
        {
            return "www.yahpp.fr";
        }

        public string RefreshFile(Models.ProjectFile file)
        {
            return String.Empty;
        }
    }
}
