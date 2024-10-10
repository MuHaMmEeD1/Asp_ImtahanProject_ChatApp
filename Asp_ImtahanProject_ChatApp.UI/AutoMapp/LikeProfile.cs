using Asp_ImtahanProject_ChatApp.Entities.Concrete;
using Asp_ImtahanProject_ChatApp.UI.Models.LikeModels;
using AutoMapper;

namespace Asp_ImtahanProject_ChatApp.UI.AutoMapp
{
    public class LikeProfile : Profile
    {
        public LikeProfile()
        {

            CreateMap<LikeCreateModel, Like>();
            CreateMap<Like, LikeModel>().ReverseMap();


        }
    }
}
