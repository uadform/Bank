namespace Bank.WebApi.Model.Entities
{
    public class TransactionEntity
    {
        public int TransactionId { get; set; }
        public int FromAccountId { get; set; }
        public int ToAccountId { get; set; }
        public decimal Amount { get; set; }
        public decimal TransactionFee { get; set; } = 1;
        public DateTime Timestamp { get; set; }
    }
}
