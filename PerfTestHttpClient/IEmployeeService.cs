namespace TestConsoleApp
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// The EmployeeService interface.
    /// </summary>
    internal interface IEmployeeService
    {
        /// <summary>
        /// The get employees async.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<IList<Employee>> GetEmployeesAsync();

        /// <summary>
        /// The display employees.
        /// </summary>
        /// <param name="employees">
        /// The employees.
        /// </param>
        void DisplayEmployees(IList<Employee> employees);
    }
}