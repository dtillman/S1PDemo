using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orders.Services
{
    public interface IPaymentService
    {
        Task<string> ChargeCardAsync();
    }
}
