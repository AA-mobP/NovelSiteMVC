using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NovelSiteMVC.Models
{
    public class ChapterModel
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(50)]
        public string Title { get; set; }
        [Required]
        public int Number { get; set; }
        [ForeignKey("Novel")]
        public int NovelId { get; set; }
        public NovelModel Novel { get; set; }
        public DateTime LastEdit { get; set; } = DateTime.UtcNow;
        [MaxLength(25)]
        public string Releaser { get; set; }
        [MaxLength(25)]
        public string? TLor { get; set; }
        [MaxLength(25)]
        public string? PRer { get; set; }
        [MaxLength(25)]
        public string? QCer { get; set; }
        public int Watches { get; set; } = 0;
    }
}
