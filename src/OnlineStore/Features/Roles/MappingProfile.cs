namespace OnlineStore.Features.Roles
{
    using AutoMapper;
    using Data.Models;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Create.Command, UserRole>(MemberList.Source)
                .ForSourceMember(c => c.SelectedPermissions, opt => opt.Ignore())
                .AfterMap((src, dest) =>
                {
                    dest.NormalizedName = src.Name.ToUpper();
                });

            CreateMap<Edit.Command, UserRole>(MemberList.Source)
                .ForSourceMember(c => c.SelectedPermissions, opt => opt.Ignore());
        }
    }
}
