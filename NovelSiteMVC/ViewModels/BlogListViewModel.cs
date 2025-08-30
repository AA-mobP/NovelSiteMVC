using NovelSiteMVC.Models;
using System.ComponentModel.DataAnnotations;

namespace NovelSiteMVC.ViewModels
{
    public class BlogListViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int? PageId { get; set; }
    }
}
