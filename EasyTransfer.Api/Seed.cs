namespace EasyTransfer.Api
{
    public class Seed
    {
        private readonly EasyTransferDBContext _context;
        public Seed(EasyTransferDBContext context)
        {
            _context = context;
        }
        public void SeedDataContext()
        {
            if (_context.Database.CanConnect())
            {
                var pendingMigraction = _context.Database.GetPendingMigrations();
                if (pendingMigraction != null && pendingMigraction.Any())
                {
                    _context.Database.Migrate();
                }
                if (!_context.BankAccounts.Any())
                {
                    SeedUsers();
                }

            }
        }
        public void SeedUsers()
        {
            var Users = new List<User>
            {
                new User
                {
                    FirstName = "John",
                    LastName = "Doe",
                    DateOfBirth = new DateTime(1990, 5, 15),
                    Email = "john.doe@example.com",
                    PasswordHash = "hashedPassword123",
                    PhoneNumber = "123-456-7890",
                    BankAccounts = new List<BankAccount>
                    {
                        new BankAccount
                        {
                            Name = "John's savings",
                            AccountNumber = "123456789",
                            Balance = 1000,
                            Currency = "EUR"
                        },
                        new BankAccount
                        {
                            Name = "John's main",
                            AccountNumber = "987654321",
                            Balance = 500,
                            Currency = "PLN"
                        }
                    }
                },
            };

            _context.AddRange(Users);
            _context.SaveChanges();
        }
    }
}
