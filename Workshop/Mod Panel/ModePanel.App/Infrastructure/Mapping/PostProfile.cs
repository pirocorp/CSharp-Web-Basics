namespace ModePanel.App.Infrastructure.Mapping
{
    using AutoMapper;
    using Data.Models;
    using Models.Home;

    public class PostProfile : Profile
    {
        public PostProfile()
        {
            this.CreateMap<Post, HomeListingModel>()
                .ForMember(
                    hl => hl.CreatedBy, 
                    cfg => cfg.MapFrom(p => p.User.Email));
        }
    }
}
