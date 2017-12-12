using System;

namespace aska.core.security.Cors
{
    public static class NancyCorsExtension
    {
        public static void EnableCors(this Nancy.Bootstrapper.IPipelines pipelines)
        {
            pipelines.AfterRequest.AddItemToEndOfPipeline(ctx =>
            {
                //TODO: remove this holly shit
                // causes exceptions with reason "Collection modified" when trying to handle request in parallel with Cache engine
                
                if (ctx.Request.Headers.Keys.AsEnumerable().Any(x =>
                    string.Equals(x, "Origin", StringComparison.InvariantCultureIgnoreCase)))
                {
                    lock (ctx.Response.Headers)
                    {
                        if (ctx.Request.Headers.Keys.AsEnumerable().Any(x =>
                            string.Equals(x, "Origin", StringComparison.InvariantCultureIgnoreCase)))
                        {
                            var origins = "" + string.Join(" ", ctx.Request.Headers["Origin"]);
                            ctx.Response.Headers["Access-Control-Allow-Origin"] = origins;
#warning TODO: return only allowed list of origins from app config

                            if (ctx.Request.Method == "OPTIONS")
                            {
                                // handle CORS preflight request
                                ctx.Response.Headers["Access-Control-Allow-Methods"] =
                                    "GET, POST, PUT, DELETE, PATCH, OPTIONS";

                                if (ctx.Request.Headers.Keys.Contains("Access-Control-Request-Headers"))
                                {
                                    var allowedHeaders = "" +
                                                         string.Join(", ",
                                                             ctx.Request.Headers["Access-Control-Request-Headers"]);
                                    ctx.Response.Headers["Access-Control-Allow-Headers"] = allowedHeaders;
                                }
                            }
                        }
                    }
                }
            });
        }
    }
    
}