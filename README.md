# HackerDetector
Middleware for ASPnet Core to report abusive IP's to a firewall (such as Cloudflare) via resillient functions

Btw. This is totally ripped off from a 3 year old blog post by Troy Hunt: https://www.troyhunt.com/azure-functions-in-practice/ read in order to know why this will help with costs and why it is free as in beer.

## Installation

Install the NugetPackage Sustainable.Web.HackerDetector for your MVC Core project,

For your functions app that will process the block and unblock messages install the nuget package Sustainable.Web.HackerDetector.Functions

and any of the Blocker implementations, currently available options are:

* Sustainable.Web.HackerDetector.CloudflareBlocker

## Usage in MVC

Todo

## Usage in Function App

### Configuration

add the following to the startup.cs in the configure method for the appropriate Blocker, in this case the CloudflareBlocker

```csharp
    builder.Services.AddCloudflareBlocker(new CloudflareBlockerOptions()
    {
        ZoneId = Environment.GetEnvironmentVariable("CloudflareZoneId", EnvironmentVariableTarget.Process),
        AuthEmail = Environment.GetEnvironmentVariable("CloudflareAuthEmail", EnvironmentVariableTarget.Process),
        AuthKey = Environment.GetEnvironmentVariable("CloudflareAuthKey", EnvironmentVariableTarget.Process)
    });
```

### Function implementation

Since functions cannot be created outside the main assembly, and fixing that will require some build targets the following is the preferred way:

Create a new class name it whatever (ie. hackerz.cs) and enter the following

```csharp
    public class Hackers
    {
        private readonly HackerDetector.Functions.IHackerBlocker blocker;

        public Hackers(HackerDetector.Functions.IHackerBlocker blocker)
        {
            this.blocker = blocker;
        }

        [FunctionName("HackerBlocked")]
        public void Run([QueueTrigger("hacker-blocked", Connection = "AzureWebJobsStorage")]string ip, ILogger log)
        {
            log.LogInformation($"Blocking : {ip}");
            blocker.Block(ip);
        }
    }
```

Make sure the connection string and the queue name matches the configuration for the MVC project.