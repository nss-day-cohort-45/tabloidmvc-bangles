using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TabloidMVC.Models.ViewModels
{
    public class PostTagViewModel
    {
        public List<int> PostTags { get; set; }
        public Post Post { get; set; }
        public int PostId { get; set; }
        public List<Tag> Tags { get; set; }
    }
}
