using APICore.Common.DTO.Request;
using APICore.Common.DTO.Response.Payments;
using APICore.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APICore.Services
{
    public interface IStripeService
    {
        Task<string> CreatePaymentIntentAsync(int amount, string stripeCustomerId, string currency = "usd", string setupFutureUsage = "on_session", string productName = "Example Product", string productId = "1");

        Task<IEnumerable<StripePaymentMethodResponse>> GetCustomerPaymentMethods(string stripeCustomerId);

        Task DeletePaymentMethod(string stripeCustomerId, string paymentMethodId);

        Task<string> CreateStripeCustomerAsync(string email, string fullName, string phoneNumber, string description);
    }
}
