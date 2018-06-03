using System;
using Xunit;
using YourHealth.Models;

namespace XUnitTestProject
{
    public class RestHttpClientTests
    {
        private RestHttpClient _restHttpClient = new RestHttpClient();

        [Fact]
        public void TestGetRequest()
        {
            var response = _restHttpClient.GetRequestRaw("http://google.com");
        }
    }
}
