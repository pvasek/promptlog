using Microsoft.AspNetCore.Http.Features;
using PromptLog.Db;
using PromptLog.Entities;
using Yarp.ReverseProxy.Configuration;

namespace PromptLog.App.Services;


public static class ProxyService
{
    public static void AddOpenAIProxy(this IServiceCollection services, AppConfiguration configuration)
    {
        services.AddReverseProxy()
            .LoadFromMemory(GetRoutes(), GetClusters(configuration));
    }
    
    public static void MapOpenAIProxy(this WebApplication app, AppConfiguration configuration)
    {
        app.MapReverseProxy(proxyPipeline =>
        {
            proxyPipeline.Use(async (context, next) =>
            {
                var syncIOFeature = context.Features.Get<IHttpBodyControlFeature>();
                if (syncIOFeature != null)
                {
                    syncIOFeature.AllowSynchronousIO = true;
                }
                var db = context.RequestServices.GetRequiredService<PromptLogDbContext>();
                context.Request.EnableBuffering();
                var originalBody = context.Response.Body;
                try
                {
                    using var memStream = new MemoryStream();
                    context.Response.Body = memStream;
                    var secret = context.Request.RouteValues["secret"] as string;
                    var experiment = context.Request.RouteValues["experiment"] as string;

                    if (string.IsNullOrEmpty(experiment))
                    {
                        context.Response.StatusCode = 400;
                        return;
                    }

                    // attempt for something like basic authentication for proxy
                    if (secret != configuration.ProxySecret)
                    {
                        context.Response.StatusCode = 401;
                        return;
                    }

                    await next();
                    context.Request.Body.Seek(0, SeekOrigin.Begin);
                    using var requestReader = new StreamReader(context.Request.Body);
                    string requestBody = requestReader.ReadToEnd();

                    memStream.Position = 0;
                    using var responseReader = new StreamReader(memStream);
                    string responseBody = responseReader.ReadToEnd();

                    memStream.Position = 0;
                    memStream.CopyTo(originalBody);

                    db.Prompts.Add(new Prompt
                    {
                        Id = Guid.NewGuid(),
                        Url = context.Request.Path,
                        Experiment = experiment,
                        Created = DateTimeOffset.UtcNow,
                        Request = requestBody,
                        Response = responseBody,
                        Raiting = 0,
                    });
                    await db.SaveChangesAsync();
                }
                finally
                {
                    context.Response.Body = originalBody;
                }
            });
        }).AllowAnonymous();
    }


    private static IReadOnlyList<ClusterConfig> GetClusters(AppConfiguration configuration)
    {
        return new List<ClusterConfig> {
        new ClusterConfig
        {
            ClusterId = "api-endpoint",
            Destinations = new Dictionary<string, DestinationConfig>(StringComparer.OrdinalIgnoreCase)
            {
                { "default", new DestinationConfig { Address = configuration.OpenaiApiBase } }
            },
        }
    };
    }


    private static IReadOnlyList<RouteConfig> GetRoutes()
    {
        return new List<RouteConfig> {
        new RouteConfig
        {
            RouteId = "api-endpoint",
            ClusterId = "api-endpoint",
            Match = new RouteMatch
            {
                Path = "/proxy-endpoint/{secret}/{experiment}/{**remainder}"
            },
            Transforms = new List<IReadOnlyDictionary<string, string>>
            {
                new Dictionary<string, string>
                {
                    { "PathPattern", "{**remainder}" }
                }
            }
        }
    };
    }
}

