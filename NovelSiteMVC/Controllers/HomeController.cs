using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NovelSiteMVC.Models;
using NovelSiteMVC.ViewModels;
using System.Diagnostics;
using System.Linq;

namespace NovelSiteMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext context;
        public HomeController(ILogger<HomeController> logger, AppDbContext _context)
        {
            _logger = logger;
            context = _context;
        }
        [ResponseCache(Duration = 300, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Index()
        {
            var ActiveList = context.tblTodos.Where(x => x.Status == StatusType.Active && x.isDeleted == false).Select(x => x.Task).ToList();

            //fetch last updated novels [order by chapter.lastupdate]
            //var model = new HomeViewModel();

            //model.LastUpdateNovels = context.tblNovels
            //    .Select(novel => new LastUpdateNovelsViewModel()
            //    {
            //        Id = novel.Id,
            //        Title = novel.Title,
            //        PhotoName = novel.PhotoName,
            //        Chapters = novel.Chapters
            //        .OrderByDescending(chapter => chapter.Number)
            //        .Take(2).ToList()
            //    })
            //    .OrderByDescending(novel => novel.Chapters.Max(chapter => chapter.LastEdit))
            //    .Take(18)
            //    .ToList();

            //model.MostReadNovels = context.tblNovels
            //    .Select(novel => new MostReadNovelsViewModel
            //    {
            //        Id = novel.Id,
            //        Title = novel.Title,
            //        PhotoName = novel.PhotoName,
            //        Genres = novel.Genres,
            //        Theme = novel.Theme,
            //        ChaptersWatches = novel.Chapters.Sum(chapter => chapter.Watches)
            //    })
            //    .OrderByDescending(novel => novel.ChaptersWatches).Take(10) .ToList();

            //model.RecentNovels = context.tblNovels
            //    .OrderByDescending(novel => novel.PublishDate)
            //    .Take(18)
            //    .Select(novel => new NovelsViewModel()
            //    {
            //        Id = novel.Id,
            //        Title = novel.Title,
            //        PhotoName = novel.PhotoName
            //    })
            //    .ToList();

            return View(ActiveList);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
