namespace NovelSiteMVC.ViewModels
{
    public class PostsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime LastEdit { get; set; }
        public int Watches { get; set; } = 0;
        public string PhotoName { get; set; }
        public string Author { get; set; } = "مينه حسين";
        public string Category { get; set; } = string.Empty;
    }
}
