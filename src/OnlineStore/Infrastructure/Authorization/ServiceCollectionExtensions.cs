namespace OnlineStore.Infrastructure.Authorization
{
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtensions
    {
        public static void AddDynamicAuthorization(
            this IServiceCollection services,
            params AuthorizationSection[] sections)
        {
            AuthorizationSectionsContainer.Initialize(sections);

            foreach (var section in AuthorizationSectionsContainer.Sections)
            {
                services.AddSingleton(section.Handler);
            }

            services.AddAuthorization(opts =>
            {
                opts.RegisterAuthorizationSections(AuthorizationSectionsContainer.Sections);
            });
        }
    }
}
