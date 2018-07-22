using System;
using System.Net;

namespace Likvido.CreditRisk.Services.WebClient
{
    public class GzipWebClient : System.Net.WebClient, IWebClient
    {
        protected override WebRequest GetWebRequest(Uri address)
        {
            HttpWebRequest request = base.GetWebRequest(address) as HttpWebRequest;
            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            return request;
        }   
    }
}
