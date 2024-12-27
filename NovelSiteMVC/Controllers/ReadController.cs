using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using NovelSiteMVC.Models;
using NovelSiteMVC.ViewModels;

namespace NovelSiteMVC.Controllers
{
    public class ReadController : Controller
    {
        private readonly AppDbContext context;
        private readonly IWebHostEnvironment Environment;
        private readonly IMemoryCache cache;

        public ReadController(AppDbContext _context, IWebHostEnvironment _Environmetn, IMemoryCache _cache)
        {
            context = _context;
            Environment = _Environmetn;
            cache = _cache;
        }
        public IActionResult Index(int ChapterId)
        {
            //fetch chapter from database
            var chapter = context.tblChapters.SingleOrDefault(x => x.Id == ChapterId);
            if (chapter is null)
                return NotFound();

            //read chapter content and send it to read view
            string filePath = Path.Combine(Environment.WebRootPath, "assets", "Novels", chapter.NovelId.ToString(), $"{chapter.Number}");

            ReadViewModel model = new();

            model.Id = ChapterId;
            model.Title = chapter.Title;
            model.Number = chapter.Number;
            model.PRer = chapter.PRer;
            model.QCer = chapter.QCer;
            model.TLor = chapter.TLor;
            model.NovelId = chapter.NovelId;
            model.PageId = chapter.PageId;
            model.Comments = context.tblComments.Include(x => x.User)
                .Where(x => x.PageId == chapter.PageId).OrderBy(x => x.Id)
                .ToHashSet() ?? new HashSet<CommentModel>();
            ViewBag.pageId = chapter.PageId;
            ViewBag.ChaptersIndex = context.tblChapters.Where(x => x.NovelId == chapter.NovelId).OrderBy(x => x.Id).Select(col => col.Id).ToArray();

            //increase watches by one
            chapter.Watches++;
            context.SaveChanges();
            //TODO: migrate to in-mimory cache when the system be bigger
            if (System.IO.File.Exists(filePath + ".txt"))
            {
                model.Content = System.IO.File.ReadAllText(filePath + ".txt");
                return View(model);
            }
            else if (System.IO.File.Exists(filePath + ".pdf"))
            {
                model.Content = filePath + ".pdf";
                return View("IndexPdf", model);
            }
            else return BadRequest();

        }

        public async Task<IActionResult> GetPdfFile(string path)
        {
            byte[] fileBytes = await System.IO.File.ReadAllBytesAsync(path);

            return File(fileBytes, "application/pdf");
        }

    }
}
