using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Interfaces;
using Backend.DTO;
using Backend.Exceptions;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;
        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet]
        public IActionResult GetAllComments()
        {
            return Ok(_commentService.GetAllComments());
        }

        [HttpGet("{id}")]
        public IActionResult GetCommentById(int id)
        {
            var comment = _commentService.GetCommentById(id);
            if (comment == null)
            {
                return NotFound();
            }
            return Ok(comment);
        }

        [HttpPost]
        public IActionResult AddComment([FromBody] CommentDto comment)
        {
            try
            {
                _commentService.AddComment(comment);
                return Created();
            }
            catch (ModelInvalidException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateComment(int id, [FromBody] CommentDto comment)
        {
            try 
            {
                _commentService.UpdateComment(comment);
                return Ok();
            }
            catch (ModelInvalidException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteComment(int id)
        {
            try
            {
                _commentService.DeleteComment(id);
                return Ok();
            }
            catch (ModelInvalidException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}