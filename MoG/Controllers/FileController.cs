using MoG.Domain.Models;
using MoG.Domain.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MoG.Controllers
{
    [MogAuthAttribut]
    public class FileController : MogController
    {
        private IProjectService serviceProject = null;
        private IFileService serviceFile = null;
        private ITempFileService serviceTempFile = null;
        private IDropBoxService serviceDropBox = null;
        private IThumbnailService serviceThumbnail = null;
       


        public FileController(IFileService _fileService
            , IUserService userService
            , IProjectService projectService
            , ITempFileService tempFileService
            , IDropBoxService dropboxService
            , IThumbnailService thumbnailService
            , ILogService logService
            )
            : base(userService, logService)
        {
            serviceFile = _fileService;
            serviceProject = projectService;
            serviceTempFile = tempFileService;
            serviceDropBox = dropboxService;
            serviceThumbnail = thumbnailService;
        }
        public ActionResult Display(int id = -1)
        {
            var file = serviceFile.GetById(id);
            VMFile model = getViewModel(file);
            //ViewBag.Comments = serviceFile.GetFileComments(id);

            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var file = serviceFile.GetById(id);
            //TODO automapper
            VMFile model = getViewModel(file);

            return PartialView(model);
        }

        private VMFile getViewModel(ProjectFile file)
        {
            VMFile model = new VMFile();
            //TODO automapper
            model.CreatedOn = file.CreatedOn;
            model.Creator = file.Creator;
            model.Deleted = file.Deleted;
            model.DeletedBy = file.DeletedBy;
            model.DeletedById = file.DeletedById;
            model.DeletedOn = file.DeletedOn;
            model.Description = file.Description;
            model.DisplayName = file.DisplayName;
            model.DownloadCount = file.DownloadCount;
            model.FileStatus = file.FileStatus;
            model.Id = file.Id;
            model.InternalName = file.InternalName;
            model.Likes = file.Likes;
            model.ModifiedOn = file.ModifiedOn;
            model.PlayCount = file.PlayCount;
            model.Project = file.Project;
            model.ProjectId = file.ProjectId;
            model.Tags = file.Tags;
            model.Metadata = file.Metadata;
            model.MetadataType = file.MetadataType;

            if (file.ThumbnailId != null)
            {
                model.ThumbnailUrl = file.Thumbnail.PublicUrl;//this.serviceDropBox.GetMedialUrl(file.Thumbnail.Path, file.StorageCredential);

            }
            else
            {
                model.ThumbnailUrl = "~/Content/Images/thumbnail_temp.png";
            }

            model.PublicUrl = file.PublicUrl;// this.serviceDropBox.GetMedialUrl(file.Path, file.StorageCredential);

            model.isPendingProcessing = file.TempFileId != null;
            return model;
        }

        [HttpPost]
        public ActionResult Edit(int id, string tags, string description, string name)
        {
            var file = serviceFile.GetById(id);
            file.Tags = tags;
            file.Description = description;
            file.InternalName = name;
            file.DisplayName = name;
            serviceFile.SaveChanges(file);
            VMFile model = getViewModel(file);
            return PartialView("Detail", model);
        }

        public ActionResult Detail(int id)
        {
            var file = serviceFile.GetById(id);
            return PartialView(file);
        }

        public JsonResult GetComments(int id = 1)
        {
            var comments = serviceFile.GetFileComments(id);
            var result = new JsonResult() { Data = comments, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            return result;
        }

        public ActionResult Create(int id)
        {
            this.serviceLog.LogMessage("FileController", "Create " + id);
            var project = serviceProject.GetById(id);
            VMFileCreate model = new VMFileCreate() { Project = project };
            var userStorages = this.serviceUser.GetUserStorages(CurrentUser);
           
            var items = userStorages.CloudStorages
                .Where(c => c.Status == CredentialStatus.Approved)
                .Select(c => new { id = c.Id, value = c.CloudService + "-" + c.Login });

            if (items.Count()>0)
            {
                SelectList CloudStorages = new SelectList(items, "id", "value");
                ViewBag.CloudStorages = CloudStorages;
            }
            else
            {
                ViewBag.CloudStorages = null;
            }
            
            return View(model);
        }

        [HttpPost]
        public ActionResult Create2(int id)
        {
            this.serviceLog.LogMessage("FileController", "Create2 " + id);
            ViewBag.ProjectId = id;

            List<VMFileCreate> model = new List<VMFileCreate>();
            var tmpFiles = this.serviceTempFile.GetByProjectId(id, CurrentUser);
            if (tmpFiles == null || tmpFiles.Count == 0)
            {
                return this.RedirectToErrorPage("Il faudrait peut etre ajouter des fichiers!");
            }
            foreach (var tmpFile in tmpFiles)
            {
                var modelFile = new VMFileCreate();

                modelFile.File = new ProjectFile();
                modelFile.File.DisplayName = modelFile.File.InternalName = System.IO.Path.GetFileNameWithoutExtension(tmpFile.Name);

                modelFile.Project = new Project() { Id = id };
                modelFile.File.Id = tmpFile.Id;
                model.Add(modelFile);
            }
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult Create3(List<VMFileCreate> files)
        {
            string log = "Create3 ";
            log += files!=null ? "fileCount = "+ files.Count : String.Empty;
            this.serviceLog.LogMessage("FileController", log);
          
            if (files == null)
            {
                return this.RedirectToErrorPage("Il faudrait peut etre ajouter de fichiers!");
            }


            foreach (var file in files)
            {//TODO : Automapper
                TempUploadedFile modelFile = new TempUploadedFile()
                {
                    Description = file.File.Description,
                    Tags = file.File.Tags,
                    Name = file.File.DisplayName,
                    Id = file.File.Id,
                    ProjectId = file.File.ProjectId,


                };
                modelFile = this.serviceTempFile.Update(modelFile);

                this.serviceFile.Create(modelFile, CurrentUser);


            }

            return RedirectToAction("Files", "Project", new { id = files[0].Project.Id });
        }


        public ActionResult CancelUpload(int id)
        {
            this.serviceLog.LogMessage("FileController", "CancelUpload " + id);
            this.serviceTempFile.Cancel(id, CurrentUser);
            return PartialView();
        }

        //[HttpPost]
        //public ActionResult Create(VMFile data, HttpPostedFileBase file)
        //{
        //    var project = serviceProject.GetById(data.Project.Id);
        //    data.File.Project = project;
        //    int fileId = this.serviceFile.Create(data.File, this.CurrentUser);
        //    if (fileId > 0)
        //    {
        //        return RedirectToAction("Detail", new { id = fileId });
        //    }
        //    return View(data);

        //}

        [HttpPost]
        public JsonResult Accept(int fileId)
        {
            JsonResult result = new JsonResult();
            if (this.serviceFile.Accept(fileId))
            {
                result.Data = true;
            }
            else
            {
                result.Data = false;
            }
            return result;
        }

        [HttpPost]
        public JsonResult Reject(int fileId)
        {
            JsonResult result = new JsonResult();
            if (this.serviceFile.Reject(fileId))
            {
                result.Data = true;
            }
            else
            {
                result.Data = false;
            }
            return result;
        }

        [HttpPost]
        public JsonResult Delete(int fileId)
        {
            JsonResult result = new JsonResult();
            var file = this.serviceFile.GetById(fileId);
            if (file != null)
            {
                int projectId = file.ProjectId;

                if (this.serviceFile.Delete(fileId, this.CurrentUser))
                {
                    result.Data = new
                    {
                        Flag = true,
                        Url = Url.Action("Files", "Project",
                            new
                            {
                                id = projectId
                            })
                    };

                }
                else
                {
                    result.Data = false;
                }


            }
            else
            {
                throw new Exception("NOT THE RIGHT FILE");
            }
            return result;

        }

        public JsonResult UploadArtwork(IEnumerable<HttpPostedFileBase> files, int projectId)
        {
            this.serviceLog.LogMessage("FileCOntroller", "UploadArtwork " + projectId);
            VMFileUpload result = new VMFileUpload();
            result.files = new List<UploadedFile>();
            if (files != null)
            {
                // only take the first one
                foreach (var baseFile in files)
                {
                    string thumbnailUrl = this.serviceThumbnail.StoreTemporaryProjectArtwork(projectId, baseFile.InputStream);
                    if (!String.IsNullOrEmpty(thumbnailUrl))
                    {
                        UploadedFile f = new UploadedFile()
                        {
                            name = thumbnailUrl,
                            size = baseFile.ContentLength,
                            thumbnailUrl = thumbnailUrl,
                            deleteUrl = "TODO",
                            deleteType = "POST"
                        };
                        result.files.Add(f);
                    }
                    break;
                }
            }

            return new JsonResult() { Data = result };
        }

        [HttpPost]
        public async Task<JsonResult> SaveArtwork(int projectId)
        {
            JsonResult json = new JsonResult();
            bool result = await this.serviceThumbnail.PromoteTemporaryProjectArtwork(projectId);
            string redirectUrl = Url.Action("Detail", "Project", new { id = projectId });
            json.Data = new { data = result, url = redirectUrl };
            return json;
        }


        public JsonResult UploadAvatar(IEnumerable<HttpPostedFileBase> files, int userId)
        {//TODO Rework (fusion with UploadArtword)
            VMFileUpload result = new VMFileUpload();
            result.files = new List<UploadedFile>();
            if (files != null)
            {
                // only take the first one
                foreach (var baseFile in files)
                {
                    string thumbnailUrl = this.serviceThumbnail.StoreTemporaryAvatar(userId, baseFile.InputStream);
                    if (!String.IsNullOrEmpty(thumbnailUrl))
                    {
                        UploadedFile f = new UploadedFile()
                        {
                            name = thumbnailUrl,
                            size = baseFile.ContentLength,
                            thumbnailUrl = thumbnailUrl,
                            deleteUrl = "TODO",
                            deleteType = "POST"
                        };
                        result.files.Add(f);
                    }
                    break;
                }
            }

            return new JsonResult() { Data = result };
        }

        [HttpPost]
        public async Task<JsonResult> SaveAvatar(int userId)
        {//TODO rework (fusion with SaveArtwork)
            JsonResult json = new JsonResult();
            bool result = await this.serviceThumbnail.PromoteTemporaryAvatar(userId);
            string redirectUrl = Url.Action("Manage", "Account", new { id = userId });
            json.Data = new { data = result, url = redirectUrl };
            return json;
        }



        public JsonResult Upload(IEnumerable<HttpPostedFileBase> files, int id, int cloudStorage)
        {
            string log = "Upload file to "+id + " into " + cloudStorage;
            log += files != null ? "count = " + files.Count(): string.Empty;
            this.serviceLog.LogMessage("FileController", log);
        
            VMFileUpload result = new VMFileUpload();
            result.files = new List<UploadedFile>();
            foreach (var baseFile in files)
            {
                TempUploadedFile tmpFile = new TempUploadedFile();

                using (var binaryReader = new BinaryReader(baseFile.InputStream))
                {
                    tmpFile.Data = binaryReader.ReadBytes(baseFile.ContentLength);
                }
                tmpFile.Name = baseFile.FileName;
                tmpFile.ProjectId = id;
                tmpFile.Size = tmpFile.Data.Length;
                tmpFile.AuthCredentialId = cloudStorage;

                tmpFile.Id = this.serviceTempFile.Create(tmpFile, CurrentUser);
                UploadedFile f = new UploadedFile()
                {
                    name = tmpFile.Name,
                    size = tmpFile.Size,
                    //url = Url.Action("GetTempFile", new { id = tmpFile.Id }),
                    //thumbnailUrl = "http//:www.google.com/picture1.jpg",
                    deleteUrl = Url.Action("DeleteTempFile", new { id = tmpFile.Id }),
                    deleteType = "POST"
                };
                result.files.Add(f);

            }



            return new JsonResult() { Data = result };
        }

        [HttpPost]
        public JsonResult DeleteTempFile(int id)
        {

            this.serviceTempFile.DeleteById(id);


            return new JsonResult() { Data = true };
        }

        public JsonResult GetUploaded(int id = 1)
        {
            VMFileUpload result = new VMFileUpload();
            result.files = new List<UploadedFile>();
            var tmpFiles = this.serviceTempFile.GetByProjectId(id, CurrentUser);
            foreach (var file in tmpFiles)
            {

                UploadedFile f = new UploadedFile()
                {
                    name = file.Name,
                    size = file.Size,
                    //url = Url.Action("GetTempFile", new { id = tmpFile.Id }),
                    //thumbnailUrl = "http//:www.google.com/picture1.jpg",
                    deleteUrl = Url.Action("DeleteTempFile", new { id = file.Id }),
                    deleteType = "POST"
                };
                result.files.Add(f);
            }


            return new JsonResult() { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public ActionResult AddToDownloadCart(int id)
        {

            return View();
        }

        [HttpPost]
        public JsonResult Played(string url)
        {
            this.serviceFile.IncrementPlayCount(url);
            return new JsonResult(){Data = url};
        }
    }
}