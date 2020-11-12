namespace TestConsoleApp
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// The employee.
    /// </summary>
    public class Employee
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the employee age.
        /// </summary>
        [JsonPropertyName("employee_age")]
        public string EmployeeAge { get; set; }

        /// <summary>
        /// Gets or sets the employee name.
        /// </summary>
        [JsonPropertyName("employee_name")]
        public string EmployeeName { get; set; }

        /// <summary>
        /// Gets or sets the employee salary.
        /// </summary>
        [JsonPropertyName("employee_salary")]
        public string EmployeeSalary { get; set; }

        /// <summary>
        /// Gets or sets the profile image.
        /// </summary>[JsonPropertyName("profile_image")]
        public string ProfileImage { get; set; }
    }
}