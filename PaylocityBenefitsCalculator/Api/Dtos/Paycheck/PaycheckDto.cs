namespace Api.Dtos.Paycheck
{
    public class PaycheckDto
    {
        public decimal GrossSalary { get; set; }
        public decimal TakeHomePay { get; set; }
        public List<PaycheckDeductionDto> PaycheckDeductions { get; set; } = new List<PaycheckDeductionDto>();
    }

}
