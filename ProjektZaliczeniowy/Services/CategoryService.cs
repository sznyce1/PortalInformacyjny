using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProjektZaliczeniowy.entities;
using ProjektZaliczeniowy.Exceptions;
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
        void Delete(int id);
    }

public class CategoryService : ICategoryService
    {
        private readonly ArticleDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryService> _logger;
        public CategoryService(ArticleDbContext dbContext, IMapper mapper, ILogger<CategoryService> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }
        public void Delete(int id)
        {
            _logger.LogError($"Category with id: {id} has been DELETED");
            var category = _dbContext
                .Categories
                .FirstOrDefault(c => c.Id == id);
            if (category is null)
            {
                throw new NotFoundException("Category Not Found");
            }
            else
            {
                _dbContext.Categories.Remove(category);
                _dbContext.SaveChanges();

            }

        }
        public Category GetByName(string name)
        {
            var category = _dbContext.Categories.Include(r => r.Articles).FirstOrDefault(c => c.Name == name);
            if (category is null)
                throw new NotFoundException("Category Not Found");
            return category;
        }
        public IEnumerable<Category> GetAll()
        {
            var categories = _dbContext.Categories.Include(r => r.Articles).ToList();
                if (categories is null)
                    throw new NotFoundException("Category Not Found");
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
