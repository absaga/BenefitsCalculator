using Api.Dtos.Paycheck;
using Api.Models;
using Api.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PaycheckController : ControllerBase
    {
        private readonly IPaycheckCalculatorService _paycheckCalculatorService;
        public PaycheckController(IPaycheckCalculatorService paycheckCalculatorService)
        {
            _paycheckCalculatorService = paycheckCalculatorService;
        }

        [SwaggerOperation(Summary = "Get employee paychecks by id")]
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<GetEmployeePaycheckDto>>> Get(int id)
        {
            var employeePaychecks = await _paycheckCalculatorService.GetYearlyPaychecksByIdAsync(id);

            if (employeePaychecks == null)
            {
                return NotFound();
            }

            return new ApiResponse<GetEmployeePaycheckDto>
            {
                Data = employeePaychecks,
                Success = true
            };
        }
    }
}
