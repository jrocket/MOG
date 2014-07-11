using MoG.Domain;
using MoG.Domain.Models;
using MoG.Domain.Service;
using MoG.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace MoG.Controllers
{
    public class ProfileController : FlabbitController
    {
        IDropBoxService serviceDropbox = null;
        ILocalStorageService serviceLocalStorage = null;
        public ProfileController(IUserService userService, IDropBoxService dropboxService
             , ILogService logService
            , ILocalStorageService localStorageService
            )
            : base(userService, logService)
        {
            this.serviceDropbox = dropboxService;
            this.serviceLocalStorage = localStorageService;
        }
        //
        // GET: /Profile/
        public ActionResult Index()
        {
            return View(CurrentUser);
        }

        public ActionResult SetLanguage(string culture)
        {
            // Validate input
            culture = CultureHelper.GetImplementedCulture(culture);
            // Save culture in a cookie
            HttpCookie cookie = Request.Cookies["_culture"];
            if (cookie != null)
                cookie.Value = culture;   // update cookie value
            else
            {
                cookie = new HttpCookie("_culture");
                cookie.Value = culture;
                cookie.Expires = DateTime.Now.AddYears(1);
            }
            Response.Cookies.Add(cookie);
            return RedirectToAction("Index");
        }

        public ActionResult Storage()
        {
            UserStorageVM model = this.serviceUser.GetUserStorages(this.CurrentUser);
            return View(model);
        }

        public ActionResult RegisterStorage(CloudStorageServices service)
        {
            string redirectUrl = "";
            switch (service)
            {
                case CloudStorageServices.Dropbox:
                    int credentialId = -1;
                    string targetUrl = this.Url.Action("RegisterDropboxStorage", "Profile", null, this.Request.Url.Scheme);
                    redirectUrl = this.serviceDropbox.AskForRegistrationUrl(this.CurrentUser, targetUrl, out credentialId);
                    TempData["tempCredentialId"] = credentialId;
                    break;
                case CloudStorageServices.GoogleDrive:
                    return this.RedirectToComingSoon();

            }
            return Redirect(redirectUrl);
        }

        public ActionResult RegisterDropboxStorage(string oauth_token)
        {
            String dropBoxToken = Request["oauth_token"];
            int tempCredentialId = (int)TempData["tempCredentialId"];
            if (String.IsNullOrEmpty(dropBoxToken))
            {
                RedirectToErrorPage("Dropbox authorization failed... your local account is not linked to dropbox");

            }
            this.serviceDropbox.RegisterAccount(tempCredentialId);
            return RedirectToAction("Storage");
        }

        [HttpPost]
        public JsonResult CancelRegistration(int id)
        {
            this.serviceUser.CancelRegistration(id);
            var result = new
            {
                result = true,
                redirectUrl = Url.Action("Storage")
            };

            JsonResult json = new JsonResult() { Data = result };
            return json;
        }

        public ActionResult Avatar(int id = -1)
        {
            UserProfileInfo model = null;
            if (id > 0)
            {
                model = this.serviceUser.GetById(id);
            }
            else
            {
                model = CurrentUser;
            }
            return View(model);
        }


        public ActionResult GetPicture(int id = -1)
        {

            if (id < 0)
                return HttpNotFound();
            var user = this.serviceUser.GetById(id);
            if (user != null)
            {//TODO : move this into the service layer
                System.IO.Stream stream = new System.IO.MemoryStream();
                if (user.PictureUrl.Contains("/Content/"))
                {
                    // Construct absolute image path

                    var imagePath = Server.MapPath("~/Content/Images/NoAvatar.png");
                    var image = System.Drawing.Image.FromFile(imagePath);
                    var resized = BitmapHelper.ResizeImage(image, 370, 211);

                    stream = new System.IO.MemoryStream();
                    resized.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                    stream.Position = 0;


                }
                else
                {
                    //TODO : arggghghh this is ugly!
                    try
                    {
                        string path = String.Format("{0}/avatars/thumbnail_200x200.png", user.Id);
                        if (this.serviceLocalStorage.DownloadFile(ref stream, path, "users"))
                        {

                            stream = BitmapHelper.ResizeImage(stream, 370, 211);
                            stream.Position = 0;

                        }
                    }
                    catch (Exception exc)
                    {
                        this.serviceLog.LogError("ProfileController::GetPicture", exc);
                        return HttpNotFound();
                    }
                   
                }


                return base.File(stream, "image/png");




            }
            return HttpNotFound();




        }

        public ActionResult Alert()
        {
            return View();
        }

        public ActionResult Dashboard()
        {
            return View();
        }


    }
}