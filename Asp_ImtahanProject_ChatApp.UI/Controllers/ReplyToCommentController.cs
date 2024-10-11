using Asp_ImtahanProject_ChatApp.Business.Abstract;
using Asp_ImtahanProject_ChatApp.Entities.Concrete;
using Asp_ImtahanProject_ChatApp.UI.Models.ReplyToCommentModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Asp_ImtahanProject_ChatApp.UI.Controllers
{
    [Authorize]

    public class ReplyToCommentController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IReplyToCommentService _replyToCommentService;
        private readonly ICommentService _commentService;



        public ReplyToCommentController(IMapper mapper, IReplyToCommentService replyToCommentService, ICommentService commentService)
        {
            _mapper = mapper;
            _replyToCommentService = replyToCommentService;
            _commentService = commentService;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] ReplyToCommentCreateModel model)
        {

            Console.WriteLine("\n\n\n");
            Console.WriteLine(model == null ? "null": model.Text);
            Console.WriteLine("\n\n\n");


            try
            {
                
                ReplyToComment replyToComment = _mapper.Map<ReplyToComment>(model);
                replyToComment.DateTime = DateTime.Now;
               


                await _replyToCommentService.AddAsync(replyToComment);
            }
            catch (Exception ex) {
                Console.WriteLine(ex);
            }


            return Ok();
        }
    }
}
