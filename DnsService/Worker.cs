using DNS.Client;
using DNS.Server;
using System.Net;

namespace DnsService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly DnsServerOptions _options;

        public Worker(ILogger<Worker> logger, Microsoft.Extensions.Options.IOptions<DnsServerOptions> options)
        {
            _logger = logger;
            _options = options.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var server = new DnsServer(new SampleRequestResolver());
            server.Listening += (_, _) => _logger.LogInformation("DNS server is listening...");
            server.Requested += (_, e) => _logger.LogInformation("Request: {Domain}", e.Request.Questions.First().Name);
            server.Errored += (_, e) =>
            {
                _logger.LogError(e.Exception, "An error occurred");
                if (e.Exception is ResponseException responseError)
                {
                    _logger.LogError("Response: {Response}", responseError.Response);
                }
            };

            await Task.WhenAny(new[]
            {
                server.Listen(port: _options.DnsPort, ip: IPAddress.Any),
                Task.Delay(-1, stoppingToken)
            });
        }
    }
}