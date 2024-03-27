
namespace EasyTransfer.Api.Services
{
    public class BankAccountService : IBankAccountService
    {
        private readonly EasyTransferDBContext _context;
        private readonly IMapper _mapper;

        public BankAccountService(EasyTransferDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public bool AddBankAccount(int? userId, BankAccountDto bankAccountDto)
        {
            var bankAccount = _mapper.Map<BankAccount>(bankAccountDto);
            bankAccount.OwnerId = (int)userId;
            if (userId == null) return false;
            var isExists = 
                _context.BankAccounts
                .Any(u => u.OwnerId == bankAccount.OwnerId && u.Currency == bankAccount.Currency);
            if (isExists)
            {
                throw new AllreadyExistsException("You can hava only one bankAcount per currency");
            }
            bankAccount.AccountNumber = GenerateAccountNumber();
            _context.Add(bankAccount);
            return Save();
        }

        public string GenerateAccountNumber()
        {
            Random d = new Random();
            StringBuilder accountNumber = new StringBuilder("PL49");
            for (int i = 0; i < 6; i++)
            {
                var l = d.Next(1000, 9999);
                accountNumber.Append(' ');
                accountNumber.Append(l.ToString()); 
            }
            return accountNumber.ToString();
        }

        public void MakeBankTransfer(int? userId, BankTransferDto bankTransferDto)
        {
            if (bankTransferDto == null) 
                throw new BadRequestException("Not all transfer details were provided");

            var senderBankAccounts = _context.BankAccounts.Where(c => c.OwnerId == userId);
            var senderBankAccount = senderBankAccounts
                .FirstOrDefault(c => c.Currency == bankTransferDto.Currency);

            var recipientsAccount = _context.BankAccounts
                .FirstOrDefault(n => n.AccountNumber == bankTransferDto.recipientsAccountNumber);

            if (recipientsAccount == null)
                throw new BadRequestException("Wrong recipient's account number");

            if (recipientsAccount.Currency != bankTransferDto.Currency)
                throw new BadRequestException("The account currency of the recipient and sender must be the same");

            if (recipientsAccount == null)
                throw new BadRequestException("Wrong recipient's account number");

            if (senderBankAccount == null)
                throw new BadRequestException($"You do not have bank account in {bankTransferDto.Currency}");

            if(senderBankAccount.Balance - bankTransferDto.Amount < 0)
                throw new BadRequestException("Insufficient funds in the account");
            
            senderBankAccount.Balance -= bankTransferDto.Amount;
            recipientsAccount.Balance += bankTransferDto.Amount;
            Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
