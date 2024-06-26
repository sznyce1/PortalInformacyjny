﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjektZaliczeniowy.entities;
using ProjektZaliczeniowy.Models;
using ProjektZaliczeniowy.Services;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;

namespace ProjektZaliczeniowy.Controllers
{
    [Route("api/category/{categoryName}/article/{articleId}/comments")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly IReactionService _reactionService;

        public CommentController(ICommentService commentService, IReactionService reactionService)
        {
            _commentService = commentService;
            _reactionService = reactionService;
        }
        [HttpPost]
        [Authorize]
        public ActionResult Post([FromRoute]string categoryName, [FromRoute]int articleId, CreateCommentDto dto)
        {
            
            var newCommentId = _commentService.Create(articleId, dto);
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
            _commentService.DeleteById(articleId, commentId);
            return NoContent();
        }
        [HttpPost("{commentId}/like")]
        [Authorize]
        public ActionResult Like([FromRoute] int commentId)
        {
            _reactionService.Like(null, commentId);
            return Ok();
        }
        [HttpPost("{commentId}/unlike")]
        [Authorize]
        public ActionResult UnLike([FromRoute] int commentId)
        {
            _reactionService.UnLike(null, commentId);
            return Ok();
        }
        [HttpPost("{commentId}/dislike")]
        [Authorize]
        public ActionResult DisLike([FromRoute] int commentId)
        {
            _reactionService.DisLike(null, commentId);
            return Ok();
        }
        [HttpPost("{commentId}/undislike")]
        [Authorize]
        public ActionResult UnDisLike([FromRoute] int commentId)
        {
            _reactionService.UnDisLike(null, commentId);
            return Ok();
        }
    }
}
