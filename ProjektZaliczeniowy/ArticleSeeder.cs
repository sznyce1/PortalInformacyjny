﻿using ProjektZaliczeniowy.entities;
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
                if (!_dbContext.Roles.Any())
                {
                    var Roles = GetRoles();
                    _dbContext.Roles.AddRange(Roles);
                    _dbContext.SaveChanges();
                }
                if (!_dbContext.Categories.Any())
                {
                    var Categories = GetCategories();
                    _dbContext.Categories.AddRange(Categories);
                    _dbContext.SaveChanges();
                }
            }
        }
        private IEnumerable<Role> GetRoles()
        {
            var roles = new List<Role>()
            {
                new Role()
                {
                    Name= "User"
                },
                new Role()
                {
                    Name= "Admin"
                },
            };
            return roles;
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
                            },
                            new Comment()
                            {
                            Content = "Lorem Ipsum Lorem Ipsum Lorem Ipsum Lorem Ipsum",
                            Likes = 0,
                            DisLikes = 0,
                            }
                        },
                        Likes = 20,
                        DisLikes = 10,
                        },
                        new Article()
                        {
                        Name = "Test2",
                        Content = "Lorem Ipsum Lorem Ipsum Lorem Ipsum Lorem Ipsum Lorem Ipsum",
                        Comments = new List<Comment>()
                        {
                            new Comment()
                            {
                            Content = "Lorem Ipsum Lorem Ipsum",
                            Likes = 1,
                            DisLikes = 1,
                            },
                            new Comment()
                            {
                            Content = "Lorem Ipsum Lorem Ipsum Lorem Ipsum Lorem Ipsum",
                            Likes = 0,
                            DisLikes = 0,
                            }
                        },
                        Likes = 100,
                        DisLikes = 100,
                        }
                    }
                }
                
            };
            return Categories;
        }
    }
}
