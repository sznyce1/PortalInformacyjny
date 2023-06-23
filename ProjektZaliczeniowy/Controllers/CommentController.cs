using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjektZaliczeniowy.entities;
using ProjektZaliczeniowy.Models;
using ProjektZaliczeniowy.Services;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;

namespace ProjektZaliczeniowy.Controllers
{
    [Route("api/category/{categoryName}/article/{articleId}")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }
        [HttpPost]
        [Authorize]
        public ActionResult Post([FromRoute]string categoryName, [FromRoute]int articleId, CreateCommentDto dto)
        {
            var userId = int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var newCommentId = _commentService.Create(articleId, dto, userId);
            return Created($"api/category/{categoryName}/article/{articleId}/comment/{newCommentId}",null);
        }
        [HttpGet("{commentId}")]
        public ActionResult<Comment> Get([FromRoute] string categoryName, [FromRoute] int articleId, [FromRoute] int commentId)
        {
            Comment comment = _commentService.GetById(articleId, commentId);
            return Ok(comment);
        }
        [HttpGet]
        public ActionResult<List<Comment>> GetAll([FromRoute] string categoryName, [FromRoute] int articleId)
        {
            var comments = _commentService.GetAll(articleId);
            return Ok(comments);
        }
        [HttpDelete]
        [Authorize(Roles ="Admin")]
        public ActionResult Delete([FromRoute] int articleId) 
        {
            _commentService.RemoveAll(articleId);

            return NoContent();
        }
        [HttpDelete("{commentId}")]
        [Authorize]
        public ActionResult DeleteById([FromRoute] int articleId, [FromRoute] int commentId)
        {
            _commentService.DeleteById(articleId, commentId, User);
            return NoContent();
        }
    }
}
