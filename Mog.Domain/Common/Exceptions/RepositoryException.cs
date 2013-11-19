using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MoG.Exceptions
{
    class RepositoryException : Exception
    {
        public RepositoryException(string message) : base(message)
        { }
    }
}
