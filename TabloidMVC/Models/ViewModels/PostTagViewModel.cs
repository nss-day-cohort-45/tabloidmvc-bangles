using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TabloidMVC.Models.ViewModels
{
    public class PostTagViewModel
    {
        public PostTag PostTag { get; set; }
        public Post Post { get; set; }
        public List<Tag> Tags { get; set; }
    }
}
