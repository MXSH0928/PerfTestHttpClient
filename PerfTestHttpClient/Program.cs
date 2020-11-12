namespace TestConsoleApp
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Net.Http;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// The program.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The base url.
        /// </summary>
        private const string BaseAddress = "http://dummy.restapiexample.com/";

        /// <summary>
        /// The path.
        /// </summary>
        private const string Path = "/api/v1/employees";

        /// <summary>
        /// The services.
        /// </summary>
        private static readonly ServiceProvider Provider;

        /// <summary>
        /// The Program logger.
        /// </summary>
        private static readonly ILogger<Program> Logger;

        /// <summary>
        /// The configuration.
        /// </summary>
        private static IConfiguration configuration;

        /// <summary>
        /// Initializes static members of the <see cref="Program"/> class.
        /// </summary>
        static Program()
        {
            var serviceCollection = new ServiceCollection();
            string[] arguments = Environment.GetCommandLineArgs();
            Configure(serviceCollection, arguments);

            Provider = serviceCollection.BuildServiceProvider();
            Logger = Provider.GetRequiredService<ILogger<Program>>();
            Logger.LogInformation($"{nameof(Program)} class has been instantiated.");
        }

        /// <summary>
        /// The main.
        /// </summary>
        /// <param name="args">
        /// The args.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public static async Task Main(string[] args)
        {
            var watch = new Stopwatch();
            watch.Start();

            var clientFactory = Provider.GetRequiredService<IHttpClientFactory>();
            var client = clientFactory.CreateClient("demoHttpClient");

            var employeeService = Provider.GetRequiredService<IEmployeeService>();
            var employees = await employeeService.GetEmployeesAsync();
            employeeService.DisplayEmployees(employees);

            for (int i = 0; i <= 10; i++)
            {
                // TEST #1: Instantiate HttpClient inside
                // await MakeHttpCall1(i);

                // TEST #2: Get HttpClient using HttpClientFactory from service provider
                await MakeHttpCall2(i);
            }

            watch.Stop();
            Console.WriteLine("\n\nElapsed: {0} ms.\n\n", watch.ElapsedMilliseconds);
        }

        /// <summary>
        /// The make HTTP call 1.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        private static async Task MakeHttpCall1(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseAddress);

                /* var builder = new UriBuilder
                                  {
                                      Scheme = Uri.UriSchemeHttps,
                                      // Host = "google.com"
                                      // Path = "app"
                                  };

                var query = HttpUtility.ParseQueryString(builder.Query);
                query["name"] = "mohammed";
                query["email"] = "test@email.com";
                builder.Query = query.ToString() ?? string.Empty;
                var url = builder.ToString(); */

                var response = await client.GetAsync(Path);

                if (!response.IsSuccessStatusCode)
                {
                    Logger.LogInformation("Call {id} failed. Http Status Code: {statusCode}", id, response.StatusCode);
                }
                else
                {
                    Logger.LogInformation("Call {id} succeed.", id);
                }
            }
        }

        /// <summary>
        /// The make HTTP call.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        private static async Task MakeHttpCall2(int id)
        {
            var clientFactory = Provider.GetRequiredService<IHttpClientFactory>();
            var client = clientFactory.CreateClient("demoHttpClient");

            // var client = clientFactory.CreateClient("demoHttpClient");
            using (Logger.BeginScope($"Scope: Call Id {id} "))
            {
                var response = await client.GetAsync(Path);

                if (!response.IsSuccessStatusCode)
                {
                    Logger.LogInformation("Call {id} failed. Http Status Code: {statusCode}", id, response.StatusCode);
                }
                else
                {
                    Logger.LogInformation("Call {id} succeed.", id);
                }
            }
        }

        /// <summary>
        /// The configure.
        /// </summary>
        /// <param name="services">
        /// The services.
        /// </param>
        /// <param name="args">
        /// The args.
        /// </param>
        private static void Configure(IServiceCollection services, string[] args)
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            IConfigurationBuilder configBuilder = new ConfigurationBuilder()
                /* .SetBasePath(Directory.GetCurrentDirectory()) */
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                /*.AddJsonFile($"appsettings.{environmentName}.json", true, true) */
                .AddEnvironmentVariables();

            var config = configBuilder.Build();

            services.AddLogging(
                builder =>
                    {
                        /* builder.ClearProviders(); */
                        /* builder.SetMinimumLevel(LogLevel.Trace); */
                        builder.AddConfiguration(config.GetSection("Logging"));
                        builder.AddConsole();
                    });

            services.AddHttpClient(
                "demoHttpClient",
                c =>
                    {
                        c.BaseAddress = new Uri(BaseAddress);
                    });

            services.AddScoped<IEmployeeService, EmployeeService>();
        }
    }
}
