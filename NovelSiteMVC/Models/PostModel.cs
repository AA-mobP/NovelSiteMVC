using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace NovelSiteMVC.Models
{
    public class PostModel
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(50)]
        public string Title { get; set; }        
        [ForeignKey("Blog")]
        public int BlogId { get; set; }
        public virtual BlogModel Blog{ get; set; }
        public DateTime LastEdit { get; set; } = DateTime.UtcNow;
        public int Watches { get; set; } = 0;
        [MaxLength(25)]
        public string Author { get; set; } = "مينه حسين";
        public string PhotoName { get; set; } = "Unkown.jpg";
        public string Category { get { return BlogId.ToString(); } private set { } }
        [ValidateNever]
        public int? PageId { get; set; }
        public bool isDeleted { get; set; } = false;
    }
}
