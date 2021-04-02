using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TabloidMVC.Models;
using TabloidMVC.Models.ViewModels;
using TabloidMVC.Repositories;

namespace TabloidMVC.Controllers
{
    [Authorize]
    public class UserProfileController : Controller
    {
        private readonly IUserProfileRepository _userProfileRepository;

        public UserProfileController(IUserProfileRepository userRepository)
        {
            _userProfileRepository = userRepository;
        }

        // GET: UserProfileController
        public ActionResult Index()
        {
            var users = _userProfileRepository.GetAll().OrderBy(u => u.DisplayName);
            return View(users);
        }

        public ActionResult Deactivated()
        {
            var users = _userProfileRepository.GetDeactivated().OrderBy(u => u.DisplayName);
            return View(users);
        }

        // GET: UserProfileController/Details/5
        public ActionResult Details(int id)
        {
            UserProfile user = _userProfileRepository.GetById(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // GET: UserProfileController/Edit/5
        public ActionResult Edit(int id)
        {
            UserProfile userToEdit = _userProfileRepository.GetById(id);
            int currentUserId = GetCurrentUserProfileId();
            UserProfile currentUser = _userProfileRepository.GetById(currentUserId);
            if (currentUser.UserTypeId == 1)
            {
                ChangeUserTypeViewModel vm = new ChangeUserTypeViewModel()
                {
                    UserTypes = _userProfileRepository.GetUserTypes(),
                    User = userToEdit
                };
                return View(vm);
            }
            else
            {
                return NotFound();
            }
        }

        // POST: UserProfileController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, ChangeUserTypeViewModel vm)
        {
            try
            {
                UserProfile user = vm.User;
                _userProfileRepository.UpdateUserType(user);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(vm);
            }
        }

        // GET: UserProfileController/Delete/5
        public ActionResult Delete(int id)
        {
            int userId = GetCurrentUserProfileId();
            UserProfile currentUser = _userProfileRepository.GetById(userId);
            if (currentUser.UserTypeId == 1)
            {
                UserProfile userToDelete = _userProfileRepository.GetById(id);
                return View(userToDelete);
            }
            else
            {
                return NotFound();
            }
        }

        // POST: UserProfileController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, UserProfile user)
        {
            try
            {
                _userProfileRepository.Deactivate(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(user);
            }
        }

        public ActionResult Reactivate(int id)
        {
            UserProfile user = _userProfileRepository.GetById(id);
            _userProfileRepository.Reactivate(user);
            return RedirectToAction("Index");
        }

        private int GetCurrentUserProfileId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }

    }
}
