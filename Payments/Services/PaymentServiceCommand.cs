using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Steeltoe.CircuitBreaker.Hystrix;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Orders.Services
{
    public class PaymentServiceCommand : HystrixCommand<string>, IPaymentService
    {
       
        ILogger<PaymentServiceCommand> _logger;
        IOptions<AppConfiguration> _config;

        public PaymentServiceCommand(IHystrixCommandOptions options, IOptions<AppConfiguration> config, ILogger<PaymentServiceCommand> logger) : base(options)
        {
            _logger = logger;
            _config = config;
            IsFallbackUserDefined = true;
        }
        public async Task<string> ChargeCardAsync()
        {
            return await ExecuteAsync();
        }
        protected override async Task<string> RunAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                var uri = _config.Value.PaymentsUri + "/charge-card";
                _logger.LogInformation("sending request to " + uri);
                try
                {
                    var response = await client.GetStringAsync(uri);
                    _logger.LogInformation("finished request to " + uri);
                    return response;
                }
                catch (Exception e)
                {
                    _logger.LogError("charge card failed", e);
                    throw;
                }
            }
        }

        protected override async Task<string> RunFallbackAsync()
        {
            _logger.LogInformation("RunFallback");
            return await Task.FromResult<string>("Unable to charge card");
        }
    }
}
