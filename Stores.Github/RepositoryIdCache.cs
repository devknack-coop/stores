﻿using Octokit;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace DevKnack.Stores.Github
{
    /// <summary>
    /// A cache so that we don't repeatedly lookup repository IDs
    /// </summary>
    public class RepositoryIdCache
    {
        private readonly GitHubClient _client;
        private static ConcurrentDictionary<string, long> _lookup = new ConcurrentDictionary<string, long>();

        public RepositoryIdCache(GitHubClient client)
        {
            _client = client;
        }

        public async Task<long> GetIdAsync(string url)
        {
            if (_lookup.TryGetValue(url, out long value))
                return value;

            var values = GitHubUrlExtractor.ExtractFromUrl(url);

            string username = values.Item1;
            string reponame = values.Item2;

            var repository = await _client.Repository.Get(username, reponame);
            if (repository == null)
                throw new DevKnack.Common.Exceptions.NotFoundException("Repository not found", url);

            _lookup.TryAdd(url, repository.Id);

            return repository.Id;
        }
    }
}