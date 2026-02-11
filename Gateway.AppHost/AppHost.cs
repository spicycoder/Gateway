using CommunityToolkit.Aspire.Hosting.Dapr;

var builder = DistributedApplication.CreateBuilder(args);

var redisPassword = builder.AddParameter("RedisPassword", true);
var redisPort = builder.AddParameter("RedisPort");
var redisPortValue = await redisPort.Resource.GetValueAsync(CancellationToken.None);

var redis = builder
    .AddRedis(
        "redis",
        int.Parse(redisPortValue!),
        redisPassword)
    .WithLifetime(ContainerLifetime.Persistent)
    .WithRedisInsight();

var servicea = builder
    .AddProject<Projects.ServiceA>("servicea")
    .WithReplicas(2)
    .WaitFor(redis)
    .WithDaprSidecar(new DaprSidecarOptions
    {
        ResourcesPaths = [Path.Combine("..", "components")]
    });

var serviceb = builder
    .AddProject<Projects.ServiceB>("serviceb")
    .WithReplicas(3)
    .WaitFor(redis)
    .WithDaprSidecar(new DaprSidecarOptions
    {
        ResourcesPaths = [Path.Combine("..", "components")]
    });

builder.Build().Run();
