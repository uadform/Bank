using Bank.WebApi.Model.DTOs;

namespace Bank.WebApi.Model.Entities
{
    public class UserEntity
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public List<Account> Accounts { get; set; }
    }
}
