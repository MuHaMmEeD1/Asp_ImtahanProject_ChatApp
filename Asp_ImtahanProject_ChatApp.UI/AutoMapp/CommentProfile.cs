using Asp_ImtahanProject_ChatApp.Business.Abstract;
using Asp_ImtahanProject_ChatApp.Entities.Concrete;
using Asp_ImtahanProject_ChatApp.UI.Models.CommentModels;
using AutoMapper;

namespace Asp_ImtahanProject_ChatApp.UI.AutoMapp
{
    public class CommentProfile : Profile
    {
       
        public CommentProfile()
        {
           



            CreateMap<Comment, CommentCreateModel>().ReverseMap();

            CreateMap<Comment, CommentModel>()
                .ForMember(c => c.UserName, cm => cm.MapFrom(mp => mp.User.FirstName + " " + mp.User.LastName))
                .ForMember(c => c.UserProfileImageUrl, cm => cm.MapFrom(mp => mp.User.ProfileImageUrl))
                .ForMember(c => c.DateTime, cm => cm.MapFrom(mp => mp.DateTime.ToString("yyyy-MM-dd HH:mm")))
                .ForMember(c => c.ReplyToComments, cm => cm.MapFrom(mp => mp.ReplyToComments));


        }



    }
}
