using Asp_ImtahanProject_ChatApp.Business.Abstract;
using Asp_ImtahanProject_ChatApp.Entities.Concrete;
using Asp_ImtahanProject_ChatApp.UI.Models.FriendshipRequestModels;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Asp_ImtahanProject_ChatApp.UI.Controllers
{
    public class FriendshipRequestController : Controller
    {
        private readonly IFriendshipRequestService _friendshipRequestService;
        private readonly IMapper _mapper;

        public FriendshipRequestController(IFriendshipRequestService friendshipRequestService, IMapper mapper)
        {
            _friendshipRequestService = friendshipRequestService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string userId)
        {

            return Ok(await _friendshipRequestService.GetListAsync(userId));

        }




        [HttpPost]      
        public async Task<IActionResult> Add(FriendshipRequestCreateModel model)
        {
            FriendshipRequest friendshipRequest = _mapper.Map<FriendshipRequest>(model);

            await _friendshipRequestService.AddAsync(friendshipRequest);
            

            return Ok(model);

        }

        [HttpPost]
        public async Task<IActionResult> Delete(FriendshipRequestDeleteModel model)
        {
            await _friendshipRequestService.DeleteAsync(model.Id);

            return Ok(model);

        }

    }
}
 