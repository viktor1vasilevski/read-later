using Data;
using Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class BookmarkService : IBookmarkService
    {
        private ReadLaterDataContext _ReadLaterDataContext;

        public BookmarkService(ReadLaterDataContext readLaterDataContext)
        {
            _ReadLaterDataContext = readLaterDataContext;
        }
        public Bookmark CreateBookmark(Bookmark bookmark)
        {
            _ReadLaterDataContext.Add(bookmark);
            _ReadLaterDataContext.SaveChanges();
            return bookmark;
        }

        public void DeleteBookmark(Bookmark bookmark)
        {
            _ReadLaterDataContext.Bookmark.Remove(bookmark);
            _ReadLaterDataContext.SaveChanges();
        }

        public List<Bookmark> GetBookmarks()
        {
            return _ReadLaterDataContext.Bookmark.Include(x => x.Category).ToList();
        }

        public Bookmark GetBookmark(int Id)
        {
            return _ReadLaterDataContext.Bookmark.Where(c => c.ID == Id).Include(x => x.Category).FirstOrDefault();
        }

        public void UpdateBookmark(Bookmark bookmark)
        {
            _ReadLaterDataContext.Update(bookmark);
            _ReadLaterDataContext.SaveChanges();
        }

        public List<Bookmark> GetBookmarksByUser(string userId)
        {
            return _ReadLaterDataContext.Bookmark
                .Include(b => b.Category)
                .Where(x => x.UserId == userId)
                .ToList();
        }
    }
}
