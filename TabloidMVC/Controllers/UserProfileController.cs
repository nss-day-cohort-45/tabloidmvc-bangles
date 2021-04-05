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
            if (userToEdit == null)
            {
                return NotFound();
            }
            int currentUserId = GetCurrentUserProfileId();
            UserProfile currentUser = _userProfileRepository.GetById(currentUserId);
            int adminCount = _userProfileRepository.GetAll().Where(up => up.UserTypeId == 1).ToList().Count;
            if (currentUser.UserTypeId == 1)
            {
                ChangeUserTypeViewModel vm = new ChangeUserTypeViewModel()
                {
                    UserTypes = _userProfileRepository.GetUserTypes(),
                    User = userToEdit,
                    AdminCount = adminCount,
                };
                return View(vm);
            }
            else
            {
                ChangeUserTypeViewModel vm = new ChangeUserTypeViewModel()
                {
                    Message = "Only an administrator can edit a user's type."
                };
                return View(vm);
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
                if (user.UserTypeId == 2 && vm.AdminCount <= 1)
                {
                    vm.Message = "Cannot change the role of the sole administrator. Elevate another user and try again.";
                    return View(vm);
                }
                else
                {
                    _userProfileRepository.UpdateUserType(user);
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                return RedirectToAction("Index");
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
                if (userToDelete == null)
                {
                    return NotFound();
                }
                int adminCount = _userProfileRepository.GetAll().Where(up => up.UserTypeId == 1).ToList().Count;
                DeleteUserProfileViewModel vm = new DeleteUserProfileViewModel()
                {
                    User = userToDelete,
                    AdminCount = adminCount,
                };

                return View(vm);
            }
            else
            {
                DeleteUserProfileViewModel vm = new DeleteUserProfileViewModel()
                {
                    Message = "Only an administrator can deactivate a user."
                };
                return View(vm);
            }
        }

        // POST: UserProfileController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, DeleteUserProfileViewModel vm)
        {
            try
            {
                UserProfile user = vm.User;
                if (user.UserTypeId == 1 && vm.AdminCount <= 1)
                {
                    vm.Message = "Cannot deactivate the sole administrator. Elevate another user and try again.";
                    return View(vm);
                }
                else
                {
                    _userProfileRepository.Deactivate(id);
                    return RedirectToAction(nameof(Index));
                }
            }
            catch
            {
                return View(vm);
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
