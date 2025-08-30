using NovelSiteMVC.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NovelSiteMVC.ViewModels
{
    public class ViewPostVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int BlogId { get; set; }
        public string Author { get; set; } = "مينه حسين";
        public string PhotoName { get; set; } = "Unkown";
        public string Category { get { return BlogId.ToString(); } private set { } }
        public int? PageId { get; set; }
        
        public string Content { get; set; }

    }
}
