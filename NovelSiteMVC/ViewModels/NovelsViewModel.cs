using NovelSiteMVC.Models;
using System.ComponentModel.DataAnnotations;

namespace NovelSiteMVC.ViewModels
{
    public class NovelsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string PhotoName { get; set; }
    }
}
