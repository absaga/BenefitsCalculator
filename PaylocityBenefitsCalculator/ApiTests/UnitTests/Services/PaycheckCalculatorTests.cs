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

namespace ApiTests.UnitTests.Services
{
    public class PaycheckCalculatorServiceTests
    {
        private readonly Mock<ISalaryDeductionCalculatorService> _salaryDeductionCalculatorServiceMock;
        private readonly Mock<IEmployeeService> _employeeServiceMock;
        //private readonly Mock<IMapperService> _mapperServiceMock;
        private readonly Mock<IMapperService> _mapperServiceMock;
        private readonly IMapperService _mapperService;
        private readonly IOptions<SalarySettings> _salarySettings;
        private readonly PaycheckCalculatorService _paycheckCalculatorService;
        private readonly Random _random;

        public PaycheckCalculatorServiceTests()
        {
            _salaryDeductionCalculatorServiceMock = new Mock<ISalaryDeductionCalculatorService>();
            // CallBase is set to true to call the actual implementation of the since the service returns using these methods.
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
            var employeeSalary = Math.Round((decimal)(_random.NextDouble() * 150000 + 60000), 2); // Random salary between 60,000 and 150,000

            var employee = new Employee
            {
                Id = 1,
                FirstName = "Abdiel",
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

            // I'll only test the base benefits fee on this one.

            var yearlyDeductions = new List<Deduction>
            {
                new Deduction { Description = "Base Benefits Fee", Amount = 12000 }
            };

            _mapperServiceMock.Setup(s => s.MapDtoToEmployee(employeeDto)).Returns(employee);
            _mapperServiceMock.Setup(m => m.MapEmployeePaychecksToDto(It.IsAny<Employee>(), It.IsAny<List<Paycheck>>()))
                              .Returns<Employee, List<Paycheck>>((emp, paychecks) => _mapperService.MapEmployeePaychecksToDto(emp, paychecks));

            _employeeServiceMock.Setup(s => s.GetByIdAsync(employee.Id)).ReturnsAsync(employeeDto);
            _salaryDeductionCalculatorServiceMock.Setup(s => s.CalculateYearlyDeduction(employee)).ReturnsAsync(yearlyDeductions);
            //_mapperServiceMock.Setup(s => s.MapDtoToEmployee(employeeDto)).Returns(employee);

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

        //[Fact]
        //public async Task CalculatesCorrectPaycheckDeductionsForEmployeeWithDependents()
        //{
        //    var employeeSalary = Math.Round((decimal)(_random.NextDouble() * 120000 + 30000), 2); // Random salary between 30,000 and 150,000

        //    var employee = new Employee
        //    {
        //        Id = 2,
        //        Salary = employeeSalary,
        //        Dependents = new List<Dependent>
        //        {
        //            new Dependent { Id = 1, FirstName = "John", LastName = "Doe", DateOfBirth = new DateTime(1990, 1, 1) }
        //        }
        //    };

        //    var employeeDto = new GetEmployeeDto
        //    {
        //        Id = employee.Id,
        //        Salary = employee.Salary,
        //        Dependents = employee.Dependents.Select(d => new GetDependentDto
        //        {
        //            Id = d.Id,
        //            FirstName = d.FirstName,
        //            LastName = d.LastName,
        //            DateOfBirth = d.DateOfBirth
        //        }).ToList()
        //    };

        //    var yearlyDeductions = new List<Deduction>
        //    {
        //        new Deduction { Description = "Base Benefits Fee", Amount = 12000 },
        //        new Deduction { Description = "Dependent Fee", Amount = 7200 } // e.g., $600 per month for 12 months
        //    };

        //    _employeeServiceMock.Setup(s => s.GetByIdAsync(employee.Id)).ReturnsAsync(employeeDto);
        //    _salaryDeductionCalculatorServiceMock.Setup(s => s.CalculateYearlyDeduction(employee)).ReturnsAsync(yearlyDeductions);
        //    _mapperServiceMock.Setup(s => s.MapDtoToEmployee(employeeDto)).Returns(employee);

        //    var result = await _paycheckCalculatorService.GetYearlyPaychecksByIdAsync(employee.Id);

        //    Assert.Equal(26, result.Paychecks.Count);

        //    decimal totalGrossSalary = 0;
        //    decimal totalBaseBenefitDeductions = 0;
        //    decimal totalDependentDeductions = 0;

        //    foreach (var paycheck in result.Paychecks)
        //    {
        //        totalGrossSalary += paycheck.GrossSalary;
        //        totalBaseBenefitDeductions += paycheck.Deductions
        //            .Where(d => d.Description == "Base Benefits Fee")
        //            .Sum(d => d.Amount);
        //        totalDependentDeductions += paycheck.Deductions
        //            .Where(d => d.Description == "Dependent Fee")
        //            .Sum(d => d.Amount);
        //    }

        //    // Assert total salary matches
        //    Assert.Equal(employeeSalary, totalGrossSalary);

        //    // Assert total base benefit deductions match
        //    Assert.Equal(yearlyDeductions.First(d => d.Description == "Base Benefits Fee").Amount, totalBaseBenefitDeductions);

        //    // Assert total dependent deductions match
        //    Assert.Equal(yearlyDeductions.First(d => d.Description == "Dependent Fee").Amount, totalDependentDeductions);
        //}
    }
}
