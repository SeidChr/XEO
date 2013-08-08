using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace XEO.Core.Web
{
    public class ExtendedWebClient : WebClient
    {
        public CookieContainer CookieContainer { get; set; }
        public IDictionary<HttpRequestHeader, string> DefaultHeaderContainer { get; set; }
        public IDictionary<string, string> CustomHeaderContainer { get; set; }

        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest webRequest = base.GetWebRequest(address);
            var httpRequest = webRequest as HttpWebRequest;
            if (httpRequest != null)
            {
                if (CookieContainer != null)
                {
                    httpRequest.CookieContainer = CookieContainer;
                }

                if (DefaultHeaderContainer != null)
                {
                    foreach (var header in DefaultHeaderContainer)
                    {
                        switch (header.Key)
                        { 
                            case HttpRequestHeader.UserAgent:
                                httpRequest.UserAgent = header.Value;
                                break;

                            default:
                                httpRequest.Headers.Set(header.Key, header.Value);
                                break;
                        }
                    }
                }

                if (CustomHeaderContainer != null)
                {
                    foreach (var header in CustomHeaderContainer)
                    {
                        httpRequest.Headers.Set(header.Key, header.Value);
                    }
                }
            }

            return webRequest;
        }

        protected override WebResponse GetWebResponse(WebRequest request, IAsyncResult result)
        {
            WebResponse response = base.GetWebResponse(request, result);
            ReadCookies(response);
            return response;
        }

        protected override WebResponse GetWebResponse(WebRequest request)
        {
            WebResponse response = base.GetWebResponse(request);
            ReadCookies(response);
            return response;
        }

        private void ReadCookies(WebResponse r)
        {
            var response = r as HttpWebResponse;
            if (response != null)
            {
                if (CookieContainer != null)
                {
                    CookieCollection cookies = response.Cookies;
                    CookieContainer.Add(cookies);
                }
            }
        }
    }
}
