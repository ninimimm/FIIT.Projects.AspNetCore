using System.Security.Cryptography.X509Certificates;

namespace Api
{
    public abstract class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseKestrel(options =>
                        {
                            var certPath = Path.Combine(Directory.GetCurrentDirectory(), "certs", "fullchain.pem");
                            var keyPath = Path.Combine(Directory.GetCurrentDirectory(), "certs", "privkey.pem");
                            var certificate = X509Certificate2.CreateFromPemFile(certPath, keyPath);
                            options.ListenAnyIP(8888, listenOptions =>
                            {
                                listenOptions.UseHttps(certificate);
                            });
                        })
                        .UseStartup<Startup>();
                });
    }
}