namespace NovelSiteMVC.ViewModels
{
    public class HomeViewModel
    {
        public ICollection<LastUpdateNovelsViewModel> LastUpdateNovels { get; set; } = new List<LastUpdateNovelsViewModel>();
        public ICollection<MostReadNovelsViewModel> MostReadNovels { get; set; } = new List<MostReadNovelsViewModel>();
        public ICollection<NovelsViewModel> RecentNovels { get; set; } = new List<NovelsViewModel>();
    }
}
