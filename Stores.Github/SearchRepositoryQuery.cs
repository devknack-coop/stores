using Octokit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DevKnack.Stores.Github
{
    public class SearchRepositoryQuery : ISearchRepositoryQuery
    {
        private readonly GitHubClient _client;

        public SearchRepositoryQuery(GitHubClient client)
        {
            _client = client;
        }

        public async Task<IEnumerable<string>> SearchAsync(string url, string extension)
        {
            if (string.IsNullOrEmpty(url))
                throw new ArgumentNullException(nameof(url));
            if (string.IsNullOrEmpty(extension))
                throw new ArgumentNullException(nameof(extension));

            string branchName = url.ExtractBranchName();
            url = url.StripBranchName(branchName);

            if (!extension.StartsWith('.'))
                extension = $".{extension}";

            var values = GitHubUrlExtractor.ExtractFromUrl(url);

            string username = values.Item1;
            string reponame = values.Item2;

            string search = $"extension:{extension}";
            var req = new SearchCodeRequest(search, username, reponame);

            if (string.IsNullOrEmpty(branchName))
            {
                var results = await _client.Search.SearchCode(req);
                // Github is searching the contents of the files too
                // Github seems generous with the match, so do a secondary filter
                var filteredResults = results.Items.Select(i => i.Path);

                return filteredResults;
            }
            else
            {
                // Search in a specific branch
                var tree = await _client.Git.Tree.GetRecursive(username, reponame, branchName);

                var filePaths = tree.Tree.Select(item => item.Path).Where(path => Path.GetExtension(path) == extension);

                return filePaths;
            }
        }
    }
}