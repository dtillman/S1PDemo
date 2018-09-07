using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Orders.Services;

namespace Orders.Controllers
{
    [Route("/process-order")]
    public class OrdersController : Controller
    {
        private ILogger<OrdersController> Logger { get; }
        private IPaymentService PaymentService { get; }
        private static Random RandomNumber { get; } = new Random();
        public OrdersController(IPaymentService payment, ILogger<OrdersController> logger)
        {
            Logger = logger;
            PaymentService = payment;
        }

        // GET /process-order
        [HttpGet]
        public async Task<IActionResult> ProcessOrder()
        {
            Logger.LogInformation("/process-order called");
            Thread.Sleep(1000 * RandomNumber.Next(3));
            return Ok(await PaymentService.ChargeCardAsync());
        }
    }
}
