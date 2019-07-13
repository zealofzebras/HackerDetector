using Microsoft.Azure.Storage.Queue;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Storage; // Namespace for CloudStorageAccount

namespace HackerDetector
{
    public class HackerDetector
    {

        private readonly CloudQueue blockQueue;

        private readonly Dictionary<IPAddress, int> accessCache = new Dictionary<IPAddress, int>();

        private readonly Dictionary<IPAddress, Collections.FixedSizedQueue<DateTime>> hammerCache = new Dictionary<IPAddress, Collections.FixedSizedQueue<DateTime>>();

        private readonly HackerDetectorOptions _options;
        public HackerDetectorOptions Options { get => _options; }

        public HackerDetector(ILoggerFactory loggerFactory, HackerDetectorOptions options)
        {



            // Retrieve storage account from connection string.
            var storageAccount = CloudStorageAccount.Parse(options.QueueConnectionString);

            // Create the queue client.
            var queueClient = storageAccount.CreateCloudQueueClient();

            // Retrieve a reference to a container.
            blockQueue = queueClient.GetQueueReference(options.BlockQueueName);

            // Create the queue if it doesn't already exist
            blockQueue.CreateIfNotExists();


            _options = options;
        }


        public async Task<bool> DetectAndBlockAsync(string path, IPAddress originIP)
        {
            var block = Detect(path, originIP);

            if (block)
                await BlockAsync(originIP);

            return block;
        }

        public bool DetectAndBlock(string path, IPAddress originIP)
        {
            var block = Detect(path, originIP);

            if (block)
                Block(originIP);

            return block;
        }

        public void Block(IPAddress originIP) => blockQueue.AddMessage(new CloudQueueMessage(originIP.ToString()));

        public Task BlockAsync(IPAddress originIP) => blockQueue.AddMessageAsync(new CloudQueueMessage(originIP.ToString()));


        public bool Detect(string path, IPAddress originIP)
        {

            if ((_options.Traps?.Contains(path)).GetValueOrDefault())
            {
                if (_options.TrapHitsToBlock == 1)
                    return true;
                else if (accessCache.TryGetValue(originIP, out var cached))
                    if (cached >= _options.TrapHitsToBlock)
                        return true;
                    else
                        accessCache[originIP] = cached + 1;
                else
                    accessCache.Add(originIP, 1);
            }

            if (_options.HammerCheckAllPaths || (_options.HammerPaths?.Contains(path)).GetValueOrDefault())
            {
                if (!hammerCache.TryGetValue(originIP, out var cached))
                    cached = new Collections.FixedSizedQueue<DateTime>(_options.HammerMaxHitsInASecond);

                cached.Enqueue(DateTime.Now);

                if ((cached.First() - cached.Last()).TotalSeconds < 1)
                    return true;
            }


            // TODO: Querystring hacks check

            // TODO: Hammer specific methods check

            // 

            return false;
        }

    }
}
