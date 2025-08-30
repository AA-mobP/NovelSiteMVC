using NovelSiteMVC.Models;
using System.ComponentModel.DataAnnotations;

namespace NovelSiteMVC.ViewModels
{
    public class CreateBlogViewModel
    {
        [Required, MaxLength(50)]
        public string Title { get; set; }
    }
}
