using System.ComponentModel.DataAnnotations;

namespace NovelSiteMVC.Models
{
    public class NovelModel
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(50)]
        public string Title { get; set; }
        [MaxLength(150)]
        public string? AlterNames { get; set; }
        [MaxLength(750)]
        public string? Synposis { get; set; }
        public DateOnly PublishDate { get; set; }
        public DateTime LastEdit { get; set; } = DateTime.UtcNow;
        [MaxLength(50)]
        public string PhotoUrl { get; set; }
        [MaxLength(25)]
        public string? Author { get; set; }
        [MaxLength(25)]
        public string? Artist { get; set; }
        [MaxLength(25)]
        public string? Publisher { get; set; }
        [MaxLength(25)]
        public string? Theme { get; set; }
        [MaxLength(50)]
        public string? Genres { get; set; }

        public ICollection<ChapterModel> Chapters { get; set; } = new HashSet<ChapterModel>();
    }
}
