using MoG.Domain.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MoG.Controllers
{
    [MogAuthAttribut]
    public class NoteController : FlabbitController
    {
        private INoteService serviceNote = null;

        public NoteController(INoteService noteService, IUserService userService
 , ILogService logService
            )
            : base(userService, logService)
        {
            this.serviceNote = noteService;
        }

        //
        // GET: /Follow/
        /// <summary>
        /// Get notes for a project
        /// </summary>
        /// <param name="id"> project Id</param>
        /// <returns></returns>
        public JsonResult GetNotes(int id)
        {
            JsonResult result = new JsonResult();
            result.Data = this.serviceNote.GetNotesByProject(id);
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        public JsonResult GetUserNotes(int id = -1)
        {
            JsonResult result = new JsonResult();
            if (id < 0)
            {
                id = CurrentUser.Id;
            }
            var NotesItems = this.serviceNote.GetNotesByUser(id);
            foreach (var noteItem in NotesItems)
            {
                noteItem.Project.ImageUrlThumb1 = Url.Content(noteItem.Project.ImageUrlThumb1);
            }
            result.Data = NotesItems;
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        public JsonResult Note(int id, string message)
        {
            JsonResult result = new JsonResult();
            int resultId = this.serviceNote.Note(id, CurrentUser.Id, message);
            result.Data = resultId > 0;
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        public JsonResult Delete(int id)
        {
            JsonResult result = new JsonResult();
            var note = this.serviceNote.GetById(id);
            if (note != null)
            {
                result.Data = this.serviceNote.Delete(note.Id);
            }
            else
            {
                result.Data = false;
            }

            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }
    }
}