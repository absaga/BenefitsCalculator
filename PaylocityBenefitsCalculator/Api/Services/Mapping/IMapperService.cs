using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Dtos.Paycheck;
using Api.Models;

namespace Api.Services.Mapping
{
    public interface IMapperService
    {
        /*
         * I'm implement a basic mapping service as I'm not too happy with doing this logic in the services.
         * The more I expand this the more I think I should have used a library for this .___.
         * 
         */
        GetEmployeeDto MapEmployeeToDto(Employee employee);
        Employee MapDtoToEmployee(GetEmployeeDto employee);
        GetDependentDto MapDependentToDto(Dependent dependent);
        Dependent MapDtoToDependent(GetDependentDto dependentDto);
        GetEmployeePaycheckDto MapEmployeePaychecksToDto(Employee employee, List<Paycheck> paychecks);
        PaycheckDto MapPaycheckToDto(Paycheck paycheck);
        PaycheckDeductionDto MapDeductionToDto(Deduction deduction);
    }
}
