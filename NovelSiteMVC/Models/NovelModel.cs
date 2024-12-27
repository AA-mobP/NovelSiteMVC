using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.Security.Policy;

namespace NovelSiteMVC.Models
{
    public class NovelModel
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(50)]
        public string Title { get; set; }
        [MaxLength(150)]
        public string AlterNames { get; set; } = string.Empty;
        [MaxLength(750)]
        public string Synposis { get; set; } = string.Empty;
        public DateOnly PublishDate { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);
        public DateTime LastEdit { get; set; } = DateTime.UtcNow;
        [ValidateNever]
        public string PhotoName { get; set; }
        [MaxLength(25)]
        public string Author { get; set; } = string.Empty;
        [MaxLength(25)]
        public string Artist { get; set; } = string.Empty;
        [MaxLength(25)]
        public string Publisher { get; set; } = string.Empty;
        [MaxLength(25)]
        public string Theme { get; set; } = string.Empty;
        [MaxLength(50)]
        public string Genres { get; set; } = string.Empty;

        public int PageId { get; set; }

        public virtual ICollection<BookmarkNovel> Bookmarks { get; set; } = new HashSet<BookmarkNovel>();
        public virtual ICollection<ChapterModel> Chapters { get; set; } = new HashSet<ChapterModel>();
    }
}
