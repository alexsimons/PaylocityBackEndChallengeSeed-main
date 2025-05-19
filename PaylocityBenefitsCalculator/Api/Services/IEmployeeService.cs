using Api.Dtos.Employee;
using Api.Dtos.Dependent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Services
{
    public interface IEmployeeService
    {
        // I debated creating a separate service for dependents, but since they are linked and "dependent" on employees, 
        // I decided to keep them in the same service. But depending on the size of the project it might be better to separate them
        Task<List<GetEmployeeDto>> GetAllEmployeesAsync();
        Task<GetEmployeeDto?> GetEmployeeByIdAsync(int id);
        Task<GetDependentDto?> GetDependentByIdAsync(int dependentId);
        Task<List<GetDependentDto>> GetAllDependentsAsync();
        Task<PaycheckDto?> CalculatePaycheckAsync(int employeeId);
    }
}
