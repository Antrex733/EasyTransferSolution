namespace EasyTransfer.Api.Dtos
{
    public class BankAccountDto
    {
        public string Name { get; set; }
        public string AccountNumber { get; set; }
        public decimal Balance { get; set; } = 0;
        public Currency Currency { get; set; } = Currency.PLN;
    }
}
