using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Dtos.Paycheck;
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

        public Employee MapDtoToEmployee(GetEmployeeDto employeeDto)
        {
            return new Employee
            {
                Id = employeeDto.Id,
                FirstName = employeeDto.FirstName,
                LastName = employeeDto.LastName,
                Salary = employeeDto.Salary,
                DateOfBirth = employeeDto.DateOfBirth,
                Dependents = employeeDto.Dependents.Select(MapDtoToDependent).ToList()
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

        public Dependent MapDtoToDependent(GetDependentDto dependentDto)
        {
            return new Dependent
            {
                Id = dependentDto.Id,
                FirstName = dependentDto.FirstName,
                LastName = dependentDto.LastName,
                DateOfBirth = dependentDto.DateOfBirth,
                Relationship = dependentDto.Relationship,
            };
        }

        public GetEmployeePaycheckDto MapEmployeePaychecksToDto(Employee employee, List<Paycheck> paychecks)
        {
            return new GetEmployeePaycheckDto
            {
                EmployeeId = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                GrossSalary = employee.Salary,
                TakeHomePay = paychecks.Sum(p => p.TakeHomePay),
                YearlyPaychecks = paychecks.Select(MapPaycheckToDto).ToList()
            };
        }

        public PaycheckDto MapPaycheckToDto(Paycheck paycheck)
        {
            return new PaycheckDto
            {
                GrossSalary = paycheck.GrossSalary,
                PaycheckDeductions = paycheck.Deductions.Select(MapDeductionToDto).ToList(),
                TakeHomePay = paycheck.TakeHomePay
            };
        }

        public PaycheckDeductionDto MapDeductionToDto(Deduction deduction)
        {
            return new PaycheckDeductionDto
            {
                Description = deduction.Description,
                Amount = deduction.Amount
            };
        }
    }
}
