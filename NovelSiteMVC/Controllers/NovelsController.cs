using Microsoft.AspNetCore.Mvc;
using NovelSiteMVC.Models;
using NovelSiteMVC.ViewModels;

namespace NovelSiteMVC.Controllers
{
    public class NovelsController : Controller
    {
        private readonly AppDbContext context;

        public NovelsController(AppDbContext _context)
        {
            context = _context;
        }
        public IActionResult Index()
        {
            //fetch all novels from db
            var model = context.tblNovels.Select(x => new NovelsViewModel()
            {
                Id = x.Id,
                Title = x.Title,
                PhotoName = x.PhotoName
            }).ToHashSet();

            return View(model);
        }
    }
}
