﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TabloidMVC.Models.ViewModels
{
    public class CommentIndexViewModel
    {
        public List<Comment> Comments { get; set; }

        public Post Post { get; set; }
    }
}
