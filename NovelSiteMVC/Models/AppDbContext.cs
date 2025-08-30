using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NovelSiteMVC.ViewModels;
using NovelSiteMVC.Models;

namespace NovelSiteMVC.Models
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext() : base()
        {
            
        }
        public AppDbContext(DbContextOptions<AppDbContext> contextOptions) : base(contextOptions) { }

        public DbSet<NovelModel> tblNovels { get; set; }
        public DbSet<ChapterModel> tblChapters { get; set; }
        public DbSet<BookmarkNovel> tblBookmarkedNovels { get; set; }
        public DbSet<CommentModel> tblComments { get; set; }
        public DbSet<TodoModel> tblTodos { get; set; }
        public DbSet<BlogModel> tblBlogs { get; set; }
        public DbSet<PostModel> tblPosts { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.HasSequence<int>("PageIdSequence");
            
            builder.Entity<NovelModel>().Property(x => x.PageId).HasDefaultValueSql("NEXT VALUE FOR PageIdSequence");
            builder.Entity<ChapterModel>().Property(x => x.PageId).HasDefaultValueSql("NEXT VALUE FOR PageIdSequence");
            builder.Entity<BlogModel>().Property(x => x.PageId).HasDefaultValueSql("NEXT VALUE FOR PageIdSequence");
            builder.Entity<PostModel>().Property(x => x.PageId).HasDefaultValueSql("NEXT VALUE FOR PageIdSequence");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=db16459.public.databaseasp.net; Database=db16459; User Id=db16459; Password=Zn7-p@Y6?8aS; Encrypt=True; TrustServerCertificate=True; MultipleActiveResultSets=True;");
            base.OnConfiguring(optionsBuilder);
        }
    }
}
