namespace OnlineStore.ViewModels
{
    using System.Linq;
    using AutoMapper;
    using Data.Models;
    using Infrastructure.ViewModels.Cart;
    using Infrastructure.ViewModels.Categories;
    using Infrastructure.ViewModels.Orders;
    using Infrastructure.ViewModels.Products;
    using Infrastructure.ViewModels.Users;
    using OnlineStore.Infrastructure.Authorization;
    using OnlineStore.Infrastructure.ViewModels.Roles;

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

            // Users
            CreateMap<User, UserViewModel>(MemberList.Destination)
                .ForMember(u => u.Roles, opt => opt.Ignore());

            // Roles
            CreateMap<UserRole, RoleViewModel>(MemberList.Destination)
                .ForMember(r => r.Claims, opt => opt.MapFrom(ur => ur.Claims.Select(c => c.ClaimType)));

            CreateMap<UserRole, RoleEditViewModel>(MemberList.Destination)
                .ForMember(r => r.AvailableSections, opt => opt.Ignore())
                .ForMember(r => r.SelectedClaims, opt => opt.MapFrom(ur => ur.Claims.Select(c => c.ClaimType).ToList()))
                .AfterMap((userRole, roleEditViewModel) =>
                {
                    roleEditViewModel.AvailableSections = AuthorizationSectionsContainer
                        .Sections
                        .Select(section => new AuthorizationSectionViewModel
                        {
                            ClaimType = section.ClaimType,
                            FriendlyName = section.FriendlyName,
                            Role = Mapper.Map<RoleViewModel>(userRole)
                        })
                        .ToList();
                });
        }
    }
}
