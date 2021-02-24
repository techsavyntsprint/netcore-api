using APICore.Common.DTO.Response.Payments;
using APICore.Services.Exceptions;
using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APICore.Services.Impls
{
    public class StripeService
    {
        private readonly IConfiguration _configuration;

        public StripeService(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <summary>
        /// Create a Stripe Payment Intent Auth
        /// </summary>
        /// <param name="amount">Amount to pay</param>
        /// <param name="stripeCustomerId">
        /// StripeCustomerId associated with the user who will make the purshase
        /// </param>
        /// <param name="currency">Currency of the transaction, default value is United State Dollar</param>
        /// <param name="setupFutureUsage">
        /// String to let stripe know how you want to use this card in the future, default value on
        /// session. If the value is not setted during the transaction the car is not stored as a
        /// payment method of the user for future usage
        /// </param>
        /// <param name="productName">
        /// Example of util information to track orders within the Stripe Dashboard
        /// </param>
        /// <param name="productId">
        /// Example of util information to track orders within the Stripe Dashboard
        /// </param>
        /// <returns>Payment Intent Client Secret</returns>
        public async Task<string> CreatePaymentIntentAsync(int amount, string stripeCustomerId, string currency = "usd", string setupFutureUsage = "on_session", string productName = "Example Product", string productId = "1")
        {
            var options = new PaymentIntentCreateOptions
            {
                Amount = amount,
                Currency = currency,
                Customer = stripeCustomerId,
                Description = "Buying product " + productName + ". {ID: " + productId + "}"
            };

            options.SetupFutureUsage = "on_session";

            var service = new PaymentIntentService();
            var paymentIntent = await service.CreateAsync(options);

            return paymentIntent.ClientSecret;
        }

        /// <summary>
        /// Get the payment methods associated to an user
        /// </summary>
        /// <param name="stripeCustomerId">
        /// StripeCustomerId associated with the user for the list of cards are requested
        /// </param>
        /// <returns>
        /// List of more important fields of the payment mehtods, more data can be added depending
        /// on the business logic and also more payments methods (This example cover credit cards)
        /// </returns>
        public async Task<IEnumerable<StripePaymentMethodResponse>> GetCustomerPaymentMethods(string stripeCustomerId)
        {
            var result = new List<StripePaymentMethodResponse>();

            if (!string.IsNullOrEmpty(stripeCustomerId))
            {
                var options = new PaymentMethodListOptions
                {
                    Customer = stripeCustomerId,
                    Type = "card"
                };

                var service = new PaymentMethodService();
                var paymentMethods = await service.ListAsync(options);

                foreach (var method in paymentMethods)
                {
                    if (method.Type == "card" && method.Card != null)
                    {
                        var card = method.Card;
                        var mapped = new StripePaymentMethodResponse
                        {
                            PaymentMethodId = method.Id,
                            Last4 = card.Last4,
                            Country = card.Country,
                            Description = card.Description,
                            ExpMonth = card.ExpMonth,
                            ExpYear = card.ExpYear,
                            Funding = card.Funding,
                            Issuer = card.Issuer
                        };

                        result.Add(mapped);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Delete a payment method associated to an user
        /// </summary>
        /// <param name="stripeCustomerId">StripeCustomerId associated to the user</param>
        /// <param name="paymentMethodId">Id of the payment method to be deleted</param>
        /// <returns></returns>
        public async Task DeletePaymentMethod(string stripeCustomerId, string paymentMethodId)
        {
            if (!string.IsNullOrEmpty(stripeCustomerId))
            {
                var service = new PaymentMethodService();
                var paymentMethod = await service.GetAsync(paymentMethodId);

                if (paymentMethod != null)
                {
                    await service.DetachAsync(paymentMethodId);
                }
            }
        }

        /// <summary>
        /// Create a customer in stripe
        /// </summary>
        /// <param name="email"></param>
        /// <param name="fullName"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="description"></param>
        /// <returns>Customer ID to be associated with the local user</returns>
        public async Task<string> CreateStripeCustomerAsync(string email, string fullName, string phoneNumber, string description)
        {
            var options = new CustomerCreateOptions
            {
                Email = email,
                Name = fullName,
                Phone = phoneNumber,
                Description = description
            };

            var service = new CustomerService();
            var customer = await service.CreateAsync(options);

            if (customer != null)
            {
                return customer.Id;
            }
            else
            {
                throw new BaseBadGatewayException();
            }
        }
    }
}
