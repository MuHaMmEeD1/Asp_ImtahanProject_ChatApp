using Asp_ImtahanProject_ChatApp.Entities.Concrete;
using Asp_ImtahanProject_ChatApp.UI.Models.MessageModels;
using AutoMapper;

namespace Asp_ImtahanProject_ChatApp.UI.AutoMapp
{
    public class MessageProfile : Profile
    {
        public MessageProfile()
        {

            CreateMap<MessageCreateModel, Message>();

            CreateMap<Message, MessageModel>()
                .ForMember(mm => mm.DateTime, m => m.MapFrom(mp => mp.DateTime.ToString("yyyy-MM-dd HH:mm")));


            CreateMap<Message, MessageHeaderModel>()
                .ForMember(mm => mm.userProfileUrl, m => m.MapFrom(mp => mp.User.ProfileImageUrl));
                



        }
    }
}
