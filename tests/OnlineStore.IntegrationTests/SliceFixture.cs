namespace OnlineStore.IntegrationTests
{
    using Data;
    using Data.Models;
    using Extensions;
    using FakeItEasy;
    using MediatR;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Respawn;
    using System;
    using System.IO;
    using System.Threading.Tasks;

    public class SliceFixture
    {
        private static readonly Checkpoint _checkpoint;
        private static readonly IConfigurationRoot _configuration;
        private static readonly IServiceScopeFactory _scopeFactory;

        static SliceFixture()
        {
            var host = A.Fake<IHostingEnvironment>();
            A.CallTo(() => host.ContentRootPath).Returns(Directory.GetCurrentDirectory());

            var startup = new Startup(host);
            _configuration = startup.Configuration;
            var services = new ServiceCollection();
            startup.ConfigureServices(services);
            var provider = services.BuildServiceProvider();
            _scopeFactory = provider.GetService<IServiceScopeFactory>();
            _checkpoint = new Checkpoint();
        }

        public static void ResetCheckpoint()
        {
            _checkpoint.Reset(_configuration["Data:DefaultConnection:ConnectionString"]);
        }

        public async Task ExecuteScopeAsync(Func<IServiceProvider, Task> action)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<ApplicationDbContext>();

                try
                {
                    dbContext.BeginTransaction();

                    await action(scope.ServiceProvider);

                    await dbContext.CommitTransactionAsync();
                }
                catch (Exception)
                {
                    dbContext.RollbackTransaction();
                    throw;
                }
            }
        }

        public async Task<T> ExecuteScopeAsync<T>(Func<IServiceProvider, Task<T>> action)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<ApplicationDbContext>();

                try
                {
                    dbContext.BeginTransaction();

                    var result = await action(scope.ServiceProvider);

                    await dbContext.CommitTransactionAsync();

                    return result;
                }
                catch (Exception)
                {
                    dbContext.RollbackTransaction();
                    throw;
                }
            }
        }

        public Task ExecuteDbContextAsync(Func<ApplicationDbContext, Task> action)
        {
            return ExecuteScopeAsync(sp => action(sp.GetService<ApplicationDbContext>()));
        }

        public Task<T> ExecuteDbContextAsync<T>(Func<ApplicationDbContext, Task<T>> action)
        {
            return ExecuteScopeAsync(sp => action(sp.GetService<ApplicationDbContext>()));
        }

        public Task InsertAsync(params IEntity[] entities)
        {
            return ExecuteDbContextAsync(db =>
            {
                foreach (var entity in entities)
                {
                    db.Add(entity);
                }
                return db.SaveChangesAsync();
            });
        }

        public TController GetController<TController>()
            where TController : Controller
        {
            var httpContext = A.Fake<HttpContext>();
            httpContext.Session = A.Fake<ISession>();

            var controller = A.Fake<TController>();
            controller.TempData = new TempDataDictionary(httpContext, A.Fake<ITempDataProvider>());

            return controller;
        }

        public Task<T> FindAsync<T>(int id)
            where T : class, IEntity
        {
            // HACK: EF7 is missing FindAsync and this is a custom implementation
            return ExecuteDbContextAsync(db => db.Set<T>().FindAsync(db, id));
        }

        public Task<TResponse> SendAsync<TResponse>(IAsyncRequest<TResponse> request)
        {
            return ExecuteScopeAsync(sp =>
            {
                var mediator = sp.GetService<IMediator>();

                return mediator.SendAsync(request);
            });
        }

        public Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
        {
            return ExecuteScopeAsync(sp =>
            {
                var mediator = sp.GetService<IMediator>();

                var response = mediator.Send(request);

                return Task.FromResult(response);
            });
        }
    }
}