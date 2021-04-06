using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TabloidMVC.Models;
using TabloidMVC.Models.ViewModels;
using TabloidMVC.Repositories;

namespace TabloidMVC.Controllers
{
    public class PostTagController : Controller
    {
        private readonly IPostTagRepository _postTagRepository;
        private readonly ITagRepository _tagRepository;

        public PostTagController(
            IPostTagRepository postTagRepository,
            ITagRepository tagRepository)
        {
            _postTagRepository = postTagRepository;
            _tagRepository = tagRepository;
        }

        // GET: PostTagController
        public ActionResult Index()
        {
            return View();
        }

        // GET: PostTagController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PostTagController/Create
        public ActionResult Create(int id)
        {
            List<Tag> tags = _tagRepository.GetAllTags();
            PostTagViewModel vm = new PostTagViewModel()
            {
                PostTags = new List<int>(),
                Tags = tags,
                PostId = id
            };
            return View(vm);
        }

        // POST: PostTagController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PostTagViewModel vm)
        {
            if (vm.PostTags == null)
            {
                return RedirectToAction("Details", "Post", new { id = vm.PostId });
            }
            else
            {
                foreach (int postTag in vm.PostTags)
                {
                    PostTag pT = new PostTag();
                    pT.TagId = postTag;
                    pT.PostId = vm.PostId;
                    _postTagRepository.AddPostTag(pT);
                }
                return RedirectToAction("Details", "Post", new { id = vm.PostId });
            }
        }

        // GET: PostTagController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PostTagController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PostTagController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PostTagController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
