using System.Linq;

namespace HackerDetector.Blockers.Cloudflare.Extensions
{
    internal static class StringExtensions
    {

        public static string ToUnderscoreCase(this string str)
        {

            return string.Concat(str.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString())).ToLower();

        }

    }
}
