using Microsoft.EntityFrameworkCore;

namespace Account.Repository.EFC;

public class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options)
{
    public DbSet<Entities.Account> Accounts { get; set; }
    
}