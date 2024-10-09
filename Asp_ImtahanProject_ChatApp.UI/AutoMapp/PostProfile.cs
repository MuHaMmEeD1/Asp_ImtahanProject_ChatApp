using AutoMapper;
using Asp_ImtahanProject_ChatApp.Entities.Concrete;
using Asp_ImtahanProject_ChatApp.UI.Models.PostModels;
using Asp_ImtahanProject_ChatApp.UI.Models.LikeModels;
using Asp_ImtahanProject_ChatApp.UI.Models.CommentModels;
using Asp_ImtahanProject_ChatApp.UI.Models.ReplyToCommentModels;
using Asp_ImtahanProject_ChatApp.Business.Abstract;

namespace Asp_ImtahanProject_ChatApp.UI.AutoMapp
{
    public class PostProfile : Profile
    {
        public PostProfile()
        {
            
            CreateMap<Post, PostModel>()
                .ForMember(dest => dest.PostId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserProfileImageUrl, opt => opt.MapFrom(src => src.User.ProfileImageUrl))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.FirstName + " " + src.User.LastName))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.User.Id))
                .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments.Select(c => new CommentModel
                {
                    Id = c.Id,
                    Text = c.Text,
                    DateTime = c.DateTime.ToString("yyyy-MM-dd HH:mm"),
                    UserProfileImageUrl = c.User.ProfileImageUrl,
                    UserName = c.User.FirstName + " " + c.User.LastName,
                    UserId = c.UserId,
                    ReplyToComments = c.ReplyToComments.Select(r => new ReplyToCommentModel
                    {
                        Text = r.Text,
                        DateTime = r.DateTime.ToString("yyyy-MM-dd HH:mm"),
                        UserProfileImageUrl = r.User.ProfileImageUrl,
                        UserName = r.User.FirstName + " " + r.User.LastName,
                        UserId = r.UserId,
                        CommentId = r.CommentId
                    }).ToList()
                }).ToList()))
                .ForMember(dest => dest.Likes, opt => opt.MapFrom(src => src.Likes.Select(l => new LikeModel
                {
                    PostId = l.PostId,
                    UserId = l.UserId
                }).ToList()))
                .ForMember(dest => dest.LikeCount, opt => opt.MapFrom(src => src.Likes.Count))
                .ForMember(dest => dest.DateTime, opt => opt.MapFrom(src => src.DateTime.ToString("yyyy-MM-dd HH:mm")));
        }
    }

}
