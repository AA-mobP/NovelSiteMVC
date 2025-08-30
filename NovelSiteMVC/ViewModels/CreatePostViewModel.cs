using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using NovelSiteMVC.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NovelSiteMVC.ViewModels
{
    public class CreatePostViewModel
    {
        public int BlogId { get; set; }
        [Required, MaxLength(50)]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        [MaxLength(25)]
        public string Author { get; set; } = "مينه حسين";
    }
}
