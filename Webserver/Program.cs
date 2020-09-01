using Microsoft.AspNetCore.StaticFiles;
using System;
using System.IO;
using System.Net;
using System.Web;

namespace Webserver
{
    class Program
    {
        static void Main(string[] args)
        {
            SimpleListenerExample(new string[] { "http://localhost:8080/" });
        }

        public static void SimpleListenerExample(string[] prefixes)
        {
            if (!HttpListener.IsSupported)
            {
                Console.WriteLine("Windows XP SP2 or Server 2003 is required to use the HttpListener class.");
                return;
            }
            // URI prefixes are required,
            if (prefixes == null || prefixes.Length == 0)
                throw new ArgumentException("prefixes");

            // Create a listener.
            HttpListener listener = new HttpListener();
            // Add the prefixes.
            foreach (string s in prefixes)
            {
                listener.Prefixes.Add(s);
            }
            listener.Start();
            Console.WriteLine("Listening...");
            while (true)
            {
                // Note: The GetContext method blocks while waiting for a request.
                HttpListenerContext context = listener.GetContext();
                HttpListenerRequest request = context.Request;

                // Obtain a response object.
                HttpListenerResponse response = context.Response;

                // Add index.html if needed
                string location = AddIndex(request.RawUrl);

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
                request.Cookies.Add(counterCookie);

                // Construct a response.
                byte [] buffer;
                string filePath = Path.Combine(Directory.GetCurrentDirectory() + location);
                if (location == "/counter.html")
                {
                    string responseString = $"<html><body><p style=font-size:42px;text-align:center>{counterCookie.Value}</br></br><a href=http://{request.Url.Authority}/index.html>Home</a></p></body></html>";
                    buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
                }
                else if (File.Exists(filePath))
                {
                    buffer = File.ReadAllBytes(filePath);
                }
                else
                {
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    continue;
                }

                response.ContentLength64 = buffer.Length;
                response.AddHeader("Expires", DateTime.Now.AddYears(1).ToString());
                response.AddHeader("StatusCode", response.StatusCode.ToString());
                response.AddHeader("Content-Type", GetContentType(filePath));

                string contentType = GetContentType(filePath);

                Stream stream = response.OutputStream;
                stream.Write(buffer, 0, buffer.Length);
                stream.Close();
                
                //listener.Stop();
            }
        }

        private static string GetContentType(string path)
        {
            string contentType;
            new FileExtensionContentTypeProvider().TryGetContentType(path, out contentType);
            return contentType ?? "application/octet-stream";
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
