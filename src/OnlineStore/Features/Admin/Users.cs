namespace OnlineStore.Features.Admin
{
    using AutoMapper;
    using Data;
    using Data.Models;
    using Infrastructure.Services.Contracts;
    using Infrastructure.ViewModels.Users;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class Users
    {
        public class Query : IAsyncRequest<IEnumerable<UserViewModel>> { }

        public class QueryHandler : IAsyncRequestHandler<Query, IEnumerable<UserViewModel>>
        {
            public QueryHandler(ApplicationDbContext db, IUsersService usersService)
            {
                this.db = db;
                this.usersService = usersService;
            }

            private readonly ApplicationDbContext db;
            private readonly IUsersService usersService;

            public async Task<IEnumerable<UserViewModel>> Handle(Query message)
            {
                var usersList = await this.db.Users
                    .Include(u => u.Roles)
                    .ToListAsync();

                var viewModels = await UpdateAndProject(usersList);

                return viewModels;
            }

            public async Task<IEnumerable<UserViewModel>> UpdateAndProject(IEnumerable<User> users)
            {
                List<UserViewModel> viewModels = new List<UserViewModel>();

                foreach (var user in users)
                {
                    var viewModel = Mapper.Map<UserViewModel>(user);

                    var roles = await this.usersService.GetRolesAsync(user.Id.ToString());
                    viewModel.Roles = Mapper.Map<ICollection<RoleViewModel>>(roles);
                    
                    viewModels.Add(viewModel);
                }

                return viewModels;
            }
        }
    }
}
