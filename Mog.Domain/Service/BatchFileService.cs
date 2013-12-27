using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoG.Domain.Service
{
    public class BatchFileService : IBatchFileService
    {
        public bool Process()
        {
            throw new NotImplementedException();
        }
    }

    public interface IBatchFileService
    {
         bool Process();
    }
}
