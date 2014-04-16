using MoG.Domain.Models;
using MoG.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoG.Domain.Service
{
    public class LogService : ILogService
    {
        private ILogRepository repoLog = null;


        public LogService(ILogRepository repo)
        {
            this.repoLog = repo;
          
        }



        public Log GetById(int id)
        {
            return this.repoLog.GetById(id);
        }


        public int LogError(string location, string message, Exception exc)
        {
            System.Diagnostics.Trace.WriteLine(location + "-" + exc.Message + exc.StackTrace, "Error");
            Log log = new Log()
            {
                Date = DateTime.Now,
                Location = location,
                Severity = EnumSeverity.Error
            };
            log.Message = message;
            log.Message += exc!=null ? exc.Message  : string.Empty;
            log.Stacktrace = exc != null ? exc.StackTrace : String.Empty; ;

            return this.repoLog.Create(log);
        }

        public int LogError(string location, Exception exc)
        {
            System.Diagnostics.Trace.WriteLine(location + "-" + exc.Message + exc.StackTrace, "Error");
            Log log = new Log()
            {
                Date = DateTime.Now,
                Location = location,
                Message = exc.Message,
                Stacktrace = exc.StackTrace,
                Severity = EnumSeverity.Error
            };
            return this.repoLog.Create(log);
        }

        public int LogMessage(string location, string message, string stacktrace = null)
        {
            System.Diagnostics.Trace.WriteLine(location + "-" +message,"Message");
            Log log = new Log()
            {
                Date = DateTime.Now,
                Location = location,
                Message = message,
                Stacktrace = stacktrace,
                Severity = EnumSeverity.Message
            };
            return this.repoLog.Create(log);
        }

        public int LogVerbose(string location, string message)
        {
            System.Diagnostics.Trace.WriteLine(location + "-" + message, "Verbose");
            Log log = new Log()
            {
                Date = DateTime.Now,
                Location = location,
                Message = message,
                Stacktrace = null,
                Severity = EnumSeverity.Verbose
            };
            return this.repoLog.Create(log);
        }

        public IList<Log> Get(int startIndex, int count)
        {
            return this.repoLog.Get(startIndex, count).ToList();
        }


        public void Delete(Log log)
        {
            this.repoLog.Delete(log);
        }


    }

    public interface ILogService
    {
          Log GetById(int id);


         int LogError(string location, Exception exc);

         int LogError(string location, string message, Exception exc);
      

         int LogMessage(string location, string message, string stacktrace = null);
       

         int LogVerbose(string location, string message);


          IList<Log> Get(int startIndex, int count);


          void Delete(Log log);
    }
}
