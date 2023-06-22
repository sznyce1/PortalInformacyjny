using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProjektZaliczeniowy.entities;
using ProjektZaliczeniowy.Exceptions;
using ProjektZaliczeniowy.Models;
using System.Collections.Generic;
using System.Linq;

namespace ProjektZaliczeniowy.Services
{
    public interface ICommentService
    {
        int Create(int articleId, CreateCommentDto dto);
        Comment GetById(int articleId, int commentId);
        List<Comment> GetAll(int articleId);
        void RemoveAll(int articleId);
        void DeleteById(int articleId, int commentId);
    }
    public class CommentService : ICommentService
    {
        private readonly ArticleDbContext _context;
        private readonly IMapper _mapper;

        public CommentService(ArticleDbContext dbContext, IMapper mapper)
        {
            _context = dbContext;
            _mapper = mapper;
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
        public void DeleteById(int articleId, int commentId)
        {
            var article = GetArticleById(articleId);
            var comment = _context.Comments.FirstOrDefault(c => c.Id == commentId);
            if (comment is null || comment.ArticleID != articleId)
            {
                throw new NotFoundException("Comment not found");
            }
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
