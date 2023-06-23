using AutoMapper.Configuration.Annotations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using ProjektZaliczeniowy.entities;
using ProjektZaliczeniowy.Exceptions;
using System.Linq;

namespace ProjektZaliczeniowy.Services
{
    public interface IReactionService
    {
        void DisLike(int? article, int? comment);
        void Like(int? article, int? comment);
        void UnDisLike(int? article, int? comment);
        void UnLike(int? article, int? comment);
    }

    public class ReactionService : IReactionService
    {
        private readonly ArticleDbContext _dbContext;
        private readonly IUserContextService _userContextService;

        public ReactionService(ArticleDbContext dbContext, IUserContextService userContextService)
        {
            _dbContext = dbContext;
            _userContextService = userContextService;
        }

        public void Like(int? articleId, int? commentId)
        {

            if (articleId is not null)
            {
                var article = _dbContext.Articles.FirstOrDefault(a => a.Id == articleId);
                article.Likes++;
                _dbContext.Update(article);
                _dbContext.SaveChanges();
            }
            if (commentId is not null)
            {
                var comment = _dbContext.Comments.FirstOrDefault(a => a.Id == commentId);
                comment.Likes++;
                _dbContext.Update(comment);
                _dbContext.SaveChanges();

            }
        }
        public void DisLike(int? articleId, int? commentId)
        {

            if (articleId is not null)
            {
                var article = _dbContext.Articles.FirstOrDefault(a => a.Id == articleId);
                article.DisLikes++;
                _dbContext.Update(article);
                _dbContext.SaveChanges();
            }
            if (commentId is not null)
            {
                var comment = _dbContext.Comments.FirstOrDefault(a => a.Id == commentId);
                comment.DisLikes++;
                _dbContext.Update(comment);
                _dbContext.SaveChanges();

            }
        }
        public void UnLike(int? articleId, int? commentId)
        {

            if (articleId is not null)
            {
                var article = _dbContext.Articles.FirstOrDefault(a => a.Id == articleId);
                article.Likes--;
                _dbContext.Update(article);
                _dbContext.SaveChanges();
            }
            if (commentId is not null)
            {
                var comment = _dbContext.Comments.FirstOrDefault(a => a.Id == commentId);
                comment.Likes--;
                _dbContext.Update(comment);
                _dbContext.SaveChanges();

            }
        }
        public void UnDisLike(int? articleId, int? commentId)
        {

            if (articleId is not null)
            {
                var article = _dbContext.Articles.FirstOrDefault(a => a.Id == articleId);
                    article.DisLikes--;
                    _dbContext.Update(article);
                    _dbContext.SaveChanges();
            }
            if (commentId is not null)
            {
                var comment = _dbContext.Comments.FirstOrDefault(a => a.Id == commentId);
                    comment.DisLikes--;
                    _dbContext.Update(comment);
                    _dbContext.SaveChanges();
                
            }
        }
        public void validate(int? article, int? comment)
        {
            if(article is null && comment is null)
            {
                throw new BadRequestException("article and comment ids are null");
            }
        }
    }
}
