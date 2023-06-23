using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjektZaliczeniowy.Authorization;
using ProjektZaliczeniowy.entities;
using ProjektZaliczeniowy.Exceptions;
using ProjektZaliczeniowy.Models;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;

namespace ProjektZaliczeniowy.Services
{
    public interface IArticleService 
    {
        int Create(string categoryName, CreateArticleDto dto);
        void DeleteAll(string categoryName);
        void DeleteById(string categoryName, int articleId);
        ArticleDto Get(string categoryName, int articleId);
        List<ArticleDto> GetAll(string categoryName);
    }
    public class ArticleService : IArticleService
    {
        private readonly ArticleDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserContextService _userContextService;

        public ArticleService(ArticleDbContext context, IMapper mapper, IAuthorizationService authorizationService, IUserContextService userContextService)
        {
            _context = context;
            _mapper = mapper;
            _authorizationService = authorizationService;
            _userContextService = userContextService;
        }
        public int Create(string categoryName, CreateArticleDto dto)
        {
            var category = GetCategoryByName(categoryName);
            
            var articleEntity = _mapper.Map<Article>(dto);
            articleEntity.CategoryId = category.Id;
            articleEntity.AuthorId = _userContextService.GetUserId;
            _context.Articles.Add(articleEntity);
            _context.SaveChanges();

            return articleEntity.Id;
        }
        public ArticleDto Get(string categoryName, int articleId)
        {
            var category = GetCategoryByName(categoryName);

            var article = _context.Articles.FirstOrDefault(a => a.Id == articleId);
            if (article is null || article.CategoryId != category.Id)
            {
                throw new NotFoundException("Article not found");
            }

            var articleDto = _mapper.Map<ArticleDto>(article);

            return articleDto;
        }

        public List<ArticleDto> GetAll(string categoryName)
        {
            var category = GetCategoryByName(categoryName);

            var articleDtos = _mapper.Map<List<ArticleDto>>(category.Articles);

            return articleDtos;
        }
        public void DeleteAll(string categoryName)
        {
            var category = GetCategoryByName(categoryName);

            _context.RemoveRange(category.Articles);
            _context.SaveChanges();
        }
        public void DeleteById(string categoryName, int articleId)
        {
            var category = GetCategoryByName(categoryName);

            var article = category.Articles.FirstOrDefault(a => a.Id == articleId);
            if (article is null)
                throw new NotFoundException("Article Not Found");

            var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, article, new ResourceOperationRequirement(ResourceOperation.Delete)).Result;
            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException();
            }

            _context.Remove(category.Articles.FirstOrDefault(a => a.Id == articleId));
            _context.SaveChanges();
        }


        private Category GetCategoryByName(string categoryName)
        {
            var category = _context
                .Categories
                .Include(c => c.Articles)
                .FirstOrDefault(c => c.Name == categoryName);
            if (category == null)
                throw new NotFoundException("Category not found");

            return category;
        }
    }
}
