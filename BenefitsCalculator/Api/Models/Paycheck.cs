namespace Api.Models
{
    public class Paycheck
    {
        public decimal GrossSalary { get; set; }
        public List<Deduction> Deductions { get; set; } = new List<Deduction>();
        public decimal TakeHomePay { get; set; }
    }

}
