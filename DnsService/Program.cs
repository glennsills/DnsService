using DnsService;


IHost host = Host.CreateDefaultBuilder(args)
    .UseSystemd()
    .ConfigureServices(services =>
    {
        services.AddOptions();
        services.AddOptions<DnsServerOptions>().BindConfiguration("DnsServerOptions");
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
