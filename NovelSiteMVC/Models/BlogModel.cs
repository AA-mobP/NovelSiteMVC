using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace NovelSiteMVC.Models
{
    public class BlogModel
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(50)]
        public string Title { get; set; }
        public int? PageId { get; set; }
        public bool isDeleted { get; set; } = false;
        public virtual ICollection<PostModel> Posts { get; set; } = new List<PostModel>();

        public override string ToString()
        {
            return Title;
        }
    }
}
