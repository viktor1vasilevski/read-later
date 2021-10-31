using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data;
using Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Services;
using AutoMapper;
using ReadLater5.Models;

namespace ReadLaterAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class BookmarksController : ControllerBase
    {
        IBookmarkService _bookmarkService;
        ICategoryService _categoryService;
        IMapper _mapper;
        public BookmarksController(IBookmarkService bookmarkService, ICategoryService categoryService, IMapper mapper)
        {
            _bookmarkService = bookmarkService;
            _categoryService = categoryService;
            _mapper = mapper;
        }

        //GET: api/Bookmarks
        [HttpGet]     
        public IActionResult GetAll()
        {
            List<Bookmark> bookmarks = _bookmarkService.GetBookmarks();
            if (bookmarks != null)
            {
                return Ok(bookmarks);
            }
            return NotFound();
        }

        //GET: api/Bookmarks/5
        [HttpGet("{id}")]
        public IActionResult GetBookmarkById(int? id)
        {
            if (id == null)
            {
                NotFound(id);
            }
            Bookmark bookmark = _bookmarkService.GetBookmark((int)id);
            if (bookmark == null)
            {
                NotFound();
            }
            return Ok(bookmark);
        }



        /*
            The Authorization does not work. The Token is generated, the User has the role User but I'm 
            missing something or the attribute assaigment is wrong

            ALL THE ACTIONS WORK NORMALLY!
         */
        [HttpPost]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        public IActionResult AddOrEdit(BookmarkViewModel model)
        {
            if (ModelState.IsValid)
            {
                
                var bookmark = _mapper.Map<Bookmark>(model);
                var id = bookmark.ID;
                if (id != 0)
                {
                    _bookmarkService.UpdateBookmark(bookmark);
                    
                } else
                {
                    _bookmarkService.CreateBookmark(bookmark);
                }
                            
                return Ok(bookmark);
            }

            return NotFound(model);
        }

        [HttpDelete("{id}")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        public IActionResult Delete(int id)
        {
            Bookmark bookmark = _bookmarkService.GetBookmark(id);
            if (bookmark == null)
            {
                return NotFound(id);
            }
            _bookmarkService.DeleteBookmark(bookmark);
            return Ok(bookmark);
        }
    }
}
