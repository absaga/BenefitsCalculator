namespace Api.Configuration
{
    public class SalaryDeductionSettings
    {
        public decimal BaseBenefitsCost { get; set; }
        public decimal BaseDependentCost { get; set; }
        public decimal OverFiftyDependentFee { get; set; }
        public decimal HighSalaryThreshold { get; set; }
        public decimal HighSalaryDeductionPercent { get; set; }
    }
}
