namespace Bank.WebApi.Model.Entities
{
    public class AccountEntity
    {
        public int AccountId { get; set; }
        public int UserId { get; set; }
        public string Type { get; set; }
        public decimal Balance { get; set; }
        public UserEntity User { get; set; }
    }
}
