using NovelSiteMVC.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace NovelSiteMVC.ViewModels
{
    public class AddChapterViewModel
    {
        public int Id { get; set; }
        [Required, MaxLength(50)]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public int Number { get; set; }
        [ValidateNever]
        public int NovelId { get; set; }
        [ValidateNever]
        public DateTime LastEdit { get; set; } = DateTime.UtcNow;
        [MaxLength(25)]
        public string? TLor { get; set; }
        [MaxLength(25)]
        public string? PRer { get; set; }
        [MaxLength(25)]
        public string? QCer { get; set; }
    }
}
