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
                .ForMember(frm => frm.OutherUserProfileImageUrl, fr => fr.MapFrom(mp => mp.User.ProfileImageUrl))
                .ForMember(frm => frm.OutherUserName, fr => fr.MapFrom(mp => mp.User.FirstName + " " + mp.User.LastName))
                .ForMember(frm => frm.DateTime, fr => fr.MapFrom(mp => mp.DateTime.ToString("yyyy-MM-dd HH:mm")))
                .ForMember(frm => frm.OtherUserId, fr => fr.MapFrom(mp => mp.UserId));

            CreateMap<FriendshipRequestCreateModel, FriendshipRequest>();

        } 
    }

}
