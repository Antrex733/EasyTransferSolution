
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

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
