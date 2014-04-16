
using DropNet;
using DropNet.Models;
using Microsoft.Live;
using MoG.Domain.Models;
using MoG.Domain.Repository;
using MoG.Domain.Skydrive;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MoG.Domain.Service
{
    public class SkydriveService : ISkydriveService
    {
        IAuthCredentialRepository repoAuthCredential = null;
        private static string clientId = "00000000401130B4";

        private string[] scopes = { "wl.signin", "wl.skydrive_update", "wl.offline_access" };
        //private static string callback = "http://dev.test1.com:15545/Home/Login";
        private static string clientSecret = "HG93paHF3jHHIkRB4ZchRKfqtkwEtvl4";
        private static string oauthUrl = "https://login.live.com/oauth20_token.srf";
        private static string skydriveApiUrl = "https://apis.live.net/v5.0/";



        public SkydriveService(IAuthCredentialRepository _repoAuthCredential)
        {
            this.repoAuthCredential = _repoAuthCredential;
        }

        public string AskForRegistrationUrl(UserProfileInfo user, string redirectUrl, out int tempCredentialId)
        {
            LiveAuthClient auth = new LiveAuthClient(clientId, clientSecret, redirectUrl);
            string url = auth.GetLoginUrl(scopes);
            tempCredentialId = -1;

            return url;
        }

        public void RequestAccessTokenByVerifier(string verifier, string redirectUrl, out OAuthToken token, out OAuthError error)
        {
            string content = String.Format("client_id={0}&redirect_uri={1}&client_secret={2}&code={3}&grant_type=authorization_code",
                HttpUtility.UrlEncode(clientId),
                HttpUtility.UrlEncode(redirectUrl),
                HttpUtility.UrlEncode(clientSecret),
                HttpUtility.UrlEncode(verifier));

            RequestAccessToken(content, out token, out error);
        }

        public void RequestAccessTokenByRefreshToken(string refreshToken, string redirectUrl, out OAuthToken token, out OAuthError error)
        {
            string content = String.Format("client_id={0}&redirect_uri={1}&client_secret={2}&refresh_token={3}&grant_type=refresh_token",
                HttpUtility.UrlEncode(clientId),
                HttpUtility.UrlEncode(redirectUrl),
                HttpUtility.UrlEncode(clientSecret),
                HttpUtility.UrlEncode(refreshToken));
            RequestAccessToken(content, out token, out error);
        }

        private static void RequestAccessToken(string postContent, out OAuthToken token, out OAuthError error)
        {
            token = null;
            error = null;

            HttpWebRequest request = WebRequest.Create(oauthUrl) as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";

            try
            {
                using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
                {
                    writer.Write(postContent);
                }

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                if (response != null)
                {
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(OAuthToken));
                    token = serializer.ReadObject(response.GetResponseStream()) as OAuthToken;
                    if (token != null)
                    {
                        return;
                    }
                }
            }
            catch (WebException e)
            {
                HttpWebResponse response = e.Response as HttpWebResponse;
                if (response != null)
                {
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(OAuthError));
                    error = serializer.ReadObject(response.GetResponseStream()) as OAuthError;
                }
            }
            catch (IOException)
            {
            }

            if (error == null)
            {
                error = new OAuthError("request_failed", "Failed to retrieve user access token.");
            }
        }



        public void RegisterAccount(OAuthToken token, UserProfileInfo user)
        {
            LiveAuthClient auth = new LiveAuthClient(clientId, clientSecret, MogConstants.SKYDRIVE_REDIRECTURL);
             
            AuthCredential credential = new AuthCredential();
            credential.CloudService = CloudStorageServices.Skydrive;
            credential.Token = token.AccessToken;
            credential.Refresh = token.RefreshToken;
            credential.Authentication = token.AuthenticationToken;
            credential.Status = CredentialStatus.Approved;
            credential.UserId = user.Id;
            credential.Login = auth.GetUserId(token.AuthenticationToken); 
            this.repoAuthCredential.Create(credential);
        }



        public MogFile UploadFile(string filename, int credentialId, string remotePath = null)
        {

            var credential = this.repoAuthCredential.GetById(credentialId);


            return UploadFile(filename, credential, remotePath);

        }

        public MogFile UploadFile(string filename, AuthCredential credential, string remotePath = null)
        {
            //upload file to skydrive
            //if everything is OK, return the file
            byte[] data = System.IO.File.ReadAllBytes(filename);
            return UploadFile(data, filename, credential, remotePath);

        }

        public MogFile UploadFile(byte[] data, string filename, AuthCredential credential, string remotePath = null)
        {
            dynamic meta = this.Upload(data, filename, credential, remotePath);
            if (meta != null && !String.IsNullOrEmpty(meta.source))
            {//ok upload went fine

                ProjectFile file = new ProjectFile();
                file.InternalName = meta.name;
                file.DisplayName = meta.name;
                file.CreatedOn = DateTime.Now;
                file.Path = meta.id;

                return file;
            }
            return null;
        }

        public MogFile UploadFile(System.IO.MemoryStream ms, string internalName, AuthCredential authCredential, string path)
        {
            byte[] buffer = ms.ToArray();
            return UploadFile(buffer, internalName, authCredential, path);


        }


        public String RefreshFile(ProjectFile file)
        {
            return file.PublicUrl;
        }

        public string getSkyDriveFolderID(string parentfolderId, string folderName, OAuthToken token, out OAuthError error)
        {
            string result = "";
            error = null;
            string[] path = folderName.Split('/');
            string url = string.Format("{0}/me/skydrive/files?filter=folders&access_token={1}", skydriveApiUrl, token.AccessToken);

            if (!string.IsNullOrEmpty(parentfolderId))
            {//this is a child folder
                url = string.Format("{0}/{1}/files?filter=folders&access_token={2}", skydriveApiUrl, parentfolderId, token.AccessToken);
            }

            try
            {
                var request = (HttpWebRequest)HttpWebRequest.Create(url);
                request.Method = "GET";

                //var result = (HttpWebResponse)request.GetResponse();
                WebResponse response = request.GetResponse();

                StreamReader reader = new StreamReader(response.GetResponseStream());

                string json = reader.ReadToEnd();
                var folders = JsonConvert.DeserializeObject<dynamic>(json);
                foreach (var folder in folders.data)
                {
                    if (String.Compare(path[0], folder.name.ToString(), true) == 0)
                    {
                        result = folder.id;
                        if (path.Length - 1 > 0)
                        {
                            var newpath = path.Skip(1).Take(path.Length - 1);
                            string newpathASString = string.Join("/", newpath);
                            result = getSkyDriveFolderID(result, newpathASString, token, out error);
                        }
                    }
                }
            }
            catch (WebException e)
            {//maybe a expired token ... refresh the token and retry
                if (e.Message.Contains("(401) Unauthorized"))
                {
                    throw new UnauthorizedAccessException("(401) Unauthorized", e);
                }

            }

            return result;

        }


        internal  string createFolderIfNotExists(string parentfolderId, string folderName, OAuthToken token, out OAuthError error)
        {
            error = null;

            string[] path = folderName.Split('/');
            foreach (var folder in path)
            {
                string folderId = "";
                if (!String.IsNullOrEmpty(folder))
                {
                    folderId = getSkyDriveFolderID(parentfolderId, folder, token, out error);
                    if (!String.IsNullOrEmpty(folderId))
                    {//folder exists, lets go to the next level
                        parentfolderId = folderId;

                    }
                    else
                    {
                        //folder does not exists
                        try
                        {

                            string url = string.Format("{0}/{1}",
             skydriveApiUrl,
             parentfolderId);
                            if (String.IsNullOrEmpty(parentfolderId))
                            {
                                url = string.Format("{0}/me/skydrive", skydriveApiUrl);

                            }

                            var request = (HttpWebRequest)HttpWebRequest.Create(url);
                            request.Method = "POST";

                            string postData = string.Format("{{name: \"{0}\"}}", folder);
                            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                            request.Headers.Add("Authorization", "Bearer " + token.AccessToken);
                            request.ContentType = "application/json";
                            request.ContentLength = byteArray.Length;

                            using (var dataStream = request.GetRequestStream())
                            {
                                dataStream.Write(byteArray, 0, byteArray.Length);
                            }
                            WebResponse response = request.GetResponse();

                            StreamReader reader = new StreamReader(response.GetResponseStream());

                            string json = reader.ReadToEnd();
                            var created = JsonConvert.DeserializeObject<dynamic>(json);
                            parentfolderId = created.id;

                        }
                        catch (Exception e)
                        {
                            //todo : do something with exception
                        }
                    }
                }

            }
            return parentfolderId;
        }

        private object Upload(byte[] data, string filename, AuthCredential credential, string remotePath = null)
        {
              string file = System.IO.Path.GetFileName(filename);
              OAuthToken token = new OAuthToken();
              token.AccessToken = credential.Token;
              token.AuthenticationToken = credential.Authentication;
              token.RefreshToken = credential.Refresh;
              dynamic result = null;
              OAuthError error = null;
              string destinationFolderId = null;
              try
              {
                   destinationFolderId = createFolderIfNotExists("", remotePath, token, out error);
              }
              catch (UnauthorizedAccessException )
              {
                  RequestAccessTokenByRefreshToken(token.RefreshToken, MogConstants.SKYDRIVE_REDIRECTURL, out token, out error);
                  credential.Token = token.AccessToken;
                  credential.Authentication = token.AuthenticationToken;
                  credential.Refresh = token.RefreshToken;
                  this.repoAuthCredential.SaveChanges(credential);
                  destinationFolderId = createFolderIfNotExists("", remotePath, token, out error);
            }
             


              var url = string.Format("{0}/{1}/files/{2}?access_token={3}",
                  skydriveApiUrl,
                  destinationFolderId,
                  filename,
                  token.AccessToken);


              try
              {
                  var request = (HttpWebRequest)HttpWebRequest.Create(url);
                  request.Method = "PUT";
                  request.ContentLength = data.Length;


                  using (var dataStream = request.GetRequestStream())
                  {
                      dataStream.Write(data, 0, data.Length);
                  }

                  WebResponse response = request.GetResponse();

                  StreamReader reader = new StreamReader(response.GetResponseStream());

                  string json = reader.ReadToEnd();
                  result = JsonConvert.DeserializeObject<dynamic>(json);

              }
              catch (Exception e)
              {
                  //todo : do something with exception
              }
              return result;

        }

        public String GetMedialUrl(string path, AuthCredential credential)
        {
            return null;
//                return (response != null ? response.Url : null);
        }

        internal void UpdateStats(int Id)
        {
           
        }






    }

    public interface ISkydriveService
    {
        string AskForRegistrationUrl(UserProfileInfo user, string redirectUrl, out int tempCredentialId);
        void RequestAccessTokenByVerifier(string verifier, string redirectUrl, out OAuthToken token, out OAuthError error);
        void RequestAccessTokenByRefreshToken(string refreshToken, string redirectUrl, out OAuthToken token, out OAuthError error);


        MogFile UploadFile(string filename, int credentialId, string remotePath = null);

        MogFile UploadFile(string filename, AuthCredential credential, string remotePath = null);

        MogFile UploadFile(byte[] data, string filename, AuthCredential credential, string remotePath = null);

        MogFile UploadFile(System.IO.MemoryStream ms, string internalName, AuthCredential authCredential, string path);

        String GetMedialUrl(string path, AuthCredential credential);




        void RegisterAccount(OAuthToken token, UserProfileInfo CurrentUser);

        string RefreshFile(ProjectFile file);
    }

}
