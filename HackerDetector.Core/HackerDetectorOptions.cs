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

        /// <summary>
        /// Very useful if there in the pipeline is a error reporting tool (like elmah.io) that should ignore these failed/blocked requests. 
        /// Set to true to stop the pipeline and return a response.
        /// </summary>
        public bool ReturnBlockedResponseWhenBlocked { get; set; } = false;

        /// <summary>
        /// Very useful if there in the pipeline is a error reporting tool (like elmah.io) that should ignore these failed/blocked requests. 
        /// Set to true to stop the pipeline and return a response.
        /// </summary>
        public bool ReturnBlockedResponseOnTraps { get; set; } = false;
    }
}
