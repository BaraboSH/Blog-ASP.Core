using AutoMapper;
using Blog.Model.Entities;
using Blog.API.ViewModels.Stories;
namespace Blog.API.ViewModels.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Story,StoryDetailViewModel>()
                .ForMember(dest => dest.OwnerUsername,map => map.MapFrom(s => s.Owner.UserName))
                .ForMember(dest => dest.LikesNumber,map => map.MapFrom(s => s.Likes.Count))
                .ForMember(dest => dest.Liked,map => map.Ignore());

            CreateMap<Story, DraftViewModel>();
            CreateMap<Story, StoryViewModel>()
                .ForMember(dest => dest.OwnerUsername,map => map.MapFrom(s => s.Owner.UserName));   
            CreateMap<Story, OwnerStoryViewModel>();
        }
    }
}