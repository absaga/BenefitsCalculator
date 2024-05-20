using Api.Configuration;
using Api.Models;
using Microsoft.Extensions.Options;

namespace Api.Rules.SalaryDeduction
{
    public class HighSalaryFeeRule : ISalaryDeductionRule
    {
        private readonly decimal _highSalaryThreshold;
        private readonly decimal _highSalaryDeductionPercent;

        public HighSalaryFeeRule(IOptions<SalaryDeductionSettings> salaryDeductionSettings)
        {
            _highSalaryThreshold = salaryDeductionSettings.Value.HighSalaryThreshold;
            _highSalaryDeductionPercent = salaryDeductionSettings.Value.HighSalaryDeductionPercent;
        }
        public bool AppliesToEmployee(Employee employee)
        {
            return employee.Salary > _highSalaryThreshold;

        }

        public Deduction CalculateYearlyDeduction(Employee employee)
        {
            // "employees that make more than $80,000 per year will incur an additional 2% of their yearly salary in benefits costs"
            // It does say "2% of their yearly salary" and not "2% of the amount over $80,000" >_>

            return new Deduction
            {
                Description = "Rich Fee",
                Amount = employee.Salary * _highSalaryDeductionPercent,
                Frequency = PaymentFrequency.Yearly
            };
        }
    }
}
