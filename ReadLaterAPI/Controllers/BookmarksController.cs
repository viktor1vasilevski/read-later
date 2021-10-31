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
        //[HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        public IActionResult GetBookmark()
        {
            List<Bookmark> bookmarks = _bookmarkService.GetBookmarks();
            return Ok(bookmarks);
        }

        // GET: api/Bookmarks/5
        /*[HttpGet("{id}")]

        public async Task<ActionResult<Bookmark>> GetBookmark(int id)
        {
            var bookmark = await _context.Bookmark.FindAsync(id);

            if (bookmark == null)
            {
                return NotFound();
            }

            return bookmark;
        }*/

        // PUT: api/Bookmarks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /*[HttpPut("{id}")]
        public async Task<IActionResult> PutBookmark(int id, Bookmark bookmark)
        //{
            if (id != bookmark.ID)
            {
                return BadRequest();
            }

            _context.Entry(bookmark).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookmarkExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }*/

        // POST: api/Bookmarks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /*[HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult<Bookmark>> PostBookmark(Bookmark bookmark)
        //{
            _context.Bookmark.Add(bookmark);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBookmark", new { id = bookmark.ID }, bookmark);
        }*/

        // DELETE: api/Bookmarks/5
        /*[HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBookmark(int id)
        //{
            var bookmark = await _context.Bookmark.FindAsync(id);
            if (bookmark == null)
            {
                return NotFound();
            }

            _context.Bookmark.Remove(bookmark);
            await _context.SaveChangesAsync();

            return NoContent();
        }*/

        //private bool BookmarkExists(int id)
        //{
        //    return _context.Bookmark.Any(e => e.ID == id);
        //}
    }
}
