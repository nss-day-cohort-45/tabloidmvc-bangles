using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TabloidMVC.Models;
using TabloidMVC.Repositories;

namespace TabloidMVC.Controllers
{
    public class CategoryController : Controller
    {
        // GET: CategoryController
        private readonly ICategoryRepository _categoryRepo;
        private readonly IUserProfileRepository _userProfileRepository;

        public CategoryController(ICategoryRepository categoryRepository, IUserProfileRepository userProfileRepository)
        {
            _categoryRepo = categoryRepository;
            _userProfileRepository = userProfileRepository;
        }

        public ActionResult Index()
        {
            UserProfile currentUser = _userProfileRepository.GetById(GetCurrentUserProfileId());
            if (currentUser.UserTypeId != 1)
            {
                return RedirectToAction("Index", "Home");
            }
            List<Category> categories = _categoryRepo.GetAll();
            return View(categories);
        }

        // GET Details
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET Create
        public ActionResult Create()
        {
            UserProfile currentUser = _userProfileRepository.GetById(GetCurrentUserProfileId());
            if (currentUser.UserTypeId != 1)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        // POST Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category category)
        {
            try
            {
                _categoryRepo.AddCategory(category);
                return RedirectToAction(nameof(Index), "Category");
            }
            catch
            {
                return View();
            }
        }

        // GET Edit
        public ActionResult Edit(int id)
        {
            UserProfile currentUser = _userProfileRepository.GetById(GetCurrentUserProfileId());
            if (currentUser.UserTypeId != 1)
            {
                return RedirectToAction("Index", "Home");
            }

            Category category = _categoryRepo.GetCategoryById(id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Category category)
        {
            try
            {
                _categoryRepo.UpdateCategory(category);

                return RedirectToAction("Index");
            }
            catch
            {
                return View(category);
            }
        }

        // GET Delete
        public ActionResult Delete(int id)
        {
            UserProfile currentUser = _userProfileRepository.GetById(GetCurrentUserProfileId());
            if (currentUser.UserTypeId != 1)
            {
                return RedirectToAction("Index", "Home");
            }

            Category category = _categoryRepo.GetCategoryById(id);
            return View(category);
        }

        // POST Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Category category)
        {
            try
            {
                _categoryRepo.DeleteCategory(id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(category);
            }
        }

        private int GetCurrentUserProfileId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }
    }
}
