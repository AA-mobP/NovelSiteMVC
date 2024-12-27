using NovelSiteMVC.Models;
using System.ComponentModel.DataAnnotations;

namespace NovelSiteMVC.ViewModels
{
    public class ProfileViewModel
    {
        public string Id { get; set; } = default!;

        [Required]
        public string UserName { get; set; }

        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required, MaxLength(25)]
        public string Country { get; set; }  

        [MaxLength(25)]
        public string? PhotoUrl { get; set; }
        [MaxLength(25)]
        public string? BackgroundPhotoUrl { get; set; }
        [MaxLength(100)]
        public virtual ICollection<BookmarkNovel> Bookmarks { get; set; } = new HashSet<BookmarkNovel>();
    }
}
