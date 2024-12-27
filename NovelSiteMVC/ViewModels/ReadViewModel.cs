using NovelSiteMVC.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NovelSiteMVC.ViewModels
{
    public class ReadViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Number { get; set; }
        public int NovelId { get; set; }
        public string? TLor { get; set; }
        public string? PRer { get; set; }
        public string? QCer { get; set; }
        public string Content { get; set; }
        public int PageId { get; set; }
        public virtual ICollection<CommentModel> Comments { get; set; } = new HashSet<CommentModel>();
    }
}
