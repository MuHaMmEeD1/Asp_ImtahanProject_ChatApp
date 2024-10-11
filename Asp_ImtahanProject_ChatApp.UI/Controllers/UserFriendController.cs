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

        public UserFriendController(IUserFriendService userFriendService, IMapper mapper)
        {
            _userFriendService = userFriendService;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<IActionResult> Get(UserFriendSearchModel? model)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            List<UserFriend> userFriends;

            if (model != null)
            {
                userFriends = await _userFriendService.GetUserFriendsOrUFFListAsync(userId, model.UserName);
            }
            else
            {
                userFriends = await _userFriendService.GetUserFriendsOrUFFListAsync(userId);
            }

            if (userFriends.Count >= 8)
            {
                List<UserFrienModel> userFrienModel_ = _mapper.Map<List<UserFrienModel>>(userFriends);
                return Ok(userFrienModel_);
            }

            int realUserNumber = 8 - userFriends.Count;
            List<UserFriend> extentionUserFriends = new List<UserFriend>();

            foreach (var friend in userFriends)
            {
                if (extentionUserFriends.Count >= realUserNumber)
                    break;

                List<UserFriend> friendsOfFriend = new List<UserFriend>();

                if (friend.UserFriendFirstId != userId)
                {
                    friendsOfFriend = (await _userFriendService.GetUserFriendsOrUFFListAsync(friend.UserFriendFirst.Id))
                        .Where(uff => uff.UserFriendFirstId != userId && uff.UserFriendSecondId != userId)
                        .ToList();

                    foreach (var uff in friendsOfFriend)
                    {
                        if (uff.UserFriendFirstId == friend.UserFriendFirstId)
                        {
                            uff.UserFriendFirstId = uff.UserFriendSecondId;
                        }
                        else if (uff.UserFriendSecondId == friend.UserFriendFirstId)
                        {
                            uff.UserFriendSecondId = uff.UserFriendFirstId;
                        }
                    }
                }
                else if (friend.UserFriendSecondId != userId)
                {
                    friendsOfFriend = (await _userFriendService.GetUserFriendsOrUFFListAsync(friend.UserFriendSecond.Id))
                        .Where(uff => uff.UserFriendFirstId != userId && uff.UserFriendSecondId != userId)
                        .ToList();

                    foreach (var uff in friendsOfFriend)
                    {
                        if (uff.UserFriendFirstId == friend.UserFriendSecondId)
                        {
                            uff.UserFriendFirstId = uff.UserFriendSecondId;
                        }
                        else if (uff.UserFriendSecondId == friend.UserFriendSecondId)
                        {
                            uff.UserFriendSecondId = uff.UserFriendFirstId;
                        }
                    }
                }

                foreach (var fof in friendsOfFriend)
                {
                    if (extentionUserFriends.Count < realUserNumber)
                    {
                        extentionUserFriends.Add(fof);
                    }
                    else
                    {
                        break;
                    }
                }
            }

            userFriends.AddRange(extentionUserFriends);

            if (userFriends.Count < 8)
            {
                return Ok(new
                {
                    Friends = userFriends,
                    Remaining = 8 - userFriends.Count
                });
            }

            userFriends = userFriends.Take(8).ToList();
            List<UserFrienModel> userFrienModel = _mapper.Map<List<UserFrienModel>>(userFriends);

            return Ok(userFrienModel);
        }



        [HttpPost]
        public async Task<IActionResult> Add(UserFriendCreateModel model)
        {

            UserFriend userFriend = _mapper.Map<UserFriend>(model);
            await _userFriendService.AddAsync(userFriend);

            return Ok(model);

        }

        [HttpPost]
        public async Task<IActionResult> Delete(UserFriendDeleteModel model)
        {

            await _userFriendService.DeleteAsync(model.Id);

            return Ok(model);
        }


    }
}
