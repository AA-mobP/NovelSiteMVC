using System.ComponentModel.DataAnnotations;

namespace NovelSiteMVC.Models
{
    public class ApplicationUserModel
    {
        [MaxLength(25)]
        public string Country { get; set; }
        [MaxLength(25)]
        public string PhotoUrl { get; set; }
        [MaxLength(25)]
        public string BackgroundPhotoUrl { get; set; }
        [MaxLength(100)]
        public string BookmarkedNovelsIDs { get; set; }
    }
}
