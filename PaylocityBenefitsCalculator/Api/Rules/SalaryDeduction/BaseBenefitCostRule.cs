using Api.Configuration;
using Api.Models;
using Microsoft.Extensions.Options;

namespace Api.Rules.SalaryDeduction
{
    public class BaseBenefitCostRule : ISalaryDeductionRule
    {
        private readonly decimal _baseBenefitsCost;
        public BaseBenefitCostRule(IOptions<SalaryDeductionSettings> salaryDeductionSettings)
        {
            _baseBenefitsCost = salaryDeductionSettings.Value.BaseBenefitsCost;
        }
        public bool AppliesToEmployee(Employee employee)
        {
            // "Employees have a base cost of $1,000 per month (for benefits)"
            // This one always applies.
            return true;
        }

        public Deduction CalculateYearlyDeduction(Employee employee)
        {
            return new Deduction
            {
                Description = "Base Benefits Cost",
                Amount = _baseBenefitsCost * 12,
                Frequency = PaymentFrequency.Yearly
            };
        }

    }
}
