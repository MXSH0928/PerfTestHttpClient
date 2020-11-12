namespace TestConsoleApp
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Logging;

    /// <summary>
    /// The employee service.
    /// https://docs.microsoft.com/en-us/aspnet/web-api/overview/advanced/calling-a-web-api-from-a-net-client
    /// </summary>
    internal class EmployeeService : IEmployeeService
    {
        /// <summary>
        /// The path.
        /// </summary>
        private const string Path = "/api/v1/employees";

        /// <summary>
        /// The client.
        /// </summary>
        private readonly HttpClient client;

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeeService"/> class.
        /// </summary>
        /// <param name="clientFactory">
        /// The client factory.
        /// </param>
        /// <param name="logger">
        /// The logger.
        /// </param>
        public EmployeeService(IHttpClientFactory clientFactory, ILogger<EmployeeService> logger)
        {
            this.client = clientFactory.CreateClient("demoHttpClient");
            this.logger = logger;
        }

        /// <summary>
        /// The get employees async.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<IList<Employee>> GetEmployeesAsync()
        {
            HttpResponseMessage responseMessage = await this.client.GetAsync(Path);
            responseMessage.EnsureSuccessStatusCode();

            var jsonString = await responseMessage.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var apiResponse = JsonSerializer.Deserialize<ApiResponse>(jsonString, options);
            var employees = apiResponse.Data;

            return employees;
        }

        /// <summary>
        /// The display employees.
        /// </summary>
        /// <param name="employees">
        /// The employees.
        /// </param>
        public void DisplayEmployees(IList<Employee> employees)
        {
            foreach (var employee in employees)
            {
                this.logger.LogInformation("Employee {employee}", employee);
            }
        }
    }
}