namespace OnlineStore.Features.Users
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Data;
    using FluentValidation;
    using Infrastructure.Services.Contracts;
    using Infrastructure.ViewModels.Users;
    using MediatR;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class Edit
    {
        public class Query : IAsyncRequest<UserEditViewModel>
        {
            public int UserId { get; set; }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(q => q.UserId).NotNull().NotEmpty();
            }
        }

        public class QueryHandler : IAsyncRequestHandler<Query, UserEditViewModel>
        {
            public QueryHandler(ApplicationDbContext db, IUsersService usersService)
            {
                this.usersService = usersService;
                this.db = db;
            }

            private readonly ApplicationDbContext db;
            private readonly IUsersService usersService;

            public async Task<UserEditViewModel> Handle(Query message)
            {
                var user = await this.db.Users
                    .Include(u => u.Roles)
                    .FirstOrDefaultAsync(u => u.Id == message.UserId);

                var userViewModel = Mapper.Map<UserViewModel>(user);
                userViewModel.Roles = Mapper.Map<ICollection<RoleViewModel>>
                    (await this.usersService.GetRolesAsync(user.Id.ToString()));

                var availableRoles = await this.db.Roles
                    .ProjectTo<RoleViewModel>()
                    .ToListAsync();

                return new UserEditViewModel
                {
                    AvailableRoles = availableRoles,
                    User = userViewModel
                };
            }
        }

        public class Command : IAsyncRequest
        {
            public int Id { get; set; }

            public string UserName { get; set; }

            public ICollection<int> SelectedRoles { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(c => c.Id).NotNull().NotEmpty();
            }
        }

        public class CommandHandler : AsyncRequestHandler<Command>
        {
            public CommandHandler(ApplicationDbContext db, IUsersService usersService)
            {
                this.db = db;
                this.usersService = usersService;
            }

            private readonly ApplicationDbContext db;
            private readonly IUsersService usersService;

            protected async override Task HandleCore(Command message)
            {
                var userInDb = await this.db.Users
                    .Include(u => u.Roles)
                    .FirstOrDefaultAsync(u => u.Id == message.Id);

                if (userInDb != null)
                {
                    if (message.SelectedRoles != null && message.SelectedRoles.Count > 0)
                    {
                        foreach (var role in message.SelectedRoles)
                        {
                            var roleInDb = await this.db.Roles
                                .Include(r => r.Users)
                                .FirstOrDefaultAsync(r => r.Id == role);

                            if (roleInDb != null)
                            {
                                var relationshipToAdd = new IdentityUserRole<int>() { RoleId = roleInDb.Id, UserId = userInDb.Id };

                                if (roleInDb.Users.FirstOrDefault(r => r.RoleId == relationshipToAdd.RoleId && r.UserId == relationshipToAdd.UserId) == null)
                                {
                                    this.db.UserRoles.Add(relationshipToAdd);
                                }
                            }
                        }
                    }
                    else
                    {
                        userInDb.Roles.Clear();
                    }

                    await this.usersService.UpdateAsync(userInDb);
                }
            }
        }
    }
}
