using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TabloidMVC.Models.ViewModels
{
    public class CommentCreateViewModel
    {
        public Comment Comment { get; set; }
        public int UserId { get; set; }
        public int PostId { get; set; }
    }
}
