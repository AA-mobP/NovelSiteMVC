using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace NovelSiteMVC.Models
{
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(25)]
        public string Country { get; set; }
        [MaxLength(25)]
        public string? PhotoUrl { get; set; }
        [MaxLength(25)]
        public string? BackgroundPhotoUrl { get; set; }
        [MaxLength(100)]
        public virtual ICollection<BookmarkNovel> Bookmarks { get; set; } = new HashSet<BookmarkNovel>();
        public virtual ICollection<CommentModel> Comments { get; set; } = new HashSet<CommentModel>();
    }
}
