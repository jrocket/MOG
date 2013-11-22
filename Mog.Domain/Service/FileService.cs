using MoG.Domain.Models;
using MoG.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoG.Domain.Service
{
    public class FileService : IFileService
    {
        public IFileRepository repoFile = null;
        public ICommentRepository repoComment = null;

        public FileService(IFileRepository _fileRepo, ICommentRepository _commentRepo)
        {
            this.repoFile = _fileRepo;
            this.repoComment = _commentRepo;
        }
        public List<MoGFile> GetProjectFile(int projectId)
        {
            return repoFile.GetByProjectId(projectId).ToList();
        }

        public MoGFile GetById(int id)
        {
            return repoFile.GetById(id);
        }



        public IQueryable<Comment> GetFileComments(int fileId)
        {
            return repoComment.GetByFileId(fileId);
        }
    }

    public interface IFileService
    {
        List<MoGFile> GetProjectFile(int projectId);


        MoGFile GetById(int id);

        IQueryable<Comment> GetFileComments(int fileId);
    }
}
