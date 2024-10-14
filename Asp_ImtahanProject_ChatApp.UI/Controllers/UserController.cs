using Asp_ImtahanProject_ChatApp.Business.Abstract;
using Asp_ImtahanProject_ChatApp.Entities.Concrete;
using Asp_ImtahanProject_ChatApp.UI.Models;
using Asp_ImtahanProject_ChatApp.UI.Models.HomeModels;
using Asp_ImtahanProject_ChatApp.UI.Models.UserModels;
using Asp_ImtahanProject_ChatApp.UI.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Asp_ImtahanProject_ChatApp.UI.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserFriendService _userFriendService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IPostService _postService;
        private readonly IPhotoService _photoService;
        public UserController(IUserFriendService userFriendService, IUserService userService, IMapper mapper, IPostService postService, IPhotoService photoService)
        {
            _userFriendService = userFriendService;
            _userService = userService;
            _mapper = mapper;
            _postService = postService;
            _photoService = photoService;
        }

        [HttpGet]
        public async Task<IActionResult> ProfileUser()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);


            User user = await _userService.GetUserByIdAsync(userId);

            UserProfileModel userProfileModel = _mapper.Map<UserProfileModel>(user);
            userProfileModel.FriendCount = (await _userFriendService.GetMyFriendAsync(userId)).Count;

            userProfileModel.LikeCount = (await _postService.GetMyPostsAsync(userId))
            .Sum(post => post.Likes.Count);

            return Ok(userProfileModel);
        }

        [HttpGet]
        public async Task<IActionResult> SetlingUser()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);


            User user = await _userService.GetUserByIdAsync(userId);

            UserSetlingsModel userProfileModel = _mapper.Map<UserSetlingsModel>(user);

            return Ok(userProfileModel);


        }

        [HttpPost]
        public async Task<IActionResult> Update([FromBody]UserSetlingsModel model)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            User user = _mapper.Map<User>(model);
            user.Id = userId;

            await _userService.UpdateAsync(user);


            return Ok();
        }


        [HttpPost]
        public async Task<IActionResult> UpdateBackgroundProfileImage([FromForm] PostCreateModel model)
        {
            string? imageUrl = null;
            if (model.Photo != null && model.Photo.Length > 0)
            {
                var photoDto = new PhotoCreationModel { File = model.Photo };
                imageUrl = await _photoService.UploadImageAsync(photoDto);

                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                User user = await _userService.GetUserByIdAsync(userId);
                user.BackgroundImageUrl = imageUrl;

                await _userService.UpdateAsync(user);

                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }


        [HttpPost]
        public async Task<IActionResult> UpdateProfileImage([FromForm] PostCreateModel model)
        {
            string? imageUrl = null;
            if (model.Photo != null && model.Photo.Length > 0)
            {
                var photoDto = new PhotoCreationModel { File = model.Photo };
                imageUrl = await _photoService.UploadImageAsync(photoDto);

                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                User user = await _userService.GetUserByIdAsync(userId);
                user.ProfileImageUrl = imageUrl;

                await _userService.UpdateAsync(user);

                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }



    }
}
