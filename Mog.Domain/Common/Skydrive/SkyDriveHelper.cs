using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;

namespace MoG.Domain.Skydrive
{
    public class SkyDriveHelper
    {
        private static string clientId = "00000000401130B4";

        // Make sure this is identical to the redirect_uri parameter passed in WL.init() call.
        private static string callback = "http://dev.test1.com:15545/Home/Login";
        private static string clientSecret = "HG93paHF3jHHIkRB4ZchRKfqtkwEtvl4";

        private static string oauthUrl = "https://login.live.com/oauth20_token.srf";

        private static string skydriveApiUrl = "https://apis.live.net/v5.0/";

        public static void RequestAccessTokenByVerifier(string verifier, out OAuthToken token, out OAuthError error)
        {
            string content = String.Format("client_id={0}&redirect_uri={1}&client_secret={2}&code={3}&grant_type=authorization_code",
                HttpUtility.UrlEncode(clientId),
                HttpUtility.UrlEncode(callback),
                HttpUtility.UrlEncode(clientSecret),
                HttpUtility.UrlEncode(verifier));

            RequestAccessToken(content, out token, out error);
        }

        public static void RequestAccessTokenByRefreshToken(string refreshToken, out OAuthToken token, out OAuthError error)
        {
            string content = String.Format("client_id={0}&redirect_uri={1}&client_secret={2}&refresh_token={3}&grant_type=refresh_token",
                HttpUtility.UrlEncode(clientId),
                HttpUtility.UrlEncode(callback),
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


        public static string GetSkyDriveFolderID(string parentfolderId, string folderName, OAuthToken token, out OAuthError error)
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
                            result = GetSkyDriveFolderID(result, newpathASString, token, out error);
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


        internal static string CreateFolderIfNotExists(string parentfolderId, string folderName, OAuthToken token, out OAuthError error)
        {
            error = null;

            string[] path = folderName.Split('/');
            foreach (var folder in path)
            {
                string folderId = "";
                if (!String.IsNullOrEmpty(folder))
                {
                    folderId = GetSkyDriveFolderID(parentfolderId, folder, token, out error);
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

        public static string UploadFile(string filename, byte[] fileBytes, string path, OAuthToken token, out OAuthError error)
        {
            error = null;
            string result = string.Empty;
            string destinationFolderId = CreateFolderIfNotExists("", path, token, out error);


            var url = string.Format("{0}/{1}/files/{2}?access_token={3}",
                skydriveApiUrl,
                destinationFolderId,
                filename,
                token.AccessToken);


            try
            {
                var request = (HttpWebRequest)HttpWebRequest.Create(url);
                request.Method = "PUT";
                request.ContentLength = fileBytes.Length;


                using (var dataStream = request.GetRequestStream())
                {
                    dataStream.Write(fileBytes, 0, fileBytes.Length);
                }

                WebResponse response = request.GetResponse();

                StreamReader reader = new StreamReader(response.GetResponseStream());

                string json = reader.ReadToEnd();
                var created = JsonConvert.DeserializeObject<dynamic>(json);
                result = created.source;

            }
            catch (Exception e)
            {
                //todo : do something with exception
            }
            return result;

        }

        public static JsonWebToken ReadUserInfoFromAuthToken(OAuthToken token)
        {
            string authenticationToken = token.AuthenticationToken;
            Dictionary<int, string> keys = new Dictionary<int, string>();
            keys.Add(0, clientSecret);

            JsonWebToken jwt = null;
            try
            {
                jwt = new JsonWebToken(authenticationToken, keys);
                return jwt;
            }
            catch (Exception e)
            {
                return null;
            }
        }

    }
}