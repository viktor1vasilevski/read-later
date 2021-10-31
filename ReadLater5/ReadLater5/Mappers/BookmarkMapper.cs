using AutoMapper;
using Entity;
using ReadLater5.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadLater5.Mappers
{
    public class BookmarkMapper : Profile
    {
        public BookmarkMapper()
        {
            CreateMap<BookmarkViewModel, Bookmark>().ReverseMap();
        }
    }
}
