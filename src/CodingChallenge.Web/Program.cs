using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace CodingChallenge
{
    /// <summary>
    /// Class to manage application entry/startup
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Progam entry
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Create/setup host
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
