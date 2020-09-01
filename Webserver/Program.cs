using System;
using System.IO;
using System.Net;

namespace Webserver
{
    class Program
    {
        static void Main(string[] args)
        {
            SimpleHTTPListener(new string[] { "http://localhost:8080/" });
        }

        public static void SimpleHTTPListener(string[] prefixes)
        {
            if (!HttpListener.IsSupported)
            {
                Console.WriteLine("Windows XP SP2 or Server 2003 is required to use the HttpListener class.");
                return;
            }
            // URI prefixes are required,
            // for example "http://contoso.com:8080/index/".
            if (prefixes == null || prefixes.Length == 0)
                throw new ArgumentException("prefixes");

            // Create a listener.
            HttpListener listener = new HttpListener();

            // Add the prefixes.
            foreach (string s in prefixes)
            {
                listener.Prefixes.Add(s);
            }

            // Start the listener
            listener.Start();
            Console.WriteLine("Listening...");

            while (true)
            {
                // Note: The GetContext method blocks while waiting for a request.
                HttpListenerContext context = listener.GetContext();
                HttpListenerRequest request = context.Request;

                // Create location string and check if we need to add Index.html
                string location = AddIndex(request.RawUrl);
                
                if (!File.Exists(Path.Combine(Directory.GetCurrentDirectory() + location)))
                    location = GetFullAdress(request);

                // Exit if it's a request for a file that doesn't exists
                if (File.Exists(Path.Combine(Directory.GetCurrentDirectory() + location)) || location == "/counter")
                {
                    // Obtain a response object.
                    HttpListenerResponse response = context.Response;

                    // Create a new cookie if there isn't one or add +1 to the counter
                    Cookie counterCookie = request.Cookies["Counter"];
                    if (counterCookie == null)
                        counterCookie = new Cookie("Counter", "1");
                    else
                        counterCookie.Value = (int.Parse(counterCookie.Value) + 1).ToString();

                    // Correct path to make it count on the same counter on all pages
                    counterCookie.Path = "/";
                    // Add the cookie to the response
                    response.Cookies.Add(counterCookie);

                    // Add a response code
                    response.StatusCode = (int)HttpStatusCode.OK;

                    // Construct a response. Depending on endpoint
                    byte [] buffer;
                    if (location == "/counter")
                    {
                        string responseString = $"<html><body><p style=font-size:42px;text-align:center>{counterCookie.Value}</br></br><a href=http://{request.Url.Authority}/content/>Home</a></p></body></html>";
                        buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
                    }
                    else
                        buffer = File.ReadAllBytes(Path.Combine(Directory.GetCurrentDirectory() + location));

                    response.ContentLength64 = buffer.Length;
                    Stream stream = response.OutputStream;
                    stream.Write(buffer, 0, buffer.Length);
                    stream.Close();
                }
            }

            //listener.Stop();
        }
        
        public static string GetFullAdress(HttpListenerRequest request)
        {
            // Check if there's no referrer
            if (request.UrlReferrer == null)
                return request.RawUrl;

            string location = request.UrlReferrer.ToString();
            
            location = location.Substring(location.IndexOf(request.Url.Authority) + request.Url.Authority.Length)
                               .Replace("index.html", "")
                               + request.Url.AbsolutePath;

            return AddIndex(location);
        }

        public static string AddIndex(string rawUrl)
        {
            if (rawUrl.EndsWith('/'))
                return rawUrl + "index.html";
            else
                return rawUrl;
        }
    }
}
