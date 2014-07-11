using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoG.Domain.Service
{
    class EmailService : IEmailService
    {

        public bool SendNotificationEmail(int userId)
        {
            throw new NotImplementedException();
        }
    }

    public interface IEmailService
    {
        bool SendNotificationEmail(int userId);
    }
}
