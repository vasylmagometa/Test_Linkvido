using Likvido.CreditRisk.Services.WebClient;

namespace Likvido.CreditRisk.Services.Factory
{
    public class WebClientFactory : IWebClientFactory
    {
        public IWebClient GetGzipWebClient()
        {
            return new GzipWebClient();
        }
    }
}
