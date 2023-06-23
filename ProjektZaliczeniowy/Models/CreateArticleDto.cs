using ProjektZaliczeniowy.entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjektZaliczeniowy.Models
{
    public class CreateArticleDto
    {
        [Required]
        [MaxLength(40)]
        public string Name { get; set; }
        [Required]
        [MaxLength(5000)]
        public string Content { get; set; }
        public int CategoryId { get; set; }
        public int? AuthorId { get; set; }
    }
}
