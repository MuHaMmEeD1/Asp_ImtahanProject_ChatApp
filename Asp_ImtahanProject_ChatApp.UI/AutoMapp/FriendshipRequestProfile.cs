using Asp_ImtahanProject_ChatApp.Entities.Concrete;
using Asp_ImtahanProject_ChatApp.UI.Models.FriendshipRequestModels;
using AutoMapper;

namespace Asp_ImtahanProject_ChatApp.UI.AutoMapp
{
    public class FriendshipRequestProfile : Profile
    {
        public FriendshipRequestProfile()
        {

            CreateMap<FriendshipRequest, FriendshipRequestModel>()
                .ForMember(frm => frm.OutherUserProfileImageUrl, fr => fr.MapFrom(mp => mp.OtherUser.ProfileImageUrl))
                .ForMember(frm => frm.OutherUserName, fr => fr.MapFrom(mp => mp.OtherUser.FirstName + " " + mp.OtherUser.LastName))
                .ForMember(frm => frm.DateTime, fr => fr.MapFrom(DateTime.Now.ToString("yyyy-MM-dd HH:mm")));

            CreateMap<FriendshipRequestCreateModel, FriendshipRequest>();
            

        }
    }
}
