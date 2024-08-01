using Api.Configuration;
using Api.Models;
using Microsoft.Extensions.Options;

namespace Api.Rules.SalaryDeduction
{
    public class OverFiftyDependentFeeRule : ISalaryDeductionRule
    {
        private readonly decimal _overFiftyDependentFee;
        public OverFiftyDependentFeeRule(IOptions<SalaryDeductionSettings> salaryDeductionSettings)
        {
            _overFiftyDependentFee = salaryDeductionSettings.Value.OverFiftyDependentFee;
        }
        public bool AppliesToEmployee(Employee employee)
        {
            return employee.Dependents.Any(d => (DateTime.Now.Year - d.DateOfBirth.Year) > 50);
        }

        public Deduction CalculateYearlyDeduction(Employee employee)
        {
            var amountOverFifty = employee.Dependents.Count(d => (DateTime.Now.Year - d.DateOfBirth.Year) > 50);

            return new Deduction
            {
                Description = "Over 50 Dependent Fee",
                Amount = (_overFiftyDependentFee * amountOverFifty) * 12,
                Frequency = PaymentFrequency.Yearly
            };
        }
    }
}
