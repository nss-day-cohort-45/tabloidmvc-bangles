﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
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
    public class CommentController : Controller
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IPostRepository _postRepository;

        public CommentController (ICommentRepository commentRepository, IPostRepository postRepository)
        {
            _commentRepository = commentRepository;
            _postRepository = postRepository;
        }
        // GET: CommentController
        public ActionResult Index(int id) //We will display only the comments related to the current post
        {
            // We use a View Model to pass a list of comments and the post object to the view
            CommentIndexViewModel vm = new CommentIndexViewModel();

            vm.Comments = _commentRepository.GetCommentsByPost(id);
            vm.Post = _postRepository.GetPublishedPostById(id);

            return View(vm);
        }

        // GET: CommentController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CommentController/Create
        public ActionResult Create(int id)
        {
            // Need to return a view that functions as a form that allows users to fill in Comment info with
            // PostId should be based on the post routed from
            // UserProfileId should be based on the current user
            Comment comment = new Comment();

            comment.PostId = id;
            comment.UserProfileId = GetCurrentUserProfileId();

            return View(comment);
        }

        // POST: CommentController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Comment comment)
        {
            try
            {
                comment.CreateDateTime = DateAndTime.Now;

                _commentRepository.Add(comment);

                // Specifies the specific Action, Controller, and Route Value to return to
                // The Route Value must be passed as an object
                return RedirectToAction("Index", "Comment", new { id = comment.PostId});
            }
            catch
            {
                return View();
            }
        }

        // GET: CommentController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CommentController/Edit/5
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

        // GET: CommentController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CommentController/Delete/5
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

        // Allows the logged in user's Id to be used by the controller
        private int GetCurrentUserProfileId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }
    }
}
