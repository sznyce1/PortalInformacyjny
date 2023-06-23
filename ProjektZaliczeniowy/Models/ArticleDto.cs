using ProjektZaliczeniowy.entities;

namespace ProjektZaliczeniowy.Models
{
    public class ArticleDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public int Likes { get; set; }
        public int DisLikes { get; set; }
        public int? AuthorId { get; set; }

    }
}
