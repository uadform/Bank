namespace Bank.WebApi.Model.DTOs
{
    public class Account
    {
        public int AccountId { get; set; }
        public int UserId { get; set; }
        public string Type { get; set; }
        public decimal Balance { get; set; }
        public User User { get; set; }
    }
}
