using ProjektZaliczeniowy.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjektZaliczeniowy
{
    public class ArticleSeeder
    {
        private readonly ArticleDbContext _dbContext;
        public ArticleSeeder(ArticleDbContext dbContext)
        {
            _dbContext = dbContext;

        }
        public void Seed()
        {
            if (_dbContext.Database.CanConnect())
            {
                if (!_dbContext.Categories.Any())
                {
                    var Categories = GetCategories();
                    _dbContext.Categories.AddRange(Categories);
                    _dbContext.SaveChanges();
                }
            }
        }
        private IEnumerable<Category> GetCategories()
        {
            var Categories = new List<Category>()
            {
                new Category
                {
                    Name = "test name",
                    Articles = new List<Article>()
                    {
                        new Article()
                        {
                        Name = "Test1",
                        Content = "Lorem Ipsum Lorem Ipsum Lorem Ipsum Lorem Ipsum Lorem Ipsum",
                        Comments = new List<Comment>()
                        {
                            new Comment()
                            {
                            Content = "Lorem Ipsum Lorem Ipsum",
                            Likes = 10,
                            DisLikes = 20,
                            }
                        },
                        Likes = 20,
                        DisLikes = 10,
                        }
                    }
                }
                
            };
            return Categories;
        }
    }
}
