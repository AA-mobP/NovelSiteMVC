using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using NovelSiteMVC.Models;
using NovelSiteMVC.ViewModels;
using System;

namespace NovelSiteMVC.Controllers
{
    public class PostController : Controller
    {
        private readonly AppDbContext context;
        private readonly IWebHostEnvironment Environment;

        public PostController(AppDbContext _context, IWebHostEnvironment _Environment)
        {
            context = _context;
            Environment = _Environment;
        }
        public IActionResult Create(int blogId)
        {
            CreatePostViewModel blogModel = new()
            {
                BlogId = blogId
            };
            return View(blogModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePostViewModel postVM, int blogId, IFormFile? image)
        {
            if (!ModelState.IsValid)
                return View(postVM);
            PostModel postModel = new()
            {
                BlogId = blogId,
                Title = postVM.Title,
                Author = postVM.Author,
            };
            //add to database
            await context.AddAsync(postModel);
            await context.SaveChangesAsync();

            //save content of the post in .txt file
            {
                string folderPath = Path.Combine(Environment.WebRootPath, "assets", "Blogs", blogId.ToString());
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);
                string filePath = Path.Combine(folderPath, $"{postModel.Id.ToString()}.txt");
                await System.IO.File.WriteAllTextAsync(filePath, postVM.Content);
            }
            //save Image to 
            if (image is not null)
            {
                string folderPath = Path.Combine(Environment.WebRootPath, "uploads", "covers");
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);
                string filePath = Path.Combine(folderPath, $"{postModel.Id.ToString()}{Path.GetExtension(image.FileName)}");
                using FileStream stream = new FileStream(filePath, FileMode.Create);
                image.CopyTo(stream);
                postModel.PhotoName = $"{postModel.Id.ToString()}{Path.GetExtension(image.FileName)}";
            }
            context.Update(postModel);
            await context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = postModel.Id });
        }

        public async Task<IActionResult> Details(int? id)
        {

            var postVM = await context.tblPosts.Select(x => new EditPostViewModel() { Id = x.Id, BlogId = x.BlogId, Title = x.Title, Author = x.Author, PhotoName = x.PhotoName })
                .FirstOrDefaultAsync(m => m.Id == id);

            if (postVM is null)
                return BadRequest();

            string contentFilePath = Path.Combine(Environment.WebRootPath, "assets", "Blogs", postVM.BlogId.ToString(), $"{postVM.Id.ToString()}.txt");
            postVM.Content = await System.IO.File.ReadAllTextAsync(contentFilePath);
            return View(postVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditPostViewModel postVM, IFormFile? image)
        {
            if (!ModelState.IsValid)
                return View("Details", postVM);

            PostModel? postModel = context.tblPosts.Find(postVM.Id);
            if (postModel is null)
                return NotFound();

            if (postModel.isDeleted)
                return BadRequest();

            postModel.Title = postVM.Title;
            postModel.Author = postVM.Author;

            //update content of the post in .txt file
            {
                string folderPath = Path.Combine(Environment.WebRootPath, "assets", "Blogs", postVM.BlogId.ToString());
                if (!Directory.Exists(folderPath))
                    return BadRequest();
                string filePath = Path.Combine(folderPath, $"{postModel.Id.ToString()}.txt");
                await System.IO.File.WriteAllTextAsync(filePath, postVM.Content);
            }

            if (image is not null && image.Length > 0)
            {
                string folderPath = Path.Combine(Environment.WebRootPath, "uploads", "covers");
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);
                
                string newPhotoName = $"{postModel.Id.ToString()}{Path.GetExtension(image.FileName)}";
                string oldFilePath = Path.Combine(folderPath, $"{postModel.PhotoName}");
                string newFilePath = Path.Combine(folderPath, newPhotoName);
                
                //delete old image
                System.IO.File.Delete(oldFilePath);

                //add new image
                using FileStream stream = new FileStream(newFilePath, FileMode.Create);
                image.CopyTo(stream);
                postModel.PhotoName = newPhotoName;
            }

            context.Update(postModel);
            await context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = postVM.Id });
        }

        public async Task<IActionResult> Delete(int? id)
        {
            var postModel = await context.tblPosts.FindAsync(id);

            if (postModel is null)
                return BadRequest();
            postModel.isDeleted = true;

            await context.SaveChangesAsync();
            return RedirectToAction("Index", "WebsiteBlog");
        }

        public async Task<IActionResult?> UploadPostImage(IFormFile? file)
        {
            if (file == null || file.Length == 0)
            {
                return Json(new { error = "No file uploaded." });
            }

            var uploadsFolder = Path.Combine(Environment.WebRootPath, "uploads", "posts");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // توليد اسم عشوائي للملف
            var fileName = DateTime.UtcNow.ToString("yyyy-MM-dd hh-mm-ss") + Path.GetExtension(file.FileName);

            var filePath = Path.Combine(uploadsFolder, fileName);

            // حفظ الملف في wwwroot/uploads
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // بناء Absolute URL
            
            var imageUrl = $"/uploads/posts/{fileName}";

            // TinyMCE يتوقع Json فيه location
            return Json(new { location = imageUrl });
        }

        public async Task<IActionResult> getRelativeToBlog(int blogId)
        {
            var listModel = await context.tblPosts.Where(post => post.BlogId == blogId && !post.isDeleted).Select(x => new PostsViewModel()
            {
                Id = x.Id,
                Author = x.Author,
                LastEdit = x.LastEdit,
                Title = x.Title,
                Watches = x.Watches
            }).ToListAsync();
            ViewBag.BlogId = blogId;
            return PartialView("getRelativeToBlog", listModel);
        }
    }
}
