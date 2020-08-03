using DevKnack.Stores;
using DevKnack.Stores.Github;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection RegisterGitHubStore(this IServiceCollection services)
        {
            services.AddScoped<RepositoryIdCache>();

            services.AddScopedWithImplementation<IFileStore, GithubFileStore>();
            services.AddScopedWithImplementation<IRepositoryQuery, RepositoryQuery>();
            services.AddScopedWithImplementation<ISearchRepositoryQuery, SearchRepositoryQuery>();
            services.AddScopedWithImplementation<IBranchService, BranchService>();

            FileStoreFactory.RegisterFileStore<GithubFileStore>("git", false);

            return services;
        }
    }
}