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
                { // It's a dummy app so I don't need these
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequiredLength = 1;
                    options.User.AllowedUserNameCharacters = null;

                }).AddEntityFrameworkStores<ApplicationDbContext, int>();

            services.AddTransient<IUsersService, UsersService>();
            services.AddTransient<ICategoryValidator, CategoryValidator>();
            services.AddTransient<IOrderValidator, OrderValidator>();
            services.AddTransient<IUserValidator, UserValidator>();

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
