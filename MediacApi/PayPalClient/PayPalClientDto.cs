using Microsoft.Identity.Client;

namespace MediacApi.PayPalClient
{
    public class PayPalClientDto
    {
        public string Mode { get; }

        public string ClientId { get; }

        public string Secret {  get; }

        public string BaseUrl => Mode == "Live"
            ? "https://api-m.paypal.com"
            : "https://api-m.sandbox.paypal.com";

        public PayPalClientDto(string clientId, string clientSecret, string mode)
        {
            this.ClientId = clientId;
            this.Secret = clientSecret;
            this.Mode = mode;
        }
    }
}
