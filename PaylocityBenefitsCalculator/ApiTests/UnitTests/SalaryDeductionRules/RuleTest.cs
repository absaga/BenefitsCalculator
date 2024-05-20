using Api.Configuration;
using Microsoft.Extensions.Options;
using Moq;
using System;

namespace ApiTests.SalaryDeductionUnitTests
{
    public class RuleTest
    {
        /*
         * I'm creating this base rule test class with some common Moq properties
         * and Random because we love Random
         * 
         */
        protected Mock<IOptions<SalaryDeductionSettings>> MockOptions { get; private set; }
        protected SalaryDeductionSettings SalaryDeductionSettings { get; private set; }
        protected Random Random { get; private set; }

        protected RuleTest()
        {
            SalaryDeductionSettings = new SalaryDeductionSettings
            {
                BaseBenefitsCost = 1000m,
                BaseDependentCost = 600m,
                OverFiftyDependentFee = 200m,
                HighSalaryThreshold = 80000m,
                HighSalaryDeductionPercent = 0.02m
            };

            MockOptions = new Mock<IOptions<SalaryDeductionSettings>>();
            MockOptions.Setup(o => o.Value).Returns(SalaryDeductionSettings);
            Random = new Random();
        }

    }
}
