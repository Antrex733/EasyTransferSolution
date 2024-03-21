
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
            _context.Add(bankAccount);
            return Save();
        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
