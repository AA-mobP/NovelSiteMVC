using System.ComponentModel.DataAnnotations;

namespace NovelSiteMVC.ViewModels
{
    public class LoginViewModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string LoginKey { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
