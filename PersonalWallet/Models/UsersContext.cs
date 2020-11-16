using Microsoft.EntityFrameworkCore;
 
namespace PersonalWallet.Models {
    public sealed class UsersContext : DbContext {
        public DbSet<User> Users { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        
        public UsersContext(DbContextOptions<UsersContext> options) : base(options) => 
            Database.EnsureCreated();
    }
}