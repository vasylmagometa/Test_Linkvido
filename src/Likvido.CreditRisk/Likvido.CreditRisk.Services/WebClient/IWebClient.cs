using System;
using System.Text;
using System.Threading.Tasks;

namespace Likvido.CreditRisk.Services.WebClient
{
    public interface IWebClient : IDisposable
    {
        Encoding Encoding { get; set; }

        Task<string> DownloadStringTaskAsync(string address);
    }
}
