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
    public class TagController : Controller
    {
        private readonly ITagRepository _tagRepository;
        private readonly IUserProfileRepository _userProfileRepository;

        public TagController(ITagRepository tagRepository, IUserProfileRepository userProfileRepository)
        {
            _tagRepository = tagRepository;
            _userProfileRepository = userProfileRepository;
        }

        // GET: TagController
        public ActionResult Index()
        {
            UserProfile currentUser = _userProfileRepository.GetById(GetCurrentUserProfileId());
            if (currentUser.UserTypeId != 1)
            {
                return RedirectToAction("Index", "Home");
            }
            List<Tag> tags = _tagRepository.GetAllTags();
            return View(tags);
        }

        //// GET: TagController/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        // GET: TagController/Create
        public ActionResult Create()
        {
            UserProfile currentUser = _userProfileRepository.GetById(GetCurrentUserProfileId());
            if (currentUser.UserTypeId != 1)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        // POST: TagController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Tag tag)
        {
            try
            {
                _tagRepository.AddTag(tag);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(tag);
            }
        }

        // GET: TagController/Edit/5
        public ActionResult Edit(int id)
        {
            UserProfile currentUser = _userProfileRepository.GetById(GetCurrentUserProfileId());
            if (currentUser.UserTypeId != 1)
            {
                return RedirectToAction("Index", "Home");
            }
            Tag tag = _tagRepository.GetTagById(id);

            if (tag == null)
            {
                return NotFound();
            }
            return View(tag);
        }

        // POST: TagController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Tag tag)
        {
            try
            {
                _tagRepository.UpdateTag(tag);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return View(tag);
            }
        }

        // GET: TagController/Delete/5
        public ActionResult Delete(int id)
        {
            UserProfile currentUser = _userProfileRepository.GetById(GetCurrentUserProfileId());
            if (currentUser.UserTypeId != 1)
            {
                return RedirectToAction("Index", "Home");
            }
            Tag tag = _tagRepository.GetTagById(id);
            return View(tag);
        }

        // POST: TagController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Tag tag)
        {
            try
            {
                _tagRepository.DeleteTag(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return View(tag);
            }
        }
        private int GetCurrentUserProfileId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }
    }
}
