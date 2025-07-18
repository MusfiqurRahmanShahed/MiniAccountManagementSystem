namespace AccountManagementSystem.Models
{
    public class VoucherEntry
    {
        public DateTime Date { get; set; }
        public string ReferenceNo { get; set; } = "";

        public List<VoucherLine>Lines { get; set; } = new();
    }
    public class VoucherLine
    {
        public int AccountId { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
    }
}
