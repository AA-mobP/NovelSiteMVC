using NovelSiteMVC.Models;
using System.ComponentModel.DataAnnotations;

namespace NovelSiteMVC.ViewModels
{
    public class LastUpdateNovelsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string PhotoName { get; set; }
        public virtual ICollection<ChapterModel> Chapters { get; set; } = new List<ChapterModel>();
    }
}
