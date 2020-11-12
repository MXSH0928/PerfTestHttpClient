namespace TestConsoleApp
{
    /// <summary>
    /// The API response.
    /// </summary>
    public class ApiResponse
    {
        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        public Employee[] Data { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        public string Status { get; set; }
    }
}