using Api.Configuration;
using Api.Models;
using Api.Rules.SalaryDeduction;
using System;
using Xunit;

namespace ApiTests.SalaryDeductionUnitTests
{
    public class HighSalaryDeductionTest : RuleTest
    {
        public decimal GenerateRandomDecimal(bool isAboveThreshold)
        {
            decimal randomDecimal;

            if (isAboveThreshold)
            {
                randomDecimal = SalaryDeductionSettings.HighSalaryThreshold + (decimal)(Random.NextDouble() * 120000);
            }
            else
            {
                randomDecimal = (decimal)Random.NextDouble() * SalaryDeductionSettings.HighSalaryThreshold;
            }

            return randomDecimal;
        }

        [Fact]
        public void ShouldReturnCorrectAmount()
        {
            var rule = new HighSalaryFeeRule(MockOptions.Object);
            var employee = new Employee
            {
                Salary = GenerateRandomDecimal(true)
            };

            var deduction = rule.CalculateYearlyDeduction(employee);

            var expectedDeduction = employee.Salary * SalaryDeductionSettings.HighSalaryDeductionPercent;
            Assert.Equal(expectedDeduction, deduction.Amount);
            Assert.Equal(PaymentFrequency.Yearly, deduction.Frequency);
        }

        [Fact]
        public void ShouldReturnTrueWhenSalaryAboveThreshold()
        {
            var rule = new HighSalaryFeeRule(MockOptions.Object);
            var employee = new Employee
            {
                Salary = GenerateRandomDecimal(true)
            };

            var appliesToEmployee = rule.AppliesToEmployee(employee);
            Assert.True(appliesToEmployee);
        }

        [Fact]
        public void ShouldReturnFalseWhenSalaryBelowThreshold()
        {
            var rule = new HighSalaryFeeRule(MockOptions.Object);
            var employee = new Employee
            {
                Salary = GenerateRandomDecimal(false)
            };

            var appliesToEmployee = rule.AppliesToEmployee(employee);
            Assert.False(appliesToEmployee);
        }

    }
}
