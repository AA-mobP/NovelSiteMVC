using System.ComponentModel.DataAnnotations.Schema;

namespace NovelSiteMVC.Models
{
    public class BookmarkedNovelModel
    {
        [ForeignKey("Novel")]
        public int NovelId { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        public DateTime BookmarkDate { get; set; } = DateTime.UtcNow;
        public NovelModel Novel { get; set; }
        public ApplicationUserModel User { get; set; }
    }
}
