using NovelSiteMVC.Models;
using System.ComponentModel.DataAnnotations;

namespace NovelSiteMVC.ViewModels
{
    public class DetailsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string PhotoName { get; set; }
        public string? AlterNames { get; set; }
        public string? Synposis { get; set; }
        public DateOnly PublishDate { get; set; }
        public DateTime LastEdit { get; set; } = DateTime.UtcNow;
        public string? Author { get; set; }
        public string? Artist { get; set; }
        public string? Publisher { get; set; }
        public string? Theme { get; set; }
        public string? Genres { get; set; }
        public int PageId { get; set; }
        public virtual ICollection<ChapterModel> Chapters { get; set; } = new HashSet<ChapterModel>();
        public virtual ICollection<CommentModel> Comments { get; set; } = new HashSet<CommentModel>();
    }
}
