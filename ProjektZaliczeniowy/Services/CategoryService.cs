using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProjektZaliczeniowy.entities;
using ProjektZaliczeniowy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjektZaliczeniowy.Services
{

public interface ICategoryService
    {
        string Create(CreateCategoryDto dto);
        IEnumerable<Category> GetAll();
        Category GetByName(string name);
        bool Delete(int id);
    }

public class CategoryService : ICategoryService
    {
        private readonly ArticleDbContext _dbContext;
        private readonly IMapper _mapper;
        public CategoryService(ArticleDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public bool Delete(int id)
        {
            var category = _dbContext
                .Categories
                .FirstOrDefault(c => c.Id == id);
            if (category is null)
            {
                return false;
            }
            else
            {
                _dbContext.Categories.Remove(category);
                _dbContext.SaveChanges();
                return true;
            }

        }
        public Category GetByName(string name)
        {
            var category = _dbContext.Categories.Include(r => r.Articles).FirstOrDefault(c => c.Name == name);
            return category;
        }
        public IEnumerable<Category> GetAll()
        {
            var categories = _dbContext.Categories.Include(r => r.Articles).ToList();
            return categories;
        }
        public string Create(CreateCategoryDto dto)
        {
            var category = _mapper.Map<Category>(dto);
            _dbContext.Categories.Add(category);
            _dbContext.SaveChanges();
            return category.Name;
        }
    }
}
