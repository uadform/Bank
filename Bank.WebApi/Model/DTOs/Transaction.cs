namespace Bank.WebApi.Model.DTOs
{
    public class Transaction
    {
        public int FromAccountId { get; set; }
        public int ToAccountId { get; set; }
        public decimal Amount { get; set; }
    }
}
