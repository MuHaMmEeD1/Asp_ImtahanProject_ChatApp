using Asp_ImtahanProject_ChatApp.Business.Abstract;
using Asp_ImtahanProject_ChatApp.Entities.Concrete;
using Asp_ImtahanProject_ChatApp.UI.Models.UserFriendModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices.Marshalling;
using System.Security.Claims;

namespace Asp_ImtahanProject_ChatApp.UI.Controllers
{
    [Authorize]

    public class UserFriendController : Controller
    {
        private readonly IUserFriendService _userFriendService; 
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserFriendController(IUserFriendService userFriendService, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _userFriendService = userFriendService;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }


        [HttpGet]
        public async Task<IActionResult> Get(string? UserName)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            List<UserFriend> userFriends;

            if (UserName != null)
            {
                userFriends = await _userFriendService.GetUserFriendsOrUFFListAsync(userId, UserName);
            }
            else
            {
                userFriends = await _userFriendService.GetUserFriendsOrUFFListAsync(userId);
            }

            var mappUserFriends = _mapper.Map<List<UserFrienModel>>(userFriends, opts =>
            {
                opts.Items["HttpContextAccessor"] = _httpContextAccessor;
                opts.Items["UserFriendService"] = _userFriendService;
            });

            return Ok(mappUserFriends);
        }   
        
        
        [HttpGet]
        public async Task<IActionResult> FriendsMessages(string? UserName)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            List<UserFriend> userFriends;

            if (UserName != null && UserName != "")
            {
                userFriends = await _userFriendService.GetUserFriendsOrUFFListAsync(userId, UserName);
            }
            else
            {
                userFriends = await _userFriendService.GetUserFriendsOrUFFListAsync(userId);
            }


            var mappUserFriends = _mapper.Map<List<UserFriendMessageModel>>(userFriends, opts =>
            {
                opts.Items["HttpContextAccessor"] = _httpContextAccessor;
                opts.Items["UserFriendService"] = _userFriendService;
            });

            mappUserFriends = mappUserFriends.OrderByDescending(f => f.IsOnline).ToList();



            return Ok(mappUserFriends);
        }



        [HttpGet]
        public async Task<IActionResult> RecentFriendsMessages()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            List<UserFriend> userFriends;

           
            
             userFriends = await _userFriendService.GetUserFriendsOrUFFListAsync(userId);


            var mappUserFriends = _mapper.Map<List<UserFriendMessageModel>>(userFriends, opts =>
            {
                opts.Items["HttpContextAccessor"] = _httpContextAccessor;
                opts.Items["UserFriendService"] = _userFriendService;
            });

            mappUserFriends = mappUserFriends.Where(f => f.IsOnline).Take(5).ToList();

           

            return Ok(mappUserFriends);
        }




        [HttpPost]
        public async Task<IActionResult> Add([FromBody]UserFriendCreateModel model)
        {


            Console.WriteLine(model == null ? "NULL":"NOT NULL");

            UserFriend userFriend = _mapper.Map<UserFriend>(model);
            await _userFriendService.AddAsync(userFriend);

            return Ok(model);

        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] UserFriendDeleteModel model)
        {

            await _userFriendService.DeleteAsync(model.Id);

            return Ok(model);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteUsOuId([FromBody] UserFriendDeleteUsOuModel model)
        {

            await _userFriendService.DeleteUserIdAdnOutherIdAsync(model.UserId, model.OtherUserId);

            return Ok(model);
        }


    }
}
