namespace DevKnack.Stores.Github
{
    public class GithubAuthSettings
    {
        public GithubAuthSettings(string clientId, string clientSecret, bool disabled)
        {
            ClientId = clientId;
            ClientSecret = clientSecret;
            Disabled = disabled;
        }

        #region OAUTH settings

        /// <summary>Github client ID</summary>
        public string ClientId { get; }

        /// <summary>Github client secret</summary>
        public string ClientSecret { get; }

        public bool Disabled { get; }

        #endregion OAUTH settings
    }
}