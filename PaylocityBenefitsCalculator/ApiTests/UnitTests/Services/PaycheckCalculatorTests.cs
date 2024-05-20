using Api.Configuration;
using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Dtos.Paycheck;
using Api.Models;
using Api.Services;
using Api.Services.Mapping;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ApiTests.UnitTests
{
    public class PaycheckCalculatorServiceTests
    {

        /*
         * These are some tests for the paycheck calculation service.
         * In a real app there would be many different tests but for now I'm thinking testing two main things:
         * 
         * 1. All 26 deductions add up to the correct yearly deduction amount
         * 2. All 26 paycheck amounts add up to the yearly salary
         * 
         * Since each rule has been tested individually to generate the correct deduction amount, I'm only testing the service here.
         * But a real life app would also need deep integration tests with the paycheck services.
         * 
         */

        private readonly Mock<ISalaryDeductionCalculatorService> _salaryDeductionCalculatorServiceMock;
        private readonly Mock<IEmployeeService> _employeeServiceMock;
        private readonly Mock<IMapperService> _mapperServiceMock;
        // since the service returns using some mapper methods I will have to config Mock to use the real mapper
        private readonly IMapperService _mapperService;
        private readonly IOptions<SalarySettings> _salarySettings;
        private readonly PaycheckCalculatorService _paycheckCalculatorService;
        private readonly Random _random;

        public PaycheckCalculatorServiceTests()
        {
            _salaryDeductionCalculatorServiceMock = new Mock<ISalaryDeductionCalculatorService>();
            _employeeServiceMock = new Mock<IEmployeeService> { CallBase = true };
            _mapperService = new MapperService();
            _mapperServiceMock = new Mock<IMapperService>();
            _salarySettings = Options.Create(new SalarySettings { YearlyPaychecksAmount = 26 });

            _paycheckCalculatorService = new PaycheckCalculatorService(
                _salaryDeductionCalculatorServiceMock.Object,
                _employeeServiceMock.Object,
                _mapperServiceMock.Object,
                _salarySettings
            );

            _random = new Random();
        }

        [Fact]
        public async Task CalculatesCorrectPaycheckDeductionsForEmployeeWithoutDependents()
        {
            // Random salary between 60,000 and 150,000
            var employeeSalary = Math.Round((decimal)(_random.NextDouble() * 150000 + 60000), 2);

            var employee = new Employee
            {
                Id = 1,
                FirstName = "Abdiel", // it's me! hi! Base16: 68697265206d6521
                LastName = "Sanchez",
                Salary = employeeSalary,
                Dependents = new List<Dependent>()
            };

            var employeeDto = new GetEmployeeDto
            {
                Id = employee.Id,
                FirstName = "Abdiel",
                LastName = "Sanchez",
                Salary = employee.Salary,
                Dependents = new List<GetDependentDto>()
            };

            // These are some random deductions
            var yearlyDeductions = new List<Deduction>
            {
                new Deduction { Description = "Base Benefits Fee", Amount = 12000 },
                new Deduction { Description = "High Salary Fee", Amount = (decimal)_random.NextDouble() * (12345.67m - 1234) + 1234 },
                new Deduction { Description = "Awesome Fee", Amount = (decimal)_random.NextDouble() * (12345.67m - 1234) + 5432 }
            };

            // The real service uses a few mapper methods so we'll mock those.
            _mapperServiceMock.Setup(s => s.MapDtoToEmployee(employeeDto)).Returns(employee);
            _employeeServiceMock.Setup(s => s.GetByIdAsync(employee.Id)).ReturnsAsync(employeeDto);
            _salaryDeductionCalculatorServiceMock.Setup(s => s.CalculateYearlyDeduction(employee)).ReturnsAsync(yearlyDeductions);

            //Make mock use the real mapper service for mapping the employee paychecks to dto
            _mapperServiceMock.Setup(m => m.MapEmployeePaychecksToDto(It.IsAny<Employee>(), It.IsAny<List<Paycheck>>()))
                              .Returns<Employee, List<Paycheck>>((emp, paychecks) => _mapperService.MapEmployeePaychecksToDto(emp, paychecks));


            var result = await _paycheckCalculatorService.GetYearlyPaychecksByIdAsync(employee.Id);

            Assert.Equal(26, result.YearlyPaychecks.Count);

            decimal totalGrossSalary = 0;
            decimal totalBaseBenefitDeductions = 0;

            foreach (var paycheck in result.YearlyPaychecks)
            {
                totalGrossSalary += paycheck.GrossSalary;
                totalBaseBenefitDeductions += paycheck.PaycheckDeductions
                    .Where(d => d.Description == "Base Benefits Fee")
                    .Sum(d => d.Amount);
            }

            Assert.Equal(employeeSalary, totalGrossSalary);
            Assert.Equal(yearlyDeductions.First().Amount, totalBaseBenefitDeductions);
        }
    }
}
