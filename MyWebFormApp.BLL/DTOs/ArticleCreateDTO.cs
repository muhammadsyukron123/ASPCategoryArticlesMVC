using System.ComponentModel.DataAnnotations;

namespace MyWebFormApp.BLL.DTOs
{
    public class ArticleCreateDTO
    {
        [Required]
        public int CategoryID { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        [StringLength(100), MinLength(5)]
        public string Details { get; set; }
        public bool IsApproved { get; set; }
        public string Pic { get; set; }
    }
}
