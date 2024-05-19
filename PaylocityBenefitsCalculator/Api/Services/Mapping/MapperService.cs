using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Models;

namespace Api.Services.Mapping
{
    public class MapperService : IMapperService
    {
        public GetEmployeeDto MapEmployeeToDto(Employee employee)
        {
            return new GetEmployeeDto
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Salary = employee.Salary,
                DateOfBirth = employee.DateOfBirth,
            };
        }

        public GetDependentDto MapDependentToDto(Dependent dependent)
        {
            return new GetDependentDto
            {
                Id = dependent.Id,
                FirstName = dependent.FirstName,
                LastName = dependent.LastName,
                DateOfBirth = dependent.DateOfBirth,
                Relationship = dependent.Relationship,
            };
        }
    }
}
