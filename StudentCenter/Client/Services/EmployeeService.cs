using StudentCenter.Shared.Models;
using System.Net.Http.Json;

namespace StudentCenter.Client.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://localhost:7026//api/Employees"; // Replace with your API base URL

        public EmployeeService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        // Get all employees
        public async Task<IEnumerable<Employee>> GetEmployeesAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<Employee[]>(BaseUrl);
            return response;
        }

        // Get a specific employee by ID
        public async Task<Employee> GetEmployeeByIdAsync(int id)
        {
            var response = await _httpClient.GetFromJsonAsync<Employee>($"{BaseUrl}/{id}");
            return response;
        }

        // Create a new employee
        public async Task<Employee> CreateEmployeeAsync(Employee employee)
        {
            var response = await _httpClient.PostAsJsonAsync(BaseUrl, employee);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<Employee>();
        }

        // Update an existing employee
        public async Task UpdateEmployeeAsync(Employee employee)
        {
            var response = await _httpClient.PutAsJsonAsync($"{BaseUrl}/{employee.EmployeeId}", employee);
            response.EnsureSuccessStatusCode();
        }

        // Delete an employee by ID
        public async Task DeleteEmployeeAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{BaseUrl}/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
