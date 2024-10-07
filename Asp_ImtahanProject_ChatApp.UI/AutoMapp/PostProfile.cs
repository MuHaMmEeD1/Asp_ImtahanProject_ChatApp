﻿using AutoMapper;
using Asp_ImtahanProject_ChatApp.Entities.Concrete;
using Asp_ImtahanProject_ChatApp.UI.Models.PostModels;
using Asp_ImtahanProject_ChatApp.UI.Models.LikeModels;
using Asp_ImtahanProject_ChatApp.UI.Models.CommentModels;

namespace Asp_ImtahanProject_ChatApp.UI.AutoMapp
{
    public class PostProfile : Profile
    {
        public PostProfile()
        {
            CreateMap<Post, PostModel>()
                .ForMember(dest => dest.PostId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserProfileImageUrl, opt => opt.MapFrom(src => src.User.ProfileImageUrl))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.FirstName+" "+src.User.LastName))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.User.Id))
                .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments.Select(c => new CommentModel
                {
                    Text = c.Text,
                    DateTime = c.DateTime.ToString("yyyy-MM-dd HH:mm"),
                    UserProfileImageUrl = c.User.ProfileImageUrl,
                    UserName = c.User.UserName
                }).ToList()))
                .ForMember(dest => dest.Likes, opt => opt.MapFrom(src => src.Likes.Select(l => new LikeModel
                {
                    PostId = l.PostId,
                    UserId = l.UserId
                }).ToList()))
                .ForMember(dest => dest.LikeCount, opt => opt.MapFrom(src => src.Likes.Count))
                .ForMember(dest => dest.DateTime, opt => opt.MapFrom(src => src.DateTime.ToString("yyyy-MM-dd HH:mm")));

            CreateMap<PostModel, Post>()
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.Comments, opt => opt.Ignore())
                .ForMember(dest => dest.Likes, opt => opt.Ignore())
                .ForMember(dest => dest.PostTags, opt => opt.Ignore());

            CreateMap<Comment, CommentModel>()
                .ForMember(dest => dest.UserProfileImageUrl, opt => opt.MapFrom(src => src.User.ProfileImageUrl))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName));

            CreateMap<Like, LikeModel>()
                .ForMember(dest => dest.PostId, opt => opt.MapFrom(src => src.PostId))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId));
        }
    }
}