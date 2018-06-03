using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;

namespace YourHealth.Models
{
    public class RestHttpClient
    {
        private readonly HttpClient _client;
        private readonly CookieContainer _cookies = new CookieContainer();
        private readonly HttpClientHandler _handler = new HttpClientHandler();

        private static readonly Lazy<RestHttpClient> Inst = new Lazy<RestHttpClient>(() => new RestHttpClient());
        public static RestHttpClient I = Inst.Value;

        public RestHttpClient()
        {
            _handler.CookieContainer = _cookies;

            _client = new HttpClient(_handler);
        }

        public RestClientResponse PostRequestRaw(string url, string data)
        {
            return new RestClientResponse()
            {
                HttpResponseMessage = _client.PostAsync(url, new StringContent(data, Encoding.UTF8, "application/json")).Result,
                Cookies = _cookies.GetCookies(new Uri(url)).Cast<Cookie>()
            };
        }

        public RestClientResponse GetRequestRaw(string url)
        {
            return new RestClientResponse()
            {
                HttpResponseMessage = _client.GetAsync(url).Result,
                Cookies = _cookies.GetCookies(new Uri(url)).Cast<Cookie>()
            };
        }
    }
}