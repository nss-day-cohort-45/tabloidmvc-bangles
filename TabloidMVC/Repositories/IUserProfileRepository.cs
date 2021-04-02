using System.Collections.Generic;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface IUserProfileRepository
    {
        UserProfile GetByEmail(string email);
        List<UserProfile> GetAll();
        List<UserProfile> GetDeactivated();
        UserProfile GetById(int id);
        void Deactivate(int id);
    }
}