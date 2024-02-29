using Microsoft.EntityFrameworkCore;

namespace Account.Repository.EFC;

public class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options)
{
    public DbSet<Model.Entities.Account> Accounts { get; set; }
    
}