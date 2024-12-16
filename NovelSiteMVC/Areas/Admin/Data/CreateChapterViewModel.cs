using NovelSiteMVC.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NovelSiteMVC.Areas.Admin.Data
{
    public class CreateChapterViewModel
    {
        [Required, MaxLength(50)]
        public string Title { get; set; }
        [Required, MaxLength(50)]
        public string ContentUrl { get; set; }
        [Required]
        public int Number { get; set; }
        public int NovelId { get; set; }
        public DateTime LastEdit { get; set; } = DateTime.UtcNow;
        [MaxLength(25)]
        public string Releaser { get; set; }
        [MaxLength(25)]
        public string? TLor { get; set; }
        [MaxLength(25)]
        public string? PRer { get; set; }
        [MaxLength(25)]
        public string? QCer { get; set; }
    }
}
