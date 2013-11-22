using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoG.Domain.Repository
{
    public class BaseRepository
    {
        public MogDbContext dbContext = null;
        IdbContextProvider contextProvider = null;

        public BaseRepository(IdbContextProvider provider)
        {
            contextProvider = provider;
            dbContext = contextProvider.GetCurrent();
        }
    }
}
