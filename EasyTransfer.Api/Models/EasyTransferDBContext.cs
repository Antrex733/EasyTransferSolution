namespace EasyTransfer.Api.Models
{
    public class EasyTransferDBContext: DbContext
    {
        public EasyTransferDBContext(DbContextOptions<EasyTransferDBContext> options) : base(options)
        {
            
        }
        public DbSet<User> Users { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BankAccount>()
                .Property(b => b.Currency)
                .HasConversion<string>(); // Mapuj wartość enum na tekst

            modelBuilder.Entity<User>()
                .HasMany(a => a.BankAccounts)
                .WithOne(o => o.Owner)
                .HasForeignKey(o => o.OwnerId);

            modelBuilder.Entity<BankAccount>()
                .Property(b => b.Balance)
                .HasPrecision(10, 2);
        }
        
    }
}
