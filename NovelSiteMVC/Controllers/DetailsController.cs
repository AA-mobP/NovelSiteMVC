using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NovelSiteMVC.Models;
using NovelSiteMVC.ViewModels;

namespace NovelSiteMVC.Controllers
{
    public class DetailsController : Controller
    {
        private readonly AppDbContext context;

        public DetailsController(AppDbContext _context)
        {
            context = _context;
        }
        public IActionResult Index(int id)
        {
            //fetch novel from db
            var novel = new NovelModel();
            novel = context.tblNovels.Include(x => x.Chapters).FirstOrDefault(x => x.Id == id);
            if (novel is null)
                return NotFound();
            
            //fill vm
            DetailsViewModel model = new DetailsViewModel();
            model.Id = id;
            model.Title = novel.Title;
            model.PhotoName = novel.PhotoName;
            model.AlterNames = novel.AlterNames;
            model.Author = novel.Author;
            model.Artist = novel.Artist;
            model.Publisher = novel.Publisher;
            model.PublishDate = novel.PublishDate;
            model.Chapters = novel.Chapters;
            model.Genres = novel.Genres;
            model.LastEdit = novel.LastEdit;
            model.Synposis = novel.Synposis;
            model.Theme = novel.Theme;

            return View(model);
        }
    }
}
