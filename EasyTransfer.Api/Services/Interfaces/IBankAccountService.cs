namespace EasyTransfer.Api.Services.Interfaces
{
    public interface IBankAccountService
    {
        public bool AddBankAccount(int? userId, BankAccountDto bankAccountDto);
        public string GenerateAccountNumber();
        public void MakeBankTransfer(int? userId, BankTransferDto bankTransferDto);
        public bool Save();
    }
}
