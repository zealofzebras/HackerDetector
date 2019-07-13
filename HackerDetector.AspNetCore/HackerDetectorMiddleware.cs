using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace HackerDetector.AspNetCore
{
    public class HackerDetectorMiddleware
    {

        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly HackerDetector _hackerDetector;

        public HackerDetectorMiddleware(RequestDelegate next, ILoggerFactory loggerFactory
            , HackerDetector hackerDetector
            )
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<HackerDetector>();
            _hackerDetector = hackerDetector;


        }


        public async Task Invoke(HttpContext context)
        {
            var path = context.Request.Path;

            if (path.HasValue)
            {
                // Use the XForwardedForMiddleware
                var originIP = context.Connection.RemoteIpAddress;

                var block = await _hackerDetector.DetectAndBlockAsync(path.Value, originIP);


                if (!block)
                    await _next.Invoke(context);
                else
                {
                    _logger.LogInformation(string.Format("Blocked: {0} for accessing {1}", originIP.ToString(), path));

                    // context.Response.StatusCode = (int)HttpStatusCode.NotAcceptable;
                    //  await context.Response.WriteAsync(Enum.GetName(typeof(Hacker.Result), result));
                }

                //watch.Stop();
                //Debug("Finished: " + path + " " + watch.ElapsedMilliseconds);
            }
            else
            {
                await _next.Invoke(context);
            }
        }
    }
}