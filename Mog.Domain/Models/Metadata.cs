using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MoG.Domain.Models
{
  [Serializable]
    public class Mp3Metadata : Metadata
    {
     
        public string Duration { get; set; }

    }
    [Serializable]
    public class Metadata
    {

    }
}
