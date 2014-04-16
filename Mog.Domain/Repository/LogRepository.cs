using MoG.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoG.Domain.Repository
{
    public class LogRepository : BaseRepository, ILogRepository
    {

        public LogRepository(IdbContextProvider provider)
            : base(provider)
        {

        }



        public Models.Log GetById(int id)
        {
            return this.dbContext.Logs.Find(id);

        }

        public int Create(Models.Log log)
        {
            this.dbContext.Logs.Add(log);
            this.dbContext.SaveChanges();
            return log.Id;
        }

        public IQueryable<Log> Get(int startIndex, int count)
        {
            return this.dbContext.Logs
                .OrderByDescending(l => l.Id)
                .Skip(startIndex)
                .Take(count);
        }


        public void Delete(Log log)
        {
            this.dbContext.Logs.Remove(log);
             this.dbContext.SaveChanges();
        }
    }

    public interface ILogRepository
    {
        Models.Log GetById(int id);


        int Create(Models.Log log);


        IQueryable<Log> Get(int startIndex, int count);


        void  Delete(Log log);
    }
}
