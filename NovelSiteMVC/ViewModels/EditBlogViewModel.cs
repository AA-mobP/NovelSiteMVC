using NovelSiteMVC.Models;
using System.ComponentModel.DataAnnotations;

namespace NovelSiteMVC.ViewModels
{
    public class EditBlogViewModel
    {
        public int Id { get; set; }
        [Required, MaxLength(50)]
        public string Title { get; set; }
        public int? PageId { get; set; }
    }
}
