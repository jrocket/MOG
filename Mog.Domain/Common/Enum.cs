﻿using System;
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
        CCBYNDNC = 3
    }

    public enum ActivityType
    {
        Project = 0,
        File = 1,
        Comment = 2,
        Like = 3

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

    }
}