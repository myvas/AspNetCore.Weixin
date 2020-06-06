using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Runtime.CompilerServices;

namespace Myvas.AspNetCore.Weixin
{
    public class ApiClient
    {
        public HttpClient Http { get; }
        public ApiClient(HttpClient client) => Http = client;
    }
}
