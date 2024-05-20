using Api.Models;
using Api.Rules.SalaryDeduction;

namespace Api.Services
{
    /*
     * This service will handle obtaining all deductions for the employee on a yearly basis
     * by running all the rules and calculating the appropriate deductions.
     *
     */
    public class SalaryDeductionCalculatorService : ISalaryDeductionCalculatorService
    {
        private readonly List<ISalaryDeductionRule> _salaryDeductionRules;
        public SalaryDeductionCalculatorService(List<ISalaryDeductionRule> salaryDeductionRules)
        {
            _salaryDeductionRules = salaryDeductionRules;
        }
        public async Task<List<Deduction>> CalculateYearlyDeduction(Employee employee)
        {
            var result = new List<Deduction>();

            foreach (var rule in _salaryDeductionRules)
            {
                if (rule.AppliesToEmployee(employee))
                {
                    result.Add(rule.CalculateYearlyDeduction(employee));
                }
            }
            return result;
        }
    }
}
