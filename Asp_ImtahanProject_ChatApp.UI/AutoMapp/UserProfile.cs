using Asp_ImtahanProject_ChatApp.Business.Abstract;
using Asp_ImtahanProject_ChatApp.Business.Concrete;
using Asp_ImtahanProject_ChatApp.Entities.Concrete;
using Asp_ImtahanProject_ChatApp.UI.Models.UserModels;
using AutoMapper;

namespace Asp_ImtahanProject_ChatApp.UI.AutoMapp
{
    public class UserProfile : Profile
    {


        public UserProfile()
        {


            CreateMap<User, UserProfileModel>();
               

            CreateMap<User, UserSetlingsModel>().ReverseMap();


        }
    }
}
