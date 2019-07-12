using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HackerDetector
{
    public class HackerDetectorOptions
    {
        public string QueueConnectionString { get; set; }

        public string BlockQueueName { get; set; } = "hacker-blocked";

        public bool IgnoreBuiltinTraps { get; set; }

        public List<string> Traps { get; set; } = new List<string>
        {
            "/wp-login.php"
        };

        public List<string> HammerPaths { get; set; } = new List<string>
        {
            "/Identity/Account/Login"
        };

        public int TrapHitsToBlock { get; set; } = 2;

        public bool HammerCheckAllPaths { get; set; }

        public int HammerMaxHitsInASecond { get; set; } = 4;
    }
}
