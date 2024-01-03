namespace Bank.WebApi.Model.DTOs
{
    public class AccountCreation
    {
        public int UserId { get; set; }
        public string Type { get; set; }
        public decimal Balance { get; set; }
    }
}
