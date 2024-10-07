using Asp_ImtahanProject_ChatApp.Business.Abstract;
using Asp_ImtahanProject_ChatApp.Business.Concrete;
using Asp_ImtahanProject_ChatApp.Entities.Concrete;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace Asp_ImtahanProject_ChatApp.UI.Hubs
{
    public class FriendHub : Hub
    {

        private readonly IUserService _userService;

        public FriendHub(IUserService userService)
        {
            _userService = userService;
        }

        public override async Task OnConnectedAsync()
        {

            string? userIdClaim = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            User user = await _userService.GetUserByIdAsync(userIdClaim!);

            string userFullName = $"{user.FirstName} {user.LastName}";

            await Clients.Caller.SendAsync("OnConnectedMethod", user.UserName, user.ProfileImageUrl, userFullName, user.Email, user.Id);
            Console.WriteLine("connected Hub"); 
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnDisconnectedAsync(exception);
        }

        public async Task PostUlReflash(string tagName)
        {
            await Clients.All.SendAsync("PostUlReflashStart",tagName);
        }


    }
}
