using System.ComponentModel.DataAnnotations;

namespace NovelSiteMVC.ViewModels
{
    public class EditPostViewModel
    {
        public int Id { get; set; }
        public int BlogId { get; set; }
        [Required, MaxLength(50)]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        [MaxLength(25)]
        public string Author { get; set; } = "مينه حسين";
        public string PhotoName { get; set; } = "Unkown.jpg";
    }
}
