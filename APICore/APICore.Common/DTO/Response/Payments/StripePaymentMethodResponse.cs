namespace APICore.Common.DTO.Response.Payments
{
    public class StripePaymentMethodResponse
    {
        public string PaymentMethodId { get; set; }
        public string Country { get; set; }
        public string Description { get; set; }
        public long ExpMonth { get; set; }
        public long ExpYear { get; set; }

        // Summary: Card funding type. Can be credit, debit, prepaid, or unknown.
        public string Funding { get; set; }

        // Summary: Issuer bank name of the card. (Only for internal use only and not typically
        // available in standard API requests.).
        public string Issuer { get; set; }

        // Summary: The last four digits of the card.
        public string Last4 { get; set; }
    }
}
