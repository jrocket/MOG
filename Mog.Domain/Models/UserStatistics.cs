using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoG.Domain.Models
{
    public class UserStatistics
    {
        public int CountProject { get; set; }

        public int CountComment { get; set; }

        public int CountFile { get; set; }

        public int CountInvit { get; set; }

        public int RatioTrackAcceptance { get; set; }
        public int RatioAcceptedTracks { get; set; }
        public int RatioFinishedProject { get; set; }
        public int RatioCollaboration { get; set; }
    }
}
