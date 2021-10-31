using Entity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadLater5.Models
{
    public class BookmarkViewModel
    {
        public int ID { get; set; }
        public string URL { get; set; }
        public string ShortDescription { get; set; }
        public int? CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public DateTime CreateDate { get; set; }
        public string UserId { get; set; }
        public List<SelectListItem> ListOfCategories { get; set; }
    }
}
