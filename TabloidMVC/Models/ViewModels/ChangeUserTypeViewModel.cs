using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TabloidMVC.Models.ViewModels
{
    public class ChangeUserTypeViewModel
    {
        public List<UserType> UserTypes { get; set; }
        public UserProfile User { get; set; }
        public int AdminCount { get; set; }
        public string Message { get; set; }
    }
}
