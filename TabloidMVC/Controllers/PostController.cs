using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using TabloidMVC.Models;
using TabloidMVC.Models.ViewModels;
using TabloidMVC.Repositories;

namespace TabloidMVC.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private readonly IPostRepository _postRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ISubscriptionRepository _subscriptionRepository;


        public PostController(IPostRepository postRepository, ICategoryRepository categoryRepository, ISubscriptionRepository subscriptionRepository)
        {
            _postRepository = postRepository;
            _categoryRepository = categoryRepository;
            _subscriptionRepository = subscriptionRepository;
        }

        public IActionResult Index()
        {
            var posts = _postRepository.GetAllPublishedPosts();
            return View(posts);
        }

        public IActionResult MyPosts()
        {
            int userId = GetCurrentUserProfileId();
            var posts = _postRepository.GetUserPosts(userId);
            return View(posts);
        }

        public IActionResult Details(int id)
        {
            var post = _postRepository.GetPublishedPostById(id);
            if (post == null)
            {
                int userId = GetCurrentUserProfileId();
                post = _postRepository.GetUserPostById(id, userId);
                if (post == null)
                {
                    return NotFound();
                }
            }
            return View(post);
        }

        public IActionResult Create()
        {
            var vm = new PostCreateViewModel();
            vm.CategoryOptions = _categoryRepository.GetAll();
            return View(vm);
        }

        [HttpPost]
        public IActionResult Create(PostCreateViewModel vm)
        {
            try
            {
                vm.Post.CreateDateTime = DateAndTime.Now;
                vm.Post.IsApproved = true;
                vm.Post.UserProfileId = GetCurrentUserProfileId();

                _postRepository.Add(vm.Post);

                return RedirectToAction("Details", new { id = vm.Post.Id });
            }
            catch
            {
                vm.CategoryOptions = _categoryRepository.GetAll();
                return View(vm);
            }
        }

        public IActionResult Edit(int id)
        {
            try
            {
                int userId = GetCurrentUserProfileId();
                Post post = _postRepository.GetPublishedPostById(id);
                List<Category> categories =  _categoryRepository.GetAll();
                if (post.UserProfileId != userId)
                {
                    throw new Exception();
                }
                else
                {
                    PostCreateViewModel vm = new PostCreateViewModel()
                    {
                        Post = post,
                        CategoryOptions = categories
                    };
                    return View(vm);
                }
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, PostCreateViewModel vm)
        {
            try
            {
            _postRepository.UpdatePost(vm.Post);

            return RedirectToAction("Index");
            }
            catch
            {
                return View(vm);
            }
        }

        // GET: Post/Delete/5
        public IActionResult Delete(int id)
        {
            try
            {
                int userId = GetCurrentUserProfileId();
                Post post = _postRepository.GetPublishedPostById(id);
                if (post.UserProfileId != userId)
                {
                    throw new Exception();
                }
                else
                {
                    return View(post);
                }

            }
            catch
            {
                return NotFound();
            }

        }

        // POST: Post/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, Post post)
        {
            try
            {
                _postRepository.Delete(id);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(post);
            }
        }

        private int GetCurrentUserProfileId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }


        public ActionResult Subscribe(int id)
        {
            // Need to return a view that functions as a form that allows users to fill in Comment info with
            // PostId should be based on the post routed from
            // UserProfileId should be based on the current user
            Subscription subscription = new Subscription();
            subscription.ProviderUserProfileId = id;
            subscription.SubscriberUserProfileId = GetCurrentUserProfileId();


            return View(subscription);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Subscribe(Subscription subscription, Post post)
        {
            subscription.BeginDateTime = DateAndTime.Now;
            subscription.EndDateTime = DateAndTime.Now;
            _subscriptionRepository.Add(subscription);

            return RedirectToAction("Details", new { id = post.Id });
        }

        



    }
}

