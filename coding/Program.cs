using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace server
{
    public class Program
    {
        public static string index = "index.html";
        public static string root = Directory.GetCurrentDirectory() + "/wwwroot/";
        public static byte[] Html = System.Text.UTF8Encoding.UTF8.GetBytes(errorHtml);
        public const string errorHtml = "<html><head><title>Server Error</title><meta charset=\"UTF-8\"/><meta name=\"viewport\" content=\"width=device-width,initial-scale=1,user-scalable=0\"/></head><body onselectstart=\"return false;\" style=\"text-align:center;background:#f9f9f9;\">not found index.html</body></html>";
        public static void LoadHtml()
        {
            if (System.IO.File.Exists(root + "index.html"))
            {
                index = "index.html";
            }
            else if (System.IO.File.Exists(root + "index.htm"))
            {
                index = "index.htm";
            }
            else if (System.IO.File.Exists(root + "default.htm"))
            {
                index = "default.htm";
            }
            else if (System.IO.File.Exists(root + "default.html"))
            {
                index = "default.html";
            }
            else if (System.IO.File.Exists(root + "welcome.html"))
            {
                index = "welcome.html";
            }
            Console.WriteLine("Load By: " + root + index);

            if (System.IO.File.Exists(root + index))
            {
                Html = System.IO.File.ReadAllBytes(root + index);
            }
            else
            {
                Html = System.Text.UTF8Encoding.UTF8.GetBytes(errorHtml);
            }
        }
        public static void Main(string[] args)
        {
            var port = 80;
            if (args.Length > 0)
            {
                try
                {
                    port = Convert.ToInt32(args[0]);
                }
                catch { }
            }
            LoadHtml();
            try
            {
                IDisposable regiser = null;
                Action<object> callback = null;
                var fileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(root);
                callback = _ =>
                {
                    if (regiser != null)
                    {
                        regiser.Dispose();
                    }
                    System.Threading.Tasks.Task.Delay(100).Wait();
                    LoadHtml();
                    regiser = fileProvider.Watch(index).RegisterChangeCallback(callback, null);
                };
                regiser = fileProvider.Watch(index).RegisterChangeCallback(callback, null);
            }
            catch
            {
                Console.WriteLine("Watch file change is error!");
            }

            var host = new WebHostBuilder()
            .UseKestrel()
            .UseContentRoot(Directory.GetCurrentDirectory())
            .UseStartup<Startup>()
            .UseKestrel(o =>
            {
                o.Listen(System.Net.IPAddress.Any, port <= 0 ? 80 : port);
            })
            .Build();
            host.Run();
        }
    }
}