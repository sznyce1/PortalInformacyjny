using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjektZaliczeniowy.entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int Likes { get; set; }
        public int DisLikes { get; set; }
        public int ArticleID { get; set; }
    }
}
