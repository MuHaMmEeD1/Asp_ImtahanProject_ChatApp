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
        public async Task<IActionResult> GetListDidItAppear()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            List<FriendshipRequestModel> friendshipRequestModel = _mapper.Map<List<FriendshipRequestModel>>(await _friendshipRequestService.GetListDidItAppearAsync(userId));

            return Ok(friendshipRequestModel);

        }
        
        [HttpGet]
        public async Task<IActionResult> GetListUnanswered()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            List<FriendshipRequestModel> friendshipRequestModel = _mapper.Map<List<FriendshipRequestModel>>(await _friendshipRequestService.GetListuUnansweredAsync(userId));

            return Ok(friendshipRequestModel);

        }

        [HttpGet]
        public async Task<IActionResult> GetCheckMyFriend(FriendshipRequestGetByIdModel model)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            List<UserFriend> userFriendList = await _userFriendService.GetUserFriendsOrUFFListAsync(userId);

            UserFriend userFriend = userFriendList.Find(uf=>uf.UserFriendFirstId == model.OutherUserId || uf.UserFriendSecondId == model.OutherUserId);
            

            
            List<FriendshipRequest> outherList = await _friendshipRequestService.GetListAsync(model.OutherUserId);



            bool FRCheck = false;
          



         
            


           

                foreach (var item in outherList)
                {
                    if (item.UserId == userId)
                    {
                        FRCheck = true;
                        break;
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
            friendshipRequest.DidItAppear = false;

            await _friendshipRequestService.AddAsync(friendshipRequest);
            

            return Ok(model);

        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromBody]FriendshipRequestDeleteModel model)
        {
            await _friendshipRequestService.DeleteAsync(model.Id);

            return Ok(model);

        }

        [HttpPost]
        public async Task<IActionResult> DeleteUsOuId([FromBody] FriendshipRequestDeleteUsOuIdModel model)

        {
            await _friendshipRequestService.DeleteUserIdAndOutherIdAsync(model.UserId, model.OtherUserId);

            return Ok(model);

        }       
        [HttpPost]
        public async Task<IActionResult> Update([FromBody] FriendshipRequestUpdateModel model)
        {

            FriendshipRequest friendshipRequest = await _friendshipRequestService.GetByIdAsync(model.Id);

            friendshipRequest.DidItAppear = model.DidItAppear;

            await _friendshipRequestService.UpdateAsync(friendshipRequest);
            return Ok(model);

        }


    }
}
 