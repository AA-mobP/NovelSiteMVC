using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.ObjectModelRemoting;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
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
        public async Task<IActionResult> Create(AddChapterViewModel chapterModel, IFormFile? pdfFile)
        {
            //some checks
            if (!ModelState.IsValid)
                return View(chapterModel);

            {
                NovelModel? novel = await context.tblNovels.FindAsync(chapterModel.NovelId);
                if (novel is null)
                    return BadRequest();
            }

            string filePath = Path.Combine(Environment.WebRootPath, "assets", "Novels", chapterModel.NovelId.ToString(), $"{chapterModel.Number}");

            if (System.IO.File.Exists(filePath + ".txt") || System.IO.File.Exists(filePath + ".pdf"))
            {
                ModelState.AddModelError("Number", "this Chapter Number is Already Exists!");
                return View(chapterModel);
            }

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

            //write the file & discuss what FileName extinsion to add
            if (pdfFile is null)
            {
                //save the content to novel folder
                await System.IO.File.WriteAllTextAsync(filePath + ".txt", chapterModel.Content);
                chapter.FileName = $"{chapter.Number}.txt";
            }
            else
            {
                FileStream file = new(filePath + ".pdf", FileMode.Create);
                await pdfFile.CopyToAsync(file);
                chapter.FileName = $"{chapter.Number}.pdf";
            }
            //save db
            context.tblChapters.Add(chapter);
            context.SaveChanges();
            //done
            return RedirectToAction(nameof(Details), "Novel", new { id = chapterModel.NovelId });
        }

        public async Task<IActionResult> getRelativeToNovel(int novelId)
        {
            var listModel = await context.tblChapters.Where(chapter => chapter.NovelId == novelId).ToListAsync();
            ViewBag.NovelId = novelId;
            return PartialView("getRelativeToNovel", listModel);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var model = await context.tblChapters.FindAsync(id);
            if (model is null)
                return BadRequest();

            string filePath = Path.Combine(Environment.WebRootPath, "assets", "novels", model.NovelId.ToString(), model.FileName);

            var viewModel = new AddChapterViewModel()
            {
                Id = model.Id,
                Title = model.Title,
                Number = model.Number,
                FileName = model.FileName,
                LastEdit = model.LastEdit,
                NovelId = model.NovelId,
                PRer = model.PRer,
                TLor = model.TLor,
                QCer = model.QCer
            };

            if (viewModel.FileName.Contains(".txt"))
                viewModel.Content = await System.IO.File.ReadAllTextAsync(filePath);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            var model = await context.tblChapters.FindAsync(id);
            if (model is not null)
                context.Remove(model);
            return View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            var model = await context.tblChapters.FindAsync(id);
            if (model is null)
                return BadRequest();

            string filePath = Path.Combine(Environment.WebRootPath, "assets", "novels", model.NovelId.ToString(), model.FileName);
            var viewModel = new AddChapterViewModel()
            {
                Id = model.Id,
                Title = model.Title,
                Number = model.Number,
                FileName = model.FileName,
                LastEdit = model.LastEdit,
                NovelId = model.NovelId,
                PRer = model.PRer,
                TLor = model.TLor,
                QCer = model.QCer
            };

            if (viewModel.FileName.Contains(".txt"))
                viewModel.Content = await System.IO.File.ReadAllTextAsync(filePath);

            return View(viewModel);
        }

        public async Task<IActionResult> Edit(AddChapterViewModel chapterModel, IFormFile? pdfFile)
        {
            //some checks
            if (!ModelState.IsValid)
                return View(chapterModel);

            var chapter = await context.tblChapters.FindAsync(chapterModel.Id);
            if (chapter is null)
                return BadRequest();
            string oldFilePath = Path.Combine(Environment.WebRootPath, "assets", "Novels", chapter.NovelId.ToString());
            if (chapterModel.Number != chapter.Number)
            {
                if (System.IO.File.Exists(Path.Combine(oldFilePath, chapterModel.FileName)))
                {
                    ModelState.AddModelError("Number", "this Chapter Number is Already Exists!");
                    return View(chapterModel);
                }
                System.IO.File.Move(Path.Combine(oldFilePath, chapter.FileName), Path.Combine(oldFilePath, chapterModel.FileName));
            }

            #region EditContent
            if (pdfFile is not null)
            {
                chapterModel.FileName = $"{chapterModel.Number}.pdf";
                string newFilePath = Path.Combine(oldFilePath, chapterModel.FileName);
                oldFilePath = Path.Combine(oldFilePath, chapter.FileName);

                //delete old file, create new instead
                System.IO.File.Delete(oldFilePath);
                FileStream file = new(newFilePath, FileMode.Create);
                await pdfFile.CopyToAsync(file);
            }
            else
            {
                chapterModel.FileName = $"{chapterModel.Number}.txt";
                //if chapter number changed => file path will be: oldFilePath + chapterModel.FileName
                //if chapter number not changed => chapterModel.FileName will equal chapter.FileName
                await System.IO.File.WriteAllTextAsync(Path.Combine(oldFilePath, chapterModel.FileName), chapterModel.Content);
            }
            #endregion

            //add info to the db

            chapter.Id = chapterModel.Id;
            chapter.Title = chapterModel.Title;
            chapter.Number = chapterModel.Number;
            chapter.NovelId = chapterModel.NovelId;
            chapter.Releaser = "admin";
            //TODO: make releaser ;ynamic when finishing identity module
            chapter.TLor = chapterModel.TLor;
            chapter.PRer = chapterModel.PRer;
            chapter.QCer = chapterModel.QCer;
            chapter.LastEdit = DateTime.UtcNow;
            chapter.FileName = chapterModel.FileName;
            //save db
            context.Update(chapter);
            context.SaveChanges();

            return RedirectToAction(nameof(Details), "Novel", new { id = chapterModel.NovelId });
        }
    }
}
