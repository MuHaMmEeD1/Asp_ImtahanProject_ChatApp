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
            user.IsOnline = true;
            await _userService.UpdateAsync(user);


            string userFullName = $"{user.FirstName} {user.LastName}";

            await Clients.Caller.SendAsync("OnConnectedMethod", user.UserName, user.ProfileImageUrl, userFullName, user.Email, user.Id);
            Console.WriteLine("connected Hub");
            await Clients.Caller.SendAsync("HeaderReflash", userIdClaim);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnDisconnectedAsync(exception);
            string? userIdClaim = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            User user = await _userService.GetUserByIdAsync(userIdClaim!);
            user.IsOnline = false;
            await _userService.UpdateAsync(user);
            await Clients.All.SendAsync("ContactReflash");
        }

        public async Task PostUlReflash(string tagName)
        {
            await Clients.All.SendAsync("PostUlReflashStart",tagName);
        }

        public async Task PostUlReflash_ID(string userId)
        {
            await Clients.All.SendAsync("PostUlReflash_ID_Start", userId);
        }

        public async Task HeaderReflash(string userId)
        {
            await Clients.All.SendAsync("HeaderReflash", userId);
        }

        public async Task FriendsReflash(string userId)
        {
            await Clients.All.SendAsync("FriendsReflash", userId);
        }

        public async Task MessageReflash(string userId, string otherUserId,string otherProfileUrl,string otherUserName)
        {
            await Clients.All.SendAsync("MessageReflash", userId, otherUserId, otherProfileUrl, otherUserName);
        } 
             
        public async Task ProfileReflash(string userId)
        {
            await Clients.All.SendAsync("ProfileReflash", userId);

        } 
        
        public async Task AllPostsRaflash()
        {
            await Clients.All.SendAsync("AllPostsRaflash");
        }
        public async Task ContactReflash()
        {
            await Clients.All.SendAsync("ContactReflash");

        }
    }
}
