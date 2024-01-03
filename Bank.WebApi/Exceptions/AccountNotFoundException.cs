namespace Bank.WebApi.Exceptions
{
    public class AccountNotFoundException : Exception
    {
        public AccountNotFoundException() : base("Account was not found")
        {

        }
    }
}
