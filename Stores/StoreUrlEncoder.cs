using System.Web;

namespace DevKnack.Stores
{
    /// <summary>
    /// Special URL encoder
    /// </summary>
    public static class StoreUrlEncoder
    {
        public static string Encode(string url)
        {
            return HttpUtility.UrlEncode(url);
            //return url.Replace("https://github.com/", "gh:").Replace("git://github.com/", "gith:").Replace("/", "|");
        }

        public static string Decode(string data)
        {
            return HttpUtility.UrlDecode(data);
            //return data.Replace("gh:", "https://github.com/").Replace("gith:", "git://github.com/").Replace("|", "/");
        }
    }
}