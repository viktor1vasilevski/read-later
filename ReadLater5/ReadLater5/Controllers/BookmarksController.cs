using AutoMapper;
using Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ReadLater5.Models;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ReadLater5.Controllers
{
    public class BookmarksController : Controller
    {
        UserManager<IdentityUser> _userManager;
        IBookmarkService _bookmarkService;
        ICategoryService _categoryService;
        IMapper _mapper;
        public BookmarksController(IBookmarkService bookmarkService, ICategoryService categoryService, IMapper mapper, UserManager<IdentityUser> userManager)
        {
            _bookmarkService = bookmarkService;
            _categoryService = categoryService;
            _mapper = mapper;
            _userManager = userManager;
        }

        // GET: Bookmarks
        public IActionResult Index()
        {
            List<Bookmark> bookmarks = new List<Bookmark>();
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId != null)
            {
                bookmarks = _bookmarkService.GetBookmarksByUser(userId);
            }
            else
            {
                bookmarks = _bookmarkService.GetBookmarks();
            }

            return View(bookmarks);
        }

        // GET: Bookmarks/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return new StatusCodeResult(Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest);
            }
            Bookmark bookmark = _bookmarkService.GetBookmark((int)id);
            if (bookmark == null)
            {
                return new StatusCodeResult(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound);
            }
            return View(bookmark);
        }


        public IActionResult Create()
        {
            var bookmarkViewModelForCreation = new BookmarkViewModel();
            var categories = _categoryService.GetCategories();
            bookmarkViewModelForCreation.ListOfCategories = _mapper.Map<List<SelectListItem>>(categories);
            return View(bookmarkViewModelForCreation);
        }

        [HttpPost]
        public IActionResult Create(BookmarkViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var bookmark = _mapper.Map<Bookmark>(model);
                if (userId != null)
                {
                    bookmark.UserId = userId;
                }
                _bookmarkService.CreateBookmark(bookmark);

            }
            return RedirectToAction("Index");
        }

        // GET: Bookmarks/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new StatusCodeResult(Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest);
            }
            Bookmark bookmark = _bookmarkService.GetBookmark((int)id);
            var bookmarkForEdit = _mapper.Map<BookmarkViewModel>(bookmark);
            var categories = _categoryService.GetCategories();
            bookmarkForEdit.ListOfCategories = _mapper.Map<List<SelectListItem>>(categories);

            if (bookmarkForEdit == null)
            {
                return new StatusCodeResult(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound);
            }
            return View(bookmarkForEdit);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(BookmarkViewModel bookmarkViewModel)
        {
            if (ModelState.IsValid)
            {
                var bookmark = _mapper.Map<Bookmark>(bookmarkViewModel);
                _bookmarkService.UpdateBookmark(bookmark);
            }

            return RedirectToAction("Index");
        }

        // GET: Bookmarks/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new StatusCodeResult(Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest);
            }
            Bookmark bookmark = _bookmarkService.GetBookmark((int)id);
            if (bookmark == null)
            {
                return new StatusCodeResult(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound);
            }
            return View(bookmark);
        }

        // POST: Bookmarks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            Bookmark bookmark = _bookmarkService.GetBookmark(id);
            _bookmarkService.DeleteBookmark(bookmark);
            return RedirectToAction("Index");
        }
    }
}
