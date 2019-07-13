using HackerDetector.Blockers.Cloudflare.Extensions;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace HackerDetector.Blockers.Cloudflare
{
    public class CloudflareBlocker : HackerDetector.Functions.IHackerBlocker
    {

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly CloudflareBlockerOptions _options;

        public CloudflareBlocker(CloudflareBlockerOptions options, IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _options = options;
        }


        public async Task Block(string ipAddress)
        {
#if DEBUG
            ipAddress = "203.0.113.1"; // part of TEST-NET-3 and should go nowhere.
#endif
            //https://api.cloudflare.com/client/v4/user/firewall/access_rules/rules
            var path = $"/client/v4/zones/{_options.ZoneId}/firewall/access_rules/rules";
            var body = new
            {
                mode = _options.BlockAction.ToString().ToUnderscoreCase(), 
                configuration = new
                {
                    target = "ip",
                    value = ipAddress
                },
                notes = $"hackerblocked-{DateTime.UtcNow.ToString("s")}"
            };

            using (var client = _httpClientFactory.CreateClient())
            {

                client.BaseAddress = new Uri("https://api.cloudflare.com");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("X-Auth-Email", _options.AuthEmail);
                client.DefaultRequestHeaders.Add("X-Auth-Key", _options.AuthKey);

                var success = await client.PostAsJsonAsync(path, body);
                try
                {
                    success.EnsureSuccessStatusCode();
                }
                catch (HttpRequestException exception) 
                {
                    // Should this throw a new exception? I don't know what the best option is.
                    exception.Data.Add("Content", await success.Content.ReadAsStringAsync());
                    exception.Data.Add("ReasonPhrase", success.ReasonPhrase);
                    throw;
                }
            }
        }

        /*
        public async Task Unblock(DateTime cutoff)
        {
            //https://api.cloudflare.com/client/v4/user/firewall/access_rules/rules?page=1&per_page=20&mode=challenge&configuration.target=ip&configuration.value=198.51.100.4&notes=my note&match=all&order=mode&direction=desc
        }
        */
    }
}
