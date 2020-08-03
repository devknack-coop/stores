using Blazored.LocalStorage;
using DevKnack.Stores;
using DevKnack.Stores.Browser;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection RegisterLocalStorageCache(this IServiceCollection services)
        {
            services.AddBlazoredLocalStorage();

            services.AddScoped<LocalStorageFileStore>();

            services.AddScoped(p => new FileStoreCache(p.GetRequiredService<IFileStore>(), p.GetRequiredService<LocalStorageFileStore>()));
            services.AddScoped<ICommitFileStore, FileStoreCache>(p => p.GetRequiredService<FileStoreCache>());

            // Add a wrapper around the cache
            // TODO: Causes exceptions
            //services.ReplaceWithScoped<IFileStore, FileStoreCache>(p => p.GetRequiredService<FileStoreCache>());

            return services;
        }
    }
}