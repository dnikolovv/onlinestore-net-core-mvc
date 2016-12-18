namespace OnlineStore.Features.Product
{
    using AutoMapper;
    using Data.Models;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, Edit.Command>();

            CreateMap<Edit.Command, Product>()
                .ForMember(c => c.DateAdded, opt => opt.Ignore());

            CreateMap<Product, Create.Command>()
                .ReverseMap();
            
            CreateMap<Product, Remove.Command>()
                .ForMember(c => c.ProductId, opt => opt.Ignore());
        }
    }
}
