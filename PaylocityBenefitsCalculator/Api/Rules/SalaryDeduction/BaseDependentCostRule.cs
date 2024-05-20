using Api.Configuration;
using Api.Models;
using Microsoft.Extensions.Options;

namespace Api.Rules.SalaryDeduction
{
    public class BaseDependentCostRule : ISalaryDeductionRule
    {
        private readonly decimal _baseDependentCost;
        public BaseDependentCostRule(IOptions<SalaryDeductionSettings> salaryDeductionSettings)
        {
            _baseDependentCost = salaryDeductionSettings.Value.BaseDependentCost;
        }
        public bool AppliesToEmployee(Employee employee)
        {
            // "each dependent represents an additional $600 cost per month (for benefits) 
            return employee.Dependents.Any();
        }

        public Deduction CalculateYearlyDeduction(Employee employee)
        {
            return new Deduction
            {
                Description = "Base Dependent Fee",
                Amount = (_baseDependentCost * employee.Dependents.Count) * 12,
                Frequency = PaymentFrequency.Yearly
            };
        }
    }
}
