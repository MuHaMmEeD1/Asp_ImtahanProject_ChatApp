using Asp_ImtahanProject_ChatApp.Business.Abstract;
using Asp_ImtahanProject_ChatApp.Entities.Concrete;
using Asp_ImtahanProject_ChatApp.UI.Models;
using Asp_ImtahanProject_ChatApp.UI.Models.HomeModels;
using Asp_ImtahanProject_ChatApp.UI.Models.UserModels;
using Asp_ImtahanProject_ChatApp.UI.Services;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
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

        [HttpPost]
        public async Task<IActionResult> UpdateSetting([FromBody] UserSetlingsModel model)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            User user = await _userService.GetUserByIdAsync(userId);

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.BackupEmail = model.BackupEmail;
            DateTime dateTime;
            DateTime.TryParse(model.DateOfBirth, out dateTime);
            user.DateOfBirth = dateTime;
            user.PhoneNo = model.PhoneNo;
            user.Occupation = model.Occupation;
            user.Gender = model.Gender;
            user.RelationStatus = model.RelationStatus;
            user.BloodGroup = model.BloodGroup;
            user.Website = model.Website;
            user.Language = model.Language;
            user.Address = model.Address;
            user.City = model.City;
            user.State = model.State;
            user.Country = model.Country;

            await _userService.UpdateAsync(user);


            return Ok(model);
        }


        [HttpPost]
        public async Task<IActionResult> UpdatePassword([FromBody] UserSettingPasswordModel model)
        {
            if (model == null ||
                string.IsNullOrEmpty(model.CurrentPasswordFirst) ||
                string.IsNullOrEmpty(model.CurrentPasswordSecond) ||
                string.IsNullOrEmpty(model.NewPassword))
            {
                return BadRequest("Invalid input data.");
            }

            if (model.CurrentPasswordFirst != model.CurrentPasswordSecond)
            {
                return BadRequest("Current passwords do not match.");
            }

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            User user = await _userService.GetUserByIdAsync(userId);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            var isPasswordValid = await _userService.VerifyPasswordAsync(user, model.CurrentPasswordFirst);
            if (!isPasswordValid)
            {
                return BadRequest("Current password is incorrect.");
            }

            var result = await _userService.ChangePasswordAsync(user, model.NewPassword);

            if (!result)
            {
                return BadRequest("Failed to update password.");
            }

            return Ok("Password updated successfully.");
        }




        [HttpPost]
        public async Task<IActionResult> UpdateMain([FromBody] UserSettingMainModel model)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            User user = await _userService.GetUserByIdAsync(userId);

            user.UserName = model.UserName;
            user.Email = model.Email;
            user.PhoneNo = model.PhoneNo;            
            user.Country = model.Country;

            await _userService.UpdateAsync(user);


            return Ok(model);
        }







        [HttpGet]
        public async Task<IActionResult> SettingAllData(UserSetlingsModel model)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            User user = await _userService.GetUserByIdAsync(userId);

            UserSetlingsModel userSetlingsModel = _mapper.Map<UserSetlingsModel>(user);

            


            return Ok(userSetlingsModel);
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
