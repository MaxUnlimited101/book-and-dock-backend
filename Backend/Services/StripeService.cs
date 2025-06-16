namespace Backend.Services
{
    using Stripe;
    using Stripe.Checkout;

    public class StripeService
    {
        public async Task<string> CreateCheckoutSessionAsync(decimal amount, string currency, string successUrl, string cancelUrl)
        {
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(amount * 100), // Stripe expects amount in cents
                        Currency = currency,
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = "Payment for service",
                        },
                    },
                    Quantity = 1,
                }
            },
                Mode = "payment",
                SuccessUrl = successUrl,
                CancelUrl = cancelUrl,
            };

            var service = new SessionService();
            Session session = await service.CreateAsync(options);
            return session.Url;
        }
    }

}
