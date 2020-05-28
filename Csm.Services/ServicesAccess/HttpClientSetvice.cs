using Csm.Services.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Csm.Services.ServicesAccess
{
    public class HttpClientSetvice : IHttpClientSetvice
    {
        private readonly HttpClient httpClient;

        public HttpClientSetvice(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public void GetImage()
        {
            throw new NotImplementedException();
        }
       
    }
}
