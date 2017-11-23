namespace OnlineStore.Features.Roles
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using AutoMapper;
    using Data;
    using FluentValidation;
    using MediatR;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using OnlineStore.Data.Models;
    using OnlineStore.Infrastructure.ViewModels.Roles;

    public class Edit
    {
        public class Query : IAsyncRequest<RoleEditViewModel>
        {
            public int? RoleId { get; set; }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(q => q.RoleId).NotNull();
            }
        }

        public class QueryHandler : IAsyncRequestHandler<Query, RoleEditViewModel>
        {
            public QueryHandler(ApplicationDbContext db)
            {
                this.db = db;
            }

            private readonly ApplicationDbContext db;

            public async Task<RoleEditViewModel> Handle(Query message)
            {

                var roleInDb = await this.db.Roles
                    .Include(r => r.Claims)
                    .FirstOrDefaultAsync(r => r.Id == message.RoleId);

                return Mapper.Map<RoleEditViewModel>(roleInDb);
            }
        }

        public class Command : IAsyncRequest
        {
            public int? Id { get; set; }

            public string Name { get; set; }
            
            public ICollection<string> SelectedClaims { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(c => c.Id).NotNull();
                RuleFor(c => c.Name).NotNull().NotEmpty();
            }
        }

        public class CommandHandler : AsyncRequestHandler<Command>
        {
            public CommandHandler(ApplicationDbContext db, RoleManager<UserRole> roleManager)
            {
                this.db = db;
                this.roleManager = roleManager;
            }

            private readonly ApplicationDbContext db;
            private readonly RoleManager<UserRole> roleManager;
            private readonly ClaimsIdentity test;

            protected override async Task HandleCore(Command message)
            {
                var roleInDb = await this.db.Roles
                    .FirstOrDefaultAsync(r => r.Id == message.Id);

                await UpdateClaims(roleInDb, message.SelectedClaims);

                Mapper.Map(message, roleInDb);
            }

            private async Task UpdateClaims(UserRole role, IEnumerable<string> selectedClaims)
            {
                foreach (var claim in await this.roleManager.GetClaimsAsync(role))
                {
                    await this.roleManager.RemoveClaimAsync(role, claim);
                }

                if (selectedClaims != null)
                {
                    foreach (var claimType in selectedClaims)
                    {
                        var existingClaim = this.db
                            .RoleClaims
                            .FirstOrDefault(rc => rc.RoleId == role.Id && rc.ClaimType == claimType)?
                            .ToClaim();

                        if (existingClaim != null)
                        {
                            await this.roleManager.AddClaimAsync(role, existingClaim);
                        }
                        else
                        {
                            await this.roleManager.AddClaimAsync(role, new Claim(claimType, claimType));
                        }
                    } 
                }

                await this.roleManager.UpdateAsync(role);
            }

            private void AddOrUpdateClaim(UserRole role, string claimType)
            {
                var claimInDb = this.db
                    .RoleClaims
                    .FirstOrDefault(rc => rc.ClaimType == claimType);

                if (claimInDb == null)
                {
                    var claimToAdd = new IdentityRoleClaim<int>()
                    {
                        ClaimType = claimType,
                        RoleId = role.Id,
                        ClaimValue = claimType
                    };

                    this.db.RoleClaims.Add(claimToAdd);
                }
                else
                {
                    role.Claims.Add(claimInDb);
                }
            }
        }
    }
}
