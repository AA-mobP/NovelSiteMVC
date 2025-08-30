using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NovelSiteMVC.Models;
using NovelSiteMVC.ViewModels;

namespace NovelSiteMVC.Controllers
{
    public class BlogController : Controller
    {
        private readonly AppDbContext context;
        private readonly IWebHostEnvironment Environment;

        public BlogController(AppDbContext _context, IWebHostEnvironment _Environment)
        {
            context = _context;
            Environment = _Environment;
        }

        //fetch all not deleted blogs from db
        public IActionResult Index()
        {
            var model = context.tblBlogs.Where(x => !x.isDeleted).Select(x => new BlogListViewModel()
            {
                Id = x.Id,
                Title = x.Title,
                PageId = x.PageId
            }).ToList();

            return View(model);
        }
        
        public IActionResult Create()
        {
            CreateBlogViewModel blogModel = new();
            return View(blogModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title")] CreateBlogViewModel blogVM)
        {
            if (!ModelState.IsValid)
                return View(blogVM);
            BlogModel blogModel = new()
            {
                Title = blogVM.Title
            };
            //add to database
            await context.AddAsync(blogModel);
            await context.SaveChangesAsync();

            //create Novel folder
            Directory.CreateDirectory(Path.Combine(Environment.WebRootPath, "assets", "Blogs", blogModel.Id.ToString()));

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Details(int? id)
        {
            var blogModel = await context.tblBlogs.Select(x => new EditBlogViewModel() { Id = x.Id, Title = x.Title, PageId = x.PageId })
                .FirstOrDefaultAsync(m => m.Id == id);

            if (blogModel is null)
                blogModel = new();
            return View(blogModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditBlogViewModel blogVM)
        {
            if (!ModelState.IsValid)
                return View("Details", blogVM);

            BlogModel? blogModel = context.tblBlogs.Find(blogVM.Id);
            if (blogModel is null)
                return NotFound();

            if (blogModel.isDeleted)
                return BadRequest();

            blogModel.Title = blogVM.Title;
            
            context.Update(blogModel);
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            var blogModel = await context.tblBlogs.Include(x => x.Posts).FirstOrDefaultAsync(x => x.Id == id);

            if (blogModel is null)
                return BadRequest();
            blogModel.isDeleted = true;

            foreach(var post in blogModel.Posts)
            {
                post.isDeleted = true;
            }

            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
