namespace EasyTransfer.Api.Services.Interfaces
{
    public interface IBankAccountService
    {
        public bool AddBankAccount(int? userId, BankAccountDto bankAccountDto);
        public bool Save();
    }
}
