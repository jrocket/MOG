using MoG.Controllers;
using MoG.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MoG.Code
{
    public class Mapper
    {
        //public List<VMActivity> MapActivities(IList<Activity> data,MogController controler)
        public List<VMActivity> MapActivities(IList<Activity> data)
        {//TOdo : use automapper

            List<VMActivity> result = new List<VMActivity>();
            foreach (var activity in data)
            {
                VMActivity oneActivity = new VMActivity()
                {
                    Id = activity.Id,
                    When = activity.When,
                    Who = activity.Who,
                    Type = activity.Type & (ActivityType.Project | ActivityType.Comment | ActivityType.File)
                };

                if ((activity.Type & ActivityType.Comment) == ActivityType.Comment
                    && (activity.Type & ActivityType.Create) == ActivityType.Create)
                {//
                    oneActivity.Description = String.Format("<strong>{0}</strong>  {2} <strong>{1}</strong>",
                        activity.Who.DisplayName,
                         activity.File.DisplayName,
                         Resources.Resource.ACTIVITY_CommentedFile);

                    oneActivity.Url = "/File/Display/"+ activity.FileId.Value;//controler.Url.Action("Display", "File", new { id = activity.FileId.Value });
                }

                if ((activity.Type & ActivityType.File) == ActivityType.File
                   && (activity.Type & ActivityType.Create) == ActivityType.Create)
                {//
                    oneActivity.Description = String.Format("<strong>{0}</strong> {2} <strong>{1}</strong>",
                        activity.Who.DisplayName,
                         activity.File.DisplayName,
                         Resources.Resource.ACTIVITY_CreatedFile);

                    oneActivity.Url = "File/Display/"+activity.FileId.Value;//controler.Url.Action("Display", "File", new { id = activity.FileId.Value });
                }

                if ((activity.Type & ActivityType.Project) == ActivityType.Project
                  && (activity.Type & ActivityType.Create) == ActivityType.Create)
                {//
                    oneActivity.Description = String.Format("<strong>{0}</strong> {2} <strong>{1}</strong>",
                        activity.Who.DisplayName,
                        activity.Project.Name,
                        Resources.Resource.ACTIVITY_CreatedProject);

                    oneActivity.Url = "Project/Detail/"+ activity.ProjectId.Value;//controler.Url.Action("Detail", "Project", new { id = activity.ProjectId.Value });
                }


                result.Add(oneActivity);
            }
            return result;
        }
    }
}