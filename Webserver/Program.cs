using Microsoft.AspNetCore.StaticFiles;
using System;
using System.IO;
using System.Net;
using System.Web;

namespace Webserver
{
    class Program
    {
        private static int counter = 0;
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
            listener.Start();
            Console.WriteLine("Listening...");
            while (true) {
                // Note: The GetContext method blocks while waiting for a request.
                HttpListenerContext context = listener.GetContext();
                HttpListenerRequest request = context.Request;
                // Obtain a response object.
                HttpListenerResponse response = context.Response;

                // Add index.html if needed
                string location = AddIndex(request.RawUrl);

                // Construct a response.
                string filePath = Path.Combine(Directory.GetCurrentDirectory() + location);
                byte[] buffer = File.ReadAllBytes(filePath);
                counter++;
                
                response.ContentLength64 = buffer.Length;
                response.AddHeader("Expires", DateTime.Now.AddYears(1).ToString());
                response.AddHeader("StatusCode", response.StatusCode.ToString());
                response.AddHeader("Content-Type", GetContentType(filePath));

                string contentType = GetContentType(filePath);
                Console.WriteLine("Counter: " + counter + " " + contentType);

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
