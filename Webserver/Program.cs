using System;
using System.IO;
using System.Net;

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
                // Construct a response.
                byte[] buffer = File.ReadAllBytes(Path.Combine(Directory.GetCurrentDirectory() + request.RawUrl));

                response.ContentLength64 = buffer.Length;
                System.IO.Stream stream = response.OutputStream;
                stream.Write(buffer, 0, buffer.Length);
                stream.Close();

                //listener.Stop();
            }
        }
    }
}
