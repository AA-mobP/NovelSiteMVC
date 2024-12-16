using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NovelSiteMVC.Models
{
    [PrimaryKey("NovelId", "UserId")]
    public class BookmarkNovel
    {
        public int NovelId { get; set; }
        public string UserId { get; set; }

        public DateTime BookmarkDate { get; set; } = DateTime.UtcNow;

        [ForeignKey("NovelId")]
        public NovelModel Novel { get; set; }
        
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
    }
}
