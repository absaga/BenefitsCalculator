using Api.Models;

namespace Api.Rules.SalaryDeduction
{

    /*
     * I have some mixed feelings about this one, but I'm thinking of creating this interface to
     * define the shape of the salary deduction rules. I'm thinking by creating each rule here and registering them
     * separately, I can make them  more modular and easier to test.
     * 
     */
    public interface ISalaryDeductionRule
    {
        bool AppliesToEmployee(Employee employee);
        Deduction CalculateYearlyDeduction(Employee employee);
    }
}
