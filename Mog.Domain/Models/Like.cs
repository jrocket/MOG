﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoG.Domain.Models
{
    public class Like
    {
        public int Id { get; set; }

        public int ProjectId { get; set; }

        public int UserId { get; set; }
    }
}