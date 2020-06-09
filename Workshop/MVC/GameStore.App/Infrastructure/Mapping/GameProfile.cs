namespace GameStore.App.Infrastructure.Mapping
{
    using AutoMapper;
    using Data.Models;
    using Models.Games;

    public class GameProfile : Profile
    {
        public GameProfile()
        {
            this.CreateMap<Game, GameListingAdminModel>()
                .ForMember(glam => glam.Name, cfg => cfg.MapFrom(g => g.Title));
        }
    }
}
