namespace OnlineStore.Features.Account
{
    using AutoMapper;
    using Data.Models;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Register.Command, User>(MemberList.None);
        }
    }
}
