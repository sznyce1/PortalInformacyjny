using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjektZaliczeniowy.entities
{
    public class Article
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }

        public virtual List<Comment> Comments { get; set; }
        public int Likes { get; set; }
        public int DisLikes { get; set; }
        public int CategoryId { get; set; }
    }
}
