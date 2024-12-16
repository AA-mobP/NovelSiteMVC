using NovelSiteMVC.Models;
using System.ComponentModel.DataAnnotations;

namespace NovelSiteMVC.ViewModels
{
    public class MostReadNovelsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string PhotoName { get; set; }
        public string? Theme { get; set; }
        public string? Genres { get; set; }
        public int ChaptersWatches { get; set; }
    }
}
