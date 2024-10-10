using Asp_ImtahanProject_ChatApp.Entities.Concrete;
using Asp_ImtahanProject_ChatApp.UI.Models.UserFriendModels;
using AutoMapper;
using System.Security.Claims;

namespace Asp_ImtahanProject_ChatApp.UI.AutoMapp
{
    public class UserFriendProfile : Profile
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserFriendProfile(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;


            CreateMap<UserFriend, UserFrienModel>()
                .ForMember(ufm => ufm.OutherUserId, uf => uf.MapFrom(mp => GetOuthetUserId(mp.UserFriendFirstId, mp.UserFriendSecondId)))
                .ForMember(ufm => ufm.UserName, uf => uf.MapFrom(mp => GetOuthetUserName(mp.UserFriendFirst, mp.UserFriendSecond)))
                .ForMember(ufm => ufm.ProfileBackgroundImageUrl, uf => uf.MapFrom(mp => GetOuthetProfileBackground(mp.UserFriendFirst, mp.UserFriendSecond)))
                .ForMember(ufm => ufm.ProfileImageUrl, uf => uf.MapFrom(mp => GetOuthetProfileImage(mp.UserFriendFirst, mp.UserFriendSecond)))
                .ForMember(ufm => ufm.LikeCount, uf => uf.MapFrom(mp => GetOuthetLikeConut(mp.UserFriendFirst, mp.UserFriendSecond)))
                .ForMember(ufm => ufm.FriendCount, uf => uf.MapFrom(mp => GetOuthetFriendCount(mp.UserFriendFirst, mp.UserFriendSecond)))
                .ForMember(ufm => ufm.ThisMyFriend, uf => uf.MapFrom(mp => ThisMyFriendCheck(mp.UserFriendFirst, mp.UserFriendSecond)));


            CreateMap<UserFriendCreateModel, UserFriend>();

        }


        private string GetOuthetUserId(string firstId, string secoundId)
        {
            if (firstId == GetCurrentUserId())
            {
                return firstId;
            }
            return secoundId;

        }
        private string GetOuthetUserName(User userFirst, User userSecound)
        {
            if (userFirst.Id == GetCurrentUserId())
            {
                return userFirst.FirstName + " " + userFirst.LastName;
            }
            return userSecound.FirstName + " " + userSecound.LastName;

        }        
        private string GetOuthetProfileBackground(User userFirst, User userSecound)
        {
            if (userFirst.Id == GetCurrentUserId())
            {
                return userFirst.BackgroundImageUrl;
            }
            return userSecound.BackgroundImageUrl;

        }        
        private string GetOuthetProfileImage(User userFirst, User userSecound)
        {
            if (userFirst.Id == GetCurrentUserId())
            {
                return userFirst.ProfileImageUrl;
            }
            return userSecound.ProfileImageUrl;

        }        
        private int GetOuthetLikeConut(User userFirst, User userSecound)
        {
            
            int likeCount = 0;

            if (userFirst.Id == GetCurrentUserId())
            {
                foreach(Post post in userFirst.Posts)
                {
                    likeCount += post.Likes.Count;
                }

                return likeCount;
            }

            foreach (Post post in userSecound.Posts)
            {
                likeCount += post.Likes.Count;
            }

            return likeCount;

        }
        private int GetOuthetFriendCount(User userFirst, User userSecound)
        {
            
            if (userFirst.Id == GetCurrentUserId())
            {
               
                return userFirst.UserFriends.Count;
            }

            return userSecound.UserFriends.Count;

        }
        private bool ThisMyFriendCheck(User userFirst, User userSecound)
        {
            if (userFirst.Id == GetCurrentUserId() || userSecound.Id == GetCurrentUserId())
            {
                return true;
            }
            return false;
        }



        private string GetCurrentUserId()
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }

}
