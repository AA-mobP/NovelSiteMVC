using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using NovelSiteMVC.Models;
using NovelSiteMVC.ViewModels;

namespace NovelSiteMVC.Controllers
{
    public class WebsitePostController : Controller
    {
        private readonly AppDbContext context;
        private readonly IWebHostEnvironment environment;

        public WebsitePostController(AppDbContext _context, IWebHostEnvironment _environment)
        {
            context = _context;
            environment = _environment;
        }

        // GET: WebsitePost
        public async Task<IActionResult> Index(int? id)
        {
            if (id is null)
                return BadRequest();

            var model = context.tblPosts.AsNoTracking().Select(x => new ViewPostVM() { 
                Id = x.Id,
                BlogId = x.BlogId,
                PageId = x.PageId,
                Title = x.Title,
                PhotoName = x.PhotoName,
                Author = x.Author})
                .FirstOrDefault(x => x.Id == id);
            if ( model is null)
                return NotFound(); 

            string folderPath = Path.Combine(environment.WebRootPath, "assets", "Blogs", model.BlogId.ToString());
            string filePath = Path.Combine(folderPath, $"{model.Id.ToString()}.txt");

            if(!System.IO.File.Exists(filePath))
                return NotFound();
            model.Content = await System.IO.File.ReadAllTextAsync(filePath);

            return View(model);
        }
    }
}
