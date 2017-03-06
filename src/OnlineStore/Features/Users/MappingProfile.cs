namespace OnlineStore.Features.Users
{
    using AutoMapper;
    using Data.Models;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Edit.Command, User>(MemberList.Source)
                .ForSourceMember(c => c.SelectedRoles, opt => opt.Ignore());
        }
    }
}
