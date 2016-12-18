namespace OnlineStore.Features.Category
{
    using AutoMapper;
    using Infrastructure.ViewModels.Categories;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Create.Command, Data.Models.Category>()
                .ForMember(c => c.Id, opt => opt.Ignore());

            CreateMap<Edit.Command, Data.Models.Category>();
        }
    }
}
