using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NovelSiteMVC.ViewModels;

namespace NovelSiteMVC.Models
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext() : base() { }
        public AppDbContext(DbContextOptions contextOptions) : base(contextOptions) { }

        public DbSet<NovelModel> tblNovels { get; set; }
        public DbSet<ChapterModel> tblChapters { get; set; }
        public DbSet<BookmarkNovel> tblBookmarkedNovels { get; set; }
        public DbSet<NovelSiteMVC.ViewModels.AddChapterViewModel> AddChapterViewModel { get; set; } = default!;
    }
}
