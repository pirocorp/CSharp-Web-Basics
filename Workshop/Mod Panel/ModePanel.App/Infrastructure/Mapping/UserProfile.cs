namespace ModePanel.App.Infrastructure.Mapping
{
    using AutoMapper;
    using Data.Models;
    using Models.Admin;

    public class UserProfile : Profile
    {
        public UserProfile()
        {
            this.CreateMap<User, AdminUserModel>()
                .ForMember(
                    au => au.Posts, 
                    cfg => cfg.MapFrom(u => u.Posts.Count));
        }
    }
}
