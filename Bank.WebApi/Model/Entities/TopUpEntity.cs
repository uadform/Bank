namespace Bank.WebApi.Model.Entitities
{
    public class TopUpEntity
    {
        public int TopUpId { get; set; }
        public int AccountId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
