using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace HttpServer
{
    class HttpServer
    {
        static void Main(string[] args)
        {
            var listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:3080/");
            listener.Start();
            while (true)
            {
                Console.WriteLine("Waiting...");
                HttpListenerContext context = listener.GetContext();
                string requestedUrl = context.Request.Url.ToString();
                if (requestedUrl == "http://localhost:3080/")
                {
                    SendResponse(context, "../../../html/index.html");
                }
                else if (requestedUrl == "http://localhost:3080/info")
                {
                    SendResponse(context, "../../../html/info.html");
                }
                else
                {
                    SendResponse(context, "../../../html/error.html");
                }
                
            }


        }

        private static void SendResponse(HttpListenerContext context, string path)
        {
            byte[] buffer;
            using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                FileInfo info = new FileInfo(path);
                buffer = new byte[info.Length];
                stream.Read(buffer, 0, Convert.ToInt32(info.Length));
            }
            context.Response.Headers.Add("HttpResponseStatus:OK");
            context.Response.ContentLength64 = buffer.Length;
            context.Response.ContentType = "text/html";
            context.Response.OutputStream.Write(buffer, 0, buffer.Length);
            context.Response.OutputStream.Flush();
            context.Response.Close();
        }
    }
}
