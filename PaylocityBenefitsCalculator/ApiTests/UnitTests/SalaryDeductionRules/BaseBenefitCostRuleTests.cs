using Api.Models;
using Api.Rules.SalaryDeduction;
using Xunit;

namespace ApiTests.SalaryDeductionUnitTests
{
    public class BaseBenefitCostRuleTests : RuleTest
    {
        // This one is the simplest rule since it applies to everyone, so we'll start with it

        [Fact]
        public void ShouldReturnCorrectAmount()
        {

            var rule = new BaseBenefitCostRule(MockOptions.Object);
            var employee = new Employee();

            var deduction = rule.CalculateYearlyDeduction(employee);

            Assert.Equal(SalaryDeductionSettings.BaseBenefitsCost * 12, deduction.Amount);
            Assert.Equal(PaymentFrequency.Yearly, deduction.Frequency);
        }

        [Fact]
        public void ShouldAlwaysReturnTrue()
        {
            var rule = new BaseBenefitCostRule(MockOptions.Object);
            var employee = new Employee();

            var appliestoEmployee = rule.AppliesToEmployee(employee);

            Assert.True(appliestoEmployee);
        }
    }
}
