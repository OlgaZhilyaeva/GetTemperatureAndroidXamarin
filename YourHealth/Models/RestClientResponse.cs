using System.Collections.Generic;
using System.Net;
using System.Net.Http;

namespace YourHealth.Models
{
    public class RestClientResponse
    {
        public HttpResponseMessage HttpResponseMessage { get; set; }
        public IEnumerable<Cookie> Cookies { get; set; }
    }
}