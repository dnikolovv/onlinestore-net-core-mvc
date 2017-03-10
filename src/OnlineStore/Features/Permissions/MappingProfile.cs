namespace OnlineStore.Features.Permissions
{
    using AutoMapper;
    using Data.Models;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Create.Command, Permission>(MemberList.Source);
            CreateMap<Edit.Command, Permission>(MemberList.Source)
                .ForSourceMember(c => c.SelectedRoles, opt => opt.Ignore());
        }
    }
}
