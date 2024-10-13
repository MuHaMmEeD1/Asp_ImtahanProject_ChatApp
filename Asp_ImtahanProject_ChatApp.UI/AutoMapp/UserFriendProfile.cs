using Asp_ImtahanProject_ChatApp.Business.Abstract;
using Asp_ImtahanProject_ChatApp.Entities.Concrete;
using Asp_ImtahanProject_ChatApp.UI.Models.UserFriendModels;
using AutoMapper;
using System.Security.Claims;

namespace Asp_ImtahanProject_ChatApp.UI.AutoMapp
{
    public class UserFriendProfile : Profile
    {
        public UserFriendProfile()
        {
            CreateMap<UserFriendCreateModel, UserFriend>();


            CreateMap<UserFriend, UserFrienModel>()
                .ForMember(ufm => ufm.OutherUserId, uf => uf.MapFrom((src, dest, destMember, context) => GetOutherUserId(src.UserFriendFirstId, src.UserFriendSecondId, context)))
                .ForMember(ufm => ufm.UserName, uf => uf.MapFrom((src, dest, destMember, context) => GetOutherUserName(src.UserFriendFirst, src.UserFriendSecond, context)))
                .ForMember(ufm => ufm.ProfileBackgroundImageUrl, uf => uf.MapFrom((src, dest, destMember, context) => GetOutherProfileBackground(src.UserFriendFirst, src.UserFriendSecond, context)))
                .ForMember(ufm => ufm.ProfileImageUrl, uf => uf.MapFrom((src, dest, destMember, context) => GetOutherProfileImage(src.UserFriendFirst, src.UserFriendSecond, context)))
                .ForMember(ufm => ufm.LikeCount, uf => uf.MapFrom((src, dest, destMember, context) => GetOutherLikeCount(src.UserFriendFirst, src.UserFriendSecond, context)))
                .ForMember(ufm => ufm.FriendCount, uf => uf.MapFrom((src, dest, destMember, context) => GetOutherFriendCountSync(src.UserFriendFirst, src.UserFriendSecond, context)))
                .ForMember(ufm => ufm.ThisMyFriend, uf => uf.MapFrom((src, dest, destMember, context) => ThisMyFriendCheck(src.UserFriendFirst, src.UserFriendSecond, context)))
                .ForMember(ufm => ufm.IsOnline, uf => uf.MapFrom((src, dest, destMember, context) => GetOutherIsOnline(src.UserFriendFirst, src.UserFriendSecond, context)));



            CreateMap<UserFriend, UserFriendMessageModel>()
                .ForMember(ufm => ufm.OutherUserId, uf => uf.MapFrom((src, dest, destMember, context) => GetOutherUserId(src.UserFriendFirstId, src.UserFriendSecondId, context)))
                .ForMember(ufm => ufm.UserName, uf => uf.MapFrom((src, dest, destMember, context) => GetOutherUserName(src.UserFriendFirst, src.UserFriendSecond, context)))
                .ForMember(ufm => ufm.ProfileImageUrl, uf => uf.MapFrom((src, dest, destMember, context) => GetOutherProfileImage(src.UserFriendFirst, src.UserFriendSecond, context)))
                .ForMember(ufm => ufm.IsOnline, uf => uf.MapFrom((src, dest, destMember, context) => GetOutherIsOnline(src.UserFriendFirst, src.UserFriendSecond, context)));


        }

        private string GetOutherUserId(string firstId, string secondId, ResolutionContext context)
        {
            var httpContextAccessor = (IHttpContextAccessor)context.Items["HttpContextAccessor"];
            var currentUserId = GetCurrentUserId(httpContextAccessor);
            return firstId != currentUserId ? firstId : secondId;
        }

        private string GetOutherUserName(User userFirst, User userSecond, ResolutionContext context)
        {
            var httpContextAccessor = (IHttpContextAccessor)context.Items["HttpContextAccessor"];
            var currentUserId = GetCurrentUserId(httpContextAccessor);
            return userFirst.Id != currentUserId ? $"{userFirst.FirstName} {userFirst.LastName}" : $"{userSecond.FirstName} {userSecond.LastName}";
        } 
        
        private bool GetOutherIsOnline(User userFirst, User userSecond, ResolutionContext context)
        {
            var httpContextAccessor = (IHttpContextAccessor)context.Items["HttpContextAccessor"];
            var currentUserId = GetCurrentUserId(httpContextAccessor);
            return userFirst.Id != currentUserId ? userFirst.IsOnline : userSecond.IsOnline;
        }

        private string GetOutherProfileBackground(User userFirst, User userSecond, ResolutionContext context)
        {
            var httpContextAccessor = (IHttpContextAccessor)context.Items["HttpContextAccessor"];
            var currentUserId = GetCurrentUserId(httpContextAccessor);
            return userFirst.Id != currentUserId ? userFirst.BackgroundImageUrl : userSecond.BackgroundImageUrl;
        }

        private string GetOutherProfileImage(User userFirst, User userSecond, ResolutionContext context)
        {
            var httpContextAccessor = (IHttpContextAccessor)context.Items["HttpContextAccessor"];
            var currentUserId = GetCurrentUserId(httpContextAccessor);
            return userFirst.Id != currentUserId ? userFirst.ProfileImageUrl : userSecond.ProfileImageUrl;
        }

        private int GetOutherLikeCount(User userFirst, User userSecond, ResolutionContext context)
        {
            var httpContextAccessor = (IHttpContextAccessor)context.Items["HttpContextAccessor"];
            var currentUserId = GetCurrentUserId(httpContextAccessor);
            var posts = userFirst.Id != currentUserId ? userFirst.Posts : userSecond.Posts;
            return posts.Sum(post => post.Likes.Count);
        }

        private int GetOutherFriendCountSync(User userFirst, User userSecond, ResolutionContext context)
        {
            // Burada async çağrı yapılmadığı için, yukarıdaki senaryoyu senkron hale getirdik
            // Async yapılacaksa, metod async hale getirilmeli ve çağrılmalıdır.
            var httpContextAccessor = (IHttpContextAccessor)context.Items["HttpContextAccessor"];
            var userFriendService = (IUserFriendService)context.Items["UserFriendService"];

            var currentUserId = GetCurrentUserId(httpContextAccessor);
            if (userFirst.Id == currentUserId)
            {
                return userFriendService.GetUserFriendsOrUFFListAsync(userFirst.Id).Result.Count;
            }
            return userFriendService.GetUserFriendsOrUFFListAsync(userSecond.Id).Result.Count;
        }

        private bool ThisMyFriendCheck(User userFirst, User userSecond, ResolutionContext context)
        {
            var httpContextAccessor = (IHttpContextAccessor)context.Items["HttpContextAccessor"];
            var currentUserId = GetCurrentUserId(httpContextAccessor);
            return userFirst.Id == currentUserId || userSecond.Id == currentUserId;
        }

        private string GetCurrentUserId(IHttpContextAccessor httpContextAccessor)
        {
            var userId = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                throw new InvalidOperationException("User is not authenticated.");
            }
            return userId;
        }
    }
}
