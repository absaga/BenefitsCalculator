using Api.Dtos.Paycheck;

namespace Api.Services
{
    /*
     * This service will be responsible for calculating the yearly paychecks for an employee.
     * 
     */
    public interface IPaycheckCalculatorService
    {
        Task<GetEmployeePaycheckDto> GetYearlyPaychecksByIdAsync(int employeeId);
    }
}
