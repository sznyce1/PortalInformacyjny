using Microsoft.AspNetCore.Mvc;
using ProjektZaliczeniowy.Models;
using ProjektZaliczeniowy.Services;
using System.Collections.Generic;

namespace ProjektZaliczeniowy.Controllers
{
    [Route("api/category/{categoryName}/article")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleService _articleService;

        public ArticleController(IArticleService articleService)
        {
            _articleService = articleService;
        }
        [HttpPost]
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
        public ActionResult Delete([FromRoute] string categoryName)
        {
            _articleService.DeleteAll(categoryName);
            return NoContent();
        }
        [HttpDelete("{articleId}")]
        public ActionResult DeleteById([FromRoute] string categoryName, [FromRoute] int articleId)
        {
            _articleService.DeleteById(categoryName, articleId);
            return NoContent();
        }
    }
}
