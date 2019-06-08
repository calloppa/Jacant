using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Jacant
{
    class HttpHelper
    {
            private readonly string DefaultUserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";

            public HttpWebResponse HttpGetRequest(string url, string referer, int? timeout, CookieCollection cookies)
            {
                HttpWebRequest request = null;

                if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                {
                    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                    request = WebRequest.Create(url) as HttpWebRequest;
                    request.ProtocolVersion = HttpVersion.Version10;
                }
                else
                    request = WebRequest.Create(url) as HttpWebRequest;

                request.Method = "GET";
                request.ContentType = "application/x-www-form-urlencoded";
                request.UserAgent = DefaultUserAgent;
                request.CookieContainer = new System.Net.CookieContainer();

                if (!string.IsNullOrEmpty(referer))
                    request.Referer = referer;

                if (timeout.HasValue)
                    request.Timeout = timeout.Value;
                else
                    request.Timeout = 25000;

                if (cookies != null)
                {
                    request.CookieContainer = new CookieContainer();
                    request.CookieContainer.Add(cookies);
                }

                return request.GetResponse() as HttpWebResponse;
            }

            private bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
            {
                return true;
            }
        }     
 }