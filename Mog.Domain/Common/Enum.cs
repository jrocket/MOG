using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MoG
{
    public enum Visibility
    {
        [Description("Public")]
        Public = 1,

        [Description("Private")]
        Private = 2
    }

    public enum Licence
    {
        [Description("CC BY")]
        CCBY = 1,
        [Description("CC BY-ND")]
        CCBYND = 2,
        [Description("CC BY-ND-NC")]
        CCBYNDNC = 3,
        [Description("Copyright")]
        COPYRIGHT = 4

    }

    [Flags]
    public enum ActivityType
    {
        Project = 1,
        File = 2,
        Comment = 4,
        Like = 8,
        Create = 16,
        Feature = 32

    }


    public enum FileStatus
    {
        Draft = 0,
        Submitted = 1,
        Accepted = 2,
        Rejected = 3
    }
    public enum FileType
    {
        [Description("Bass")]
        Bass = 0,
        [Description("Guitar")]
        Guitar = 1,
        [Description("Drums")]
        Drums = 2,
        [Description("Mixdown")]
        Mixdown = 3,
        [Description("Idea")]
        Idea = 4,
        [Description("Unknown")]
        Unknown = 5,

    }
}