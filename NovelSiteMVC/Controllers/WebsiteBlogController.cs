using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NovelSiteMVC.Models;
using NovelSiteMVC.ViewModels;

namespace NovelSiteMVC.Controllers
{
    public class WebsiteBlogController : Controller
    {
        private readonly AppDbContext context;

        public WebsiteBlogController(AppDbContext _context)
        {
            context = _context;
        }
        public IActionResult Index(int? category = 0)
        {
            //fetch all posts from db
            List<PostsViewModel>? model;
            if (category == 0)
            {
                model = context.tblPosts.AsNoTracking().Where(x => !x.isDeleted).Select(x => new PostsViewModel()
                {
                    Id = x.Id,
                    Title = x.Title,
                    Author = x.Author,
                    Watches = x.Watches,
                    LastEdit = x.LastEdit,
                    PhotoName = x.PhotoName
                }).ToList();
            }
            else
            {
                model = context.tblPosts.AsNoTracking().Where(x => !x.isDeleted && x.BlogId == category).Select(x => new PostsViewModel()
                {
                    Id = x.Id,
                    Title = x.Title,
                    Author = x.Author,
                    Watches = x.Watches,
                    LastEdit = x.LastEdit,
                    PhotoName = x.PhotoName
                }).ToList();
            }
            ViewBag.Categories = context.tblBlogs.AsNoTracking().Where(x => !x.isDeleted).Select(x => new { x.Id, x.Title }).ToList();
            return View(model);
        }
    }
}
