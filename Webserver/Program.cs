using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Net.Http.Headers;
using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;

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
            ProcessHttpListener(listener);
        }

        private static void ProcessHttpListener(HttpListener listener)
        {
            while (true)
            {
                // Note: The GetContext method blocks while waiting for a request.
                HttpListenerContext context = listener.GetContext();
                HttpListenerRequest request = context.Request;
                // Obtain a response object.
                HttpListenerResponse response = context.Response;
                // Construct a response.
                string filePath = Path.Combine(Directory.GetCurrentDirectory() + request.RawUrl);
                byte[] buffer;
                Stream stream = response.OutputStream;

                counter++;
                response.Cookies.Add(new Cookie("counter", counter.ToString()));
                if (request.RawUrl == "/counter")
                {
                    buffer = Encoding.ASCII.GetBytes(response.Cookies["counter"].Value);
                }
                else
                {
                    buffer = File.ReadAllBytes(filePath);
                    response.AddHeader("Content-Type", GetContentType(filePath));
                    response.AddHeader("ETag", "\"" + CalculateMD5Hash(filePath) + "\"");
                }

                response.ContentLength64 = buffer.Length;
                response.AddHeader("Expires", DateTime.Now.AddYears(1).ToString());
                response.AddHeader("StatusCode", response.StatusCode.ToString());
                stream.Write(buffer, 0, buffer.Length);
                stream.Close();
            }

            //listener.Stop();
        }

        private static string GetContentType(string path)
        {
            string contentType;
            new FileExtensionContentTypeProvider().TryGetContentType(path, out contentType);
            return contentType ?? "application/octet-stream";
        }

        static string CalculateMD5Hash(string filePath)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filePath))
                {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }
    }
}