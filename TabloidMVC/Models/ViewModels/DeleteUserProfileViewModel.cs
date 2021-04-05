using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TabloidMVC.Models.ViewModels
{
    public class DeleteUserProfileViewModel
    {
        public UserProfile User { get; set; }
        public int AdminCount { get; set; }
        public string Message { get; set; }
    }
}
