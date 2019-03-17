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
                .ForMember(dest => dest.OwnerUsername,map => map.MapFrom(s => s.Owner.UserName));
            CreateMap<Story, DraftViewModel>();
            CreateMap<Story, StoryViewModel>();    
        }
    }
}