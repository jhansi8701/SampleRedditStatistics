using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SampleRedditStatistics.Services;

var builder = CreateHostBuilder(args);

var host = builder.Build();

MainService mainService = host.Services.GetRequiredService<MainService>();
//pass subreddit name to the service
mainService.StartService("music");
Console.WriteLine("Service Running..");
Console.ReadLine();

static IHostBuilder CreateHostBuilder(string[] args)
{
    return Host.CreateDefaultBuilder(args)
        .ConfigureServices(
            (_, services) => services
                .AddSingleton<MainService, MainService>()
                .AddSingleton<IStatistics, Statistics>()
                .AddLogging(configure =>
                {
                    configure.AddConsole();
                }).AddSingleton<MainService>()
                );
}