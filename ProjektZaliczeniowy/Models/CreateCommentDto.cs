using ProjektZaliczeniowy.entities;
using System.ComponentModel.DataAnnotations;

namespace ProjektZaliczeniowy.Models
{
    public class CreateCommentDto
    {
        [Required]
        [MaxLength(1000)]
        public string Content { get; set; }
        [Required]
        public int ArticleID { get; set; }
        public int? AuthorId { get; set; }
    }
}
