using Api.Dtos.Employee;

namespace Api.Services
{
    public interface IEmployeeService
    {
        /*
         * This service will be used to handle the business logic for dependents.
         * This should communicate between the controller and the repository.
         * 
         */

        Task<GetEmployeeDto?> GetByIdAsync(int id);
        Task<List<GetEmployeeDto>> GetAllAsync();
    }
}
