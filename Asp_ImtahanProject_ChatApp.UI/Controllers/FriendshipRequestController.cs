using Asp_ImtahanProject_ChatApp.Business.Abstract;
using Asp_ImtahanProject_ChatApp.Entities.Concrete;
using Asp_ImtahanProject_ChatApp.UI.Models.FriendshipRequestModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Asp_ImtahanProject_ChatApp.UI.Controllers
{
    [Authorize]
    public class FriendshipRequestController : Controller
    {
        private readonly IFriendshipRequestService _friendshipRequestService;
        private readonly IMapper _mapper;
        private readonly IUserFriendService _userFriendService;

        public FriendshipRequestController(IFriendshipRequestService friendshipRequestService, IMapper mapper, IUserFriendService userFriendService)
        {
            _friendshipRequestService = friendshipRequestService;
            _mapper = mapper;
            _userFriendService = userFriendService;
        }

        [HttpGet]
        public async Task<IActionResult> GetList()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            List<FriendshipRequestModel> friendshipRequestModel = _mapper.Map<List<FriendshipRequestModel>>(await _friendshipRequestService.GetListAsync(userId));

            return Ok(friendshipRequestModel);

        }

        [HttpGet]
        public async Task<IActionResult> GetCheckMyFriend(FriendshipRequestGetByIdModel model)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            List<UserFriend> userFriendList = await _userFriendService.GetUserFriendsOrUFFListAsync(userId);

            UserFriend userFriend = userFriendList.Find(uf=>uf.UserFriendFirstId == model.OutherUserId || uf.UserFriendSecondId == model.OutherUserId);


            ///
            List<FriendshipRequest> myList = await _friendshipRequestService.GetListAsync(userId);
            List<FriendshipRequest> outherList = await _friendshipRequestService.GetListAsync(model.OutherUserId);



            bool FRCheck = false;
          



         
            foreach (var item in myList)
            {

                if (item.UserId == model.OutherUserId)
                {
                    FRCheck = true;
                    break;
                }
            }


            if (!FRCheck)
            {

                foreach (var item in outherList)
                {
                    if (item.UserId == userId)
                    {
                        FRCheck = true;
                        break;
                    }
                }
            }
      


            if (userFriend != null)
            {
                return Ok(2);
            }
            else if (FRCheck)
            {
                return Ok(1);

            }

            return Ok(0);


        }


        [HttpPost]      
        public async Task<IActionResult> Add([FromBody]FriendshipRequestCreateModel model)
        {
            FriendshipRequest friendshipRequest = _mapper.Map<FriendshipRequest>(model);
            friendshipRequest.DateTime = DateTime.Now;

            await _friendshipRequestService.AddAsync(friendshipRequest);
            

            return Ok(model);

        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromBody]FriendshipRequestDeleteModel model)
        {
            await _friendshipRequestService.DeleteAsync(model.Id);

            return Ok(model);

        }

    }
}
 