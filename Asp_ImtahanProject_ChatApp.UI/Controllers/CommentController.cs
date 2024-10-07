using Asp_ImtahanProject_ChatApp.Business.Abstract;
using Asp_ImtahanProject_ChatApp.Entities.Concrete;
using Asp_ImtahanProject_ChatApp.UI.Models.CommentModels;
using AutoMapper;
using AutoMapper.Execution;
using Microsoft.AspNetCore.Mvc;

namespace Asp_ImtahanProject_ChatApp.UI.Controllers
{
    public class CommentController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ICommentService _commentService;

        public CommentController(IMapper mapper, ICommentService commentService)
        {
            _mapper = mapper;
            _commentService = commentService;
        }

        [HttpPost]
        public async Task<IActionResult> AddComment([FromBody] CommentCreateModel model)
        {

            Comment comment = _mapper.Map<Comment>(model);
            comment.DateTime = DateTime.Now;

            try
            {
               

                await _commentService.AddAsync(comment);
                return Ok();
            }
            catch (Exception ex)
            {
               
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }




    }
}
