using System.Collections.Generic;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface IUserProfileRepository
    {
        UserProfile GetByEmail(string email);
        List<UserProfile> GetAll();
        List<UserProfile> GetDeactivated();
        void AddUser(UserProfile user);
        UserProfile GetById(int id);
        void Deactivate(int id);
        void Reactivate(UserProfile user);
        void UpdateUserType(UserProfile user);
        List<UserType> GetUserTypes();
    }
}