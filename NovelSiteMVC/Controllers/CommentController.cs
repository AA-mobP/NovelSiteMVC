using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NovelSiteMVC.Models;

namespace NovelSiteMVC.Controllers
{
    public class CommentController : Controller
    {
        private readonly AppDbContext context;

        public CommentController(AppDbContext _context)
        {
            context = _context;
        }
        public IActionResult Index()
        {
            var model = context.tblComments.Include(x => x.User).Where(x => x.PageId == 1).OrderBy(x => x.Id).ToList();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(CommentModel model)
        {
            context.tblComments.Add(model);
            await context.SaveChangesAsync();
            
            return Json(model);
        }
    }
}
