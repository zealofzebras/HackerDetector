using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace HackerDetector
{
    public class HackerDetectorMiddleware
    {

        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly HackerDetector _hackerDetector;
        
        public HackerDetectorMiddleware(RequestDelegate next, ILoggerFactory loggerFactory
            ,HackerDetector hackerDetector
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
                // This handles load balancers passing the original client IP
                // through this header. 
                // WARNING: If your load balancer is not passing original client IP
                // through this header, then you will be blocking your load balancer,
                // causing a total outage. Also ensure this Header cannot be spoofed.
                // Your load balancer should be configured in a way that it does not accept
                // this header from the request, instead it always sets it itself.
                var originIP = context.Connection.RemoteIpAddress;
                //if (context.Request.Headers.ContainsKey(XForwardedForHeader))
                //    originIP = IPAddress.Parse(context.Request.Headers[XForwardedForHeader]).MapToIPv4();

                var block = await _hackerDetector.DetectAndBlockAsync(path.Value, originIP);

                await _next.Invoke(context);

                /*
                if (!block)
                    //await _next.Invoke(context);
                else
                {
                    //  _logger. ("Blocked: " + path);

                    // context.Response.StatusCode = (int)HttpStatusCode.NotAcceptable;
                    //  await context.Response.WriteAsync(Enum.GetName(typeof(Hacker.Result), result));
                }
                */
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