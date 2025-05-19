namespace Api.Dtos.Employee
{
    public class PaycheckDto
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public decimal GrossPay { get; set; }
        public decimal EmployeeDeductions { get; set; }
        public decimal DependentDeductions { get; set; }
        public decimal DependentOver50Deductions { get; set; }
        public decimal HighEarnerDeductions { get; set; }
        public decimal TotalDeductions { get; set; }
        public decimal NetPay { get; set; }
    }
}
