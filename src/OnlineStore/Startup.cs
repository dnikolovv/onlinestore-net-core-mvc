namespace OnlineStore
{
    using AutoMapper;
    using Data;
    using Data.Initializer;
    using Data.Models;
    using Features.Account.Util;
    using Features.Category.Util;
    using Features.Order.Util;
    using FluentValidation.AspNetCore;
    using Infrastructure;
    using Infrastructure.Constants;
    using Infrastructure.Conventions;
    using Infrastructure.Services.Concrete;
    using Infrastructure.Services.Contracts;
    using MediatR;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using OnlineStore.Features.Roles.Util;
    using OnlineStore.Infrastructure.Authorization;
    using OnlineStore.Infrastructure.Authorization.Requirements;

    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    this.Configuration["Data:DefaultConnection:ConnectionString"]));

            services.AddMediatR(typeof(Startup));
            services.AddAutoMapper(typeof(Startup));

            Mapper.AssertConfigurationIsValid();

            services.AddMemoryCache();
            services.AddSession();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddIdentity<User, UserRole>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequiredLength = 1;
                    options.User.AllowedUserNameCharacters = null;

                })
                .AddEntityFrameworkStores<ApplicationDbContext, int>();

            services.AddTransient<IUsersService, UsersService>();
            services.AddTransient<ICategoryValidator, CategoryValidator>();
            services.AddTransient<IRolesValidator, RolesValidator>();
            services.AddTransient<IOrderValidator, OrderValidator>();
            services.AddTransient<IUserValidator, UserValidator>();

            services.AddDynamicAuthorization(new[]
            {
                new AuthorizationSection(
                    Policies.PRODUCT_MANAGER,
                    Claims.PRODUCT_MANAGER,
                    "Product Manager",
                    new HasClaimOrIsAdminRequirement(Claims.PRODUCT_MANAGER),
                    new HasClaimOrIsAdminHandler()),
                new AuthorizationSection(
                    Policies.CATEGORY_MANGER,
                    Claims.CATEGORY_MANGER,
                    "Category Manager",
                    new HasClaimOrIsAdminRequirement(Claims.CATEGORY_MANGER),
                    new HasClaimOrIsAdminHandler()),
                new AuthorizationSection(
                    Policies.ORDER_MANAGER,
                    Claims.ORDER_MANAGER,
                    "Order Manager",
                    new HasClaimOrIsAdminRequirement(Claims.ORDER_MANAGER),
                    new HasClaimOrIsAdminHandler()),
                new AuthorizationSection(
                    Policies.USER_MANAGER,
                    Claims.USER_MANAGER,
                    "User Manager",
                    new HasClaimOrIsAdminRequirement(Claims.USER_MANAGER),
                    new HasClaimOrIsAdminHandler()),
                new AuthorizationSection(
                    Policies.ROLE_MANAGER,
                    Claims.ROLE_MANAGER,
                    "Role Manager",
                    new HasClaimOrIsAdminRequirement(Claims.ROLE_MANAGER),
                    new HasClaimOrIsAdminHandler())
            });

            services.AddMvc(opt =>
            {
                opt.Conventions.Add(new FeatureConvention());
                opt.Filters.Add(typeof(DbContextTransactionFilter));
                opt.Filters.Add(typeof(ValidatorActionFilter));
            })
                .AddRazorOptions(options =>
                {
                    // {0} - Action Name
                    // {1} - Controller Name
                    // {2} - Area Name
                    // {3} - Feature Name
                    // Replace normal view location entirely
                    options.ViewLocationFormats.Clear();
                    options.ViewLocationFormats.Add("/Features/{3}/{1}/{0}.cshtml");
                    options.ViewLocationFormats.Add("/Features/{3}/{1}/Views/{0}.cshtml");
                    options.ViewLocationFormats.Add("/Features/{3}/Views/{0}.cshtml");
                    options.ViewLocationFormats.Add("/Features/{3}/{0}.cshtml");
                    options.ViewLocationFormats.Add("/Features/Shared/{0}.cshtml");
                    options.ViewLocationExpanders.Add(new FeatureViewLocationExpander());
                })
                .AddFluentValidation(cfg => { cfg.RegisterValidatorsFromAssemblyContaining<Startup>(); });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseStatusCodePages();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();
            app.UseSession();
            app.UseIdentity();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "Error",
                    template: "Error",
                    defaults: new { controller = "Error", action = "Error" });

                routes.MapRoute(
                    name: null,
                    template: "{category}/Page{page:int}",
                    defaults: new { controller = "Product", action = "List", pageSize = ProductConstants.DEFAULT_PRODUCTS_PER_PAGE });

                routes.MapRoute(
                    name: null,
                    template: "Page{page:int}",
                    defaults: new { controller = "Product", action = "List", page = 1, pageSize = ProductConstants.DEFAULT_PRODUCTS_PER_PAGE });

                routes.MapRoute(
                    name: null,
                    template: "{category}",
                    defaults: new { controller = "Product", action = "List", page = 1, pageSize = ProductConstants.DEFAULT_PRODUCTS_PER_PAGE });

                routes.MapRoute(
                    name: null,
                    template: "",
                    defaults: new { controller = "Product", action = "ListLatest", numberOfItems = ProductConstants.DEFAULT_FEATURED_PRODUCTS_COUNT });

                routes.MapRoute(name: null, template: "{controller}/{action}/{id?}");
            });

            DbInitializer.InitializeDb(app);
        }
    }
}
