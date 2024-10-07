using Asp_ImtahanProject_ChatApp.Entities.Concrete;
using Asp_ImtahanProject_ChatApp.UI.Models.ReplyToCommentModels;
using AutoMapper;

namespace Asp_ImtahanProject_ChatApp.UI.AutoMapp
{
    public class ReplyToCommentProfile : Profile
    {
        public ReplyToCommentProfile()
        {

            CreateMap<ReplyToComment, ReplyToCommentModel>()
                .ForMember(rc => rc.DateTime, rcm => rcm.MapFrom(mp => mp.DateTime.ToString("yyyy-MM-dd HH:mm")))
                .ForMember(rc => rc.UserProfileImageUrl, rcm => rcm.MapFrom(mp => mp.User.ProfileImageUrl))
                .ForMember(rc => rc.UserName, rcm => rcm.MapFrom(mp => mp.User.FirstName + " " + mp.User.LastName));

            CreateMap<ReplyToCommentCreateModel, ReplyToComment>().ReverseMap();

        }
    }
}
