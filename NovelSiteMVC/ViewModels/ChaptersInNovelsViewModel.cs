using NovelSiteMVC.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NovelSiteMVC.ViewModels
{
    public class ChaptersInNovelsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Number { get; set; }
        public DateTime LastEdit { get; set; }
        public int Watches { get; set; } = 0;
    }
}
