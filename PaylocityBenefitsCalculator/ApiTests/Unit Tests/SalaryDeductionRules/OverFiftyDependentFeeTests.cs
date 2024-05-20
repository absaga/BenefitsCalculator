using Api.Models;
using Api.Rules.SalaryDeduction;
using System;
using System.Collections.Generic;
using Xunit;

namespace ApiTests.UnitTests
{
    public class OverFiftyDependentFeeTests : RuleTest
    {
        [Fact]
        public void CalculateYearlyDeduction_ShouldReturnCorrectAmount()
        {
            var rule = new OverFiftyDependentFeeRule(MockOptions.Object);
            var amountOfDependentsOverFifty = Random.Next(1, 10);
            var dependents = new List<Dependent>();

            for (int i = 0; i < amountOfDependentsOverFifty; i++)
            {
                dependents.Add(new Dependent { DateOfBirth = DateTime.Now.AddYears(-51) });
            }

            var employee = new Employee
            {
                Dependents = dependents
            };

            var deduction = rule.CalculateYearlyDeduction(employee);

            Assert.Equal((SalaryDeductionSettings.OverFiftyDependentFee * amountOfDependentsOverFifty) * 12, deduction.Amount);
            Assert.Equal(PaymentFrequency.Yearly, deduction.Frequency);
        }

        [Fact]
        public void AppliesTo_ShouldReturnTrueWhenOver50DependentsExist()
        {

            var rule = new OverFiftyDependentFeeRule(MockOptions.Object);
            var dependents = new List<Dependent>
            {
                new Dependent { DateOfBirth = DateTime.Now.AddYears(-51) }
            };

            var employee = new Employee
            {
                Dependents = dependents
            };

            var appliesToEmployee = rule.AppliesToEmployee(employee);

            Assert.True(appliesToEmployee);
        }

        [Fact]
        public void AppliesTo_ShouldReturnFalseWhenNoOver50Dependents()
        {
            var rule = new OverFiftyDependentFeeRule(MockOptions.Object);
            var dependents = new List<Dependent>
            {
                new Dependent { DateOfBirth = DateTime.Now.AddYears(-20) }
            };

            var employee = new Employee
            {
                Dependents = dependents
            };

            var appliesToEmployee = rule.AppliesToEmployee(employee);
            Assert.False(appliesToEmployee);
        }
    }
}
