using Octokit;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevKnack.Stores.Github
{
    public class RepositoryQuery : IRepositoryQuery
    {
        private readonly GitHubClient _client;

        public RepositoryQuery(GitHubClient client)
        {
            _client = client;
        }

        public async Task<IEnumerable<string>> GetRepositoriesAsync()
        {
            var repositries = await _client.Repository.GetAllForCurrent();

            return repositries.Select(r => r.GitUrl).ToList();
        }
    }
}