namespace OnlineStore.ViewModels
{
    using AutoMapper;
    using Data.Models;
    using Infrastructure.ViewModels.Cart;
    using Infrastructure.ViewModels.Categories;
    using Infrastructure.ViewModels.Orders;
    using Infrastructure.ViewModels.Products;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Products
            CreateMap<Product, ProductViewModel>()
                .ForMember(p => p.Category, opt => opt.MapFrom(p => p.Category.Name));

            CreateMap<Product, ProductEditViewModel>()
                .ForMember(p => p.Categories, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<ProductEditViewModel, Features.Product.Edit.Command>();

            // Cart
            CreateMap<CartItem, CartItemViewModel>();

            CreateMap<Cart, CartViewModel>()
                .ForMember(c => c.TotalSum, opt => opt.MapFrom(c => c.TotalSum()));

            // Orders
            CreateMap<Order, OrderViewModel>();

            // Categories
            CreateMap<Category, CategoryViewModel>();

            CreateMap<CategoryEditViewModel, Features.Category.Edit.Command>();
        }
    }
}
