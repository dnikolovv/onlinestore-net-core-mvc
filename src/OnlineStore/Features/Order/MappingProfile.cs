namespace OnlineStore.Features.Order
{
    using AutoMapper;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Data.Models.Order, Checkout.Command>().ReverseMap();
        }
    }
}
