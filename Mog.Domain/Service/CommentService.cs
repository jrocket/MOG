﻿using MoG.Domain.Models;
using MoG.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoG.Domain.Service
{
    public class CommentService : ICommentService
    {
        private ICommentRepository repoComment = null;
        private IActivityService servActivity = null;

        public CommentService(ICommentRepository repo, IActivityService activityService)
        {
            repoComment = repo;
            this.servActivity = activityService;

        }

        public Models.Comment Create(Models.Comment newComment)
        {
            newComment.CreatedOn = DateTime.Now;
            newComment.ModifiedOn = DateTime.Now;
            if (repoComment.Create(newComment))
            {
                //to be sure to get the complete object from DB
                Comment c = this.repoComment.GetById(newComment.Id);
                this.servActivity.LogCommentCreation(c);
            }

            return newComment;
        }


        public List<Models.Comment> GetByProjectId(int id)
        {
            return this.repoComment.GetByProjectId(id).ToList();
        }


        public bool Delete(int id,UserProfileInfo userProfile)
        {
            Comment commentToDelete = GetById(id);
            if (commentToDelete.Creator.Id != userProfile.Id)
            {
                return false;
            }
            var flag = this.servActivity.DeleteByCommentId(id);
            if (!flag)
                return false;
            this.servActivity.DeleteByCommentId(id);
            return this.repoComment.DeleteById(id);
        }

        public Comment GetById(int id)
        {
            return this.repoComment.GetById(id);
        }




        public bool Edit(Comment comment)
        {
            comment.ModifiedOn = DateTime.Now;
           var result =  this.repoComment.SaveChanges(comment);
            return result.Id>0;
        }
    }

    public interface ICommentService
    {

        Models.Comment Create(Models.Comment newComment);

        List<Models.Comment> GetByProjectId(int id);

        bool Delete(int id, UserProfileInfo userProfile);

        Comment GetById(int id);

        bool Edit(Comment comment);
    }
}
