using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ProjektZaliczeniowy.Authorization;
using ProjektZaliczeniowy.entities;
using ProjektZaliczeniowy.Exceptions;
using ProjektZaliczeniowy.Models;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace ProjektZaliczeniowy.Services
{
    public interface ICommentService
    {
        int Create(int articleId, CreateCommentDto dto);
        Comment GetById(int articleId, int commentId);
        List<Comment> GetAll(int articleId);
        void RemoveAll(int articleId);
        void DeleteById(int articleId, int commentId, ClaimsPrincipal user);
    }
    public class CommentService : ICommentService
    {
        private readonly ArticleDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthorizationService _authorizationService;

        public CommentService(ArticleDbContext dbContext, IMapper mapper, IAuthorizationService authorizationService)
        {
            _context = dbContext;
            _mapper = mapper;
            _authorizationService = authorizationService;
        }
        public int Create(int articleId, CreateCommentDto dto)
        {
            var article = GetArticleById(articleId);
            var commentEntity = _mapper.Map<Comment>(dto);
            _context.Comments.Add(commentEntity);
            _context.SaveChanges();

            return commentEntity.Id;                
        }
        public Comment GetById(int articleId, int commentId)
        {
            var article = GetArticleById(articleId);
            var comment = _context.Comments.FirstOrDefault(c => c.Id == commentId);
            if(comment is null || comment.ArticleID != articleId)
            {
                throw new NotFoundException("Comment not found");
            }
            return comment;
        }
        public List<Comment> GetAll(int articleId)
        {
            var article = GetArticleById(articleId);
            return article.Comments;
        }
        public void RemoveAll(int articleId)
        {
            var article = GetArticleById(articleId);
            _context.RemoveRange(article.Comments);
            _context.SaveChanges();
        }
        public void DeleteById(int articleId, int commentId, ClaimsPrincipal user)
        {
            var article = GetArticleById(articleId);
            var comment = _context.Comments.FirstOrDefault(c => c.Id == commentId);
            if (comment is null || comment.ArticleID != articleId)
            {
                throw new NotFoundException("Comment not found");
            }

            var authorizationResult = _authorizationService.AuthorizeAsync(user, article, new ResourceOperationRequirement(ResourceOperation.Delete)).Result;

            _context.Remove(article.Comments.FirstOrDefault(c => c.Id == commentId));
            _context.SaveChanges();

        }
        private Article GetArticleById(int articleId)
        {
            var article = _context
                .Articles
                .Include(a => a.Comments)
                .FirstOrDefault(a => a.Id == articleId);
            if (article is null)
            {
                throw new NotFoundException("Article not found");
            }
            return article;
        }

        
    }
}
