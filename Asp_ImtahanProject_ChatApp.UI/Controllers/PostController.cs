using Asp_ImtahanProject_ChatApp.Business.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Asp_ImtahanProject_ChatApp.UI.Models.PostModels;
using Asp_ImtahanProject_ChatApp.Entities.Concrete;

namespace Asp_ImtahanProject_ChatApp.UI.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private readonly IPostService _postService;
        private readonly IMapper _mapper;

        public PostController(IPostService postService, IMapper mapper)
        {
            _postService = postService;
            _mapper = mapper;
        }

        [HttpGet("Post/GetSearchPost/{tagName}")]
        public async Task<IActionResult> GetSearchPost(string tagName)
        {
            try
            {
                var posts = await _postService.GetIncludeListAsync(p =>
                    p.PostTags.Any(pt => pt.Tag.Name == tagName));

                var postModels = _mapper.Map<List<PostModel>>(posts);

                return Ok(postModels);
            }
            catch (Exception ex)
            {
              
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("/Post/FriendsPosts/{userId}")]
        public async Task<IActionResult> FriendsPosts(string userId)
        {
            try
            {
                List<Post> posts = await _postService.GetFrendsPostsAsync(userId);

                var postModels = _mapper.Map<List<PostModel>>(posts);

                return Ok(postModels);
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


    }
}
