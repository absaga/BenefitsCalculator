using Api.Configuration;
using Api.Dtos.Paycheck;
using Api.Models;
using Api.Services.Mapping;
using Microsoft.Extensions.Options;

namespace Api.Services
{
    /*
     * This service will be responsible for calculating the yearly paychecks for an employee.
     * It uses the SalaryDeductionCalculatorService to calculate the yearly deductions for an employee.
     * It handles the logic of splitting the amounts evenly into the amount of yearly paychecks
     * 
     */
    public class PaycheckCalculatorService : IPaycheckCalculatorService
    {
        private readonly ISalaryDeductionCalculatorService _salaryDeductionCalculatorService;
        private readonly IEmployeeService _employeeService;
        private readonly IMapperService _mapperService;
        private readonly int _numberOfPaychecks;

        public PaycheckCalculatorService(ISalaryDeductionCalculatorService salaryDeductionCalculatorService, IEmployeeService employeeService, IMapperService mapperService, IOptions<SalarySettings> salarySettings)
        {
            _salaryDeductionCalculatorService = salaryDeductionCalculatorService;
            _employeeService = employeeService;
            _mapperService = mapperService;
            _numberOfPaychecks = salarySettings.Value.YearlyPaychecksAmount;
        }

        public async Task<GetEmployeePaycheckDto> GetYearlyPaychecksByIdAsync(int employeeId)
        {
            var employeeDto = await _employeeService.GetByIdAsync(employeeId);
            var employee = _mapperService.MapDtoToEmployee(employeeDto);
            var yearlyDeductions = await _salaryDeductionCalculatorService.CalculateYearlyDeduction(employee);

            var paychecks = new List<Paycheck>();
            int numberOfPaychecks = _numberOfPaychecks;

            var grossSalaries = DistributeAmount(employee.Salary, numberOfPaychecks);

            for (int i = 0; i < numberOfPaychecks; i++)
            {
                paychecks.Add(new Paycheck
                {
                    GrossSalary = grossSalaries[i],
                    Deductions = new List<Deduction>()
                });
            }

            foreach (var yearlyDeduction in yearlyDeductions)
            {
                var deductionAmounts = DistributeAmount(yearlyDeduction.Amount, numberOfPaychecks);
                for (int i = 0; i < numberOfPaychecks; i++)
                {
                    paychecks[i].Deductions.Add(new Deduction
                    {
                        Description = yearlyDeduction.Description,
                        Amount = deductionAmounts[i]
                    });
                }
            }

            foreach (var paycheck in paychecks)
            {
                paycheck.TakeHomePay = paycheck.GrossSalary - paycheck.Deductions.Sum(d => d.Amount);
            }

            return _mapperService.MapEmployeePaychecksToDto(employee, paychecks);
        }

        private List<decimal> DistributeAmount(decimal total, int parts)
        {
            // This is from: https://stackoverflow.com/questions/2039405/evenly-divide-a-dollar-amount-decimal-by-an-integer
            var amounts = new List<decimal>();
            while (parts > 0)
            {
                decimal amount = Math.Round(total / parts, 2);
                amounts.Add(amount);
                total -= amount;
                parts--;
            }
            return amounts;
        }
    }
}
