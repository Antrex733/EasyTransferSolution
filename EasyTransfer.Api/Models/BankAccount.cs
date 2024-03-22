namespace EasyTransfer.Api.Models
{
    public class BankAccount
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string AccountNumber { get; set; }
        public decimal Balance { get; set; } = 0;
        public Currency Currency { get; set; } = Currency.PLN;


        public int OwnerId { get; set; }
        public User Owner { get; set; }

    }
}