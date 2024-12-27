using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace NovelSiteMVC.Models
{
    [Index(nameof(PageId))]
    public class CommentModel
    {
        public int Id { get; set; }
        public int PageId { get; set; }
        public int? ReplyTo { get; set; }
        
        public string Content { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;

        public virtual ApplicationUser User { get; set; }
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
    }
}
