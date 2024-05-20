using Api.Models;
using Api.Rules.SalaryDeduction;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ApiTests.SalaryDeductionUnitTests
{
    public class BaseDependentCost : RuleTest
    {
        [Fact]
        public void ShouldReturnCorrectAmount()
        {
            var rule = new BaseDependentCostRule(MockOptions.Object);
            var dependentAmount = Random.Next(0, 10);

            var employee = new Employee
            {
                Dependents = new List<Dependent>()
            };

            for (int i = 0; i < dependentAmount; i++)
            {
                employee.Dependents.Add(new Dependent());
            }

            var deduction = rule.CalculateYearlyDeduction(employee);

            Assert.Equal(SalaryDeductionSettings.BaseDependentCost * dependentAmount * 12, deduction.Amount);
            Assert.Equal(PaymentFrequency.Yearly, deduction.Frequency);
        }

        [Fact]
        public void ShouldReturnTrueWhenDependentsExist()
        {
            var rule = new BaseDependentCostRule(MockOptions.Object);
            var dependentAmount = Random.Next(1, 5);

            var employee = new Employee
            {
                Dependents = new List<Dependent>()
            };

            for (int i = 0; i < dependentAmount; i++)
            {
                employee.Dependents.Add(new Dependent());
            }

            var appliesToEmployee = rule.AppliesToEmployee(employee);

            Assert.Equal(employee.Dependents.Any(), appliesToEmployee);
        }

        [Fact]
        public void ShouldReturnFalseWhenNoDependentsExist()
        {
            var rule = new BaseDependentCostRule(MockOptions.Object);

            var employee = new Employee
            {
                Dependents = new List<Dependent>()
            };

            var appliesToEmployee = rule.AppliesToEmployee(employee);
            Assert.False(appliesToEmployee);
        }
    }
}
