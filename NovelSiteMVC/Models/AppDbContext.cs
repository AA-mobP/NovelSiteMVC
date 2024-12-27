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
        public DbSet<CommentModel> tblComments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.HasSequence<int>("PageIdSequence");
            
            builder.Entity<NovelModel>().Property(x => x.PageId).HasDefaultValueSql("NEXT VALUE FOR PageIdSequence");
            builder.Entity<ChapterModel>().Property(x => x.PageId).HasDefaultValueSql("NEXT VALUE FOR PageIdSequence");
        }

    }
}
