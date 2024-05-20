using Api.Models;

namespace Api.Services
{
    public interface ISalaryDeductionCalculatorService
    {
        /*
         * I'm thinking of creating this service to calculate the deductions for the employee on a yearly basis.
         * 
         */

        List<Deduction> CalculateYearlyDeduction(Employee employee);
    }
}
