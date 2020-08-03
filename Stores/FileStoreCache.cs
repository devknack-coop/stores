using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevKnack.Stores
{
    /// <summary>
    /// Storage in a cache with fallback to a service
    /// </summary>
    public class FileStoreCache : ICommitFileStore
    {
        private readonly IFileStore _store;
        private readonly IFileStore _cache;
        private readonly ConcurrentQueue<(string,string)> _changes = new ConcurrentQueue<(string, string)>();

        public FileStoreCache(IFileStore store, IFileStore cache)
        {
            _store = store;
            _cache = cache;
        }

        public async Task<string?> ReadStringFileAsync(string url, string path)
        {
            string? result = await _cache.ReadStringFileAsync(url, path);

            // If cache doesn't have the result, then check the store and update cache with the result
            if (string.IsNullOrEmpty(result))
            {
                result = await _store.ReadStringFileAsync(url, path);

                if (!string.IsNullOrEmpty(result))
                    await _cache.WriteStringFileAsync(url, path, result);
            }
            return result;
        }

        public async Task WriteStringFileAsync(string url, string path, string contents)
        {
            // TODO: We should write to the cache, and have a separate operation to persist the cache contents?
            //await _store.WriteStringFileAsync(url, path, contents);
            await _cache.WriteStringFileAsync(url, path, contents);

            _changes.Enqueue((url, path));
        }

        public Task<IEnumerable<(string, string)>> GetChangesAsync()
        {
            return Task.FromResult<IEnumerable<(string, string)>>(_changes);
        }

        public async Task CommitAllAsync()
        {
            while (_changes.TryDequeue(out var nextItem))
            {
                string url = nextItem.Item1;
                string path = nextItem.Item2;
                string? contents = await _cache.ReadStringFileAsync(url, path);
                if (!string.IsNullOrEmpty(contents))
                {
                    await _store.WriteStringFileAsync(url, path, contents);
                }
            }
        }
    }
}