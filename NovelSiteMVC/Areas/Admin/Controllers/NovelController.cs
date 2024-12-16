using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NovelSiteMVC.Models;

namespace NovelSiteMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class NovelController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment Environment;

        public NovelController(AppDbContext context, IWebHostEnvironment _Environment)
        {
            _context = context;
            Environment = _Environment;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.tblNovels.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            NovelModel? novelModel = await _context.tblNovels
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (novelModel is null)
                novelModel = new();
            return View(novelModel);
        }

        public IActionResult Create()
        {
            NovelModel novelModel = new();
            return View(novelModel);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,AlterNames,Synposis,PublishDate,LastEdit,Author,Artist,Publisher,Theme,Genres")] NovelModel novelModel, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                //create Novel folder
                Directory.CreateDirectory(Path.Combine(Environment.WebRootPath, "assets", "Novels", novelModel.Title));

                //save image
                string PhotoName = $"{novelModel.Title}.{image.FileName.Split('.')[^1]}";
                string filePath = Path.Combine(Environment.WebRootPath, "assets", "image", "imag_novel", PhotoName);

                using FileStream fileStream = new FileStream(filePath, FileMode.Create);
                image.CopyTo(fileStream);

                //add to database
                novelModel.PhotoName = PhotoName;
                _context.Add(novelModel);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(novelModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("Id,Title,PhotoName,AlterNames,Synposis,PublishDate,LastEdit,Author,Artist,Publisher,Theme,Genres")] NovelModel novelModel, IFormFile? image)
        {
            if (!ModelState.IsValid)
                return View(novelModel);

            NovelModel? model = _context.tblNovels.Find(novelModel.Id);
            if (model is null)
                return NotFound();
            if (model.Title != novelModel.Title)
            {
                //rename Novel folder
                string dirPath = Path.Combine(Environment.WebRootPath, "assets", "Novels");
                Directory.Move(Path.Combine(dirPath, model.Title), Path.Combine(dirPath, novelModel.Title));
            }
            if (image is not null)
            {
                //remove old image, put newer instead
                string PhotoName = $"{novelModel.Title}.{image.FileName.Split('.')[^1]}";
                string filePath = Path.Combine(Environment.WebRootPath, "assets", "image", "imag_novel", PhotoName);
                
                System.IO.File.Delete(Path.Combine(Environment.WebRootPath, "assets", "image", "imag_novel", model.PhotoName));
                using FileStream fileStream = new FileStream(filePath, FileMode.Create);
                image.CopyTo(fileStream);
                fileStream.Close();
                novelModel.PhotoName = PhotoName;
            }
            _context.Entry(model).State = EntityState.Detached;
            _context.Update(novelModel);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var novelModel = await _context.tblNovels.FindAsync(id);
            if (novelModel is null)
                return NotFound();

            return View(novelModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var novelModel = await _context.tblNovels.FindAsync(id);
            if (novelModel != null)
            {
                _context.tblNovels.Remove(novelModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
