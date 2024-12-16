using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NovelSiteMVC.Areas.Admin.Data;
using NovelSiteMVC.Models;
using NovelSiteMVC.ViewModels;

namespace NovelSiteMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ChapterController : Controller
    {
        private readonly AppDbContext context;
        private readonly IWebHostEnvironment Environment;

        public ChapterController(AppDbContext _context, IWebHostEnvironment _Environment)
        {
            context = _context;
            Environment = _Environment;
        }
        public IActionResult Index(int novelId)
        {
            var model = context.tblChapters.Where(chapter => chapter.NovelId == novelId).ToList();
            return View(model);
        }

        public IActionResult Create(int novelId)
        {
            //spcify which novel the chapter belongs to
            AddChapterViewModel chapter = new();
            chapter.NovelId = novelId;

            return View(chapter);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddChapterViewModel chapterModel)
        {
            //some checks
            if (!ModelState.IsValid)
                return View(chapterModel);

            NovelModel? novel = await context.tblNovels.FindAsync(chapterModel.NovelId);
            if (novel is null)
                return BadRequest();
            
            string filePath = Path.Combine(Environment.WebRootPath, "assets", "Novels", novel.Title, $"{chapterModel.Number}.txt");
            
            if (System.IO.File.Exists(filePath))
            {
                ModelState.AddModelError("Number", "this Chapter Number is Already Exists!");
                return View(chapterModel);
            }

            //save the content to novel folder
            await System.IO.File.WriteAllTextAsync(filePath, chapterModel.Content);

            //add info to the db
            ChapterModel chapter = new()
            {
                Id = chapterModel.Id,
                Title = chapterModel.Title,
                Number = chapterModel.Number,
                NovelId = chapterModel.NovelId,
                Releaser = "admin",
                //TODO: make releaser Dynamic when finishing identity module
                TLor = chapterModel.TLor,
                PRer = chapterModel.PRer,
                QCer = chapterModel.QCer,
                Watches = 0,
                LastEdit = DateTime.UtcNow
            };
            context.tblChapters.Add(chapter);
            context.SaveChanges();
            
            return RedirectToAction(nameof(Index), new { novelId= chapterModel.NovelId });
        }

        public async Task<IActionResult> getRelativeToNovel(int novelId)
        {
            var listModel = await context.tblChapters.Where(chapter => chapter.NovelId == novelId).ToListAsync();
            return PartialView("getRelativeToNovel", listModel);
        }
    }
}
