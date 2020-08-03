using Common.Stores.HttpClient;
using DevKnack.Stores;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection RegisterHttpFileStore(this IServiceCollection services)
        {
            services.AddScoped<HttpFileStore>();
            services.ReplaceWithScoped<IFileStore, HttpFileStore>();

            services.AddScopedWithImplementation<IRepositoryQuery, HttpRepositoryQuery>();
            services.AddScopedWithImplementation<ISearchRepositoryQuery, HttpSearchRepositoryQuery>();
            services.AddScopedWithImplementation<IBranchService, HttpBranchService>();

            return services;
        }
    }
}