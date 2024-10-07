using Asp_ImtahanProject_ChatApp.Business.Abstract;
using Asp_ImtahanProject_ChatApp.Entities.Concrete;
using Asp_ImtahanProject_ChatApp.UI.Models.ReplyToCommentModels;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Asp_ImtahanProject_ChatApp.UI.Controllers
{
    public class ReplyToCommentController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IReplyToCommentService _replyToCommentService;

        public ReplyToCommentController(IMapper mapper, IReplyToCommentService replyToCommentService)
        {
            _mapper = mapper;
            _replyToCommentService = replyToCommentService;
        }

        public async Task<IActionResult> Add([FromBody] ReplyToCommentCreateModel model)
        {
            ReplyToComment replyToComment = _mapper.Map<ReplyToComment>(model);
            replyToComment.DateTime = DateTime.Now;


            await _replyToCommentService.AddAsync(replyToComment);


            return Ok();
        }
    }
}
