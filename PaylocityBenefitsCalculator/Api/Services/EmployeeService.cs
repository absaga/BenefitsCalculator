using Api.Dtos.Employee;
using Api.Models;
using Api.Repositories;
using Api.Services.Mapping;

namespace Api.Services
{
    public class EmployeeService : IEmployeeService
    {
        /*
         * I'm creating this service to handle communication with the controller and the repository.
         * What I'm thinking is that by abstracting each layer it will make it easier to both
         * test each one independently (or expand them in the future).
         * 
         */

        private readonly IRepository<Employee> _employeeRepository;
        private readonly IDependentRepository _dependentRepository;
        private readonly IMapperService _mapperService;

        public EmployeeService(IRepository<Employee> employeeRepository, IDependentRepository dependentRepository, IMapperService mapperService)
        {
            _employeeRepository = employeeRepository;
            _dependentRepository = dependentRepository;
            _mapperService = mapperService;
        }

        public async Task<GetEmployeeDto?> GetByIdAsync(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null)
            {
                return null;
            }

            var employeeDto = _mapperService.MapEmployeeToDto(employee);

            var dependents = await _dependentRepository.GetByEmployeeId(id);
            if (dependents.Any())
            {
                employeeDto.Dependents = dependents.Select(_mapperService.MapDependentToDto).ToList();
            }

            return employeeDto;
        }
        public async Task<List<GetEmployeeDto>> GetAllAsync()
        {
            // TODO: "An employee may only have 1 spouse or domestic partner (not both)"
            // If an employee were to have more than one spouse or domestic partner, how should I handle that? Throw an error?

            var result = new List<GetEmployeeDto>();
            var employees = await _employeeRepository.GetAllAsync();

            foreach (var employee in employees)
            {
                var employeeDto = _mapperService.MapEmployeeToDto(employee);
                var dependents = await _dependentRepository.GetByEmployeeId(employee.Id);
                if (dependents.Any())
                {
                    employeeDto.Dependents = dependents.Select(_mapperService.MapDependentToDto).ToList();
                }
                result.Add(employeeDto);
            }

            return result;
        }
    }
}
