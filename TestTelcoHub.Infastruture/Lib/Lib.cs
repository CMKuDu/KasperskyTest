using System.Net;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;

namespace TestTelcoHub.Infastruture.Lib
{
    public interface IHttpClientHelper
    {
        Task<HttpResponseMessage> PostDataAsync(HttpClient client, Uri endpoint, StringContent content);
        HttpClient CreateHttpClient();
        StringContent CreateJsonContent(object data);
        Task<HttpResponseMessage> GetDataAsync(HttpClient client, Uri endpoint);
    }
    public class HttpClientHelper : IHttpClientHelper
    {
        public async Task<HttpResponseMessage> PostDataAsync(HttpClient client, Uri endpoint, StringContent content)
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            return await client.PostAsync(endpoint, content);
        }
        public HttpClient CreateHttpClient()
        {
            var client = new HttpClient(GetHttpClientHandler());
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("Cookie", "NSC_TMAS=58741d2bb4e574acbea92392ec94471d");
            return client;
        }
        private HttpClientHandler GetHttpClientHandler()
        {
            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            X509Certificate2 cert = new X509Certificate2("D:\\ProjectVTC\\DocsVTC\\API_KES_Onesme\\New folder\\41a6aabd427d43babb8229bf4d64bb6b50367847de834bfeba7968136641554a.pfx", "@@Vtc123");
            handler.ClientCertificates.Add(cert);
            return handler;
        }
        public StringContent CreateJsonContent(object data)
        {
            var json = JsonSerializer.Serialize(data);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }
        public async Task<HttpResponseMessage> GetDataAsync(HttpClient client, Uri endpoint)
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            return await client.GetAsync(endpoint);
        }
    }
}
