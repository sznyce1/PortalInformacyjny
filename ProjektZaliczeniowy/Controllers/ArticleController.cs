using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjektZaliczeniowy.entities;
using ProjektZaliczeniowy.Models;
using ProjektZaliczeniowy.Services;
using System.Collections.Generic;
using System.Data;
using System.Security.Claims;

namespace ProjektZaliczeniowy.Controllers
{
    [Route("api/category/{categoryName}/article")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleService _articleService;
        private readonly IReactionService _reactionService;

        public ArticleController(IArticleService articleService, IReactionService reactionService)
        {
            _articleService = articleService;
            _reactionService = reactionService;
        }
        [HttpPost]
        [Authorize]
        public ActionResult Post([FromRoute] string categoryName, [FromBody] CreateArticleDto dto)
        {

            var newArticleId = _articleService.Create(categoryName, dto);

            return Created($"api/category/{categoryName}/article/{newArticleId}", null);

        }
        [HttpGet("{articleId}")]
        public ActionResult<ArticleDto> Get([FromRoute] string categoryName, [FromRoute] int articleId)
        {
            ArticleDto artricle = _articleService.Get(categoryName, articleId);

            return Ok(artricle);
        }
        [HttpGet]
        public ActionResult<List<ArticleDto>> GetAll([FromRoute] string categoryName)
        {
            var result = _articleService.GetAll(categoryName);

            return Ok(result);
        }
        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete([FromRoute] string categoryName)
        {
            _articleService.DeleteAll(categoryName);
            return NoContent();
        }
        [HttpDelete("{articleId}")]
        [Authorize]
        public ActionResult DeleteById([FromRoute] string categoryName, [FromRoute] int articleId)
        {
            _articleService.DeleteById(categoryName, articleId);
            return NoContent();
        }
        [HttpPost("{articleId}/like")]
        [Authorize]
        public ActionResult Like([FromRoute] int articleId)
        {
            _reactionService.Like(articleId, null);
            return Ok();
        }
        [HttpPost("{articleId}/unlike")]
        [Authorize]
        public ActionResult UnLike([FromRoute] int articleId)
        {
            _reactionService.UnLike(articleId, null);
            return Ok();
        }
        [HttpPost("{articleId}/dislike")]
        [Authorize]
        public ActionResult DisLike([FromRoute] int articleId)
        {
            _reactionService.DisLike(articleId, null);
            return Ok();
        }
        [HttpPost("{articleId}/undislike")]
        [Authorize]
        public ActionResult UnDisLike([FromRoute] int articleId)
        {
            _reactionService.UnDisLike(articleId, null);
            return Ok();
        }
    }
}
