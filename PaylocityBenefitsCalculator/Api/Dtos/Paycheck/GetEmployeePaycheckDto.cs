namespace Api.Dtos.Paycheck
{
    public class GetEmployeePaycheckDto
    {

        public int EmployeeId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public decimal GrossSalary { get; set; }
        public decimal TakeHomePay { get; set; }
        public List<PaycheckDto> YearlyPaychecks { get; set; } = new List<PaycheckDto>();
    }
}
