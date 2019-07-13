using System;
using System.Collections.Generic;
using System.Text;

namespace HackerDetector.Blockers.Cloudflare
{
    public class CloudflareBlockerOptions
    {
        public string ZoneId { get; set; }
        public string AuthEmail { get; set; }
        public string AuthKey { get; set; }
        public CloudFlareBlockerAction BlockAction { get; set; } = CloudFlareBlockerAction.JsChallenge;
    }

    public enum CloudFlareBlockerAction
    {
        Block, Challenge, Whitelist, JsChallenge
    }
}
