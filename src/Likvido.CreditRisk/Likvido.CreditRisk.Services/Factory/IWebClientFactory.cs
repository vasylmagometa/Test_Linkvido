using Likvido.CreditRisk.Services.WebClient;

namespace Likvido.CreditRisk.Services.Factory
{
    public interface IWebClientFactory
    {
        IWebClient GetGzipWebClient();
    }
}
